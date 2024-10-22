using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using back_SV_users.Data;
using back_sv_users.Models.Entities;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[AllowAnonymous] // Ajusta esto según tu lógica de seguridad
[ApiController]
public class CouponsController : ControllerBase
{
    private readonly DatabaseContext _context;

    public CouponsController(DatabaseContext context)
    {
        _context = context;
    }

    // Endpoint para obtener todos los cupones de un emprendimiento
    [HttpGet("entrepreneurship/{entrepreneurshipId}/coupons")]
    public async Task<IActionResult> GetCouponsByEntrepreneurship(long entrepreneurshipId)
    {
        // Consulta para obtener cupones vinculados al emprendimiento
        var coupons = await _context.CouponEntrepreneurships
            .Where(ce => ce.IdEntrepreneurship == entrepreneurshipId)
            .Select(ce => ce.IdCouponNavigation) // Navegamos hacia los cupones
            .ToListAsync();

        if (coupons == null || !coupons.Any())
        {
            return NotFound("No coupons found for the given entrepreneurship.");
        }

        return Ok(coupons);
    }

    // Puedes agregar otros endpoints si los necesitas, como agregar, editar o eliminar cupones.
}
