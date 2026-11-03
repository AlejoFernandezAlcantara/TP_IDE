using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class PracticaRepository : IPracticaRepository
    {
        private readonly AppDbContext _context;

        public PracticaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Practica practica)
        {
            _context.Practicas.Add(practica);
            await _context.SaveChangesAsync();
        }
        
        public async Task<List<Practica>> GetAllAsync() =>
            await _context.Practicas.OrderBy(p => p.CodigoPractica).ToListAsync();
        public async Task<Practica?> GetByCodigoAsync(int codigo) =>
            await _context.Practicas.FirstOrDefaultAsync(p => p.CodigoPractica == codigo);
        public async Task UpdateAsync(Practica practica)
        {
            var existente = await GetByCodigoAsync(practica.CodigoPractica)
                ?? throw new InvalidOperationException("Práctica no encontrada.");

            existente.Actualizar(practica.Detalle, practica.Precio);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int codigo)
        {
            var existente = await GetByCodigoAsync(codigo)
                ?? throw new InvalidOperationException("Práctica no encontrada.");

            if (await _context.ReservaPracticas.AnyAsync(d => d.PracticaCodigo == codigo))
                throw new InvalidOperationException(
                    "No se puede eliminar: la práctica está usada en odontogramas.");

            _context.Practicas.Remove(existente);
            await _context.SaveChangesAsync();
        }
    }
}
