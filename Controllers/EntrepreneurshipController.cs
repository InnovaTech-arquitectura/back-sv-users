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
                Description = user.Description
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
    
}