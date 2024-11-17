using Microsoft.AspNetCore.Mvc;
using back_SV_users.Data;
using Models;
using Microsoft.EntityFrameworkCore;
using Custom;
using DTO;
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

[Route("NavBar")]
[AllowAnonymous]
[ApiController]
public class PlanCuponController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly Utilities _utilities;

    public PlanCuponController(DatabaseContext context, Utilities utilities)
    {
        _context = context;
        _utilities = utilities;
    }

    // Obtener funcionalidades de un plan específico
    [HttpGet("{planId}/functionalities")]
    public async Task<IActionResult> GetFunctionalitiesByPlan(long planId)
    {
        try
        {
            var planExists = await _context.Plans.AnyAsync(p => p.Id == planId);
            if (!planExists)
            {
                return NotFound("Plan not found.");
            }

            var functionalities = await _context.PlanFunctionalities
                .Where(pf => pf.PlanId == planId)
                .Include(pf => pf.Functionality) // Incluye la relación con Functionality
                .Select(pf => new
                {
                    FunctionalityId = pf.Functionality.Id,
                    Name = pf.Functionality.Name,
                    Description = pf.Functionality.Description
                })
                .ToListAsync();

            if (!functionalities.Any())
            {
                return NotFound("No functionalities found for this plan.");
            }

            return Ok(functionalities);
        }
        catch (Exception ex)
        {
            // Registro detallado del error
            Console.WriteLine($"Error fetching functionalities: {ex.Message}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    // Obtener todas las funcionalidades de todos los planes
    [HttpGet("functionalities")]
    public async Task<IActionResult> GetAllFunctionalitiesByPlans()
    {
        try
        {
            var functionalities = await _context.PlanFunctionalities
                .Join(
                    _context.Functionalities,
                    pf => pf.FunctionalityId,
                    f => f.Id,
                    (pf, f) => new
                    {
                        PlanId = pf.PlanId,
                        FunctionalityId = f.Id,
                        Name = f.Name,
                        Description = f.Description
                    }
                )
                .OrderBy(result => result.PlanId)
                .ToListAsync();

            return Ok(functionalities);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching functionalities: {ex.Message}");
            return StatusCode(500, "Internal server error.");
        }
    }



   [HttpGet("users/{userId}/functionalities")]
public async Task<IActionResult> GetUserFunctionalities(long userId)
{
    try
    {
        // Verificar si el usuario tiene un emprendimiento asociado
        var entrepreneurship = await _context.Entrepreneurships
            .Where(e => e.UserEntityId == userId)
            .FirstOrDefaultAsync();

        if (entrepreneurship == null)
        {
            return NotFound("Entrepreneurship not found for this user.");
        }

        // Verificar si el emprendimiento tiene un plan activo
        var subscription = await _context.Subscriptions
            .Where(s => s.EntrepreneurshipId == entrepreneurship.Id)
            .OrderByDescending(s => s.ExpirationDate) // Asumimos que el más reciente es el activo
            .FirstOrDefaultAsync();

        if (subscription == null)
        {
            return NotFound("No active plan found for the user's entrepreneurship.");
        }

        // Obtener funcionalidades asociadas al plan del emprendimiento
        var planFunctionalities = await _context.PlanFunctionalities
            .Where(pf => pf.PlanId == subscription.IdPlan)
            .Include(pf => pf.Functionality)
            .Select(pf => new
            {
                FunctionalityId = pf.Functionality.Id,
                Name = pf.Functionality.Name,
                Description = pf.Functionality.Description,
                Source = "Plan" // Indica que viene del plan
            })
            .ToListAsync();

        // Obtener funcionalidades asociadas a cupones activos
        var activeCouponsFunctionalities = await _context.CouponEntrepreneurships
            .Where(ce => ce.IdEntrepreneurship == entrepreneurship.Id && ce.Active) // Solo cupones activos
            .Join(
                _context.CouponFunctionalities,
                ce => ce.IdCoupon,
                cf => cf.IdCoupon,
                (ce, cf) => cf
            )
            .Include(cf => cf.IdFunctionalityNavigation) // Incluye la funcionalidad asociada
            .Select(cf => new
            {
                FunctionalityId = cf.IdFunctionalityNavigation.Id,
                Name = cf.IdFunctionalityNavigation.Name,
                Description = cf.IdFunctionalityNavigation.Description,
                Source = "Coupon" // Indica que viene de un cupón
            })
            .ToListAsync();

        // Combinar ambas listas de funcionalidades y eliminar duplicados
        var functionalities = planFunctionalities
            .Concat(activeCouponsFunctionalities)
            .GroupBy(f => f.FunctionalityId) // Agrupar por ID de funcionalidad
            .Select(g => g.First()) // Tomar la primera entrada por cada ID único
            .ToList();

        if (!functionalities.Any())
        {
            return NotFound("No functionalities found for the user's plan or active coupons.");
        }

        return Ok(functionalities);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error fetching functionalities for user: {ex.Message}");
        return StatusCode(500, $"Internal server error: {ex.Message}");
    }
}

}




