using System;

namespace Domain.Model
{
    public class Mutual
    {
        // "Particular" es la cobertura reservada que usan las reservas sin mutual
        public const string NombreParticular = "Particular";

        public int Cuit { get; set; }
        public string Nombre { get; set; } = string.Empty;

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

            var limpio = nombre.Trim();

            if (string.Equals(limpio, NombreParticular, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"\"{NombreParticular}\" es una cobertura reservada y no puede usarse como nombre de mutual.");
            }

            Nombre = limpio;
        }
    }
}