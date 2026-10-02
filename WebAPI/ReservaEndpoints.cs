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

            group.MapDelete("/{id}", async (int id, IReservaService service) =>
            {
                await service.EliminarAsync(id);
                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

        }
    }
}
