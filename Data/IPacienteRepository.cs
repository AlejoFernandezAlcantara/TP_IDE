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
        List<Paciente> GetAll();
        Paciente? GetByNroPaciente(int nroPaciente);
        void Add(Paciente paciente);
        void Update(Paciente paciente);
        void Delete(int nroPaciente);
    }
}