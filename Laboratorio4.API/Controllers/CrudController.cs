using Laboratorio4.API.Context;
using Laboratorio4.API.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Laboratorio4.API.Controllers;

[ApiController]
public abstract class CrudController<TEntity, TRequest>(AplicacionDbContext db) : ControllerBase
    where TEntity : class, new()
{
    protected abstract void Aplicar(TEntity entity, TRequest request);
    protected abstract object Respuesta(TEntity entity);

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var entities = await db.Set<TEntity>().AsNoTracking().ToListAsync();
        return Ok(entities.Select(Respuesta));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Obtener(int id)
    {
        var entity = await db.Set<TEntity>().FindAsync(id);
        return entity is null ? NotFound(new { mensaje = "Registro no encontrado." }) : Ok(Respuesta(entity));
    }

    [HttpPost]
    [RequiereClaveAdministracion]
    public async Task<IActionResult> Crear([FromBody] TRequest request)
    {
        var entity = new TEntity();
        Aplicar(entity, request);
        db.Set<TEntity>().Add(entity);
        await db.SaveChangesAsync();
        var id = db.Entry(entity).Property<int>("Id").CurrentValue;
        return CreatedAtAction(nameof(Obtener), new { id }, Respuesta(entity));
    }

    [HttpPut("{id:int}")]
    [RequiereClaveAdministracion]
    public async Task<IActionResult> Actualizar(int id, [FromBody] TRequest request)
    {
        var entity = await db.Set<TEntity>().FindAsync(id);
        if (entity is null) return NotFound(new { mensaje = "Registro no encontrado." });
        Aplicar(entity, request);
        await db.SaveChangesAsync();
        return Ok(Respuesta(entity));
    }

    [HttpDelete("{id:int}")]
    [RequiereClaveAdministracion]
    public async Task<IActionResult> Eliminar(int id)
    {
        var entity = await db.Set<TEntity>().FindAsync(id);
        if (entity is null) return NotFound(new { mensaje = "Registro no encontrado." });
        db.Set<TEntity>().Remove(entity);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
