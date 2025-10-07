using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace preparacion_pt_bdv.models
{
    // Representa la entidad Inmueble en la base de datos.
    public class Inmueble
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public string? Nombre { get; set; }
        public string? Direccion { get; set; }

        // Define el precio con una precisión específica para datos monetarios.
        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal? Precio { get; set; }

        public string? ImagenUrl { get; set; }

        // Campos de auditoría para el seguimiento de cambios.
        public Guid? UsuarioCreaId { get; set; }
        public Guid? UsuarioModificaId { get; set; }
        public Guid? UsuarioEliminaId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }
        
        // Indica si el registro está activo para borrado lógico.
        public bool Activo { get; set; } = true;
    }
}