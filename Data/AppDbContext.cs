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
        /* public DbSet<Paciente> Pacientes { get; set; }
         public DbSet<Odontologo> Odontologos { get; set; }
         public DbSet<Administrador> Administradores { get; set; }   

         private readonly string _connectionString;

         public AppDbContext(string connectionString)
         {
             _connectionString = connectionString;


         }

         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
         {
             if (!optionsBuilder.IsConfigured)
             {
                 optionsBuilder.UseSqlServer(_connectionString); 
             }
         }
         */

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Odontologo> Odontologos { get; set; }
        public DbSet<Administrador> Administradores { get; set; }

        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<ReservaPractica> ReservaPracticas { get; set; }
        public DbSet<Practica> Practicas { get; set; }
        public DbSet<PracticaDiente> PracticaDientes { get; set; }
        public DbSet<Diente> Dientes { get; set; }
        public DbSet<Cara> Caras { get; set; }
        public DbSet<DienteCara> DienteCaras { get; set; }
        public DbSet<Mutual> Mutuales { get; set; }
        public DbSet<PacienteMutual> PacienteMutuales { get; set; }
        public DbSet<OdontologoMutual> OdontologoMutuales { get; set; }

        private readonly string _connectionString;

        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===================== HERENCIA DE USUARIO (TPH) =====================
            modelBuilder.Entity<Usuario>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Usuario>()
                .HasDiscriminator<string>("TipoUsuario")
                .HasValue<Paciente>("Paciente")
                .HasValue<Odontologo>("Odontologo")
                .HasValue<Administrador>("Administrador");

            modelBuilder.Entity<Paciente>()
                .HasAlternateKey(p => p.NroPaciente);

            modelBuilder.Entity<Odontologo>()
                .HasAlternateKey(o => o.Matricula);

            // ===================== ENTIDADES CON PK PROPIA =====================
            modelBuilder.Entity<Practica>().HasKey(p => p.CodigoPractica);
            modelBuilder.Entity<Diente>().HasKey(d => d.NroDiente);
            modelBuilder.Entity<Cara>().HasKey(c => c.IdCara);
            modelBuilder.Entity<Mutual>().HasKey(m => m.Cuit);
            modelBuilder.Entity<Turno>().HasKey(t => t.Codigo);

            // ===================== RESERVA (clave compuesta) =====================
            modelBuilder.Entity<Reserva>()
                .HasKey(r => new { r.PacienteId, r.OdontologoMatricula, r.FechaCreacion });

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Paciente)
                .WithMany()
                .HasForeignKey(r => r.PacienteId)
                .HasPrincipalKey(p => p.NroPaciente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Odontologo)
                .WithMany()
                .HasForeignKey(r => r.OdontologoMatricula)
                .HasPrincipalKey(o => o.Matricula)
                .OnDelete(DeleteBehavior.Restrict);

            // ===================== RESERVAPRACTICA (Reserva <-> Practica) =====================
            modelBuilder.Entity<ReservaPractica>()
                .HasKey(rp => new { rp.ReservaPacienteId, rp.ReservaOdontologoMatricula, rp.ReservaFechaCreacion, rp.PracticaCodigo });

            modelBuilder.Entity<ReservaPractica>()
                .HasOne(rp => rp.Reserva)
                .WithMany()
                .HasForeignKey(rp => new { rp.ReservaPacienteId, rp.ReservaOdontologoMatricula, rp.ReservaFechaCreacion })
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReservaPractica>()
                .HasOne(rp => rp.Practica)
                .WithMany()
                .HasForeignKey(rp => rp.PracticaCodigo)
                .OnDelete(DeleteBehavior.Restrict);

            // ===================== TURNO =====================
            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Odontologo)
                .WithMany()
                .HasForeignKey(t => t.OdontologoMatricula)
                .HasPrincipalKey(o => o.Matricula)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Reserva)
                .WithMany()
                .HasForeignKey(t => new { t.ReservaPacienteId, t.ReservaOdontologoMatricula, t.ReservaFechaCreacion })
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // ===================== PRACTICADIENTE (Practica <-> Diente) =====================
            modelBuilder.Entity<PracticaDiente>()
                .HasKey(pd => new { pd.PracticaCodigo, pd.DienteNro });

            modelBuilder.Entity<PracticaDiente>()
                .HasOne(pd => pd.Practica)
                .WithMany()
                .HasForeignKey(pd => pd.PracticaCodigo)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PracticaDiente>()
                .HasOne(pd => pd.Diente)
                .WithMany()
                .HasForeignKey(pd => pd.DienteNro)
                .OnDelete(DeleteBehavior.Restrict);

            // ===================== DIENTECARA (Diente <-> Cara) =====================
            modelBuilder.Entity<DienteCara>()
                .HasKey(dc => new { dc.DienteNro, dc.CaraId });

            modelBuilder.Entity<DienteCara>()
                .HasOne(dc => dc.Diente)
                .WithMany()
                .HasForeignKey(dc => dc.DienteNro)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DienteCara>()
                .HasOne(dc => dc.Cara)
                .WithMany()
                .HasForeignKey(dc => dc.CaraId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===================== PACIENTEMUTUAL (Paciente <-> Mutual) =====================
            modelBuilder.Entity<PacienteMutual>()
                .HasKey(pm => new { pm.PacienteId, pm.MutualCuit });

            modelBuilder.Entity<PacienteMutual>()
                .HasOne(pm => pm.Paciente)
                .WithMany()
                .HasForeignKey(pm => pm.PacienteId)
                .HasPrincipalKey(p => p.NroPaciente)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PacienteMutual>()
                .HasOne(pm => pm.Mutual)
                .WithMany()
                .HasForeignKey(pm => pm.MutualCuit)
                .OnDelete(DeleteBehavior.Restrict);

            // ===================== ODONTOLOGOMUTUAL (Odontologo <-> Mutual) =====================
            modelBuilder.Entity<OdontologoMutual>()
                .HasKey(om => new { om.OdontologoMatricula, om.MutualCuit });

            modelBuilder.Entity<OdontologoMutual>()
                .HasOne(om => om.Odontologo)
                .WithMany()
                .HasForeignKey(om => om.OdontologoMatricula)
                .HasPrincipalKey(o => o.Matricula)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OdontologoMutual>()
                .HasOne(om => om.Mutual)
                .WithMany()
                .HasForeignKey(om => om.MutualCuit)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
