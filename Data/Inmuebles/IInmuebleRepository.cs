using preparacion_pt_bdv.models;

namespace preparacion_pt_bdv.Data.Inmuebles
{
    // Define el contrato para el repositorio de inmuebles.
    public interface IInmuebleRepository
    {
        // Guarda los cambios pendientes en la unidad de trabajo.
        bool SaveChanges();

        // Obtiene todos los inmuebles.
        Task<IEnumerable<Inmueble>> GetAllInmuebles();

        // Obtiene un inmueble por su ID.
        Task<Inmueble?> GetInmuebleById(int id);

        // Agrega un nuevo inmueble.
        Task CreateInmueble(Inmueble inmueble);

        // Actualiza un inmueble existente.
        Task UpdateInmueble(Inmueble inmueble);

        // Elimina un inmueble por su ID.
        Task DeleteInmueble(int id);
    }
}