using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Domain.Model;

namespace DTO
{
    public class OdontologoDTO
    {
        public string Matricula { get; set; } = string.Empty;
        public int NroDocumento { get; set; }
        public tiposEnumerados TipoDocumento { get; set; }
        public string Especialidad { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}