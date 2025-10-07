using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using preparacion_pt_bdv.models;

namespace preparacion_pt_bdv.Token
{
    public class JwtGenerador : IJwtGenerador
    {

        //Inyección de la configuración de la aplicación
        private readonly IConfiguration _configuration;

        /**
        * @brief Constructor que inyecta la configuración de la aplicación.
        * @param configuration El servicio de configuración para acceder a appsettings y User Secrets.
        */
        public JwtGenerador(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CrearToken(Usuario usuario)
        {
            //Lista de claims que se incluirán en el token
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.NameId, usuario.UserName!),
                new(JwtRegisteredClaimNames.Email, usuario.Email!),
                new("userId", usuario.Id)
            };

            //Clave secreta para firmar el token (debe ser segura y almacenada de forma segura)
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            //Credenciales de firma utilizando el algoritmo HMAC SHA256
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = credenciales
            };

            //Genera el token JWT
            var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(tokenDescriptor);

            //Retorna el token como una cadena
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
