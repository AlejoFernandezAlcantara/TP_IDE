using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Cara
    {
        public int IdCara { get; set; }
        public string Nombre { get; set; }
        public string Detalle { get; set; }

        public Cara(string nombre, string detalle) {
            SetIdCara();
            SetNombre(nombre);
            SetDetalle(detalle);
        }
        public void SetIdCara()
        {
            IdCara = 0;
        }
        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre de la cara no puede estar vacío.");
            }
            Nombre = nombre;
        }
        public void SetDetalle(string detalle)
        {
            if (string.IsNullOrWhiteSpace(detalle))
            {
                throw new ArgumentException("El detalle de la cara no puede estar vacío.");
            }
            Detalle = detalle;
        }
    }
}
