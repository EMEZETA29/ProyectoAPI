using AutoMapper;
using ProyectoAPI.Modelos;
using ProyectoAPI.Modelos.Dto;

namespace ProyectoAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Vehiculo, VehiculoDto>().ReverseMap();
            CreateMap<Vehiculo, VehiculoCrearDto>().ReverseMap();
            CreateMap<Vehiculo, VehiculoUpdateDto>().ReverseMap();
        }
    }
}
