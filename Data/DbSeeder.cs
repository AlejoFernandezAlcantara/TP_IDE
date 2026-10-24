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

            // 4. Caras del diente (5 caras anatómicas estándar)
            if (!context.Caras.Any())
            {
                var caras = new List<Cara>
                {
                    new Cara("Vestibular", "Cara externa orientada hacia los labios o mejillas"),
                    new Cara("Lingual / Palatina", "Cara interna orientada hacia la lengua o paladar"),
                    new Cara("Oclusal / Incisal", "Superficie de masticación o corte"),
                    new Cara("Mesial", "Cara lateral más cercana a la línea media"),
                    new Cara("Distal", "Cara lateral más alejada de la línea media")
                };
                context.Caras.AddRange(caras);
                context.SaveChanges();
            }

            // 5. Los 32 Dientes permanentes (Nomenclatura FDI)
            if (!context.Dientes.Any())
            {
                var dientes = new List<Diente>
                {
                    // Cuadrante 1 (Superior Derecho)
                    new Diente(18, "Tercer molar superior derecho", TipoDiente.Molar),
                    new Diente(17, "Segundo molar superior derecho", TipoDiente.Molar),
                    new Diente(16, "Primer molar superior derecho", TipoDiente.Molar),
                    new Diente(15, "Segundo premolar superior derecho", TipoDiente.Premolar),
                    new Diente(14, "Primer premolar superior derecho", TipoDiente.Premolar),
                    new Diente(13, "Canino superior derecho", TipoDiente.Canino),
                    new Diente(12, "Incisivo lateral superior derecho", TipoDiente.Incisivo),
                    new Diente(11, "Incisivo central superior derecho", TipoDiente.Incisivo),

                    // Cuadrante 2 (Superior Izquierdo)
                    new Diente(21, "Incisivo central superior izquierdo", TipoDiente.Incisivo),
                    new Diente(22, "Incisivo lateral superior izquierdo", TipoDiente.Incisivo),
                    new Diente(23, "Canino superior izquierdo", TipoDiente.Canino),
                    new Diente(24, "Primer premolar superior izquierdo", TipoDiente.Premolar),
                    new Diente(25, "Segundo premolar superior izquierdo", TipoDiente.Premolar),
                    new Diente(26, "Primer molar superior izquierdo", TipoDiente.Molar),
                    new Diente(27, "Segundo molar superior izquierdo", TipoDiente.Molar),
                    new Diente(28, "Tercer molar superior izquierdo", TipoDiente.Molar),

                    // Cuadrante 3 (Inferior Izquierdo)
                    new Diente(38, "Tercer molar inferior izquierdo", TipoDiente.Molar),
                    new Diente(37, "Segundo molar inferior izquierdo", TipoDiente.Molar),
                    new Diente(36, "Primer molar inferior izquierdo", TipoDiente.Molar),
                    new Diente(35, "Segundo premolar inferior izquierdo", TipoDiente.Premolar),
                    new Diente(34, "Primer premolar inferior izquierdo", TipoDiente.Premolar),
                    new Diente(33, "Canino inferior izquierdo", TipoDiente.Canino),
                    new Diente(32, "Incisivo lateral inferior izquierdo", TipoDiente.Incisivo),
                    new Diente(31, "Incisivo central inferior izquierdo", TipoDiente.Incisivo),

                    // Cuadrante 4 (Inferior Derecho)
                    new Diente(41, "Incisivo central inferior derecho", TipoDiente.Incisivo),
                    new Diente(42, "Incisivo lateral inferior derecho", TipoDiente.Incisivo),
                    new Diente(43, "Canino inferior derecho", TipoDiente.Canino),
                    new Diente(44, "Primer premolar inferior derecho", TipoDiente.Premolar),
                    new Diente(45, "Segundo premolar inferior derecho", TipoDiente.Premolar),
                    new Diente(46, "Primer molar inferior derecho", TipoDiente.Molar),
                    new Diente(47, "Segundo molar inferior derecho", TipoDiente.Molar),
                    new Diente(48, "Tercer molar inferior derecho", TipoDiente.Molar)
                };
                context.Dientes.AddRange(dientes);
                context.SaveChanges();
            }

            // 6. DienteCara: relacionar las 5 caras con cada uno de los 32 dientes
            if (!context.DienteCaras.Any())
            {
                var dientes = context.Dientes.ToList();
                var caras = context.Caras.ToList();
                var listaDienteCaras = new List<DienteCara>();

                foreach (var d in dientes)
                {
                    foreach (var c in caras)
                    {
                        listaDienteCaras.Add(new DienteCara
                        {
                            DienteNro = d.NroDiente,
                            CaraId = c.IdCara
                        });
                    }
                }
                context.DienteCaras.AddRange(listaDienteCaras);
                context.SaveChanges();
            }

            // 7. Odontólogos de prueba
            var passHash = BCrypt.Net.BCrypt.HashPassword("Odonto123!");
            if (!context.Odontologos.Any())
            {
                var odo1 = new Odontologo("MN1001", 28123456, tiposEnumerados.DNI, "Ortodoncia", "Carlos", "Gómez", "carlos.gomez@clinica.com", passHash);
                var odo2 = new Odontologo("MN1002", 31456789, tiposEnumerados.DNI, "Endodoncia", "Lucía", "Fernández", "lucia.fernandez@clinica.com", passHash);
                var odo3 = new Odontologo("MN1003", 26789123, tiposEnumerados.DNI, "Cirugía Maxilofacial", "Martín", "Álvarez", "martin.alvarez@clinica.com", passHash);
                var odo4 = new Odontologo("MN1004", 33654321, tiposEnumerados.DNI, "Odontopediatría", "Mariana", "López", "mariana.lopez@clinica.com", passHash);

                context.Odontologos.AddRange(odo1, odo2, odo3, odo4);
                context.SaveChanges();

                // Asociar odontólogos con mutuales
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

            // 8. Pacientes de prueba
            var passPaciente = BCrypt.Net.BCrypt.HashPassword("Paciente123!");
            if (!context.Pacientes.Any())
            {
                var pac1 = new Paciente("Juan", "Pérez", "Av. Rivadavia 1234", "1144556677", "juan.perez@gmail.com", 35123456, tiposEnumerados.DNI, passPaciente);
                var pac2 = new Paciente("Ana", "Martínez", "Calle Falsa 456", "1155667788", "ana.martinez@gmail.com", 38987654, tiposEnumerados.DNI, passPaciente);
                var pac3 = new Paciente("Roberto", "Sánchez", "San Martín 789", "1166778899", "roberto.sanchez@gmail.com", 29876543, tiposEnumerados.DNI, passPaciente);
                var pac4 = new Paciente("Sofía", "Romero", "Belgrano 1010", "1177889900", "sofia.romero@gmail.com", 42334455, tiposEnumerados.DNI, passPaciente);

                context.Pacientes.AddRange(pac1, pac2, pac3, pac4);
                context.SaveChanges();

                // Asociar pacientes a mutuales
                var mutualesDb = context.Mutuales.ToList();
                if (mutualesDb.Count >= 2)
                {
                    context.PacienteMutuales.AddRange(
                        new PacienteMutual(987654, true) { PacienteId = pac1.NroPaciente, MutualCuit = mutualesDb[0].Cuit },
                        new PacienteMutual(456789, true) { PacienteId = pac2.NroPaciente, MutualCuit = mutualesDb[1].Cuit },
                        new PacienteMutual(123456, false) { PacienteId = pac3.NroPaciente, MutualCuit = mutualesDb[2].Cuit }
                    );
                    context.SaveChanges();
                }

                // Odontogramas iniciales para los pacientes
                var odoPac1 = new Odontograma(pac1.NroPaciente);
                var odoPac2 = new Odontograma(pac2.NroPaciente);
                context.Odontogramas.AddRange(odoPac1, odoPac2);
                context.SaveChanges();

                // Detalle en el odontograma del paciente 1
                var practicasDb = context.Practicas.ToList();
                if (practicasDb.Any())
                {
                    var det1 = new Detalle(odoPac1.Id, practicasDb[0].CodigoPractica, 11, null, "Caries incipiente cara mesial");
                    context.Detalles.Add(det1);
                    context.SaveChanges();
                }
            }

            // 9. Reservas y Turnos de prueba en distintos estados
            if (!context.Reservas.Any() && !context.Turnos.Any())
            {
                var pac = context.Pacientes.First();
                var pac2 = context.Pacientes.Skip(1).FirstOrDefault() ?? pac;
                var odo = context.Odontologos.First();
                var odo2 = context.Odontologos.Skip(1).FirstOrDefault() ?? odo;

                // Reserva 1: Confirmada
                var res1 = new Reserva("Dolor de muela constante", 25000f, 5000f)
                {
                    PacienteId = pac.NroPaciente,
                    OdontologoMatricula = odo.Matricula,
                    Estado = EstadoReserva.Confirmada
                };

                // Reserva 2: Pendiente
                var res2 = new Reserva("Control y limpieza general", 20000f, 0f)
                {
                    PacienteId = pac2.NroPaciente,
                    OdontologoMatricula = odo2.Matricula,
                    Estado = EstadoReserva.Pendiente
                };

                // Reserva 3: Completada
                var res3 = new Reserva("Extracción pieza 18", 35000f, 7000f)
                {
                    PacienteId = pac.NroPaciente,
                    OdontologoMatricula = odo.Matricula,
                    Estado = EstadoReserva.Completada,
                    FechaRealizacion = DateTime.Today.AddDays(-2),
                    Resultado = "Extracción realizada con éxito sin complicaciones"
                };

                context.Reservas.AddRange(res1, res2, res3);
                context.SaveChanges();

                // Turnos para Odontólogo 1
                var manana = DateTime.Today.AddDays(1);
                var t1 = new Turno(manana.AddHours(9)) { OdontologoMatricula = odo.Matricula, Estado = EstadoTurno.Reservado, ReservaId = res1.Id };
                var t2 = new Turno(manana.AddHours(9).AddMinutes(30)) { OdontologoMatricula = odo.Matricula, Estado = EstadoTurno.Disponible };
                var t3 = new Turno(manana.AddHours(10)) { OdontologoMatricula = odo.Matricula, Estado = EstadoTurno.Disponible };
                var t4 = new Turno(manana.AddHours(10).AddMinutes(30)) { OdontologoMatricula = odo.Matricula, Estado = EstadoTurno.Cancelado };

                // Turnos para Odontólogo 2
                var t5 = new Turno(manana.AddHours(14)) { OdontologoMatricula = odo2.Matricula, Estado = EstadoTurno.Reservado, ReservaId = res2.Id };
                var t6 = new Turno(manana.AddHours(14).AddMinutes(30)) { OdontologoMatricula = odo2.Matricula, Estado = EstadoTurno.Disponible };
                var t7 = new Turno(manana.AddHours(15)) { OdontologoMatricula = odo2.Matricula, Estado = EstadoTurno.Disponible };

                // Turno pasado completado
                var tPasado = new Turno(DateTime.Today.AddDays(-2).AddHours(11))
                {
                    OdontologoMatricula = odo.Matricula,
                    Estado = EstadoTurno.Reservado,
                    ReservaId = res3.Id
                };

                context.Turnos.AddRange(t1, t2, t3, t4, t5, t6, t7, tPasado);
                context.SaveChanges();
            }
        }
    }
}
