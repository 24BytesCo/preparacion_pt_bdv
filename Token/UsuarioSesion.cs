using System.Security.Claims;

namespace preparacion_pt_bdv.Token
{
    // Implementa la lógica para obtener la sesión del usuario a partir del contexto HTTP.
    public class UsuarioSesion : IUsuarioSesion
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioSesion(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Obtiene el nombre de usuario (o ID) del token JWT del usuario autenticado.
        public string? ObtenerUsuarioSesion()
        {
            // Accede a los claims del usuario y busca el identificador de nombre.
            return _httpContextAccessor.HttpContext!.User?.Claims?
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}