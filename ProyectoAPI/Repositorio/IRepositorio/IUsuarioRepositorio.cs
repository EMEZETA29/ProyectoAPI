using ProyectoAPI.Modelos;
using ProyectoAPI.Modelos.Dto;

namespace ProyectoAPI.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        bool IsUsuarioUnico(string Email);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<Usuario> Registrar(RegistroRequestDTO registroRequestDTO); 
    }
}
