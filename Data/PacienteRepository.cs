using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;


namespace Data
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly List<Paciente> _pacientes = new();

        public List<Paciente> GetAll() => _pacientes;

        public Paciente? GetByNroPaciente(int nroPaciente) =>
            _pacientes.FirstOrDefault(p => p.NroPaciente == nroPaciente);

        public void Add(Paciente paciente)
        {
            if (GetByNroPaciente(paciente.NroPaciente) != null)
                throw new InvalidOperationException("Ya existe un paciente con ese número.");
            _pacientes.Add(paciente);
        }

        public void Update(Paciente paciente)
        {
            var existente = GetByNroPaciente(paciente.NroPaciente);
            if (existente == null)
                throw new InvalidOperationException("Paciente no encontrado.");
            _pacientes.Remove(existente);
            _pacientes.Add(paciente);
        }

        public void Delete(int nroPaciente)
        {
            var existente = GetByNroPaciente(nroPaciente);
            if (existente == null)
                throw new InvalidOperationException("Paciente no encontrado.");
            _pacientes.Remove(existente);
        }
    }
}