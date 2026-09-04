using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class OdontologoRepository : IOdontologoRepository
    {
        private readonly AppDbContext _context;

        public OdontologoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Odontologo>> GetAllAsync()
        {
            return await _context.Odontologos.ToListAsync();
        }

        public async Task<Odontologo?> GetByMatriculaAsync(string matricula)
        {
            return await _context.Odontologos
                .FirstOrDefaultAsync(o => o.Matricula == matricula);
        }

        public async Task AddAsync(Odontologo odontologo)
        {
            _context.Odontologos.Add(odontologo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Odontologo odontologo)
        {
            var existente = await _context.Odontologos
                .FirstOrDefaultAsync(o => o.Matricula == odontologo.Matricula);

            if (existente == null)
                throw new InvalidOperationException("Odontologo no encontrado.");

            existente.SetNombre(odontologo.Nombre);
            existente.SetApellido(odontologo.Apellido);
            existente.SetNroDoc(odontologo.NroDocumento);
            existente.SetTipoDoc(odontologo.TipoDocumento);
            existente.SetEspecialidad(odontologo.Especialidad);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string matricula)
        {
            var existente = await _context.Odontologos
                .FirstOrDefaultAsync(o => o.Matricula == matricula);

            if (existente == null)
                throw new InvalidOperationException("Odontologo no encontrado.");

            _context.Odontologos.Remove(existente);
            await _context.SaveChangesAsync();
        }
    }
}