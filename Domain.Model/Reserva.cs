using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Reserva
    {
        public DateTime FechaCreacion { get; set; }
        public EstadoReserva Estado { get; set; }
        public string Observaciones { get; set; }
        public float Importe { get; set; }
        public float Coseguro { get; set; }
        public DateTime FechaRealizacion { get; set; }
        public string Resultado { get; set; }

        //FK Paciente
        public int PacienteId { get; set; }
        public Paciente Paciente { get; set; }


        //FK Odontologo
        public string OdontologoMatricula { get; set; }
        public Odontologo Odontologo { get; set; }



        public enum EstadoReserva
        {
            Pendiente,
            Confirmada,
            Cancelada,
            Completada
        }

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

        }
        public void SetEst()
        {

        }
        public void SetObs(string obs)
        {

        }
        public void SetImp(float importe)
        {

        }
        public void SetC(float coseguro)
        {

        }
        public void SetFechaRealizacion(DateTime fecha)
        {

        }
        public void SetResultado(string resultado)
        {

        }




    }
}





































