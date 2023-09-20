using ProyectoAPI.Modelos;

namespace ProyectoAPI.Repositorio.IRepositorio
{
    public interface IVehiculoRepositorio :IRepositorio<Vehiculo>
    {
        Task<Vehiculo> Actualizar(Vehiculo entidad);
    }
}
