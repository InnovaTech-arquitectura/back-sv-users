using Microsoft.AspNetCore.Mvc;
using back_SV_users.Data;
using Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

[Route("cupones")]
[ApiController]
public class CuponesController : ControllerBase
{
    private readonly DatabaseContext _context;

    public CuponesController(DatabaseContext context)
    {
        _context = context;
    }

    // Nueva acción: Listar todos los CouponEntrepreneurship según el ID del usuario
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(long userId)
    {
        // Obtener todos los emprendimientos del usuario
        var entrepreneurships = await _context.Entrepreneurships
            .Where(e => e.UserEntityId == userId) // Filtrar por UserEntityId
            .Select(e => e.Id) // Seleccionar solo los IDs de los emprendimientos
            .ToListAsync();

        if (!entrepreneurships.Any())
            return NotFound(new { message = $"No se encontraron emprendimientos para el usuario con ID {userId}." });

        // Obtener todos los cupones asociados con los emprendimientos del usuario
        var coupons = await _context.CouponEntrepreneurships
            .Where(ce => entrepreneurships.Contains(ce.IdEntrepreneurship))
            .ToListAsync();

        if (!coupons.Any())
            return NotFound(new { message = $"No se encontraron cupones para los emprendimientos del usuario con ID {userId}." });

        return Ok(coupons);
    }

    // Listar todos los CouponEntrepreneurship según el ID del emprendimiento
    [HttpGet("entrepreneurship/{entrepreneurshipId}")]
    public async Task<IActionResult> GetByEntrepreneurshipId(long entrepreneurshipId)
    {
        var coupons = await _context.CouponEntrepreneurships
            .Where(ce => ce.IdEntrepreneurship == entrepreneurshipId)
            .ToListAsync();

        if (!coupons.Any())
            return NotFound(new { message = $"No se encontraron cupones para el emprendimiento con ID {entrepreneurshipId}." });

        return Ok(coupons);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCouponEntrepreneurship(long id)
    {
        var couponEntrepreneurship = await _context.CouponEntrepreneurships
            .FirstOrDefaultAsync(ce => ce.id == id);

        if (couponEntrepreneurship == null)
            return NotFound();

        return Ok(couponEntrepreneurship);
    }

    // Obtener un CouponEntrepreneurship por ID según el emprendimiento
    [HttpGet("{id}/entrepreneurship/{entrepreneurshipId}")]
    public async Task<IActionResult> GetByIdAndEntrepreneurship(long id, long entrepreneurshipId)
    {
        var coupon = await _context.CouponEntrepreneurships
            .FirstOrDefaultAsync(ce => ce.id == id && ce.IdEntrepreneurship == entrepreneurshipId);

        if (coupon == null)
            return NotFound(new { message = $"No se encontró el cupón con ID {id} para el emprendimiento con ID {entrepreneurshipId}." });

        return Ok(coupon);
    }

    // Actualizar solo el valor del estado (Active) según el ID del CouponEntrepreneurship
    [HttpPatch("{id}/toggle-active")]
    public async Task<IActionResult> ToggleActive(long id)
    {
        var couponEntrepreneurship = await _context.CouponEntrepreneurships
            .FirstOrDefaultAsync(ce => ce.id == id);

        if (couponEntrepreneurship == null)
            return NotFound(new { message = $"No se encontró el cupón con ID {id}." });

        couponEntrepreneurship.Active = !couponEntrepreneurship.Active;
        _context.Entry(couponEntrepreneurship).Property(c => c.Active).IsModified = true;

        try
        {
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Estado cambiado a {(couponEntrepreneurship.Active ? "activo" : "inactivo")} para el cupón con ID {id}." });
        }
        catch (DbUpdateException dbEx)
        {
            return StatusCode(500, new { message = $"Error de actualización de base de datos: {dbEx.Message}" });
        }
    }
}
