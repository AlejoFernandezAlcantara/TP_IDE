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

        public async Task<ReservaDTO?> GetByIdAsync(int id)
        {
            var r = await _repository.GetByIdAsync(id);
            return r != null ? ToDto(r) : null;
        }

        public async Task CrearAsync(ReservaDTO dto)
        {
            var reserva = new Reserva(dto.Observaciones ?? string.Empty, dto.Importe ?? 0, dto.Coseguro ?? 0)
            {
                PacienteId = dto._pacienteId,
                OdontologoMatricula = dto._odontologoMatricula,
                ObraSocial = string.IsNullOrWhiteSpace(dto.ObraSocial) ? "Particular" : dto.ObraSocial,
                NroAfiliado = dto.ObraSocial == "Particular" ? null : dto.NroAfiliado
            };

            await _repository.AddAsync(reserva);
        }

        public async Task ActualizarAsync(ReservaDTO dto)
        {
            var actual = await _repository.GetByIdAsync(dto.Id)
                ?? throw new InvalidOperationException("Reserva no encontrada.");
            actual.ValidarModificable();

            var reserva = new Reserva(dto.Observaciones ?? string.Empty, dto.Importe ?? 0, dto.Coseguro ?? 0)
            {
                Id = dto.Id,
                PacienteId = dto._pacienteId,
                OdontologoMatricula = dto._odontologoMatricula,
                FechaCreacion = dto.FechaCreacion,
                Estado = dto.Estado,
                FechaRealizacion = dto.FechaRealizacion,
                Resultado = dto.Resultado,
                ObraSocial = dto.ObraSocial,
                NroAfiliado = dto.NroAfiliado
            };

            await _repository.UpdateAsync(reserva);
        }

        public async Task EliminarAsync(int id)
        {
            var actual = await _repository.GetByIdAsync(id)
                ?? throw new InvalidOperationException("Reserva no encontrada.");
            actual.ValidarModificable();

            await _repository.DeleteAsync(id);
        }

        public async Task AtenderReservaAsync(AtenderReservaDTO dto)
        {
            var reserva = await _repository.GetByIdAsync(dto.ReservaId);
            if (reserva == null)
                throw new InvalidOperationException("Reserva no encontrada.");

            var listaPracticas = dto.Practicas.Select(p => new ReservaPractica
            {
                ReservaId = dto.ReservaId,
                PracticaCodigo = p.PracticaCodigo,
                DientesCaras = p.DientesCaras,
                Precio = p.Precio,
                Observaciones = p.Observaciones
            }).ToList();

            reserva.CompletarAtencion(dto.Resultado, listaPracticas);
            if (dto.ImporteTotal.HasValue && dto.ImporteTotal.Value > 0)
            {
                reserva.Importe = dto.ImporteTotal.Value;
            }

            await _repository.UpdateAsync(reserva);
        }

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
            ObraSocial = r.ObraSocial,
            NroAfiliado = r.NroAfiliado,
            _pacienteId = r.PacienteId,
            _odontologoMatricula = r.OdontologoMatricula,
            Turnos = r.Turnos?
                .OrderBy(t => t.FechaHoraInicio)
                .Select(t => new TurnoReservaDTO { Codigo = t.Codigo, FechaHoraInicio = t.FechaHoraInicio, Duracion = t.Duracion })
                .ToList() ?? new(),
            PracticasRealizadas = r.PracticasRealizadas?.Select(p => new ReservaPracticaDTO
            {
                Id = p.Id,
                ReservaId = p.ReservaId,
                PracticaCodigo = p.PracticaCodigo,
                PracticaDetalle = p.Practica?.Detalle,
                DientesCaras = p.DientesCaras,
                Precio = p.Precio,
                Observaciones = p.Observaciones
            }).ToList() ?? new()
        };
    }
}