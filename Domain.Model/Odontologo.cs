using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Domain.Model
{
    public class Odontologo
    {
        public string Matricula { get; private set; }

        public int NroDocumento { get; private set; }

        public EstadoTipoDoc TipoDocumento { get; private set; }

        public string Especialidad { get; private set; }

        public string Nombre { get; private set; }

        public string Apellido { get; private set; }

        public string Email { get; private set; }

        public enum EstadoTipoDoc
        {
            DNI,
            Pasaporte
        }

        public Odontologo(string matricula, int nroDocumento, EstadoTipoDoc tipoDocumento, string especialidad, string nombre, string apellido, string email )
        {

            SetMatricula(matricula);
            SetNroDoc(nroDocumento);
            SetTipoDoc(tipoDocumento);
            SetEspecialidad(especialidad);
            SetNombre(nombre);
            SetApellido(apellido);
            SetEmail(email);

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

        public void SetEmail(string email)
        {
            if (!EsEmailValido(email))
                throw new ArgumentException("El email no tiene un formato válido.", nameof(email));
            Email = email;
        }

        private static bool EsEmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public void SetMatricula(string matricula)
        {
            if (string.IsNullOrWhiteSpace(matricula))
                throw new ArgumentException("La matrícula no puede ser nulo o vacío.", nameof(matricula));
            Matricula = matricula;

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
        public void SetEspecialidad(string especialidad) 
        {
            if (string.IsNullOrWhiteSpace(especialidad))
                throw new ArgumentException("La especialidad no puede ser nulo o vacío.", nameof(especialidad));
            Especialidad = especialidad;
        }
    }
}
