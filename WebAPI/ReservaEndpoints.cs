using System;
using Applications.Services;
using DTO;

namespace WebAPI
{
    public static class ReservaEndpoints
    {
        public static void MapReservaEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/reservas").WithTags("Reservas");

            group.MapGet("/", async (IReservaService service) =>
                Results.Ok(await service.GetAllAsync()))
                .RequireAuthorization();

            group.MapGet("/paciente/{pacienteId}", async (int pacienteId, IReservaService service) =>
                Results.Ok(await service.GetByPacienteAsync(pacienteId)))
                .RequireAuthorization();

            group.MapPost("/", async (ReservaDTO dto, IReservaService service) =>
            {
                await service.CrearAsync(dto);
                return Results.Created("/api/reservas", dto);
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador", "Odontologo"));

            group.MapPut("/", async (ReservaDTO dto, IReservaService service) =>
            {
                await service.ActualizarAsync(dto);
                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador", "Odontologo"));

            // fechaCreacion va por query string, ej: ?fechaCreacion=2026-09-05T10:00:00
            group.MapDelete("/{pacienteId}/{odontologoMatricula}", async (int pacienteId, string odontologoMatricula, DateTime fechaCreacion, IReservaService service) =>
            {
                await service.EliminarAsync(pacienteId, odontologoMatricula, fechaCreacion);
                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));
        }
    }
}
