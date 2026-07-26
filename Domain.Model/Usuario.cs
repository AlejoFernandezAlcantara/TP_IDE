using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public abstract class Usuario
    {
        public int Id { get; protected set; } //id manual (hacerlo autoincremental en la base de datos)
        public string Email { get; protected set; }
        public string PasswordHash { get; protected set; }
        public bool Activo { get; protected set; } = true;

        // DEFINICION DE ROLES
        public abstract string Rol { get; }

        protected Usuario(string email, string passwordHash)
        {
            SetEmail(email);
            SetPasswordHash(passwordHash);
        }

        private void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email no puede ser vacío.");
            Email = email;
        }

        private void SetPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("El password no puede ser vacío.");
            PasswordHash = passwordHash;
        }
    }
}