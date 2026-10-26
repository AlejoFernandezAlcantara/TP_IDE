using Domain.Model;

namespace DTO
{
    public class ReservaDTO //HAY UN POCO DE MANEJO EN ESTE DTO,COMPARTIDO ENTRE LA API, BLAZOR ,Y WINFORMS PARA SIMPLIFICAR
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public EstadoReserva Estado { get; set; }
        public string? Observaciones { get; set; }
        public float? Importe { get; set; }
        public float? Coseguro { get; set; }
        public DateTime? FechaRealizacion { get; set; }
        public string? Resultado { get; set; }

        public string ObraSocial { get; set; } = "Particular";
        public string? NroAfiliado { get; set; }

        public int _pacienteId { get; set; }
        public string _odontologoMatricula { get; set; } = string.Empty;

        // Turnos que se bloquean con esta reserva
        public List<int> TurnoCodigos { get; set; } = new();

        // Turnos asociados a la reserva
        public List<TurnoReservaDTO> Turnos { get; set; } = new();

        // Primer turno reservado (para ordenar/mostrar en listados)
        public DateTime? PrimerTurno => Turnos.Count > 0 ? Turnos.Min(t => t.FechaHoraInicio) : (DateTime?)null;

        public string TurnosTexto => Turnos.Count == 0
            ? "-"
            : string.Join(" | ", Turnos.OrderBy(t => t.FechaHoraInicio).Select(t => t.FechaHoraInicio.ToString("dd/MM/yyyy HH:mm")));

        // Prácticas realizadas, PARA VER HISTORIAL
        public List<ReservaPracticaDTO> PracticasRealizadas { get; set; } = new();
    }

    public class TurnoReservaDTO
    {
        public int Codigo { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public int Duracion { get; set; }
    }

    // Reserva de uno o varios turnos en una sola operación
    public class ReservarTurnosDTO
    {
        public int PacienteId { get; set; }
        public List<int> TurnoCodigos { get; set; } = new();
        public string ObraSocial { get; set; } = "Particular";
        public string? NroAfiliado { get; set; }
    }

    public class ReservaPracticaDTO
    {
        public int Id { get; set; }
        public int ReservaId { get; set; }
        public int PracticaCodigo { get; set; }
        public string? PracticaDetalle { get; set; }
        public string DientesCaras { get; set; } = string.Empty;
        public float Precio { get; set; }
        public string? Observaciones { get; set; }
    }

    public class AtenderReservaDTO
    {
        public int ReservaId { get; set; }
        public string? Resultado { get; set; }
        public float? ImporteTotal { get; set; }
        public List<ReservaPracticaItemDTO> Practicas { get; set; } = new();
    }

    public class ReservaPracticaItemDTO
    {
        public int PracticaCodigo { get; set; }
        public string DientesCaras { get; set; } = string.Empty; // Ej: "14-M, 15-O"
        public float Precio { get; set; }
        public string? Observaciones { get; set; }
    }
}