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
                context.Mutuales.AddRange(
                    new Mutual(301111111, "OSDE"),
                    new Mutual(302222222, "Swiss Medical"),
                    new Mutual(303333333, "Galeno"),
                    new Mutual(304444444, "Medifé"),
                    new Mutual(305555555, "IOMA"),
                    new Mutual(306666666, "PAMI")
                );
                context.SaveChanges();
            }

            // 3. Prácticas odontológicas (CodigoPractica lo genera la base)
            var pConsulta = new Practica("Consulta y Diagnóstico", 15000f);
            var pLimpieza = new Practica("Limpieza y Profilaxis", 22000f);
            var pObturacion = new Practica("Obturación con Composite (Arreglo)", 28000f);
            var pExtraccion = new Practica("Extracción Simple", 35000f);
            var pEndodoncia = new Practica("Tratamiento de Conducto (Endodoncia)", 65000f);
            var pCorona = new Practica("Corona de Porcelana", 120000f);
            var pBlanqueo = new Practica("Blanqueamiento Dental", 80000f);
            var pPerno = new Practica("Perno y Corona", 95000f);

            if (!context.Practicas.Any())
            {
                context.Practicas.AddRange(
                    pConsulta, pLimpieza, pObturacion, pExtraccion,
                    pEndodoncia, pCorona, pBlanqueo, pPerno
                );
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

                var mutualesDb = context.Mutuales.OrderBy(m => m.Cuit).ToList();
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

            // 5. Pacientes de prueba
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
                var pac = context.Pacientes.OrderBy(p => p.NroPaciente).First();
                var pac2 = context.Pacientes.OrderBy(p => p.NroPaciente).Skip(1).FirstOrDefault() ?? pac;
                var odo = context.Odontologos.OrderBy(o => o.Matricula).First();
                var odo2 = context.Odontologos.OrderBy(o => o.Matricula).Skip(1).FirstOrDefault() ?? odo;

                // Reserva 1: Confirmada con OSDE
                var res1 = new Reserva("Dolor de muela constante", 25000f, 5000f)
                {
                    PacienteId = pac.NroPaciente,
                    OdontologoMatricula = odo.Matricula,
                    Estado = EstadoReserva.Confirmada,
                    ObraSocial = "OSDE",
                    NroAfiliado = "987654321-01"
                };

                // Reserva 2: Pendiente, particular
                var res2 = new Reserva("Control y limpieza general", 20000f, 0f)
                {
                    PacienteId = pac2.NroPaciente,
                    OdontologoMatricula = odo2.Matricula,
                    Estado = EstadoReserva.Pendiente,
                    ObraSocial = "Particular",
                    NroAfiliado = null
                };

                // Reserva 3: Completada con prácticas registradas
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

                // Prácticas realizadas (usa las referencias de arriba, ya con su código generado)
                context.ReservaPracticas.AddRange(
                    new ReservaPractica
                    {
                        ReservaId = res3.Id,
                        PracticaCodigo = pObturacion.CodigoPractica,
                        DientesCaras = "14-M, 15-O",
                        Precio = pObturacion.Precio,
                        Observaciones = "Arreglo con resina compuesta fotocurable"
                    },
                    new ReservaPractica
                    {
                        ReservaId = res3.Id,
                        PracticaCodigo = pLimpieza.CodigoPractica,
                        DientesCaras = "Todos",
                        Precio = pLimpieza.Precio,
                        Observaciones = "Profilaxis completa"
                    }
                );
                context.SaveChanges();

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

            // 7. Datos de muestra para reportes y gráficos (historial de 12 meses + agenda próxima)
            SeedDemo(context);
        }

        // Genera datos de muestra de forma determinística (semilla fija).
        // Se ejecuta una sola vez: si ya existen reservas marcadas con "[DEMO]", no hace nada.
        private static void SeedDemo(AppDbContext context)
        {
            if (context.Reservas.Any(r => r.Observaciones != null && r.Observaciones.StartsWith("[DEMO]")))
                return;

            var rnd = new Random(2026);
            var hoy = DateTime.Today;
            var ahora = DateTime.Now;

            // ---------- Pacientes adicionales ----------
            var passPaciente = BCrypt.Net.BCrypt.HashPassword("Paciente123!");
            var nombres = new[]
            {
                "Lucas", "Valentina", "Mateo", "Camila", "Facundo", "Julieta", "Santiago", "Agustina",
                "Nicolás", "Micaela", "Tomás", "Florencia", "Ignacio", "Carolina", "Gonzalo", "Lorena"
            };
            var apellidos = new[]
            {
                "Ruiz", "Díaz", "Torres", "Acosta", "Benítez", "Castro", "Medina", "Ortiz",
                "Navarro", "Suárez", "Flores", "Herrera", "Vega", "Molina", "Ríos", "Peralta"
            };

            if (!context.Pacientes.Any(p => p.Email == "paciente.demo1@mail.com"))
            {
                for (int i = 0; i < nombres.Length; i++)
                {
                    context.Pacientes.Add(new Paciente(
                        nombres[i],
                        apellidos[i],
                        $"Calle {i + 1} {100 + i * 37}",
                        "11" + (40000000 + i * 1111111).ToString(),
                        $"paciente.demo{i + 1}@mail.com",
                        20000007 + i * 1000000,
                        tiposEnumerados.DNI,
                        passPaciente));
                }
                context.SaveChanges();
            }

            var pacientes = context.Pacientes.ToList();
            var odontologos = context.Odontologos.ToList();
            var practicas = context.Practicas.ToList();
            if (pacientes.Count == 0 || odontologos.Count == 0 || practicas.Count == 0)
                return;

            // Turnos ya ocupados (odontólogo + horario de inicio), para no superponer
            var ocupados = new HashSet<(string, DateTime)>();
            foreach (var t in context.Turnos.ToList())
                ocupados.Add((t.OdontologoMatricula, t.FechaHoraInicio));

            var obras = new[] { "OSDE", "Swiss Medical", "Galeno", "Medifé", "IOMA", "PAMI", "Particular", "Particular", "Particular" };
            var motivos = new[]
            {
                "Dolor de muela", "Control general", "Limpieza dental", "Caries",
                "Extracción", "Consulta de urgencia", "Blanqueamiento", "Control de ortodoncia",
                "Tratamiento de conducto", "Revisión de implante"
            };
            var resultados = new[]
            {
                "Atención finalizada sin complicaciones.",
                "Se realizó el tratamiento indicado. Control en 6 meses.",
                "Procedimiento completado. Paciente conforme.",
                "Se indica higiene reforzada y control periódico."
            };

            var pendientes = new List<(Reserva Reserva, List<Practica> Practicas, List<DateTime> Slots, string Matricula)>();

            // ---------- Reservas: 12 meses pasados + próximo mes ----------
            for (int offset = -12; offset <= 1; offset++)
            {
                var primerDia = new DateTime(hoy.Year, hoy.Month, 1).AddMonths(offset);
                int cantidad = offset == 1 ? 8 : 14 + rnd.Next(10);

                for (int n = 0; n < cantidad; n++)
                {
                    var dia = primerDia.AddDays(rnd.Next(DateTime.DaysInMonth(primerDia.Year, primerDia.Month)));
                    if (dia.DayOfWeek == DayOfWeek.Sunday)
                        dia = dia.AddDays(1);

                    var inicio = dia.AddHours(8 + rnd.Next(9)).AddMinutes(rnd.Next(2) * 30);
                    var odo = odontologos[rnd.Next(odontologos.Count)];

                    // Una reserva puede ocupar 1 o 2 turnos consecutivos del mismo odontólogo
                    int cantTurnos = rnd.NextDouble() < 0.25 ? 2 : 1;
                    var slots = new List<DateTime>();
                    for (int k = 0; k < cantTurnos; k++)
                        slots.Add(inicio.AddMinutes(30 * k));

                    if (slots.Any(s => ocupados.Contains((odo.Matricula, s))))
                        continue;
                    foreach (var s in slots)
                        ocupados.Add((odo.Matricula, s));

                    var paciente = pacientes[rnd.Next(pacientes.Count)];
                    var obra = obras[rnd.Next(obras.Length)];
                    var motivo = motivos[rnd.Next(motivos.Length)];

                    EstadoReserva estado;
                    var azar = rnd.NextDouble();
                    if (azar < 0.12)
                        estado = EstadoReserva.Cancelada;
                    else if (inicio < ahora)
                        estado = EstadoReserva.Completada;
                    else
                        estado = azar < 0.7 ? EstadoReserva.Confirmada : EstadoReserva.Pendiente;

                    // Prácticas presupuestadas; si la reserva se realizó, quedan registradas
                    var elegidas = practicas.OrderBy(_ => rnd.Next()).Take(1 + rnd.Next(3)).ToList();
                    float importe = elegidas.Sum(p => p.Precio);
                    float coseguro = obra == "Particular" ? 0f : (float)Math.Round(importe * 0.2, 0);
                    bool realizada = estado == EstadoReserva.Completada;

                    var reserva = new Reserva($"[DEMO] {motivo}", importe, coseguro)
                    {
                        PacienteId = paciente.NroPaciente,
                        OdontologoMatricula = odo.Matricula,
                        Estado = estado,
                        ObraSocial = obra,
                        NroAfiliado = obra == "Particular" ? null : $"{PrefijoObra(obra)}-{rnd.Next(10000000, 99999999)}",
                        FechaCreacion = inicio.AddDays(-rnd.Next(1, 15)),
                        FechaRealizacion = realizada ? (DateTime?)inicio : null,
                        Resultado = realizada ? resultados[rnd.Next(resultados.Length)] : null
                    };

                    pendientes.Add((reserva, realizada ? elegidas : new List<Practica>(), slots, odo.Matricula));
                }
            }

            // Primera pasada: guarda las reservas para obtener sus Id
            context.Reservas.AddRange(pendientes.Select(pe => pe.Reserva));
            context.SaveChanges();

            // Segunda pasada: prácticas realizadas y turnos asociados
            foreach (var pe in pendientes)
            {
                foreach (var pr in pe.Practicas)
                {
                    context.ReservaPracticas.Add(new ReservaPractica
                    {
                        ReservaId = pe.Reserva.Id,
                        PracticaCodigo = pr.CodigoPractica,
                        DientesCaras = DientesAzar(rnd),
                        Precio = pr.Precio,
                        Observaciones = "Registrado en la atención"
                    });
                }

                bool cancelada = pe.Reserva.Estado == EstadoReserva.Cancelada;
                foreach (var horario in pe.Slots)
                {
                    context.Turnos.Add(new Turno(horario)
                    {
                        OdontologoMatricula = pe.Matricula,
                        // Una reserva cancelada libera sus turnos (igual que la lógica de la app)
                        Estado = cancelada ? EstadoTurno.Disponible : EstadoTurno.Reservado,
                        ReservaId = cancelada ? (int?)null : pe.Reserva.Id
                    });
                }
            }

            // ---------- Agenda libre de los próximos días ----------
            for (int d = 1; d <= 10; d++)
            {
                var dia = hoy.AddDays(d);
                if (dia.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                foreach (var odo in odontologos)
                {
                    foreach (var hora in new[] { 9, 10, 11, 15, 16 })
                    {
                        var horario = dia.AddHours(hora);
                        if (ocupados.Contains((odo.Matricula, horario)))
                            continue;
                        if (rnd.NextDouble() < 0.5)
                            continue;

                        ocupados.Add((odo.Matricula, horario));
                        context.Turnos.Add(new Turno(horario)
                        {
                            OdontologoMatricula = odo.Matricula,
                            Estado = EstadoTurno.Disponible
                        });
                    }
                }
            }

            context.SaveChanges();
        }

        private static string PrefijoObra(string obra) => obra switch
        {
            "OSDE" => "OS",
            "Swiss Medical" => "SM",
            "Galeno" => "GA",
            "Medifé" => "MF",
            "IOMA" => "IO",
            "PAMI" => "PA",
            _ => "AF"
        };

        private static string DientesAzar(Random rnd)
        {
            var opciones = new[] { "Todos", "14-M, 15-O", "36-O", "11-V", "24-P, 25-M", "46-O" };
            return opciones[rnd.Next(opciones.Length)];
        }
    }
}