using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsForms
{
    public static class Sesion
    {
        public static string? Email { get; set; }
        public static string? Rol { get; set; }
        public static string? Nombre { get; set; }
        public static Usuario? UsuarioActual { get; set; }
    }
}
