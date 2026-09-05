using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO;

namespace Applications.Services
{
    public interface IReservaService
    {
        Task<List<ReservaDTO>> GetAllAsync();
        Task<List<ReservaDTO>> GetByPacienteAsync(int pacienteId);
        Task CrearAsync(ReservaDTO dto);
        Task ActualizarAsync(ReservaDTO dto);
        Task EliminarAsync(int pacienteId, string odontologoMatricula, DateTime fechaCreacion);
    }
}