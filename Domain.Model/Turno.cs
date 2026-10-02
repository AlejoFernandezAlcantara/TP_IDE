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

            // FK Odontologo
            public string OdontologoMatricula { get; set; } = string.Empty;
            public Odontologo? Odontologo { get; set; }

            // FK Reserva (la que usa EF)
            public int? ReservaId { get; set; }
            public Reserva? Reserva { get; set; }

            // ---- Compatibilidad con código existente (EF las ignora) ----
            private int _reservaPacienteId;
            private string _reservaOdontologoMatricula = string.Empty;
            private DateTime _reservaFechaCreacion;

            public int ReservaPacienteId
            {
                get => Reserva?.PacienteId ?? _reservaPacienteId;
                set => _reservaPacienteId = value;
            }

            public string ReservaOdontologoMatricula
            {
                get => Reserva?.OdontologoMatricula ?? _reservaOdontologoMatricula;
                set => _reservaOdontologoMatricula = value;
            }

            public DateTime ReservaFechaCreacion
            {
                get => Reserva?.FechaCreacion ?? _reservaFechaCreacion;
                set => _reservaFechaCreacion = value;
            }

            // Para EF
            protected Turno() { }

            public Turno(DateTime fechaHoraInicio)
            {
                SetFechaIni(fechaHoraInicio);
                SetDuracion();
                SetEstado();
            }

            public void SetFechaIni(DateTime fecha) { FechaHoraInicio = fecha; }
            public void SetDuracion() { Duracion = 30; }
            public void SetEstado() { Estado = EstadoTurno.Disponible; }
        }
    
}
