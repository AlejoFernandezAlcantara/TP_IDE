using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public int NroDocumento { get; private set; }
        public EstadoTipoDoc TipoDocumento { get; private set; }
        public enum EstadoTipoDoc
        {
            DNI,
            Pasaporte
        }

        //public class Odontograma { get; private set; }
        public Paciente(int nroPaciente, string nombre, string apellido, string direccion, string telefono, string mail, int nroDocumento, EstadoTipoDoc tipoDocumento)
        {
            SetNroPaciente(nroPaciente);
            SetNroDoc(nroDocumento);
            SetTipoDoc(tipoDocumento);
            SetNombre(nombre);
            SetApellido(apellido);
            SetDireccion(direccion);
            SetTelefono(telefono);
            SetMail(mail);
        }

        public void SetNroPaciente(int nroPaciente)
        {
            if (nroPaciente <= 0) throw new ArgumentException("El número de paciente debe ser mayor a cero.");
            NroPaciente = nroPaciente;
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

        public void SetMail(string mail)
        {
            if (!EsEmailValido(mail))
                throw new ArgumentException("El email no tiene un formato válido.", nameof(mail));
            Mail = mail;
        }

        private static bool EsEmailValido(string mail)  
        {
            if (string.IsNullOrWhiteSpace(mail))
                return false;
            return Regex.IsMatch(mail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
        public void SetNroDoc(int nroDocumento)
        {
            if (nroDocumento <= 0)
                throw new ArgumentException("El número de documento debe ser mayor que 0.", nameof(nroDocumento));
            NroDocumento = nroDocumento;

        }
        public void SetTipoDoc(EstadoTipoDoc tipoDocumento)
        {
            TipoDocumento = tipoDocumento;
        }

        public void SetDireccion(string direccion)
        {
            Direccion = direccion;
        }

        public void SetTelefono(string telefono)
        {
            Telefono = telefono;
        }

       
    }
}
