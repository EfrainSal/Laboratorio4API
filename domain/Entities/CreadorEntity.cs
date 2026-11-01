using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace domain.Entities
{
    public class CreadorEntity
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}
