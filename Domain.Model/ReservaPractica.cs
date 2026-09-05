using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class ReservaPractica
    {
        public string? Observaciones { get; set; }

        //FK DE PRACTICA
        private int _practicaCodigo;
        private Practica? _practica;
        // FK DE RESERVA
        private int _reservaPacienteId;
        private string _reservaOdontologoMatricula = string.Empty;
        private DateTime _reservaFechaCreacion;
        private Reserva? _reserva;

        //GET Y SET DE LAS 2FK
        public int PracticaCodigo
        {
            get => _practica?.CodigoPractica ?? _practicaCodigo;
            set => _practicaCodigo = value;
        }
        public Practica? Practica
        {
            get => _practica;
            set
            {
                _practica = value;
                if (value != null && _practicaCodigo != value.CodigoPractica)
                {
                    _practicaCodigo = value.CodigoPractica; // Sincronizar automáticamente
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

        //CONSTRUCTOR
        public ReservaPractica(string observaciones)
        {
            SetObservaciones(observaciones);
        }
        public void SetObservaciones(string observaciones)
        {
            if (string.IsNullOrWhiteSpace(observaciones))
            {
                throw new ArgumentException("Las observaciones no pueden estar vacías.");
            }
            Observaciones = observaciones;
        }
    }
}