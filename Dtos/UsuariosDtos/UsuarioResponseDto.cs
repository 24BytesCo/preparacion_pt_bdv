using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace preparacion_pt_bdv.Dtos.UsuariosDtos
{
    public class UsuarioResponseDto
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Token { get; set; }
    }
}