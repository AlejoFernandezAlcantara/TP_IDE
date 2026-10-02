using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;
using Data;
using DTO;

namespace Applications.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _repository;

        public ReservaService(IReservaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ReservaDTO>> GetAllAsync() =>
            (await _repository.GetAllAsync()).Select(ToDto).ToList();

        public async Task<List<ReservaDTO>> GetByPacienteAsync(int pacienteId) =>
            (await _repository.GetByPacienteAsync(pacienteId)).Select(ToDto).ToList();

        public async Task CrearAsync(ReservaDTO dto)
        {
            var reserva = new Reserva(dto.Observaciones ?? string.Empty, dto.Importe ?? 0, dto.Coseguro ?? 0)
            {
                PacienteId = dto._pacienteId,
                OdontologoMatricula = dto._odontologoMatricula
            };

            await _repository.AddAsync(reserva);
        }

        public async Task ActualizarAsync(ReservaDTO dto)
        {
            var reserva = new Reserva(dto.Observaciones ?? string.Empty, dto.Importe ?? 0, dto.Coseguro ?? 0)
            {
                Id = dto.Id,
                PacienteId = dto._pacienteId,
                OdontologoMatricula = dto._odontologoMatricula,
                FechaCreacion = dto.FechaCreacion,
                Estado = dto.Estado,
                FechaRealizacion = dto.FechaRealizacion,
                Resultado = dto.Resultado
            };

            await _repository.UpdateAsync(reserva);
        }

        public async Task EliminarAsync(int id) => await _repository.DeleteAsync(id);

        private static ReservaDTO ToDto(Reserva r) => new ReservaDTO
        {
            Id = r.Id,
            FechaCreacion = r.FechaCreacion,
            Estado = r.Estado,
            Observaciones = r.Observaciones,
            Importe = r.Importe,
            Coseguro = r.Coseguro,
            FechaRealizacion = r.FechaRealizacion,
            Resultado = r.Resultado,
            _pacienteId = r.PacienteId,
            _odontologoMatricula = r.OdontologoMatricula
        };
    }
}
