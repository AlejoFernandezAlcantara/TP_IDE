using BCrypt.Net;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            // 1. Administrador por defecto
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

            // 2. Mutuales
            if (!context.Mutuales.Any())
            {
                var mutuales = new List<Mutual>
                {
                    new Mutual(301111111, "OSDE"),
                    new Mutual(302222222, "Swiss Medical"),
                    new Mutual(303333333, "Galeno"),
                    new Mutual(304444444, "Medifé"),
                    new Mutual(305555555, "IOMA"),
                    new Mutual(306666666, "PAMI")
                };
                context.Mutuales.AddRange(mutuales);
                context.SaveChanges();
            }

            // 3. Prácticas odontológicas
            if (!context.Practicas.Any())
            {
                var practicas = new List<Practica>
                {
                    new Practica("Consulta y Diagnóstico", 15000f),
                    new Practica("Limpieza y Profilaxis", 22000f),
                    new Practica("Obturación con Composite (Arreglo)", 28000f),
                    new Practica("Extracción Simple", 35000f),
                    new Practica("Tratamiento de Conducto (Endodoncia)", 65000f),
                    new Practica("Corona de Porcelana", 120000f),
                    new Practica("Blanqueamiento Dental", 80000f),
                    new Practica("Perno y Corona", 95000f)
                };
                context.Practicas.AddRange(practicas);
                context.SaveChanges();
            }

            // 4. Odontólogos de prueba
            var passHash = BCrypt.Net.BCrypt.HashPassword("Odonto123!");
            if (!context.Odontologos.Any())
            {
                var odo1 = new Odontologo("MN1001", 28123456, tiposEnumerados.DNI, "Ortodoncia", "Carlos", "Gómez", "carlos.gomez@clinica.com", passHash);
                var odo2 = new Odontologo("MN1002", 31456789, tiposEnumerados.DNI, "Endodoncia", "Lucía", "Fernández", "lucia.fernandez@clinica.com", passHash);
                var odo3 = new Odontologo("MN1003", 26789123, tiposEnumerados.DNI, "Cirugía Maxilofacial", "Martín", "Álvarez", "martin.alvarez@clinica.com", passHash);
                var odo4 = new Odontologo("MN1004", 33654321, tiposEnumerados.DNI, "Odontopediatría", "Mariana", "López", "mariana.lopez@clinica.com", passHash);

                context.Odontologos.AddRange(odo1, odo2, odo3, odo4);
                context.SaveChanges();

                var mutualesDb = context.Mutuales.ToList();
                if (mutualesDb.Count >= 3)
                {
                    context.OdontologoMutuales.AddRange(
                        new OdontologoMutual(101) { OdontologoMatricula = "MN1001", MutualCuit = mutualesDb[0].Cuit },
                        new OdontologoMutual(102) { OdontologoMatricula = "MN1001", MutualCuit = mutualesDb[1].Cuit },
                        new OdontologoMutual(103) { OdontologoMatricula = "MN1002", MutualCuit = mutualesDb[0].Cuit },
                        new OdontologoMutual(104) { OdontologoMatricula = "MN1002", MutualCuit = mutualesDb[2].Cuit },
                        new OdontologoMutual(105) { OdontologoMatricula = "MN1003", MutualCuit = mutualesDb[1].Cuit },
                        new OdontologoMutual(106) { OdontologoMatricula = "MN1004", MutualCuit = mutualesDb[0].Cuit }
                    );
                    context.SaveChanges();
                }
            }

            // 5. Pacientes de prueba (sin atadura fija de obra social)
            var passPaciente = BCrypt.Net.BCrypt.HashPassword("Paciente123!");
            if (!context.Pacientes.Any())
            {
                var pac1 = new Paciente("Juan", "Pérez", "Av. Rivadavia 1234", "1144556677", "juan.perez@gmail.com", 35123456, tiposEnumerados.DNI, passPaciente);
                var pac2 = new Paciente("Ana", "Martínez", "Calle Falsa 456", "1155667788", "ana.martinez@gmail.com", 38987654, tiposEnumerados.DNI, passPaciente);
                var pac3 = new Paciente("Roberto", "Sánchez", "San Martín 789", "1166778899", "roberto.sanchez@gmail.com", 29876543, tiposEnumerados.DNI, passPaciente);
                var pac4 = new Paciente("Sofía", "Romero", "Belgrano 1010", "1177889900", "sofia.romero@gmail.com", 42334455, tiposEnumerados.DNI, passPaciente);

                context.Pacientes.AddRange(pac1, pac2, pac3, pac4);
                context.SaveChanges();
            }

            // 6. Reservas y Turnos de prueba
            if (!context.Reservas.Any() && !context.Turnos.Any())
            {
                var pac = context.Pacientes.First();
                var pac2 = context.Pacientes.Skip(1).FirstOrDefault() ?? pac;
                var odo = context.Odontologos.First();
                var odo2 = context.Odontologos.Skip(1).FirstOrDefault() ?? odo;

                // Reserva 1: Confirmada con Obra Social OSDE
                var res1 = new Reserva("Dolor de muela constante", 25000f, 5000f)
                {
                    PacienteId = pac.NroPaciente,
                    OdontologoMatricula = odo.Matricula,
                    Estado = EstadoReserva.Confirmada,
                    ObraSocial = "OSDE",
                    NroAfiliado = "987654321-01"
                };

                // Reserva 2: Pendiente con Particular
                var res2 = new Reserva("Control y limpieza general", 20000f, 0f)
                {
                    PacienteId = pac2.NroPaciente,
                    OdontologoMatricula = odo2.Matricula,
                    Estado = EstadoReserva.Pendiente,
                    ObraSocial = "Particular",
                    NroAfiliado = null
                };

                // Reserva 3: Completada (Atendida) con prácticas registradas y formateadas ("14-M, 15-O")
                var res3 = new Reserva("Tratamiento finalizado", 63000f, 10000f)
                {
                    PacienteId = pac.NroPaciente,
                    OdontologoMatricula = odo.Matricula,
                    Estado = EstadoReserva.Completada,
                    FechaRealizacion = DateTime.Today.AddDays(-2),
                    Resultado = "Atención finalizada con éxito. Se obturó pieza 14 y 15.",
                    ObraSocial = "Swiss Medical",
                    NroAfiliado = "SM-12345678"
                };

                context.Reservas.AddRange(res1, res2, res3);
                context.SaveChanges();

                var practicasDb = context.Practicas.ToList();
                if (practicasDb.Count >= 2)
                {
                    var rp1 = new ReservaPractica
                    {
                        ReservaId = res3.Id,
                        PracticaCodigo = practicasDb[2].CodigoPractica, // Obturación
                        DientesCaras = "14-M, 15-O",
                        Precio = practicasDb[2].Precio,
                        Observaciones = "Arreglo con resina compuesta fotocurable"
                    };

                    var rp2 = new ReservaPractica
                    {
                        ReservaId = res3.Id,
                        PracticaCodigo = practicasDb[1].CodigoPractica, // Limpieza
                        DientesCaras = "Todos",
                        Precio = practicasDb[1].Precio,
                        Observaciones = "Profilaxis completa"
                    };

                    context.ReservaPracticas.AddRange(rp1, rp2);
                    context.SaveChanges();
                }

                // Turnos
                var manana = DateTime.Today.AddDays(1);
                var t1 = new Turno(manana.AddHours(9)) { OdontologoMatricula = odo.Matricula, Estado = EstadoTurno.Reservado, ReservaId = res1.Id };
                var t2 = new Turno(manana.AddHours(9).AddMinutes(30)) { OdontologoMatricula = odo.Matricula, Estado = EstadoTurno.Disponible };
                var t3 = new Turno(manana.AddHours(10)) { OdontologoMatricula = odo.Matricula, Estado = EstadoTurno.Disponible };
                var t4 = new Turno(manana.AddHours(10).AddMinutes(30)) { OdontologoMatricula = odo.Matricula, Estado = EstadoTurno.Cancelado };

                var t5 = new Turno(manana.AddHours(14)) { OdontologoMatricula = odo2.Matricula, Estado = EstadoTurno.Reservado, ReservaId = res2.Id };
                var t6 = new Turno(manana.AddHours(14).AddMinutes(30)) { OdontologoMatricula = odo2.Matricula, Estado = EstadoTurno.Disponible };

                var tPasado = new Turno(DateTime.Today.AddDays(-2).AddHours(11))
                {
                    OdontologoMatricula = odo.Matricula,
                    Estado = EstadoTurno.Reservado,
                    ReservaId = res3.Id
                };

                context.Turnos.AddRange(t1, t2, t3, t4, t5, t6, tPasado);
                context.SaveChanges();
            }
        }
    }
}