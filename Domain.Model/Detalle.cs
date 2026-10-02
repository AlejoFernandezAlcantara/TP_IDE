using System;

namespace Domain.Model
{
    // Registro de una práctica realizada sobre un diente dentro de un odontograma.
    // Reemplaza a ReservaPractica y PracticaDiente.
    public class Detalle
    {
        public int Id { get; private set; }
        public DateTime? FechaRealizacion { get; private set; }
        public string? Observaciones { get; private set; }

        // FK ODONTOGRAMA
        public int OdontogramaId { get; private set; }
        public Odontograma? Odontograma { get; set; }

        // FK PRACTICA
        public int PracticaCodigo { get; private set; }
        public Practica? Practica { get; set; }

        // FK DIENTE
        public int DienteNro { get; private set; }
        public Diente? Diente { get; set; }

        // FK RESERVA (opcional)
        public int? ReservaId { get; private set; }
        public Reserva? Reserva { get; set; }

        // Constructor para EF Core
        private Detalle() { }

        public Detalle(int odontogramaId, int practicaCodigo, int dienteNro,
                       int? reservaId = null, string? observaciones = null)
        {
            SetOdontograma(odontogramaId);
            SetPractica(practicaCodigo);
            SetDiente(dienteNro);
            SetReserva(reservaId);
            SetObservaciones(observaciones);
        }

        public void SetOdontograma(int odontogramaId)
        {
            if (odontogramaId <= 0)
                throw new ArgumentException("El odontograma no es válido.", nameof(odontogramaId));
            OdontogramaId = odontogramaId;
        }

        public void SetPractica(int practicaCodigo)
        {
            if (practicaCodigo <= 0)
                throw new ArgumentException("La práctica no es válida.", nameof(practicaCodigo));
            PracticaCodigo = practicaCodigo;
        }

        public void SetDiente(int dienteNro)
        {
            if (dienteNro <= 0)
                throw new ArgumentException("El diente no es válido.", nameof(dienteNro));
            DienteNro = dienteNro;
        }

        public void SetReserva(int? reservaId)
        {
            if (reservaId.HasValue && reservaId.Value <= 0)
                throw new ArgumentException("La reserva no es válida.", nameof(reservaId));
            ReservaId = reservaId;
        }

        public void SetObservaciones(string? observaciones)
        {
            Observaciones = string.IsNullOrWhiteSpace(observaciones) ? null : observaciones;
        }

        public void SetFechaRealizacion()
        {
            FechaRealizacion = DateTime.Now;
        }
    }
}
