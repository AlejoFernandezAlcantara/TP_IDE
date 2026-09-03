using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AppDbContext : DbContext
    {
         DbSet<Paciente> Pacientes
        // TODO: DbSet<Odontologo> Odontologos
        // TODO: DbSet<Administrador> Administradores

        private readonly string _connectionString;

        public AppDbContext(string connectionString)
        {
            // TODO: guardar connectionString en el campo
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // TODO: si todavía no está configurado, usar UseSqlServer con _connectionString
        }
    }
}