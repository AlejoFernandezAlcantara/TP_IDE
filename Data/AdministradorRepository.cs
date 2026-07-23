using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Domain.Model;
using BCrypt.Net;

namespace Data
{
    public class AdministradorRepository : IAdministradorRepository
    {
        private readonly List<Administrador> _administradores = new();

        // Se precarga un único admin al levantar la app
        public AdministradorRepository()
        {
            var admin = new Administrador(
                nombre: "Super",
                apellido: "Admin",
                email: "admin@clinica.com",
                passwordHash: BCrypt.Net.BCrypt.HashPassword("Admin123!")
            );
            _administradores.Add(admin);
        }

        public Administrador? GetByEmail(string email) =>
            _administradores.FirstOrDefault(a => a.Email == email);
    }
}