using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Turno
    {
        public int Codigo { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public int Duracion { get; set; }
        public EstadoTurno Estado { get; set; }

        public string OdontologoMatricula { get; set; }
        public Odontologo Odontologo { get; set; }

        // FK de reserva
        public int ReservaPacienteId { get; set; }
        public string ReservaOdontologoMatricula { get; set; }
        public DateTime ReservaFechaCreacion { get; set; }
        public Reserva Reserva { get; set; }
    }

    public enum EstadoTurno
    {
        Disponible,
        Reservado,
        Cancelado
    }
}