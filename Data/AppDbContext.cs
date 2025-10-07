using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using preparacion_pt_bdv.models;

namespace preparacion_pt_bdv.Data
{
    // DbContext que hereda de IdentityDbContext para manejar la autenticación y autorización dentro de la base de datos
    public class AppDbContext : IdentityDbContext<Usuario>
    {
        
        public AppDbContext(DbContextOptions<AppDbContext> opt)
            : base(opt)
        {
        }

        // Método que se ejecuta al crear el modelo de la base de datos y permite configurar las entidades y sus relaciones
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        // Propiedad que representa la tabla de Inmuebles en la base de datos
        public DbSet<Inmueble> Inmuebles { get; set; }
    }
}