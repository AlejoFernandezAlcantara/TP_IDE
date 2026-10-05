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
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            // Confirmar, completar, cargar resultado. Odontólogo: solo reservas propias.
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