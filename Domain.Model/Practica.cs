using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Practica
    {
        private static int incCodigoPractica = 0;
        public int CodigoPractica { get; set; }

        public string Detalle { get; set; }

        public float Precio { get; set; }


    public Practica( string detalle, float precio)
        {
            SetCodigoPractica();
            SetDetalle(detalle);
            SetPrecio(precio);
        }
        public void SetCodigoPractica()
        {
            incCodigoPractica++;
            CodigoPractica = incCodigoPractica;
        }
        public void SetDetalle(string detalle)
        {
            if (string.IsNullOrWhiteSpace(detalle))
            {
                throw new ArgumentException("El detalle de la práctica no puede estar vacío.");
            }
            Detalle = detalle;
        }
        public void SetPrecio(float precio)
        {
            if (precio < 0)
            {
                throw new ArgumentException("El precio de la práctica no puede ser negativo.");
            }
            Precio = precio;
        }
    }
}
