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
public class UsersController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly Utilities _utilities;
    private readonly EmailService _emailService;
    private readonly ILogger<UsersController> _logger;

    private static string? _recoveryCode;
    private static string? _recoveryEmail;

    public UsersController(DatabaseContext context, Utilities utilities, EmailService emailService, ILogger<UsersController> logger)
    {
        _context = context;
        _utilities = utilities;
        _emailService = emailService;
        _logger = logger;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] UserDTO user)
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
            Password = _utilities.ComputeSHA256Hash(user.Password), // Cambiado aquí
            RoleId = role.Id
        };

        try
        {
            await _context.Users.AddAsync(modelUser);
            await _context.SaveChangesAsync();
            return Ok(new { isSuccess = true });
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

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO user)
    {
        _logger.LogInformation("Login attempt");
        _logger.LogInformation($"User: {user.Email}");
        _logger.LogInformation($"Password: {user.Password}");
        if (user == null)
        {
            return BadRequest("User data is required.");
        }

        var userFind = await _context.Users
            .Where(u =>
                u.Email == user.Email &&
                u.Password == _utilities.ComputeSHA256Hash(user.Password)
            ).FirstOrDefaultAsync();

        if (userFind == null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new { isSuccess = false, token = "", userId = "" });
        }
        else
        {
            var token = _utilities.GenerateJWT(userFind); 
            _logger.LogInformation($"User {userFind.Name} logged in.");
            _logger.LogInformation($"role: {userFind.RoleId}");
            _logger.LogInformation($"Token: {token}");
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = token, userId = userFind.Id , role = userFind.RoleId});
        }
    }

    // Los métodos restantes se mantienen igual excepto por la llamada de encriptación
    // en el método `SetNewPassword`.

    [HttpPost("set-new-password")]
    public IActionResult SetNewPassword([FromBody] PasswordChangeDTO model)
    {
        if (model.NewPassword != model.ConfirmNewPassword)
        {
            return BadRequest("Passwords do not match.");
        }

        var user = _context.Users.FirstOrDefault(u => u.Email == _recoveryEmail);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        user.Password = _utilities.ComputeSHA256Hash(model.NewPassword); // Cambiado aquí
        _context.SaveChanges();

        _emailService.SendPasswordChangedConfirmation(user.Email);

        return Ok("Password updated successfully.");
    }
}
