using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Administrador : Usuario
    {
        public string Nombre { get; private set; }
        public string Apellido { get; private set; }

        public override string Rol => "Administrador";

        public Administrador(string nombre, string apellido,
                              string email, string passwordHash)
            : base(email, passwordHash)
        {
            Nombre = nombre;
            Apellido = apellido;
        }
    }
}