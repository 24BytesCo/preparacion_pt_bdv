using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using preparacion_pt_bdv.models;
using preparacion_pt_bdv.Token;

namespace preparacion_pt_bdv.Data.Inmuebles
{
    public class InmuebleRepository : IInmuebleRepository
    {
        private readonly AppDbContext _context;
        private readonly IUsuarioSesion _usuarioSesion;

        private readonly UserManager<Usuario> _userManager;

        /// <summary>
        /// Constructor del repositorio de inmuebles que inicializa las dependencias necesarias
        /// </summary>
        /// <param name="context">Contexto de la base de datos para operaciones con inmuebles</param>
        /// <param name="usuarioSesion">Servicio para obtener información del usuario actual en sesión</param>
        /// <param name="userManager">Administrador de usuarios de Identity para operaciones de autenticación</param>
        public InmuebleRepository(AppDbContext context, IUsuarioSesion usuarioSesion, UserManager<Usuario> userManager)
        {
            _context = context;
            _usuarioSesion = usuarioSesion;
            _userManager = userManager;
        }


        /// <summary>
        /// Crea un nuevo inmueble en la base de datos asignando automáticamente 
        /// la fecha de creación y el usuario que lo crea
        /// </summary>
        /// <param name="inmueble">Objeto inmueble a crear en la base de datos</param>
        /// <returns>Tarea asíncrona que representa la operación de creación</returns>
        /// <remarks>
        /// Este método establece automáticamente:
        /// - FechaCreacion: Fecha y hora actual
        /// - UsuarioCreaId: ID del usuario actualmente autenticado
        /// </remarks>
        public async Task CreateInmueble(Inmueble inmueble)
        {
            var usuarioBd = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion()!);

            inmueble.FechaCreacion = DateTime.Now;
            inmueble.UsuarioCreaId = Guid.Parse(usuarioBd!.Id);

            _context.Inmuebles.Add(inmueble);
        }

        /// <summary>
        /// Realiza una eliminación lógica de un inmueble marcándolo como inactivo 
        /// en lugar de eliminarlo físicamente de la base de datos
        /// </summary>
        /// <param name="id">Identificador único del inmueble a eliminar</param>
        /// <returns>Tarea asíncrona que representa la operación de eliminación</returns>
        /// <remarks>
        /// Este método realiza una eliminación suave estableciendo:
        /// - FechaEliminacion: Fecha y hora actual de eliminación
        /// - UsuarioEliminaId: ID del usuario que realiza la eliminación
        /// - Activo: false para marcar el inmueble como eliminado
        /// Si el inmueble no existe, la operación se ignora silenciosamente
        /// </remarks>
        public async Task DeleteInmueble(int id)
        {
            var usuarioBd = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion()!);

            var inmueble = await _context.Inmuebles.FindAsync(id);
            if (inmueble == null) return;

            inmueble.FechaEliminacion = DateTime.Now;
            inmueble.UsuarioEliminaId = Guid.Parse(usuarioBd!.Id);
            inmueble.Activo = false;

            _context.Inmuebles.Update(inmueble);
        }

        /// <summary>
        /// Obtiene todos los inmuebles activos de la base de datos
        /// </summary>
        /// <returns>
        /// Colección asíncrona de inmuebles activos. Solo incluye inmuebles 
        /// donde la propiedad Activo es verdadera
        /// </returns>
        /// <remarks>
        /// Este método filtra automáticamente los inmuebles eliminados (Activo = false)
        /// para mostrar únicamente los inmuebles disponibles
        /// </remarks>
        public async Task<IEnumerable<Inmueble>> GetAllInmuebles()
        {
            return await _context.Inmuebles.Where(i => i.Activo).ToListAsync();
        }

        /// <summary>
        /// Busca y obtiene un inmueble específico por su identificador único
        /// </summary>
        /// <param name="id">Identificador único del inmueble a buscar</param>
        /// <returns>
        /// El inmueble encontrado si existe y está activo, o null si no se encuentra 
        /// o si está marcado como eliminado
        /// </returns>
        /// <remarks>
        /// Este método solo retorna inmuebles activos (Activo = true).
        /// Los inmuebles eliminados lógicamente no serán retornados
        /// </remarks>
        public async Task<Inmueble?> GetInmuebleById(int id)
        {
            return await _context.Inmuebles.FirstOrDefaultAsync(i => i.Id == id && i.Activo);
        }

        /// <summary>
        /// Guarda todos los cambios pendientes en el contexto de la base de datos
        /// </summary>
        /// <returns>
        /// true si los cambios se guardaron exitosamente (al menos 0 registros afectados),
        /// false en caso contrario
        /// </returns>
        /// <remarks>
        /// Este método debe ser llamado después de realizar operaciones como Create, Update o Delete
        /// para persistir los cambios en la base de datos. Si no se llama, los cambios se perderán
        /// </remarks>
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        /// <summary>
        /// Actualiza un inmueble existente en la base de datos estableciendo automáticamente 
        /// la fecha de modificación y el usuario que realiza la actualización
        /// </summary>
        /// <param name="inmueble">Objeto inmueble con los datos actualizados</param>
        /// <returns>Tarea asíncrona que representa la operación de actualización</returns>
        /// <remarks>
        /// Este método establece automáticamente:
        /// - FechaModificacion: Fecha y hora actual de la modificación
        /// - UsuarioModificaId: ID del usuario actualmente autenticado que realiza la modificación
        /// El inmueble debe existir previamente en la base de datos
        /// </remarks>
        public async Task UpdateInmueble(Inmueble inmueble)
        {
            var usuarioBd = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion()!);

            inmueble.FechaModificacion = DateTime.Now;
            inmueble.UsuarioModificaId = Guid.Parse(usuarioBd!.Id);

            _context.Inmuebles.Update(inmueble);
        }
    }
}