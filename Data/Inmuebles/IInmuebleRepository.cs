using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using preparacion_pt_bdv.models;

namespace preparacion_pt_bdv.Data.Inmuebles
{
    public interface IInmuebleRepository
    {
        bool SaveChanges();

        Task<IEnumerable<Inmueble>> GetAllInmuebles();

        Task<Inmueble?> GetInmuebleById(int id);

        Task CreateInmueble(Inmueble inmueble);

        Task UpdateInmueble(Inmueble inmueble);

        Task DeleteInmueble(int id);

    }
}