using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class TurnoRepository : ITurnoRepository
    {
        private readonly AppDbContext _context;

        public TurnoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Turno>> GetAllAsync()
        {
            return await _context.Turnos
                .Include(t => t.Odontologo)
                .Include(t => t.Reserva)
                .ToListAsync();
        }

        public async Task<Turno?> GetByCodigoAsync(int codigo)
        {
            return await _context.Turnos
                .Include(t => t.Odontologo)
                .Include(t => t.Reserva)
                .FirstOrDefaultAsync(t => t.Codigo == codigo);
        }

        public async Task<List<Turno>> GetByOdontologoAsync(string matricula)
        {
            return await _context.Turnos
                .Include(t => t.Reserva)
                .Where(t => t.OdontologoMatricula == matricula)
                .ToListAsync();
        }

        public async Task AddAsync(Turno turno)
        {
            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Turno turno)
        {
            var existente = await _context.Turnos.FirstOrDefaultAsync(t => t.Codigo == turno.Codigo);

            if (existente == null)
                throw new InvalidOperationException("Turno no encontrado.");

            existente.FechaHoraInicio = turno.FechaHoraInicio;
            existente.Duracion = turno.Duracion;
            existente.Estado = turno.Estado;
            existente.OdontologoMatricula = turno.OdontologoMatricula;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int codigo)
        {
            var existente = await _context.Turnos.FirstOrDefaultAsync(t => t.Codigo == codigo);

            if (existente == null)
                throw new InvalidOperationException("Turno no encontrado.");

            if (existente.Estado == EstadoTurno.Reservado)
                throw new InvalidOperationException("El turno tiene una reserva activa. Cancelá la reserva antes de eliminarlo.");

            _context.Turnos.Remove(existente);
            await _context.SaveChangesAsync();
        }

        public async Task<Reserva> ReservarAsync(int codigoTurno, int pacienteId)
        {
            var turno = await _context.Turnos.FirstOrDefaultAsync(t => t.Codigo == codigoTurno)
                ?? throw new InvalidOperationException("Turno no encontrado.");

            if (turno.Estado != EstadoTurno.Disponible)
                throw new InvalidOperationException("El turno no está disponible.");

            if (turno.FechaHoraInicio < DateTime.Now)
                throw new InvalidOperationException("No se puede reservar un turno que ya pasó.");

            if (!await _context.Pacientes.AnyAsync(p => p.NroPaciente == pacienteId))
                throw new InvalidOperationException("Paciente no encontrado.");

            var reserva = new Reserva(string.Empty, 0, 0)
            {
                PacienteId = pacienteId,
                OdontologoMatricula = turno.OdontologoMatricula
            };
            _context.Reservas.Add(reserva);

            // Vincula el turno con la reserva
            turno.Reserva = reserva;
            turno.Estado = EstadoTurno.Reservado;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new InvalidOperationException("El turno ya fue reservado por otra persona.");
            }

            return reserva;
        }

        // Una sola reserva que agrupa uno o varios turnos del mismo odontólogo
        public async Task<Reserva> ReservarVariosAsync(IReadOnlyCollection<int> codigosTurno, int pacienteId, string? obraSocial, string? nroAfiliado)
        {
            var codigos = (codigosTurno ?? Array.Empty<int>()).Distinct().ToList();
            if (codigos.Count == 0)
                throw new InvalidOperationException("Seleccioná al menos un turno.");

            var turnos = await _context.Turnos.Where(t => codigos.Contains(t.Codigo)).ToListAsync();

            if (turnos.Count != codigos.Count)
                throw new InvalidOperationException("Alguno de los turnos seleccionados no existe.");

            if (turnos.Any(t => t.Estado != EstadoTurno.Disponible))
                throw new InvalidOperationException("Alguno de los turnos seleccionados ya no está disponible.");

            if (turnos.Any(t => t.FechaHoraInicio < DateTime.Now))
                throw new InvalidOperationException("No se puede reservar un turno que ya pasó.");

            if (turnos.Select(t => t.OdontologoMatricula).Distinct().Count() > 1)
                throw new InvalidOperationException("Los turnos de una misma reserva deben ser del mismo odontólogo.");

            if (!await _context.Pacientes.AnyAsync(p => p.NroPaciente == pacienteId))
                throw new InvalidOperationException("Paciente no encontrado.");

            var cobertura = string.IsNullOrWhiteSpace(obraSocial) ? "Particular" : obraSocial.Trim();
            if (cobertura != "Particular" && string.IsNullOrWhiteSpace(nroAfiliado))
                throw new InvalidOperationException($"El número de afiliado es obligatorio para {cobertura}.");

            var reserva = new Reserva(string.Empty, 0, 0)
            {
                PacienteId = pacienteId,
                OdontologoMatricula = turnos[0].OdontologoMatricula,
                ObraSocial = cobertura,
                NroAfiliado = cobertura == "Particular" ? null : nroAfiliado!.Trim()
            };
            _context.Reservas.Add(reserva);

            foreach (var turno in turnos)
            {
                turno.Reserva = reserva;
                turno.Estado = EstadoTurno.Reservado;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new InvalidOperationException("Alguno de los turnos ya fue reservado por otra persona.");
            }

            return reserva;
        }

        public async Task CancelarReservaAsync(int codigoTurno)
        {
            var turno = await _context.Turnos
                .Include(t => t.Reserva)
                .FirstOrDefaultAsync(t => t.Codigo == codigoTurno)
                ?? throw new InvalidOperationException("Turno no encontrado.");

            if (turno.Estado != EstadoTurno.Reservado)
                throw new InvalidOperationException("El turno no tiene una reserva activa.");

            if (turno.Reserva != null)
            {
                turno.Reserva.ValidarModificable();
                turno.Reserva.Estado = EstadoReserva.Cancelada;

                // La reserva puede abarcar varios turnos: se liberan todos
                var delaReserva = await _context.Turnos
                    .Where(t => t.ReservaId == turno.ReservaId && t.Codigo != turno.Codigo)
                    .ToListAsync();
                foreach (var otro in delaReserva)
                {
                    otro.Estado = EstadoTurno.Disponible;
                    otro.ReservaId = null;
                }
            }

            turno.Estado = EstadoTurno.Disponible;
            turno.Reserva = null;
            turno.ReservaId = null;

            await _context.SaveChangesAsync();
        }
    }
}
