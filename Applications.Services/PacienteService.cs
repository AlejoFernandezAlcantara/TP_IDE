using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Data;

namespace Applications.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _repository;

        public PacienteService(IPacienteRepository repository)
        {
            _repository = repository;
        }

        public List<Paciente> GetAll() => _repository.GetAll();

        public Paciente? GetByNroPaciente(int nroPaciente) => _repository.GetByNroPaciente(nroPaciente);

        public void Crear(Paciente paciente) => _repository.Add(paciente);

        public void Actualizar(Paciente paciente) => _repository.Update(paciente);

        public void Eliminar(int nroPaciente) => _repository.Delete(nroPaciente);
    }
}