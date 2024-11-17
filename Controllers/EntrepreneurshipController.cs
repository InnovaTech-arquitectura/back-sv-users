using Microsoft.AspNetCore.Mvc;
using back_SV_users.Data;
using back_SV_users;
using System.Linq;
using Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Microsoft.AspNetCore.Authorization;
using Custom;
using DTO;
using System;
using Plan = back_sv_users.Models.Entities.Plan; // Incluye solo el modelo Plan en el controlador
using Subscription = back_sv_users.Models.Entities.Subscription;

[Route("api/[controller]")]
[ApiController]
public class EntrepreneurshipController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly Utilities _utilities;
    private readonly EmailService _emailService;

    private static string SECRET_KEY = "mySecretKey12345"; // Clave secreta utilizada en el frontend

    public EntrepreneurshipController(DatabaseContext context, Utilities utilities, EmailService emailService)
    {
        _context = context;
        _utilities = utilities;
        _emailService = emailService;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterEntrepreneurshipDTO user)
    {
        if (user == null)
        {
            return BadRequest("User data is required.");
        }

        // Desencriptamos la contraseña usando XOR
        string decryptedPassword = DecryptPassword(user.Password);

        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == user.RoleName);
        if (role == null)
        {
            return BadRequest("Role does not exist.");
        }

        var modelUser = new User
        {
            Id_card = user.Id_card,
            Name = user.Name,
            Email = user.Email,
            Password = _utilities.ComputeSHA256Hash(decryptedPassword), // Usamos la contraseña desencriptada
            RoleId = role.Id
        };
    
        try
        {
            // Agregar primero el usuario
            await _context.Users.AddAsync(modelUser);
            await _context.SaveChangesAsync(); // Guardar para obtener el ID del usuario creado

            // Crear el emprendimiento (entrepreneurship) vinculado al usuario
            var entrepreneurship = new Entrepreneurship
            {
                UserEntityId = modelUser.Id,
                Name = user.Name,
                Names = user.Names,
                LastNames = user.LastNames,
                Description = user.Description,
                Logo = "null"
            };

            await _context.Entrepreneurships.AddAsync(entrepreneurship);
            await _context.SaveChangesAsync(); // Guardar para obtener el ID del entrepreneurship

            // Crear la suscripción con el Plan 1
            var plan1 = await _context.Plans.FindAsync((long)1); // Suponiendo que el ID del Plan 1 es 1
            if (plan1 == null)
            {
                return BadRequest("Default Plan (Plan 1) does not exist.");
            }

            var subscription = new Subscription
            {
                EntrepreneurshipId = entrepreneurship.Id,
                IdPlan = plan1.Id,
                Amount = plan1.Price,
                InitialDate = DateOnly.FromDateTime(DateTime.UtcNow),
                ExpirationDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1))
            };

            await _context.Subscriptions.AddAsync(subscription);
            await _context.SaveChangesAsync(); // Guardar los cambios

            return Ok(new { isSuccess = true, UserId = modelUser.Id, EntrepreneurshipId = entrepreneurship.Id, SubscriptionId = subscription.Id });
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"Database Update Error: {dbEx.InnerException?.Message}");
            return StatusCode(500, $"Database Update Error: {dbEx.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Internal server error: {ex.Message}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // Método para desencriptar la contraseña utilizando XOR
    private string DecryptPassword(string encryptedPassword)
    {
        // Primero, decodificamos la contraseña cifrada de Base64 a texto
        string encrypted = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encryptedPassword));
        string decrypted = "";

        // Desencriptamos la contraseña utilizando XOR con la clave secreta
        for (int i = 0; i < encrypted.Length; i++)
        {
            decrypted += (char)(encrypted[i] ^ SECRET_KEY[i % SECRET_KEY.Length]);
        }

        return decrypted;
    }

    //Método para cambiar el plan del emprendimiento
    [HttpPost]
    [Route("SelectPlan")]
    public async Task<IActionResult> SelectPlan([FromBody] SelectPlanDTO selection)
    {
        if (selection == null || selection.Id <= 0 || selection.Id_plan <= 0)
            return BadRequest("Entrepreneurship ID and Plan ID are required.");

        var entrepreneurship = await _context.Entrepreneurships
            .Where(e => e.Id == selection.Id)
            .FirstOrDefaultAsync();

        if (entrepreneurship == null)
            return BadRequest("Entrepreneurship does not exist.");

        var plan = await _context.Plans.FindAsync((long)selection.Id_plan);
        if (plan == null)
            return BadRequest("Plan does not exist.");

        // Buscar la suscripción existente para este entrepreneurship_id
        var subscription = await _context.Subscriptions
            .Where(s => s.EntrepreneurshipId == selection.Id)
            .FirstOrDefaultAsync();

        if (subscription != null)
        {
            // Si ya existe una suscripción, actualizamos el id_plan y otros detalles
            subscription.IdPlan = selection.Id_plan;
            subscription.Amount = plan.Price;
            subscription.InitialDate = DateOnly.FromDateTime(DateTime.UtcNow);
            subscription.ExpirationDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1));
        }
        else
        {
            // Si no existe una suscripción, se crea una nueva (esto es opcional y depende de la lógica de negocio)
            subscription = new Subscription
            {
                EntrepreneurshipId = selection.Id,
                IdPlan = selection.Id_plan,
                Amount = plan.Price,
                InitialDate = DateOnly.FromDateTime(DateTime.UtcNow),
                ExpirationDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1))
            };

            await _context.Subscriptions.AddAsync(subscription);
        }

        try
        {
            await _context.SaveChangesAsync();
            return Ok(new { isSuccess = true, SubscriptionId = subscription.Id });
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"Database Update Error: {dbEx.InnerException?.Message}");
            return StatusCode(500, $"Database Update Error: {dbEx.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Internal server error: {ex.Message}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("plans")]
    public async Task<IActionResult> GetPlans()
    {
        var plans = await _context.Plans.ToListAsync();
        return Ok(plans);
    }

    [HttpGet]
    [Route("ActivePlan/{entrepreneurshipId}")]
    public async Task<IActionResult> GetActivePlan(int entrepreneurshipId)
    {
        var subscription = await _context.Subscriptions
            .Where(s => s.EntrepreneurshipId == entrepreneurshipId && s.ExpirationDate > DateOnly.FromDateTime(DateTime.UtcNow))
            .FirstOrDefaultAsync();

        if (subscription == null)
            return NotFound("No active plan found for this entrepreneurship.");

        // Cargar el plan utilizando el IdPlan de la suscripción
        var plan = await _context.Plans.FindAsync(subscription.IdPlan);
        if (plan == null)
            return NotFound("Plan associated with the subscription not found.");

        var result = new
        {
            SubscriptionId = subscription.Id,
            Amount = subscription.Amount,
            InitialDate = subscription.InitialDate,
            ExpirationDate = subscription.ExpirationDate,
            Plan = new
            {
                PlanId = plan.Id,
                PlanName = plan.Name,
                PlanPrice = plan.Price
            }
        };

        return Ok(result);
    }
}