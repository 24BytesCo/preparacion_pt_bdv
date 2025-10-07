namespace preparacion_pt_bdv.Dtos.UsuariosDtos
{
    // DTO para la solicitud de login de un usuario.
    public class UsuarioLoginRequestDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}