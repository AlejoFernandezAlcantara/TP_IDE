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
    }
}
