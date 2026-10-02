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
                .ToListAsync();
        }

        public async Task<Turno?> GetByCodigoAsync(int codigo)
        {
            return await _context.Turnos
                .Include(t => t.Odontologo)
                .FirstOrDefaultAsync(t => t.Codigo == codigo);
        }

        public async Task<List<Turno>> GetByOdontologoAsync(string matricula)
        {
            return await _context.Turnos
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
            existente.ReservaPacienteId = turno.ReservaPacienteId;
            existente.ReservaOdontologoMatricula = turno.ReservaOdontologoMatricula;
            existente.ReservaFechaCreacion = turno.ReservaFechaCreacion;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int codigo)
        {
            var existente = await _context.Turnos.FirstOrDefaultAsync(t => t.Codigo == codigo);

            if (existente == null)
                throw new InvalidOperationException("Turno no encontrado.");

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

            turno.Estado = EstadoTurno.Reservado;
            turno.ReservaPacienteId = reserva.PacienteId;
            turno.ReservaOdontologoMatricula = reserva.OdontologoMatricula;
            turno.ReservaFechaCreacion = reserva.FechaCreacion;

            await _context.SaveChangesAsync();
            return reserva;
        }

        public async Task CancelarReservaAsync(int codigoTurno)
        {
            var turno = await _context.Turnos.FirstOrDefaultAsync(t => t.Codigo == codigoTurno)
                ?? throw new InvalidOperationException("Turno no encontrado.");

            if (turno.Estado != EstadoTurno.Reservado)
                throw new InvalidOperationException("El turno no tiene una reserva activa.");

            var reserva = await _context.Reservas.FirstOrDefaultAsync(r =>
                r.PacienteId == turno.ReservaPacienteId &&
                r.OdontologoMatricula == turno.ReservaOdontologoMatricula &&
                r.FechaCreacion == turno.ReservaFechaCreacion);

            if (reserva != null)
                reserva.Estado = EstadoReserva.Cancelada;

            turno.Estado = EstadoTurno.Disponible;
            turno.ReservaPacienteId = 0;
            turno.ReservaOdontologoMatricula = string.Empty;
            turno.ReservaFechaCreacion = default;

            await _context.SaveChangesAsync();
        }
    }
}
