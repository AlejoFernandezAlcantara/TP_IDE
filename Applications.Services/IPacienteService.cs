using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Data;
using DTO;

//hacer asincrono
namespace Applications.Services
{
    public interface IPacienteService
    {
        Task<List<PacienteDTO>> GetAllAsync();
        Task<PacienteDTO?> GetByNroPacienteAsync(int nroPaciente);
        Task CrearAsync(PacienteDTO paciente);
        Task ActualizarAsync(PacienteDTO paciente);
        Task EliminarAsync(int nroPaciente);
    }
}