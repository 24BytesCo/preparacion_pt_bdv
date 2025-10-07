using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace preparacion_pt_bdv.Models
{
    // Representa los datos de configuración para crear el usuario inicial.
    public class InitialUserData
    {
        public string? UserName { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Password { get; set; }
    }
}