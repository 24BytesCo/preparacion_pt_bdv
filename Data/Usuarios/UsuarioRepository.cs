using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using preparacion_pt_bdv.Dtos.UsuariosDtos;
using preparacion_pt_bdv.Middleware;
using preparacion_pt_bdv.models;
using preparacion_pt_bdv.Token;

namespace preparacion_pt_bdv.Data.Usuarios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IJwtGenerador _jwtGenerador;
        private readonly IUsuarioSesion _usuarioSesion;

        // Inyecta las dependencias necesarias.
        public UsuarioRepository(AppDbContext context, UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador, IUsuarioSesion usuarioSesion)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerador = jwtGenerador;
            _usuarioSesion = usuarioSesion;
        }

        // Convierte una entidad Usuario a un DTO de respuesta, incluyendo el token JWT.
        private UsuarioResponseDto? MapearUsuarioAResponseDto(Usuario usuario)
        {
            if (usuario == null) return null;

            return new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Telefono = usuario.Telefono,
                Email = usuario.Email,
                UserName = usuario.UserName,
                Token = _jwtGenerador.CrearToken(usuario)
            };
        }

        // Obtiene los datos del usuario actualmente autenticado.
        public async Task<UsuarioResponseDto?> GetUsuario()
        {
            var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion()!)
                          ?? throw new MiddlewareException(HttpStatusCode.Unauthorized, new { mensaje = "Usuario no encontrado o no autenticado." });

            return MapearUsuarioAResponseDto(usuario!);
        }

        // Autentica a un usuario con su email y contraseña.
        public async Task<UsuarioResponseDto> Login(UsuarioLoginRequestDto usuarioLogin)
        {
            if (string.IsNullOrEmpty(usuarioLogin.Email) || string.IsNullOrEmpty(usuarioLogin.Password))
            {
                throw new MiddlewareException(HttpStatusCode.BadRequest, new { mensaje = "El email y la contraseña son requeridos." });
            }

            var usuario = await _userManager.FindByEmailAsync(usuarioLogin.Email)
                          ?? throw new MiddlewareException(HttpStatusCode.Unauthorized, new { mensaje = "Credenciales no válidas." });

            var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, usuarioLogin.Password, false);

            if (resultado.Succeeded)
            {
                return MapearUsuarioAResponseDto(usuario)!;
            }

            throw new MiddlewareException(HttpStatusCode.Unauthorized, new { mensaje = "Credenciales no válidas." });
        }

        // Registra un nuevo usuario en el sistema.
        public async Task<UsuarioResponseDto?> RegistrarUsuario(UsuarioRegistroRequestDto usuarioRegistro)
        {
            await ValidacionesRegistroAsync(usuarioRegistro);

            var usuario = new Usuario
            {
                Nombre = usuarioRegistro.Nombre,
                Apellido = usuarioRegistro.Apellido,
                Telefono = usuarioRegistro.Telefono,
                Email = usuarioRegistro.Email,
                UserName = usuarioRegistro.UserName
            };
            var resultado = await _userManager.CreateAsync(usuario, usuarioRegistro.Password!);

            if (resultado.Succeeded)
            {
                return MapearUsuarioAResponseDto(usuario);
            }

            throw new MiddlewareException(HttpStatusCode.BadRequest, new { mensaje = "No se pudo registrar el usuario: " + string.Join(", ", resultado.Errors.Select(e => e.Description)) });
        }

        // Centraliza las validaciones para el registro de un nuevo usuario.
        private async Task ValidacionesRegistroAsync(UsuarioRegistroRequestDto usuarioRegistro)
        {
            if (string.IsNullOrEmpty(usuarioRegistro.UserName) ||
                string.IsNullOrEmpty(usuarioRegistro.Email) ||
                string.IsNullOrEmpty(usuarioRegistro.Nombre) ||
                string.IsNullOrEmpty(usuarioRegistro.Password))
            {
                throw new MiddlewareException(HttpStatusCode.BadRequest, new { mensaje = "El nombre de usuario, nombre, email y contraseña son requeridos." });
            }

            if (await _userManager.FindByEmailAsync(usuarioRegistro.Email) != null)
            {
                throw new MiddlewareException(HttpStatusCode.BadRequest, new { mensaje = "El email ya está en uso." });
            }

            if (await _context.Users.AnyAsync(u => u.UserName == usuarioRegistro.UserName))
            {
                throw new MiddlewareException(HttpStatusCode.BadRequest, new { mensaje = "El nombre de usuario ya está en uso." });
            }
        }

        // Guarda los cambios pendientes en la base de datos.
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}