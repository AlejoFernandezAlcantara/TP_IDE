using System.Security.Claims;
using Applications.Services;
using DTO;

namespace WebAPI
{
    public static class TurnoEndpoints
    {
        private const int HorasLimiteCancelacionPaciente = 12;

        public static void MapTurnoEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/turnos").WithTags("Turnos");

            group.MapGet("/", async (ITurnoService service) =>
                Results.Ok(await service.GetAllAsync()))
                .RequireAuthorization();

            group.MapGet("/{codigo}", async (int codigo, ITurnoService service) =>
            {
                var turno = await service.GetByCodigoAsync(codigo);
                return turno is null ? Results.NotFound() : Results.Ok(turno);
            })
            .RequireAuthorization();

            group.MapGet("/odontologo/{matricula}", async (string matricula, ITurnoService service) =>
                Results.Ok(await service.GetByOdontologoAsync(matricula)))
                .RequireAuthorization();

            // Admin: cualquier odontólogo. Odontólogo: solo turnos propios.
            group.MapPost("/", async Task<IResult> (TurnoDTO dto, ClaimsPrincipal user, ITurnoService service) =>
            {
                if (!user.PuedeGestionarTurnosDe(dto._odontologoMatricula))
                    return Results.Forbid();

                await service.CrearAsync(dto);
                return Results.Created("/api/turnos", dto);
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador", "Odontologo"));

            group.MapPut("/", async Task<IResult> (TurnoDTO dto, ClaimsPrincipal user, ITurnoService service) =>
            {
                var existente = await service.GetByCodigoAsync(dto.Codigo);
                if (existente is null)
                    return Results.NotFound();

                // Debe ser dueño del turno actual y no puede pasarlo a otro odontólogo
                if (!user.PuedeGestionarTurnosDe(existente._odontologoMatricula) ||
                    !user.PuedeGestionarTurnosDe(dto._odontologoMatricula))
                    return Results.Forbid();

                await service.ActualizarAsync(dto);
                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador", "Odontologo"));

            group.MapDelete("/{codigo}", async Task<IResult> (int codigo, ClaimsPrincipal user, ITurnoService service) =>
            {
                var existente = await service.GetByCodigoAsync(codigo);
                if (existente is null)
                    return Results.NotFound();

                if (!user.PuedeGestionarTurnosDe(existente._odontologoMatricula))
                    return Results.Forbid();

                await service.EliminarAsync(codigo);
                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador", "Odontologo"));

            // Paciente: siempre reserva para sí mismo (se ignora el body).
            // Admin y odontólogo: indican el paciente en el body.
            group.MapPost("/{codigo}/reservar", async Task<IResult> (int codigo, ReservarTurnoRequest? req, ClaimsPrincipal user, ITurnoService service) =>
            {
                int pacienteId;

                if (user.IsInRole("Paciente"))
                {
                    var propio = user.NroPaciente();
                    if (propio is null)
                        return Results.Forbid();
                    pacienteId = propio.Value;
                }
                else
                {
                    if (req is null || req.PacienteId <= 0)
                        return Results.BadRequest(new { error = "Indicá el paciente." });
                    pacienteId = req.PacienteId;
                }

                try
                {
                    return Results.Ok(await service.ReservarAsync(codigo, pacienteId));
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Conflict(new { error = ex.Message });
                }
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador", "Odontologo", "Paciente"));

            // Paciente: solo sus reservas y hasta 12 hs antes. Odontólogo: solo en sus turnos. Admin: todas.
            group.MapPost("/{codigo}/cancelar-reserva", async Task<IResult> (int codigo, ClaimsPrincipal user, ITurnoService service) =>
            {
                var turno = await service.GetByCodigoAsync(codigo);
                if (turno is null)
                    return Results.NotFound();

                if (user.IsInRole("Paciente"))
                {
                    if (turno._reservaPacienteId != user.NroPaciente())
                        return Results.Forbid();

                    if (turno.FechaHoraInicio - DateTime.Now < TimeSpan.FromHours(HorasLimiteCancelacionPaciente))
                        return Results.Conflict(new
                        {
                            error = $"Solo podés cancelar hasta {HorasLimiteCancelacionPaciente} horas antes del turno."
                        });
                }
                else if (!user.PuedeGestionarTurnosDe(turno._odontologoMatricula))
                {
                    return Results.Forbid();
                }

                try
                {
                    await service.CancelarReservaAsync(codigo);
                    return Results.NoContent();
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Conflict(new { error = ex.Message });
                }
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador", "Odontologo", "Paciente"));
        }
    }

    public record ReservarTurnoRequest(int PacienteId);
}