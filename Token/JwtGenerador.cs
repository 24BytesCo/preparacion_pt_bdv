using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using preparacion_pt_bdv.models;

namespace preparacion_pt_bdv.Token
{
    // Implementa la generación de JSON Web Tokens (JWT).
    public class JwtGenerador : IJwtGenerador
    {
        private readonly IConfiguration _configuration;

        // Inyecta la configuración para acceder a la clave secreta del JWT.
        public JwtGenerador(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Crea un token JWT para un usuario específico.
        public string CrearToken(Usuario usuario)
        {
            // Define los claims que contendrá el token.
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.NameId, usuario.UserName!),
                new(JwtRegisteredClaimNames.Email, usuario.Email!),
                new("userId", usuario.Id)
            };

            // Obtiene la clave secreta desde la configuración segura.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            // Crea las credenciales de firma con el algoritmo de seguridad.
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Describe el token, incluyendo claims, expiración y credenciales.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = credenciales
            };

            // Genera y escribe el token en formato de cadena.
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}