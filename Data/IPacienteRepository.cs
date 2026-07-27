using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;

namespace Data
{
    public interface IPacienteRepository
    {
        Task<List<Paciente>> GetAllAsync();
        Task<Paciente?> GetByNroPacienteAsync(int nroPaciente);
        Task AddAsync(Paciente paciente);
        Task UpdateAsync(Paciente paciente);
        Task DeleteAsync(int nroPaciente);
    }
}