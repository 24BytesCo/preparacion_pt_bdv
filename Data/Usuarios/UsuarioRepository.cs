using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using preparacion_pt_bdv.Dtos.UsuariosDtos;
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


        public UsuarioRepository(AppDbContext context, UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador, IUsuarioSesion usuarioSesion)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerador = jwtGenerador;
            _usuarioSesion = usuarioSesion;
        }

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


        public async Task<UsuarioResponseDto?> GetUsuario()
        {
            var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion()!);

            if (usuario == null) return null;

            return MapearUsuarioAResponseDto(usuario!);
        }

        /**
         * @brief Valida las credenciales de un usuario y, si son correctas, genera un DTO con sus datos y un token JWT.
         * @param usuarioLogin Un objeto DTO que contiene el email y la contraseña del usuario.
         * @returns Un DTO de respuesta con los datos del usuario y el token si el login es exitoso.
         * @throws ArgumentException si el email o la contraseña no son proporcionados.
         * @throws UnauthorizedAccessException si las credenciales son inválidas (usuario no encontrado o contraseña incorrecta).
         */
        public async Task<UsuarioResponseDto> Login(UsuarioLoginRequestDto usuarioLogin)
        {
            // Validación de entrada
            if (string.IsNullOrEmpty(usuarioLogin.Email) || string.IsNullOrEmpty(usuarioLogin.Password))
            {
                throw new ArgumentException("El email y la contraseña son requeridos.");
            }

            // Búsqueda del usuario
            var usuario = await _userManager.FindByEmailAsync(usuarioLogin.Email) ?? throw new UnauthorizedAccessException("Credenciales no válidas.");

            // Verificación de la contraseña
            var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, usuarioLogin.Password, false);

            if (resultado.Succeeded)
            {
                // Si todo es correcto, mapeamos y devolvemos el DTO.
                return MapearUsuarioAResponseDto(usuario)!;
            }

            // Si la contraseña fue incorrecta, lanzamos error genérico.
            throw new UnauthorizedAccessException("Credenciales no válidas.");
        }

        public async Task<UsuarioResponseDto?> RegistrarUsuario(UsuarioRegistroRequestDto usuarioRegistro)
        {
            // Validación básica de entrada
            if (string.IsNullOrEmpty(usuarioRegistro.UserName) ||
                string.IsNullOrEmpty(usuarioRegistro.Email) ||
                string.IsNullOrEmpty(usuarioRegistro.Nombre) ||
                string.IsNullOrEmpty(usuarioRegistro.Password))
            {
                throw new ArgumentException("El nombre de usuario, nombre, email y contraseña son requeridos.");
            }

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

            // Si hubo errores, lanzamos una excepción con los mensajes de error.
            throw new InvalidOperationException("No se pudo registrar el usuario: " + string.Join(", ", resultado.Errors.Select(e => e.Description)));
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}