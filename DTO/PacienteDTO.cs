using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PacienteDTO
    {
        public int NroPaciente { get; private set; }
        public string Nombre { get; private set; }
        public string Apellido { get; private set; }
        public string Direccion { get; private set; }
        public string Telefono { get; private set; }
        public string Mail { get; private set; }
        public int NroDocumento { get; private set; }
        public EstadoTipoDoc TipoDocumento { get; private set; }
        public enum EstadoTipoDoc
        {
            DNI,
            Pasaporte
        }
    }
}
