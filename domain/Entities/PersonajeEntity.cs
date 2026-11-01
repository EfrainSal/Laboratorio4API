using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace domain.Entities
{
   
    public class PersonajeEntity
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; }
        public string Genero { get; set; }
        public string Vestimenta { get; set; }
        public string Habilidad { get; set; }
        public string Biografia { get; set; }
        public string ImagenUrl { get; set; }
    }
}
