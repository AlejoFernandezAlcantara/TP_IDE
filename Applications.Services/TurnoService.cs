using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;
using Data;
using DTO;

namespace Applications.Services
{
    public class TurnoService : ITurnoService
    {
        private readonly ITurnoRepository _repository;

        public TurnoService(ITurnoRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TurnoDTO>> GetAllAsync() =>
            (await _repository.GetAllAsync()).Select(ToDto).ToList();

        public async Task<TurnoDTO?> GetByCodigoAsync(int codigo)
        {
            var domain = await _repository.GetByCodigoAsync(codigo);
            return domain is null ? null : ToDto(domain);
        }

        public async Task<List<TurnoDTO>> GetByOdontologoAsync(string matricula) =>
            (await _repository.GetByOdontologoAsync(matricula)).Select(ToDto).ToList();

        public async Task CrearAsync(TurnoDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto._odontologoMatricula))
                throw new InvalidOperationException("Indicá el odontólogo del turno.");

            var duracion = dto.Duracion > 0 ? dto.Duracion : 30;

            if (duracion > 240)
                throw new InvalidOperationException("La duración del turno no puede superar las 4 horas.");

            if (dto.FechaHoraInicio < DateTime.Now)
                throw new InvalidOperationException("No se puede cargar un turno en el pasado.");

            
            var fin = dto.FechaHoraInicio.AddMinutes(duracion); // No se puede superponer con otro turno (no cancelado) del mismo odontólogo(((()))))
            var existentes = await _repository.GetByOdontologoAsync(dto._odontologoMatricula);
            var solapado = existentes.Any(t =>
                t.Estado != EstadoTurno.Cancelado &&
                t.FechaHoraInicio < fin &&
                dto.FechaHoraInicio < t.FechaHoraInicio.AddMinutes(t.Duracion));

            if (solapado)
                throw new InvalidOperationException(
                    $"Ya existe un turno que se superpone con {dto.FechaHoraInicio:dd/MM/yyyy HH:mm}.");

            var turno = new Turno(dto.FechaHoraInicio)
            {
                OdontologoMatricula = dto._odontologoMatricula,
                Duracion = duracion,
                Estado = EstadoTurno.Disponible
            };

            await _repository.AddAsync(turno);
        }

        public async Task ActualizarAsync(TurnoDTO dto)
        {
            var turno = new Turno(dto.FechaHoraInicio)
            {
                Codigo = dto.Codigo,
                Duracion = dto.Duracion,
                Estado = dto.Estado,
                OdontologoMatricula = dto._odontologoMatricula,
                ReservaPacienteId = dto._reservaPacienteId,
                ReservaOdontologoMatricula = dto._reservaOdontologoMatricula,
                ReservaFechaCreacion = dto._reservaFechaCreacion,
            };

            await _repository.UpdateAsync(turno);
        }

        public async Task EliminarAsync(int codigo) => await _repository.DeleteAsync(codigo);

        private static TurnoDTO ToDto(Turno t) => new TurnoDTO
        {
            Codigo = t.Codigo,
            FechaHoraInicio = t.FechaHoraInicio,
            Duracion = t.Duracion,
            Estado = t.Estado,
            ReservaId = t.ReservaId,
            _odontologoMatricula = t.OdontologoMatricula,
            _reservaPacienteId = t.ReservaPacienteId,
            _reservaOdontologoMatricula = t.ReservaOdontologoMatricula,
            _reservaFechaCreacion = t.ReservaFechaCreacion,
        };
        public async Task<ReservaDTO> ReservarAsync(int codigo, int pacienteId)
        {
            var r = await _repository.ReservarAsync(codigo, pacienteId);
            return new ReservaDTO
            {
                Id = r.Id,
                FechaCreacion = r.FechaCreacion,
                Estado = r.Estado,
                Observaciones = r.Observaciones,
                Importe = r.Importe,
                Coseguro = r.Coseguro,
                _pacienteId = r.PacienteId,
                _odontologoMatricula = r.OdontologoMatricula
            };
        }

        public async Task CancelarReservaAsync(int codigo) => await _repository.CancelarReservaAsync(codigo);
    }
}