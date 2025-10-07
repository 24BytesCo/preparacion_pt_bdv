using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace preparacion_pt_bdv.models
{
    public class Usuario : IdentityUser
    {

        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Telefono { get; set; }

        //Nombre completo
        public string NombreCompleto
        {
            get
            {
                return $"{Nombre} {Apellido}";
            }
        }

    }
}