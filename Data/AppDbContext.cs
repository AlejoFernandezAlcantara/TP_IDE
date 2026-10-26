using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Odontologo> Odontologos { get; set; }
        public DbSet<Administrador> Administradores { get; set; }

        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<ReservaPractica> ReservaPracticas { get; set; }
        public DbSet<Practica> Practicas { get; set; }
        public DbSet<Mutual> Mutuales { get; set; }
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
                // DIAGNÓSTICO TEMPORAL: muestra en la terminal de la API cada SQL y su duración
                optionsBuilder.UseSqlServer(_connectionString)
                    .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===================== USUARIOS =====================
            modelBuilder.Ignore<Usuario>();

            modelBuilder.Entity<Paciente>().HasKey(p => p.NroPaciente);
            modelBuilder.Entity<Paciente>().Property(p => p.NroPaciente).ValueGeneratedOnAdd();

            modelBuilder.Entity<Odontologo>().HasKey(o => o.Matricula);
            modelBuilder.Entity<Administrador>().HasKey(a => a.Id);

            // ===================== CATALOGOS BASICOS =====================
            modelBuilder.Entity<Practica>().HasKey(p => p.CodigoPractica);
            modelBuilder.Entity<Practica>().Property(p => p.CodigoPractica).ValueGeneratedOnAdd();

            modelBuilder.Entity<Mutual>().HasKey(m => m.Cuit);
            modelBuilder.Entity<Mutual>().Property(m => m.Cuit).ValueGeneratedNever();

            // ===================== RESERVA =====================
            modelBuilder.Entity<Reserva>().HasKey(r => r.Id);
            modelBuilder.Entity<Reserva>().Property(r => r.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Paciente)
                .WithMany()
                .HasForeignKey(r => r.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Odontologo)
                .WithMany()
                .HasForeignKey(r => r.OdontologoMatricula)
                .OnDelete(DeleteBehavior.Restrict);

            // ===================== RESERVA -> PRACTICAS REALIZADAS =====================
            modelBuilder.Entity<ReservaPractica>().HasKey(rp => rp.Id);
            modelBuilder.Entity<ReservaPractica>().Property(rp => rp.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<ReservaPractica>()
                .HasOne(rp => rp.Reserva)
                .WithMany(r => r.PracticasRealizadas)
                .HasForeignKey(rp => rp.ReservaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReservaPractica>()
                .HasOne(rp => rp.Practica)
                .WithMany()
                .HasForeignKey(rp => rp.PracticaCodigo)
                .OnDelete(DeleteBehavior.Restrict);

            // ===================== TURNO -> RESERVA =====================
            modelBuilder.Entity<Turno>().HasKey(t => t.Codigo);
            modelBuilder.Entity<Turno>().Property(t => t.Codigo).ValueGeneratedOnAdd();
            modelBuilder.Entity<Turno>().Property(t => t.Estado).IsConcurrencyToken();

            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Reserva)
                .WithMany(r => r.Turnos)
                .HasForeignKey(t => t.ReservaId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Turno>().Ignore(t => t.ReservaPacienteId);
            modelBuilder.Entity<Turno>().Ignore(t => t.ReservaOdontologoMatricula);
            modelBuilder.Entity<Turno>().Ignore(t => t.ReservaFechaCreacion);

            // ===================== ODONTOLOGOMUTUAL =====================
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