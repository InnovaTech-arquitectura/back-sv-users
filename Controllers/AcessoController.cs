using Microsoft.AspNetCore.Mvc;
using back_SV_users.Data;
using back_SV_users;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Custom;
using DTO;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly Utilities _utilities;

    private readonly EmailService _emailService;
    private readonly ILogger<UsersController> _logger;

      private static string? _recoveryCode;
    private static string? _recoveryEmail;

    // Clave secreta para el desencriptado (debe coincidir con la del frontend)
    private static string SECRET_KEY = "mySecretKey12345";

    public UsersController(DatabaseContext context, Utilities utilities, EmailService emailService, ILogger<UsersController> logger)
    {
        _context = context;
        _utilities = utilities;
        _emailService = emailService;
        _logger = logger;
    }
    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO user)
    {
        if (user == null)
        {
            return BadRequest("User data is required.");
        }

        // Desencriptamos la contraseña usando XOR
        string decryptedPassword = DecryptPassword(user.Password);

        // Buscamos el usuario en la base de datos con el correo y la contraseña desencriptada
        var userFind = await _context.Users
            .Where(u =>
                u.Email == user.Email &&
                u.Password == _utilities.ComputeSHA256Hash(decryptedPassword) // Aquí se compara con la contraseña encriptada
            ).FirstOrDefaultAsync();

        if (userFind == null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new { isSuccess = false, token = "", userId = "" });
        }
        else
        {
            // Generamos el JWT para el usuario encontrado
            var token = _utilities.GenerateJWT(userFind); 
            _logger.LogInformation($"User {userFind.Name} logged in.");
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = token, userId = userFind.Id , role = userFind.RoleId});
        }
    }

    // Método para desencriptar la contraseña utilizando XOR
    private string DecryptPassword(string encryptedPassword)
    {
        _logger.LogInformation($"Decrypting password:");
        _logger.LogInformation($"Decrypting password:");
        _logger.LogInformation($"Decrypting password: {encryptedPassword}");
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