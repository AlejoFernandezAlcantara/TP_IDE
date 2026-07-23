using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Odontologo
    {
        public string Matricula { get; private set; }

        public int NroDocumento { get; private set; }

        public string TipoDocumento { get; private set; }

        public string Especialidad { get; private set; }

        public string Nombre { get; private set; }

        public string Apellido { get; private set; }

        public string Email { get; private set; }

    }
}
