using System.Security.Claims;
using Applications.Services;
using DTO;

namespace WebAPI
{
    public static class ReservaEndpoints
    {
        public static void MapReservaEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/reservas").WithTags("Reservas");

            // Admin: todas. Odontólogo: solo las suyas.
            group.MapGet("/", async (ClaimsPrincipal user, IReservaService service) =>
            {
                var reservas = await service.GetAllAsync();

                if (user.IsInRole("Odontologo"))
                    reservas = reservas.Where(r => r._odontologoMatricula == user.Matricula()).ToList();

                return Results.Ok(reservas);
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador", "Odontologo"));

            // Obtener reserva por Id
            group.MapGet("/{id:int}", async Task<IResult> (int id, ClaimsPrincipal user, IReservaService service) =>
            {
                var reserva = await service.GetByIdAsync(id);
                if (reserva == null)
                    return Results.NotFound();

                if (user.IsInRole("Odontologo") && reserva._odontologoMatricula != user.Matricula())
                    return Results.Forbid();

                if (user.IsInRole("Paciente") && reserva._pacienteId != user.NroPaciente())
                    return Results.Forbid();

                return Results.Ok(reserva);
            })
            .RequireAuthorization();

            // Paciente: solo las propias. Odontólogo: solo las de sus turnos. Admin: cualquiera.
            group.MapGet("/paciente/{pacienteId}", async Task<IResult> (int pacienteId, ClaimsPrincipal user, IReservaService service) =>
            {
                if (user.IsInRole("Paciente") && user.NroPaciente() != pacienteId)
                    return Results.Forbid();

                var reservas = await service.GetByPacienteAsync(pacienteId);

                if (user.IsInRole("Odontologo"))
                    reservas = reservas.Where(r => r._odontologoMatricula == user.Matricula()).ToList();

                return Results.Ok(reservas);
            })
            .RequireAuthorization();

            group.MapPost("/", async (ReservaDTO dto, IReservaService service) =>
            {
                await service.CrearAsync(dto);
                return Results.Created("/api/reservas", dto);
            })
            .RequireAuthorization();

            // Atender reserva (Odontograma multipráctica con formato string "14-M, 15-O")
            group.MapPost("/atender", async Task<IResult> (AtenderReservaDTO dto, ClaimsPrincipal user, IReservaService service) =>
            {
                var reserva = await service.GetByIdAsync(dto.ReservaId);
                if (reserva == null)
                    return Results.NotFound("Reserva no encontrada");

                if (!user.PuedeGestionarTurnosDe(reserva._odontologoMatricula))
                    return Results.Forbid();

                await service.AtenderReservaAsync(dto);
                return Results.Ok(new { mensaje = "Reserva atendida y completada con éxito." });
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador", "Odontologo"));

            // Actualizar general
            group.MapPut("/", async Task<IResult> (ReservaDTO dto, ClaimsPrincipal user, IReservaService service) =>
            {
                if (!user.PuedeGestionarTurnosDe(dto._odontologoMatricula))
                    return Results.Forbid();

                await service.ActualizarAsync(dto);
                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador", "Odontologo"));

            group.MapDelete("/{id}", async (int id, IReservaService service) =>
            {
                await service.EliminarAsync(id);
                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));
        }
    }
}