using System;
using System.Collections.Generic;
using System.Text;

namespace domain.dto
{
    public class PersonajeDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Rol { get; set; }
        public string? Genero { get; set; }
        public string? Vestimenta { get; set; }
        public string? Habilidad { get; set; }
        public string? Biografia { get; set; }
        public string? ImagenUrl { get; set; }
    }
}
