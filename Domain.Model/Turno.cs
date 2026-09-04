using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Turno
    {
        private static int incCodigo = 0;
        public int Codigo { get; set; } 
        public DateTime FechaHoraInicio { get; set; }
        public int Duracion { get; set; }
        public EstadoTurno Estado { get; set; }

        // FK DE ODONTOLOGO
        public string _odontologoMatricula { get; set; }
        public Odontologo _odontologo { get; set; }

        // FK DE RESERVA
        public int _reservaPacienteId { get; set; }
        public string _reservaOdontologoMatricula { get; set; }
        public DateTime _reservaFechaCreacion { get; set; }
        public Reserva _reserva { get; set; }

        //GET Y SET DE LAS 2FK
        public string OdontologoMatricula
        {
            get => _odontologo?.Matricula ?? _odontologoMatricula;
            set => _odontologoMatricula = value;
        }

        public Odontologo? Odontologo
        {
            get => _odontologo;
            set
            {
                _odontologo = value;
                if (value != null && _odontologoMatricula != value.Matricula)
                {
                    _odontologoMatricula = value.Matricula; // Sincronizar automáticamente
                }
            }
        }
        public int ReservaPacienteId
        {
            get => _reserva?.PacienteId ?? _reservaPacienteId;
            set => _reservaPacienteId = value;
        }
        public string ReservaOdontologoMatricula
        {
            get => _reserva?.OdontologoMatricula ?? _reservaOdontologoMatricula;
            set => _reservaOdontologoMatricula = value;
        }
        public DateTime ReservaFechaCreacion
        {
            get => _reserva?.FechaCreacion ?? _reservaFechaCreacion;
            set => _reservaFechaCreacion = value;
        }
        public Reserva? Reserva
        {
            get => _reserva;
            set
            {
                _reserva = value;
                if (value != null)
                {
                    _reservaPacienteId = value.PacienteId;
                    _reservaOdontologoMatricula = value.OdontologoMatricula;
                    _reservaFechaCreacion = value.FechaCreacion;
                }
            }
        }
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
            incCodigo++;
            Codigo = incCodigo;
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