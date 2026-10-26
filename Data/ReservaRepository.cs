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
            // Solo lo que usa el DTO (turnos y prácticas). Paciente y Odontólogo se
            // resuelven en la pantalla, así la consulta trae menos datos.
            return await _context.Reservas
                .AsNoTracking()
                .AsSplitQuery()
                .Include(r => r.Turnos)
                .Include(r => r.PracticasRealizadas)
                    .ThenInclude(p => p.Practica)
                .ToListAsync();
        }
        public async Task<Reserva?> GetByIdAsync(int id)
        {
            return await _context.Reservas
                .Include(r => r.Paciente)
                .Include(r => r.Odontologo)
                .Include(r => r.Turnos)
                .Include(r => r.PracticasRealizadas)
                    .ThenInclude(p => p.Practica)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<List<Reserva>> GetByPacienteAsync(int pacienteId)
        {
            return await _context.Reservas
                .AsNoTracking()
                .AsSplitQuery()
                .Include(r => r.Odontologo)
                .Include(r => r.Turnos)
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

        // Crea la reserva y bloquea sus turnos.
        public async Task AddConTurnosAsync(Reserva reserva, IReadOnlyCollection<int> codigosTurno)
        {
            var codigos = (codigosTurno ?? Array.Empty<int>()).Distinct().ToList();
            if (codigos.Count == 0)
                throw new InvalidOperationException("La reserva debe incluir al menos un turno.");

            if (!await _context.Pacientes.AnyAsync(p => p.NroPaciente == reserva.PacienteId))
                throw new InvalidOperationException("Paciente no encontrado.");

            var turnos = await _context.Turnos
                .Where(t => codigos.Contains(t.Codigo))
                .ToListAsync();

            if (turnos.Count != codigos.Count)
                throw new InvalidOperationException("Alguno de los turnos seleccionados no existe.");

            if (turnos.Any(t => t.Estado != EstadoTurno.Disponible))
                throw new InvalidOperationException("Alguno de los turnos seleccionados ya no está disponible.");

            if (turnos.Any(t => t.FechaHoraInicio < DateTime.Now))
                throw new InvalidOperationException("No se puede reservar un turno que ya pasó.");

            if (turnos.Any(t => t.OdontologoMatricula != reserva.OdontologoMatricula))
                throw new InvalidOperationException("Todos los turnos deben ser del odontólogo indicado en la reserva.");

            // Chequear turnos son uno atras de otro y sin huecos en el medio
            var ordenados = turnos.OrderBy(t => t.FechaHoraInicio).ToList();
            for (int i = 0; i < ordenados.Count - 1; i++)
            {
                var finActual = ordenados[i].FechaHoraInicio.AddMinutes(ordenados[i].Duracion);
                if (finActual != ordenados[i + 1].FechaHoraInicio)
                    throw new InvalidOperationException("Los turnos de una reserva deben estar seguidos, sin huecos ni superposición.");
            }

            var cobertura = string.IsNullOrWhiteSpace(reserva.ObraSocial) ? "Particular" : reserva.ObraSocial.Trim();
            if (cobertura != "Particular" && string.IsNullOrWhiteSpace(reserva.NroAfiliado))
                throw new InvalidOperationException($"El número de afiliado es obligatorio para {cobertura}.");

            reserva.ObraSocial = cobertura;
            reserva.NroAfiliado = cobertura == "Particular" ? null : reserva.NroAfiliado!.Trim();

            foreach (var turno in ordenados)
            {
                turno.Reserva = reserva;
                turno.Estado = EstadoTurno.Reservado;
            }

            _context.Reservas.Add(reserva);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new InvalidOperationException("Alguno de los turnos ya fue reservado por otra persona.");
            }
        }

        public async Task UpdateAsync(Reserva reserva)
        {
            var existente = await _context.Reservas
                .Include(r => r.PracticasRealizadas)
                .Include(r => r.Turnos)
                .FirstOrDefaultAsync(r => r.Id == reserva.Id);

            if (existente == null)
                throw new InvalidOperationException("Reserva no encontrada.");

            // Al cancelar la reserva, sus turnos vuelven a quedar disponibles
            if (reserva.Estado == EstadoReserva.Cancelada && existente.Estado != EstadoReserva.Cancelada)
            {
                foreach (var turno in existente.Turnos.ToList())
                {
                    turno.Estado = EstadoTurno.Disponible;
                    turno.ReservaId = null;
                }
            }

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

        // Al eliminar una reserva los turnos vuelven a quedar disponibles
        public async Task DeleteAsync(int id)
        {
            var existente = await _context.Reservas
                .Include(r => r.Turnos)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (existente == null)
                throw new InvalidOperationException("Reserva no encontrada.");

            existente.ValidarModificable();

            foreach (var turno in existente.Turnos.ToList())
            {
                turno.Estado = EstadoTurno.Disponible;
                turno.ReservaId = null;
                turno.Reserva = null;
            }

            _context.Reservas.Remove(existente);
            await _context.SaveChangesAsync();
        }
    }
}