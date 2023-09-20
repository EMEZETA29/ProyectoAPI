using ProyectoAPI.Datos;
using ProyectoAPI.Modelos;
using ProyectoAPI.Repositorio.IRepositorio;

namespace ProyectoAPI.Repositorio
{
    public class VehiculoRepositorio : Repositorio<Vehiculo>, IVehiculoRepositorio
    {
        private readonly ApplicationDbContext _db;

        public VehiculoRepositorio(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }
        public async Task<Vehiculo> Actualizar(Vehiculo entidad)
        {
            //entidad.FechaActualizacion = DateTime.Now;
            _db.Vehiculo.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;

        }
    }
}
