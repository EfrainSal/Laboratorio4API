using domain.Entities;
using Laboratorio4.API.Context;
using Laboratorio4.API.Contracts;
using Laboratorio4.API.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace Laboratorio4.API.Controllers;

[ApiController]
public sealed class RankingController(AplicacionDbContext db) : ControllerBase
{
    [HttpGet("api/laboratorio/ranking")]
    public async Task<IActionResult> ObtenerRanking([FromQuery] int limite = 50, CancellationToken cancellationToken = default)
    {
        if (limite is < 1 or > 100) return BadRequest(new { mensaje = "El límite debe estar entre 1 y 100." });
        var ranking = await db.Rankings.AsNoTracking()
            .OrderByDescending(r => r.Score).ThenBy(r => r.Id)
            .Take(limite).ToListAsync(cancellationToken);
        return Ok(ranking);
    }

    [HttpPost("api/administracion/ranking")]
    [RequiereClaveAdministracion]
    [EnableRateLimiting("ranking-escritura")]
    [RequestSizeLimit(4096)]
    public async Task<IActionResult> Registrar([FromBody] RankingRequest request, CancellationToken cancellationToken)
    {
        var entity = new RankingEntity
        {
            PlayerName = request.PlayerName.Trim(), Score = request.Score, Time = request.Time,
            CharacterId = request.CharacterId, Date = request.Date.Trim(), Uploaded = request.Uploaded
        };
        db.Rankings.Add(entity);
        await db.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = entity.Id }, entity);
    }

    [HttpGet("api/administracion/ranking/{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id, CancellationToken cancellationToken)
    {
        var entry = await db.Rankings.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        return entry is null ? NotFound(new { mensaje = "Resultado no encontrado." }) : Ok(entry);
    }

    [HttpDelete("api/administracion/ranking/{id:int}")]
    [RequiereClaveAdministracion]
    [EnableRateLimiting("ranking-escritura")]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancellationToken)
    {
        var entry = await db.Rankings.FindAsync(new object[] { id }, cancellationToken);
        if (entry is null) return NotFound(new { mensaje = "Resultado no encontrado." });
        db.Rankings.Remove(entry);
        await db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
