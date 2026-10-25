using System;

namespace Domain.Model
{
    public class ReservaPractica
    {
        public int Id { get; set; }

        public int ReservaId { get; set; }
        public Reserva? Reserva { get; set; }

        public int PracticaCodigo { get; set; }
        public Practica? Practica { get; set; }

        // Formato: "14-M, 15-O" (diente-cara)
        public string DientesCaras { get; set; } = string.Empty;

        public float Precio { get; set; }
        public string? Observaciones { get; set; }
    }
}