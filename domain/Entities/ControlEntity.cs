using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace domain.Entities
{
    public class ControlEntity
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public string Tecla { get; set; }
        public string Accion { get; set; }
    }
}
