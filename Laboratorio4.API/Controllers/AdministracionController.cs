using domain.Entities;
using Laboratorio4.API.Context;
using Laboratorio4.API.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Laboratorio4.API.Controllers;

[Route("api/administracion/historias")]
public sealed class HistoriasController(AplicacionDbContext db) : CrudController<HistoriaEntity, HistoriaRequest>(db)
{
    protected override void Aplicar(HistoriaEntity e, HistoriaRequest r) { e.Titulo = r.Titulo; e.Subtitulo = r.Subtitulo; e.Contenido = r.Contenido; }
    protected override object Respuesta(HistoriaEntity e) => new { e.Id, e.Titulo, e.Subtitulo, e.Contenido };
}

[Route("api/administracion/personajes")]
public sealed class PersonajesController(AplicacionDbContext db) : CrudController<PersonajeEntity, PersonajeRequest>(db)
{
    protected override void Aplicar(PersonajeEntity e, PersonajeRequest r)
    {
        e.Nombre = r.Nombre; e.Rol = r.Rol; e.Genero = r.Genero; e.Vestimenta = r.Vestimenta;
        e.Habilidad = r.Habilidad; e.Biografia = r.Biografia; e.ImagenUrl = r.ImagenUrl;
    }
    protected override object Respuesta(PersonajeEntity e) => new { e.Id, e.Nombre, e.Rol, e.Genero, e.Vestimenta, e.Habilidad, e.Biografia, e.ImagenUrl };
}

[Route("api/administracion/controles")]
public sealed class ControlesController(AplicacionDbContext db) : CrudController<ControlEntity, ControlRequest>(db)
{
    protected override void Aplicar(ControlEntity e, ControlRequest r) { e.Tecla = r.Tecla; e.Accion = r.Accion; }
    protected override object Respuesta(ControlEntity e) => new { e.Id, e.Tecla, e.Accion };
}

[Route("api/administracion/creditos")]
public sealed class CreditosController(AplicacionDbContext db) : CrudController<CreditoEntity, CreditoRequest>(db)
{
    protected override void Aplicar(CreditoEntity e, CreditoRequest r) { e.Grupo = r.Grupo; e.Descripcion = r.Descripcion; }
    protected override object Respuesta(CreditoEntity e) => new { e.Id, e.Grupo, e.Descripcion };
}

[Route("api/administracion/creadores")]
public sealed class CreadoresController(AplicacionDbContext db) : CrudController<CreadorEntity, CreadorRequest>(db)
{
    protected override void Aplicar(CreadorEntity e, CreadorRequest r) { e.Nombre = r.Nombre; }
    protected override object Respuesta(CreadorEntity e) => new { e.Id, e.Nombre };
}
