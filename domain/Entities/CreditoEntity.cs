using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace domain.Entities
{
    public class CreditoEntity
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public string Grupo { get; set; }
        public string Descripcion { get; set; }
    }
}
