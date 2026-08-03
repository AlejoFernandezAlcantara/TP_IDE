using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Diente
    {
        public int NroDiente{ get; private set; }

        public string NombreDiente { get; private set; }

        public TipoDiente TipoDiente { get; private set; }

        public Diente(int nroDiente, string nombreDiente, TipoDiente tipoDiente) { 
            SetNroDiente(nroDiente);
            SetNombreDiente(nombreDiente);
            SetTipoDiente(tipoDiente);
        }

        public void SetNroDiente(int nroDiente)
        {
            if (nroDiente <= 0)
            {
                throw new ArgumentException("El número de diente debe ser mayor que cero.");
            }
            NroDiente = nroDiente;
        }

        public void SetNombreDiente(string nombreDiente)
        {
            if (string.IsNullOrWhiteSpace(nombreDiente))
            {
                throw new ArgumentException("El nombre del diente no puede estar vacío.");
            }
            NombreDiente = nombreDiente;
        }

        public void SetTipoDiente(TipoDiente tipoDiente)
        {
            TipoDiente = tipoDiente;
        }
    }
}
