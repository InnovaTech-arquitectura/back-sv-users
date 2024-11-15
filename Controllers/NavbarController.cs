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

[Route("navbar")]
[AllowAnonymous]
[ApiController]
public class NavbarController : ControllerBase
{
    private readonly DatabaseContext _context;

 public NavbarController(DatabaseContext context)
    {
        _context = context;
       
    }
[HttpGet("user/{userId}/functionalities")]
public async Task<IActionResult> GetFunctionalitiesByUserId(long userId)
{
    // Obtener todos los emprendimientos asociados al usuario
    var entrepreneurships = await _context.Entrepreneurships
        .Where(e => e.UserEntityId == userId)
        .Select(e => e.Id)
        .ToListAsync();

    if (!entrepreneurships.Any())
        return NotFound(new { message = $"No se encontraron emprendimientos para el usuario con ID {userId}." });

    // Funcionalidades del plan activo
    var planFunctionalities = await _context.Subscriptions
        .Where(s => entrepreneurships.Contains(s.EntrepreneurshipId.Value) && s.ExpirationDate > DateOnly.FromDateTime(DateTime.UtcNow))
        .Include(s => s.IdPlanNavigation) // Incluye la navegación hacia el Plan
        .ThenInclude(p => p.PlanFunctionalities)
        .SelectMany(s => s.IdPlanNavigation.PlanFunctionalities
            .Where(pf => pf.Functionality != null)
            .Select(pf => pf.Functionality!))
        .ToListAsync();

    // Funcionalidades de los cupones activos
    var couponFunctionalities = await _context.CouponEntrepreneurships
        .Where(ce => entrepreneurships.Contains(ce.IdEntrepreneurship) && ce.Active)
        .Include(ce => ce.Coupon)
        .ThenInclude(c => c.CouponFunctionalities)
        .SelectMany(ce => ce.Coupon.CouponFunctionalities
            .Where(cf => cf.IdFunctionalityNavigation != null)
            .Select(cf => cf.IdFunctionalityNavigation))
        .ToListAsync();

    // Combinar y eliminar duplicados
    var allFunctionalities = planFunctionalities.Concat(couponFunctionalities)
        .Distinct()
        .ToList();

    if (!allFunctionalities.Any())
        return NotFound(new { message = $"No se encontraron funcionalidades activas para los emprendimientos del usuario con ID {userId}." });

    return Ok(allFunctionalities);
}



}

    