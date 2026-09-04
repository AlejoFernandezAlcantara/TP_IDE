using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Text.RegularExpressions;

namespace Domain.Model
{
    public class Paciente : Usuario
    {
        private static int incNroPaciente = 0;
        public int NroPaciente { get; private set; }
        public string Nombre { get; private set; }
        public string Apellido { get; private set; }
        public string Direccion { get; private set; }
        public string Telefono { get; private set; }   
        public int NroDni { get; private set; }
        public tiposEnumerados  TipoDni { get; private set; }


        public override string Rol => "Paciente";

        public Paciente( string nombre, string apellido, string direccion, string telefono, string email,int nro,tiposEnumerados tipo, string passwordHash)
            : base(email, passwordHash)
        {
            SetNroPaciente();
            SetNombre(nombre);
            SetApellido(apellido);
            SetDireccion(direccion);
            SetTelefono(telefono);
            SetDniTipo(nro, tipo);
        }
        public void SetDniTipo(int nro, tiposEnumerados tipo) 
        {
            if (nro <= 0)
                throw new ArgumentException("El número de DNI debe ser mayor a cero.", nameof(nro));
            NroDni = nro;
            TipoDni = tipo;
        }

        public void SetNroPaciente()
        {
            incNroPaciente++;
            NroPaciente = incNroPaciente;

        }

        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede ser nulo o vacío.", nameof(nombre));
            Nombre = nombre;
        }

        public void SetApellido(string apellido)
        {
            if (string.IsNullOrWhiteSpace(apellido))
                throw new ArgumentException("El apellido no puede ser nulo o vacío.", nameof(apellido));
            Apellido = apellido;
        }

        public void SetDireccion(string direccion)
        {
            if (string.IsNullOrWhiteSpace(direccion))
                throw new ArgumentException("La dirección no puede ser nula o vacía.", nameof(direccion));
            Direccion = direccion;
        }

        public void SetTelefono(string telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono))
                throw new ArgumentException("El teléfono no puede ser nulo o vacío.", nameof(telefono));
            Telefono = telefono;
        }
    }
}