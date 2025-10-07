using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace preparacion_pt_bdv.models
{
    public class Inmueble
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public string? Nombre { get; set; }
        public string? Direccion { get; set; }

        // Precio del inmueble y el tipo de dato decimal con precision 18 y escala 4
        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal? Precio { get; set; }

        public string? ImagenUrl { get; set; }

        public DateTime? FechaCreacion { get; set; }
    }
}