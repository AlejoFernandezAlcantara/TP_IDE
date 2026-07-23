using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Paciente
    {
        public int NroPaciente { get; private set; }

        public string Nombre { get; private set; }

        public string Apellido { get; private set; }

        public string Direccion { get; private set; }

        public string Telefono { get; private set; }

        public string Mail { get; private set; }

        //public class Odontograma { get; private set; }
    }
}
