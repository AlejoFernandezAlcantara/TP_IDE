using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Model;

namespace Data
{
    public interface IReservaRepository
    {
        Task<List<Reserva>> GetAllAsync();
        Task<Reserva?> GetByIdAsync(int id);
        Task<List<Reserva>> GetByPacienteAsync(int pacienteId);
        Task AddAsync(Reserva reserva);
        Task AddConTurnosAsync(Reserva reserva, IReadOnlyCollection<int> codigosTurno);
        Task UpdateAsync(Reserva reserva);
        Task DeleteAsync(int id);
    }
}