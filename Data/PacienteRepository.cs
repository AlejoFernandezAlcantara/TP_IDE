using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly AppDbContext _context;

        public PacienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Paciente>> GetAllAsync()
        {
            return await _context.Pacientes.ToListAsync();
        }

        public async Task<Paciente?> GetByNroPacienteAsync(int nroPaciente)
        {
            return await _context.Pacientes
                .FirstOrDefaultAsync(p => p.NroPaciente == nroPaciente);
        }

        public async Task AddAsync(Paciente paciente)
        {
            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Paciente paciente)
        {
            var existente = await _context.Pacientes
                .FirstOrDefaultAsync(p => p.NroPaciente == paciente.NroPaciente);

            if (existente == null)
                throw new InvalidOperationException("Paciente no encontrado.");

            existente.SetNombre(paciente.Nombre);
            existente.SetApellido(paciente.Apellido);
            existente.SetDireccion(paciente.Direccion);
            existente.SetTelefono(paciente.Telefono);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int nroPaciente)
        {
            var existente = await _context.Pacientes
                .FirstOrDefaultAsync(p => p.NroPaciente == nroPaciente);

            if (existente == null)
                throw new InvalidOperationException("Paciente no encontrado.");

            _context.Pacientes.Remove(existente);
            await _context.SaveChangesAsync();
        }
    }
}