using Microsoft.EntityFrameworkCore;
using ProyectoAPI.Modelos;


namespace ProyectoAPI.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Vehiculo> Vehiculo { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehiculo>().HasData(
                new Vehiculo()
                {
                    Id = 1,
                    Patente = "RPRP42",
                    MarcaId = 1,
                    ModeloId = 1,
                    CarroceriaId = 1,
                    Color = "Celeste",
                    //FechaActualizacion=DateTime.Now,
                    //FechaCreacion=DateTime.Now
                },

                new Vehiculo()
                {
                    Id = 2,
                    Patente = "RPRP50",
                    MarcaId = 1,
                    ModeloId = 1,
                    CarroceriaId = 1,
                    Color = "Celeste",
                    //FechaActualizacion = DateTime.Now,
                    //FechaCreacion = DateTime.Now
                }

            );
        }


    }
}
