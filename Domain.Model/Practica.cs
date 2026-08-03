using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Practica
    {
        public int CodigoPractica { get; set; }

        public string Detalle { get; set; }

        public float Precio { get; set; }


    public Practica(int codigoPractica, string detalle, float precio)
        {
            SetCodigoPractica(codigoPractica);
            SetDetalle(detalle);
            SetPrecio(precio);
        }
        public void SetCodigoPractica(int codigoPractica)
        {
            if (codigoPractica <= 0)
            {
                throw new ArgumentException("El código de la práctica debe ser un número positivo.");
            }
            CodigoPractica = codigoPractica;
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
