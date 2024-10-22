

using back_SV_users.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Services;

[Route("api/account")]
[AllowAnonymous]
[ApiController]
public class UserController : ControllerBase
{

    private readonly DatabaseContext _context;

    private readonly MinioService _minioService;

    public UserController(DatabaseContext context,MinioService minioService)
    {
        _context = context;
        _minioService = minioService;
    }

    [HttpGet ("client/{id}")]
    public async Task<IActionResult> GetAccountCli(int id)
    {
       // Obtener el usuario asociado al ID del cliente
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        // Devolver el objeto User
        return Ok(user);
    }
    
    [HttpGet ("entrepreneurship/{id}")]
    public async Task<IActionResult> GetAccountEnt(int id)
    {
        // Obtener el usuario basado en el ID
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        //obtneder el emprendimiento buscando por el id del usuario
       var entrepreneurship = await _context.Entrepreneurships
                                         .FirstOrDefaultAsync(e => e.Id_user == user.Id);

        //var entrepreneurship = user.Entrepreneurship;

        if (entrepreneurship == null)
        {
            return NotFound("Entrepreneurship not found.");
        }

        //obtenerfoto de minio

        Console.WriteLine("Logo Stream Content (as text):");

        
        var logoStream = await _minioService.GetObjectAsync("P-"+entrepreneurship.Id);

        //imprimir logoStream
       
        byte[] logo;
        using (var memoryStream = new MemoryStream())
        {
            await logoStream.CopyToAsync(memoryStream);
            logo = memoryStream.ToArray();
        }

        var entrepreneurshipInfoDto = new EntrepreneurshipAccountInfoDTO
        {
            NameTitular =user.Name,
            Id_card = user.Id_card,
            email = user.Email,
            NameEntrepreneurship = entrepreneurship.Name,
            Description = entrepreneurship.Description,
            Logo = logo
        };

        // Devolver el DTO
        return Ok(entrepreneurshipInfoDto);
    }

    [HttpPut ("client/{id}")]
    public async Task<IActionResult> PostAccountCli(int id, [FromBody] ClientAccount account)
    {
        // Obtener el usuario basado en el ID
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        // Actualizar los datos del usuario
        user.Name = account.Name;
        user.Email = account.Email;
        user.Id_card = account.Id_card;

        // Guardar los cambios en la base de datos
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return Ok(user);
    }

    [HttpPut ("entrepreneurship/{id}")]
    public async Task<IActionResult> PostAccountEnt(int id, [FromForm] EntrepreneurshipAccountDTO account)
    {
        //actualizar user con los datos enviados
       //obtener user del id enviado
       //actalizarlo y guardarlo 
       var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound("User not found.");
        }


        user.Name = account.NameTitular;
        user.Email = account.email;
        user.Id_card = account.Id_card;


       //obtener entrepreneurship del id del emprendedor que se obtuvo del user
       //actalizarlo y guardarlo

        var entrepreneurship = await _context.Entrepreneurships
                                         .FirstOrDefaultAsync(e => e.Id_user == user.Id);

        if (entrepreneurship != null)
        {
            // Actualizar el emprendimiento
            entrepreneurship.Name = account.NameEntrepreneurship;
            entrepreneurship.Description = account.Description;
            _context.Entrepreneurships.Update(entrepreneurship);

            // Subir el logo a Minio
            //var logoStream = account.Logo.OpenReadStream();
            await _minioService.UploadFileAsync("P-"+entrepreneurship.Id, account.Logo);
        }
        else
        {
            return NotFound("Entrepreneurship not found.");
        }

        // Guardar los cambios en la base de datos
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return Ok(new { User = user, Entrepreneurship = entrepreneurship });
        //imprimir account.Logo

       
    }


}

