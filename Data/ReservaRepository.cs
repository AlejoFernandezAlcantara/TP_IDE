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
                .Include(r => r.PracticasRealizadas)
                    .ThenInclude(p => p.Practica)
                .ToListAsync();
        }

        public async Task<Reserva?> GetByIdAsync(int id)
        {
            return await _context.Reservas
                .Include(r => r.Paciente)
                .Include(r => r.Odontologo)
                .Include(r => r.PracticasRealizadas)
                    .ThenInclude(p => p.Practica)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Reserva>> GetByPacienteAsync(int pacienteId)
        {
            return await _context.Reservas
                .Include(r => r.Odontologo)
                .Include(r => r.PracticasRealizadas)
                    .ThenInclude(p => p.Practica)
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
            var existente = await _context.Reservas
                .Include(r => r.PracticasRealizadas)
                .FirstOrDefaultAsync(r => r.Id == reserva.Id);

            if (existente == null)
                throw new InvalidOperationException("Reserva no encontrada.");

            existente.Estado = reserva.Estado;
            existente.Observaciones = reserva.Observaciones;
            existente.Importe = reserva.Importe;
            existente.Coseguro = reserva.Coseguro;
            existente.ObraSocial = reserva.ObraSocial;
            existente.NroAfiliado = reserva.NroAfiliado;
            existente.FechaRealizacion = reserva.FechaRealizacion;
            existente.Resultado = reserva.Resultado;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existente = await _context.Reservas.FirstOrDefaultAsync(r => r.Id == id);

            if (existente == null)
                throw new InvalidOperationException("Reserva no encontrada.");

            _context.Reservas.Remove(existente);
            await _context.SaveChangesAsync();
        }
    }
}