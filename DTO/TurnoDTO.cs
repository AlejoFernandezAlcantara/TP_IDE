using Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class TurnoDTO
    {
        public int Codigo { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public int Duracion { get; set; }
        public EstadoTurno Estado { get; set; }

        //FK DE ODONTOLOGO
        public string _odontologoMatricula { get; set; }

        //FK DE RESERVA
        public int _reservaPacienteId { get; set; }
        public string _reservaOdontologoMatricula { get; set; }
        public DateTime _reservaFechaCreacion { get; set; }

    }
}
