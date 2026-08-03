using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Domain.Model
{
    public class Reserva
    {
        public DateTime FechaCreacion { get; set; }
        public EstadoReserva Estado { get; set; }
        public string? Observaciones { get; set; }
        public float? Importe { get; set; }
        public float? Coseguro { get; set; }
        public DateTime? FechaRealizacion { get; set; }
        public string? Resultado { get; set; }

        //FK PACIENTE
        private int _pacienteId;
        private Paciente? _paciente;

        //FK ODONTOLOGO
        private string _odontologoMatricula = string.Empty;
        private Odontologo? _odontologo;

        
        //GET Y SET DE LAS 2FK
        public int PacienteId
        {
            get => _paciente?.NroPaciente ?? _pacienteId;
            set => _pacienteId = value;
        }

        public Paciente? Paciente
        {
            get => _paciente;
            set
            {
                _paciente = value;
                if (value != null && _pacienteId != value.NroPaciente)
                {
                    _pacienteId = value.NroPaciente; // Sincronizar automáticamente
                }
            }
        }
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
        //constructor
        public Reserva(string obs, float importe, float coseguro)
        {
            SetFecha();
            SetEst();
            SetObs(obs);
            SetImp(importe);
            SetC(coseguro);

        }
        public void SetFecha()
        {
            FechaCreacion = DateTime.Now;
        }
        public void SetEst()
        {
            Estado = EstadoReserva.Pendiente;
        }
        public void SetObs(string obs)
        {
            if (string.IsNullOrWhiteSpace(obs))
            {
                Observaciones = null;
            }
            else 
            { 
                Observaciones = obs; 
            }
        }
        public void SetImp(float importe)
        {
            if(importe < 0)
            {
                throw new ArgumentException("El importe no puede ser negativo.");
            }
            else
            {
                Importe = importe;
            }

        }
        public void SetC(float coseguro)
        {
            if (coseguro < 0)
            {
                throw new ArgumentException("El coseguro no puede ser negativo.");
            }
            else
            {
                Coseguro = coseguro;
            }
        }
        public void SetFechaRealizacion()
        {
            FechaRealizacion = DateTime.Now;

        }
        public void SetResultado(string resultado)
        {
            if (string.IsNullOrWhiteSpace(resultado))
            {
                Resultado = null;
            }
            else
            {
                Resultado = resultado;
            }
        }
    }
}







































































