using Domain.Model;
using System;
using System.Collections.Generic;

namespace DTO
{
    public class ReservaDTO
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

        // Prácticas realizadas (para ver historial en reservas completadas)
        public List<ReservaPracticaDTO> PracticasRealizadas { get; set; } = new();
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