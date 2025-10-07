using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using preparacion_pt_bdv.Middleware;
using preparacion_pt_bdv.models;
using preparacion_pt_bdv.Token;

namespace preparacion_pt_bdv.Data.Inmuebles
{
    public class InmuebleRepository : IInmuebleRepository
    {
        private readonly AppDbContext _context;
        private readonly IUsuarioSesion _usuarioSesion;
        private readonly UserManager<Usuario> _userManager;

        // Inyecta las dependencias del constructor.
        public InmuebleRepository(AppDbContext context, IUsuarioSesion usuarioSesion, UserManager<Usuario> userManager)
        {
            _context = context;
            _usuarioSesion = usuarioSesion;
            _userManager = userManager;
        }

        // Crea un nuevo registro de Inmueble.
        public async Task CreateInmueble(Inmueble inmueble)
        {
            if (inmueble == null)
            {
                throw new MiddlewareException(HttpStatusCode.BadRequest, errors: new { mensaje = "Los datos del inmueble no pueden ser nulos." });
            }

            var usuarioBd = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion()!) ?? throw new MiddlewareException(HttpStatusCode.NotFound, errors: new { mensaje = "El usuario no es válido o no existe." });
            inmueble.FechaCreacion = DateTime.Now;
            inmueble.UsuarioCreaId = Guid.Parse(usuarioBd!.Id);

            await _context.Inmuebles.AddAsync(inmueble);

            if (await _context.SaveChangesAsync() == 0)
            {
                throw new MiddlewareException(HttpStatusCode.InternalServerError, errors: new { mensaje = "No se pudo crear el inmueble." });
            }
        }

        // Realiza un borrado lógico del Inmueble.
        public async Task DeleteInmueble(int id)
        {
            var usuarioBd = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion()!);

            var inmueble = await _context.Inmuebles.FindAsync(id)
                           ?? throw new MiddlewareException(HttpStatusCode.NotFound, errors: new { mensaje = "El inmueble no existe." });

            inmueble.FechaEliminacion = DateTime.Now;
            inmueble.UsuarioEliminaId = Guid.Parse(usuarioBd!.Id);
            inmueble.Activo = false;

            _context.Inmuebles.Update(inmueble);

            if (await _context.SaveChangesAsync() == 0)
            {
                throw new MiddlewareException(HttpStatusCode.InternalServerError, errors: new { mensaje = "No se pudo eliminar el inmueble." });
            }
        }

        // Obtiene todos los inmuebles activos.
        public async Task<IEnumerable<Inmueble>> GetAllInmuebles()
        {
            return await _context.Inmuebles.Where(i => i.Activo).ToListAsync();
        }

        // Obtiene un inmueble activo por su ID.
        public async Task<Inmueble?> GetInmuebleById(int id)
        {
            if (id <= 0)
            {
                throw new MiddlewareException(HttpStatusCode.BadRequest, errors: new { mensaje = "El ID del inmueble no es válido." });
            }

            return await _context.Inmuebles.FirstOrDefaultAsync(i => i.Id == id && i.Activo);
        }

        // Guarda los cambios en la base de datos.
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        // Actualiza un registro de Inmueble existente.
        public async Task UpdateInmueble(Inmueble inmueble)
        {
            var usuarioBd = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion()!);

            if (inmueble == null)
            {
                throw new MiddlewareException(HttpStatusCode.BadRequest, errors: new { mensaje = "Los datos del inmueble no pueden ser nulos." });
            }

            inmueble.FechaModificacion = DateTime.Now;
            inmueble.UsuarioModificaId = Guid.Parse(usuarioBd!.Id);

            _context.Inmuebles.Update(inmueble);

            if (await _context.SaveChangesAsync() == 0)
            {
                throw new MiddlewareException(HttpStatusCode.InternalServerError, errors: new { mensaje = "No se pudo actualizar el inmueble." });
            }
        }
    }
}