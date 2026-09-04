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

        public async Task<List<PacienteDTO>> GetAllAsync() => (await _repository.GetAllAsync()).Select(ToDto).ToList();

        public async Task<PacienteDTO?> GetByNroPacienteAsync(int nroPaciente)
        {
            var domain = await _repository.GetByNroPacienteAsync(nroPaciente);
            return domain is null ? null : ToDto(domain);
        }

        public async Task CrearAsync(PacienteDTO paciente)
        {
            var domain = ToDomain(paciente);
            await _repository.AddAsync(domain);
        }

        public async Task ActualizarAsync(PacienteDTO paciente)
        {
            var domain = ToDomain(paciente);
            await _repository.UpdateAsync(domain);
        }

        public async Task EliminarAsync(int nroPaciente) => await _repository.DeleteAsync(nroPaciente);

        private static PacienteDTO ToDto(Paciente p)
      => new PacienteDTO
      {
          NroPaciente = p.NroPaciente,
          Nombre = p.Nombre,
          Apellido = p.Apellido,
          NroDni = p.NroDni,
          TipoDni = p.TipoDni,
          Direccion = p.Direccion,
          Telefono = p.Telefono,
          Email = p.Email,
          Password = string.Empty
      };

        private static Paciente ToDomain(PacienteDTO dto)
    => new Paciente(
        dto.Nombre,
        dto.Apellido,
        dto.Direccion,
        dto.Telefono,
        dto.Email,
        dto.NroDni,
        dto.TipoDni,
        dto.Password ?? string.Empty );
    }
}