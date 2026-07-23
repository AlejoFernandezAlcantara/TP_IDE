using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class ReservaPractica
    {
        // FK compuesta hacia Reserva
        public int ReservaPacienteId { get; set; }
        public string ReservaOdontologoMatricula { get; set; }
        public DateTime ReservaFechaCreacion { get; set; }
        public Reserva Reserva { get; set; }

        // FK hacia Practica
        public int PracticaCodigoPractica { get; set; }
        public Practica Practica { get; set; }
    }
}