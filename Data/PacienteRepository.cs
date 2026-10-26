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

            // Las reservas bloquean el borrado (FK Restrict) para conservar el historial clínico
            bool tieneReservas = await _context.Reservas.AnyAsync(r => r.PacienteId == nroPaciente);
            if (tieneReservas)
                throw new InvalidOperationException(
                    "No se puede eliminar el paciente porque tiene reservas registradas. " +
                    "Las reservas atendidas no se pueden borrar para conservar el historial.");

            _context.Pacientes.Remove(existente);
            await _context.SaveChangesAsync();
        }
    }
}