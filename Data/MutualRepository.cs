using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class MutualRepository : IMutualRepository
    {
        private readonly AppDbContext _context;

        public MutualRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Mutual>> GetAllAsync()
        {
            return await _context.Mutuales
                .AsNoTracking()
                .OrderBy(m => m.Nombre)
                .ToListAsync();
        }

        public async Task<Mutual?> GetByCuitAsync(int cuit)
        {
            return await _context.Mutuales
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Cuit == cuit);
        }

        // El CUIT es la clave: no se genera, lo carga el usuario. 
        public async Task AddAsync(Mutual mutual)
        {
            if (await _context.Mutuales.AnyAsync(m => m.Cuit == mutual.Cuit))
                throw new InvalidOperationException("Ya existe una mutual con ese CUIT.");

            if (await _context.Mutuales.AnyAsync(m => m.Nombre == mutual.Nombre))
                throw new InvalidOperationException("Ya existe una mutual con ese nombre.");

            _context.Mutuales.Add(mutual);
            await _context.SaveChangesAsync();
        }

        // Solo se puede cambiar el nombre: el CUIT es la clave y no se modifica.
        public async Task UpdateAsync(Mutual mutual)
        {
            var existente = await _context.Mutuales
                .FirstOrDefaultAsync(m => m.Cuit == mutual.Cuit);

            if (existente == null)
                throw new InvalidOperationException("Mutual no encontrada.");

            if (await _context.Mutuales.AnyAsync(m => m.Nombre == mutual.Nombre && m.Cuit != mutual.Cuit))
                throw new InvalidOperationException("Ya existe otra mutual con ese nombre.");

            existente.SetNombre(mutual.Nombre);

            await _context.SaveChangesAsync();
        }

        // No se puede eliminar una mutual que ya figura como obra social en alguna reserva.
        public async Task DeleteAsync(int cuit)
        {
            var existente = await _context.Mutuales
                .FirstOrDefaultAsync(m => m.Cuit == cuit);

            if (existente == null)
                throw new InvalidOperationException("Mutual no encontrada.");

            if (await _context.Reservas.AnyAsync(r => r.ObraSocial == existente.Nombre))
                throw new InvalidOperationException("No se puede eliminar la mutual porque tiene reservas asociadas.");

            _context.Mutuales.Remove(existente);
            await _context.SaveChangesAsync();
        }
    }
}