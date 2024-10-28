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
[AllowAnonymous]
[ApiController]
public class EntrepreneurshipController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly Utilities _utilities;
    private readonly EmailService _emailService;

    private static string _recoveryCode;
    private static string _recoveryEmail;


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
            Password = _utilities.encriptarSHA256(user.Password),
            RoleId = role.Id
        };
    
        try
        {
            // Agregamos primero el usuario
            await _context.Users.AddAsync(modelUser);
            await _context.SaveChangesAsync(); // Guardamos para obtener el ID del usuario creado

            
            var entrepreneurship = new Entrepreneurship
            {
                Id_user = modelUser.Id,  
                Name = user.Name,
                Names = user.Names,
                LastNames = user.LastNames,
                Description = user.Description,
                Logo = "null"
            };

            await _context.Entrepreneurships.AddAsync(entrepreneurship); // Agregamos el cliente
            await _context.SaveChangesAsync(); // Guardamos los cambios

            return Ok(new { isSuccess = true, UserId = modelUser.Id, EntrepreneurshipId = entrepreneurship.Id });
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


    //Cambiar el plan del emprendomineto
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





    
}