using Data;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace WindowsForms
{
    internal static class Program
    {
        public static string ConnectionString =
            "Server=(localdb)\\MSSQLLocalDB;Database=ClinicaOdontologicaDB;Trusted_Connection=True;TrustServerCertificate=True;";

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            using var context = new AppDbContext(ConnectionString);

            InicializarBaseDeDatos(context);
            InicializarContadorPacientes(context);

            Application.Run(new Form1());
        }

        private static void InicializarBaseDeDatos(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Administradores.Any())
            {
                var admin = new Administrador(
                    nombre: "Super",
                    apellido: "Admin",
                    email: "admin@clinica.com",
                    passwordHash: BCrypt.Net.BCrypt.HashPassword("Admin123!")
                );

                context.Administradores.Add(admin);
                context.SaveChanges();
            }
        }

        private static void InicializarContadorPacientes(AppDbContext context)
        {
            var maximo = context.Pacientes.Max(p => (int?)p.NroPaciente) ?? 0;
            Paciente.InicializarContador(maximo);
        }
    }
}