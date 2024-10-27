using Microsoft.AspNetCore.Mvc;
using back_SV_users.Data;
using Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class CouponEntrepreneurshipController : ControllerBase
{
    private readonly DatabaseContext _context;

    public CouponEntrepreneurshipController(DatabaseContext context)
    {
        _context = context;
    }

    // Listar todos los CouponEntrepreneurship según el ID del emprendimiento
    [HttpGet("by-entrepreneurship/{entrepreneurshipId}")]
    public async Task<IActionResult> GetByEntrepreneurshipId(long entrepreneurshipId)
    {
        var coupons = await _context.CouponEntrepreneurships
            .Where(ce => ce.IdEntrepreneurship == entrepreneurshipId)
            .ToListAsync();

        if (!coupons.Any())
            return NotFound(new { message = $"No se encontraron cupones para el emprendimiento con ID {entrepreneurshipId}." });

        return Ok(coupons);
    }

    // Obtener un CouponEntrepreneurship por ID según el emprendimiento
    [HttpGet("{id}/by-entrepreneurship/{entrepreneurshipId}")]
    public async Task<IActionResult> GetByIdAndEntrepreneurship(long id, long entrepreneurshipId)
    {
        var coupon = await _context.CouponEntrepreneurships
            .FirstOrDefaultAsync(ce => ce.Id == id && ce.IdEntrepreneurship == entrepreneurshipId);

        if (coupon == null)
            return NotFound(new { message = $"No se encontró el cupón con ID {id} para el emprendimiento con ID {entrepreneurshipId}." });

        return Ok(coupon);
    }

    // Actualizar solo el valor del estado (Active) según el ID del CouponEntrepreneurship
    [HttpPatch("{id}/toggle-active")]
    public async Task<IActionResult> ToggleActive(long id)
    {
        var coupon = await _context.CouponEntrepreneurships.FindAsync(id);

        if (coupon == null)
            return NotFound(new { message = $"No se encontró el cupón con ID {id}." });

        // Cambiar solo el estado activo
        coupon.Active = !coupon.Active;
        _context.Entry(coupon).Property(c => c.Active).IsModified = true;

        try
        {
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Estado cambiado a {(coupon.Active ? "activo" : "inactivo")} para el cupón con ID {id}." });
        }
        catch (DbUpdateException dbEx)
        {
            return StatusCode(500, new { message = $"Error de actualización de base de datos: {dbEx.Message}" });
        }
    }
}
