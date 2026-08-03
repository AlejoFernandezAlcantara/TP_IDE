using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Mutual
    {
        public int Cuit { get; set; }
        public string Nombre { get; set; }

        public Mutual(int cuit, string nombre)
        {
            SetCuit(cuit);
            SetNombre(nombre);

        }
        public void SetCuit(int cuit)
        {
            if (cuit <= 0)
            {
                throw new ArgumentException("El CUIT debe ser un número positivo.");
            }
            Cuit = cuit;
        }
        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre no puede estar vacío.");
            }
            Nombre = nombre;
        }
    }
}
