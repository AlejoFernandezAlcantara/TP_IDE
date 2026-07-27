using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Turno
    {
        public int Codigo { get; set; } //ver forma de asignacion de codigo
        public DateTime FechaHoraInicio { get; set; }
        public int Duracion { get; set; }
        public EstadoTurno Estado { get; set; }

        // FK DE ODONTOLOGO
        public string OdontologoMatricula { get; set; }
        public Odontologo Odontologo { get; set; }

        // FK DE RESERVA
        public int ReservaPacienteId { get; set; }
        public string ReservaOdontologoMatricula { get; set; }
        public DateTime ReservaFechaCreacion { get; set; }
        public Reserva Reserva { get; set; }
    
         //GET Y SET DE LAS 2FK





    
    // CONSTRUCTOR

    public Turno(DateTime FechaIni)
        {
            SetCod();
            SetFechaIni(FechaIni);
            SetDuracion();
            SetEstado();
        }
    public void SetCod()
        {
            Codigo = 0;
        }
    public void SetFechaIni(DateTime fecha)
        {
            FechaHoraInicio = fecha;
        }
    public void SetDuracion()
        {
            Duracion = 30;
        }
    public void SetEstado()
        {
            Estado = EstadoTurno.Disponible;
        }

    }
}