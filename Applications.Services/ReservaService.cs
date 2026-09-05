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
                PacienteId = dto._pacienteId,
                OdontologoMatricula = dto._odontologoMatricula,
                FechaCreacion = dto.FechaCreacion,
                Estado = dto.Estado
            };

            await _repository.UpdateAsync(reserva);
        }

        public async Task EliminarAsync(int pacienteId, string odontologoMatricula, DateTime fechaCreacion) =>
            await _repository.DeleteAsync(pacienteId, odontologoMatricula, fechaCreacion);

        private static ReservaDTO ToDto(Reserva r) => new ReservaDTO
        {
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
