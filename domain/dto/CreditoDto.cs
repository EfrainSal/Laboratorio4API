using System;
using System.Collections.Generic;
using System.Text;

namespace domain.dto
{
    public class CreditoDto
    {
        public string? Grupo { get; set; }
        public string? Descripcion { get; set; }
        public List<string?> Creadores { get; set; }
    }
}
