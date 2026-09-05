using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly AppDbContext _context;

        public ReservaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reserva>> GetAllAsync()
        {
            return await _context.Reservas
                .Include(r => r.Paciente)
                .Include(r => r.Odontologo)
                .ToListAsync();
        }

        public async Task<Reserva?> GetByIdAsync(int pacienteId, string odontologoMatricula, DateTime fechaCreacion)
        {
            return await _context.Reservas
                .Include(r => r.Paciente)
                .Include(r => r.Odontologo)
                .FirstOrDefaultAsync(r =>
                    r.PacienteId == pacienteId &&
                    r.OdontologoMatricula == odontologoMatricula &&
                    r.FechaCreacion == fechaCreacion);
        }

        public async Task<List<Reserva>> GetByPacienteAsync(int pacienteId)
        {
            return await _context.Reservas
                .Include(r => r.Odontologo)
                .Where(r => r.PacienteId == pacienteId)
                .ToListAsync();
        }

        public async Task AddAsync(Reserva reserva)
        {
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reserva reserva)
        {
            var existente = await _context.Reservas.FirstOrDefaultAsync(r =>
                r.PacienteId == reserva.PacienteId &&
                r.OdontologoMatricula == reserva.OdontologoMatricula &&
                r.FechaCreacion == reserva.FechaCreacion);

            if (existente == null)
                throw new InvalidOperationException("Reserva no encontrada.");

            existente.Estado = reserva.Estado;
            existente.SetObs(reserva.Observaciones ?? string.Empty);
            existente.SetImp(reserva.Importe ?? 0);
            existente.SetC(reserva.Coseguro ?? 0);

            if (reserva.FechaRealizacion.HasValue)
                existente.SetFechaRealizacion();

            if (!string.IsNullOrWhiteSpace(reserva.Resultado))
                existente.SetResultado(reserva.Resultado);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int pacienteId, string odontologoMatricula, DateTime fechaCreacion)
        {
            var existente = await _context.Reservas.FirstOrDefaultAsync(r =>
                r.PacienteId == pacienteId &&
                r.OdontologoMatricula == odontologoMatricula &&
                r.FechaCreacion == fechaCreacion);

            if (existente == null)
                throw new InvalidOperationException("Reserva no encontrada.");

            _context.Reservas.Remove(existente);
            await _context.SaveChangesAsync();
        }
    }
}