using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace domain.Entities
{
    public class HistoriaEntity
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Subtitulo { get; set; }
        public string Contenido { get; set; }
    }
}
