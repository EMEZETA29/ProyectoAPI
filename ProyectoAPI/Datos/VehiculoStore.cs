using ProyectoAPI.Modelos.Dto;

namespace ProyectoAPI.Datos
{
    public static class VehiculoStore
    {
        public static List<VehiculoDto> vehiculoList = new List<VehiculoDto>
        {
            new VehiculoDto{Id=1, Patente="LZLZ14", ModeloId= 10, MarcaId=11, Color="Color", CarroceriaId=40},
            new VehiculoDto{Id=2, Patente="MXMX29", ModeloId= 10, MarcaId=11, Color="Color", CarroceriaId=40}
        };
    }
}
