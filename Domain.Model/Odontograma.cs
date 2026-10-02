using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Odontograma
    {
        public int Id { get; private set; }
        public DateTime FechaCreacion { get; private set; }
        public EstadoOdontograma Estado { get; private set; }

        // FK PACIENTE (relación 1:1, única en la base)
        public int PacienteId { get; private set; }
        public Paciente? Paciente { get; set; }

        public ICollection<Detalle> Detalles { get; } = new List<Detalle>();

        // Constructor para EF Core
        private Odontograma() { }

        public Odontograma(int pacienteId)
        {
            SetPaciente(pacienteId);
            SetFechaCreacion();
            SetEstado(EstadoOdontograma.Activo);
        }

        public void SetPaciente(int pacienteId)
        {
            if (pacienteId <= 0)
                throw new ArgumentException("El paciente del odontograma no es válido.", nameof(pacienteId));
            PacienteId = pacienteId;
        }

        public void SetFechaCreacion()
        {
            FechaCreacion = DateTime.Now;
        }

        public void SetEstado(EstadoOdontograma estado)
        {
            Estado = estado;
        }
    }
}
