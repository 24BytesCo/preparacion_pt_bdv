namespace preparacion_pt_bdv.Dtos.InmuebleDtos
{
    public class InmuebleResponseDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Direccion { get; set; }
        public decimal Precio { get; set; }
        public string? ImagenUrl { get; set; }
        public string? NombreCompletoUsuarioCrea { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}