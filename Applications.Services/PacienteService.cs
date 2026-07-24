using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Data;
using DTO;

namespace Applications.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _repository;

        public PacienteService(IPacienteRepository repository)
        {
            _repository = repository;
        }

        public List<PacienteDTO> GetAll() => _repository.GetAll().Select(ToDto).ToList();

        public PacienteDTO? GetByNroPaciente(int nroPaciente)
        {
            var domain = _repository.GetByNroPaciente(nroPaciente);
            return domain is null ? null : ToDto(domain);
        }

        public void Crear(PacienteDTO paciente)
        {
            var domain = ToDomain(paciente);
            _repository.Add(domain);
        }

        public void Actualizar(PacienteDTO paciente)
        {
            var domain = ToDomain(paciente);
            _repository.Update(domain);
        }

        public void Eliminar(int nroPaciente) => _repository.Delete(nroPaciente);

        private static PacienteDTO ToDto(Paciente p) //mapeo entre DTO y modelo de dominiooo
            => new PacienteDTO
            {
                NroPaciente = p.NroPaciente,
                Nombre = p.Nombre,
                Apellido = p.Apellido,
                Direccion = p.Direccion,
                Telefono = p.Telefono,
                Email = p.Email,
                Password = string.Empty 
            };

        private static Paciente ToDomain(PacienteDTO dto)
            => new Paciente(
                dto.NroPaciente,
                dto.Nombre,
                dto.Apellido,
                dto.Direccion,
                dto.Telefono,
                dto.Email,
                dto.Password ?? string.Empty 
            );
    }
}