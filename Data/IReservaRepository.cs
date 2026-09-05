using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;

namespace Data
{
    public interface IReservaRepository
    {
        Task<List<Reserva>> GetAllAsync();
        Task<Reserva?> GetByIdAsync(int pacienteId, string odontologoMatricula, DateTime fechaCreacion);
        Task<List<Reserva>> GetByPacienteAsync(int pacienteId);
        Task AddAsync(Reserva reserva);
        Task UpdateAsync(Reserva reserva);
        Task DeleteAsync(int pacienteId, string odontologoMatricula, DateTime fechaCreacion);
    }
}
