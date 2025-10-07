using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace preparacion_pt_bdv.Dtos.UsuariosDtos
{
    public class UsuarioLoginRequestDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}