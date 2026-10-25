using System.Collections.Generic;
using System.Threading.Tasks;
using DTO;

namespace Applications.Services
{
    public interface IReservaService
    {
        Task<List<ReservaDTO>> GetAllAsync();
        Task<List<ReservaDTO>> GetByPacienteAsync(int pacienteId);
        Task<ReservaDTO?> GetByIdAsync(int id);
        Task CrearAsync(ReservaDTO dto);
        Task ActualizarAsync(ReservaDTO dto);
        Task EliminarAsync(int id);
        Task AtenderReservaAsync(AtenderReservaDTO dto);
    }
}