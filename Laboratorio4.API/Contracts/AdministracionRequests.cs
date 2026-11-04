using System.ComponentModel.DataAnnotations;

namespace Laboratorio4.API.Contracts;

public sealed class HistoriaRequest
{
    [Required] public string Titulo { get; set; } = "";
    [Required] public string Subtitulo { get; set; } = "";
    [Required] public string Contenido { get; set; } = "";
}

public sealed class PersonajeRequest
{
    [Required] public string Nombre { get; set; } = "";
    [Required] public string Rol { get; set; } = "";
    [Required] public string Genero { get; set; } = "";
    [Required] public string Vestimenta { get; set; } = "";
    [Required] public string Habilidad { get; set; } = "";
    [Required] public string Biografia { get; set; } = "";
    [Required] public string ImagenUrl { get; set; } = "";
}

public sealed class ControlRequest
{
    [Required] public string Tecla { get; set; } = "";
    [Required] public string Accion { get; set; } = "";
}

public sealed class CreditoRequest
{
    [Required] public string Grupo { get; set; } = "";
    [Required] public string Descripcion { get; set; } = "";
}

public sealed class CreadorRequest
{
    [Required] public string Nombre { get; set; } = "";
}
