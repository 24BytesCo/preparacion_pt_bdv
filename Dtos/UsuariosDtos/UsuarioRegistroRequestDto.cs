namespace preparacion_pt_bdv.Dtos.UsuariosDtos
{
    // DTO para la solicitud de registro de un nuevo usuario.
    public class UsuarioRegistroRequestDto
    {
        public string? UserName { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Password { get; set; }
    }
}