using System.Collections.Generic;
using System.Threading.Tasks;
using DTO;

namespace Applications.Services
{
    public interface ITurnoService
    {
        Task<List<TurnoDTO>> GetAllAsync();
        Task<TurnoDTO?> GetByCodigoAsync(int codigo);
        Task<List<TurnoDTO>> GetByOdontologoAsync(string matricula);
        Task CrearAsync(TurnoDTO dto);
        Task ActualizarAsync(TurnoDTO dto);
        Task EliminarAsync(int codigo);
    }
}
