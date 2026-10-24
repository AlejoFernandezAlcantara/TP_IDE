using System;
using System.Collections.Generic;

namespace Domain.Model
{
    public class Reserva
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public EstadoReserva Estado { get; set; }
        public string? Observaciones { get; set; }
        public float? Importe { get; set; }
        public float? Coseguro { get; set; }
        public DateTime? FechaRealizacion { get; set; }
        public string? Resultado { get; set; }

        // Cobertura específica de esta reserva
        public string ObraSocial { get; set; } = "Particular";
        public string? NroAfiliado { get; set; }

        // FK PACIENTE
        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }

        // FK ODONTOLOGO
        public string OdontologoMatricula { get; set; } = string.Empty;
        public Odontologo? Odontologo { get; set; }

        // Prácticas realizadas en la atención con detalle de piezas ("14-M, 15-O")
        public ICollection<ReservaPractica> PracticasRealizadas { get; set; } = new List<ReservaPractica>();

        // Constructor para EF
        public Reserva()
        {
            FechaCreacion = DateTime.Now;
            Estado = EstadoReserva.Pendiente;
        }

        public Reserva(string observaciones, float? importe, float? coseguro)
        {
            FechaCreacion = DateTime.Now;
            Estado = EstadoReserva.Pendiente;
            Observaciones = string.IsNullOrWhiteSpace(observaciones) ? null : observaciones;
            Importe = importe >= 0 ? importe : 0;
            Coseguro = coseguro >= 0 ? coseguro : 0;
        }

        public void CompletarAtencion(string? resultado, IEnumerable<ReservaPractica> practicas)
        {
            Estado = EstadoReserva.Completada;
            FechaRealizacion = DateTime.Now;
            Resultado = resultado;
            
            float total = 0;
            foreach (var p in practicas)
            {
                p.ReservaId = this.Id;
                PracticasRealizadas.Add(p);
                total += p.Precio;
            }

            if (Importe == null || Importe == 0)
            {
                Importe = total;
            }
        }
    }
}
