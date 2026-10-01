using Applications.Services;
using Domain.Model;
using DTO;
using System.Security.Claims;

namespace WebAPI
{
    public static class PacienteEndpoints
    {
        public static void MapPacienteEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/pacientes").WithTags("Pacientes");

            // Obtener todos
            group.MapGet("/", async (IPacienteService service) =>
                Results.Ok(await service.GetAllAsync()))
                .RequireAuthorization();

            // Obtener por número de paciente
            group.MapGet("/{nroPaciente}", async Task<IResult> (int nroPaciente, ClaimsPrincipal user, IPacienteService service) =>
            {
                if (user.IsInRole("Paciente") && user.NroPaciente() != nroPaciente)
                    return Results.Forbid();

                var paciente = await service.GetByNroPacienteAsync(nroPaciente);
                return paciente is null ? Results.NotFound() : Results.Ok(paciente);
            })
            .RequireAuthorization();


            // Crear paciente
            group.MapPost("/", async (PacienteDTO dto, IPacienteService service) =>
            {
                dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password ?? string.Empty);

                await service.CrearAsync(dto);

                return Results.Created($"/api/pacientes/{dto.NroPaciente}", dto);
            });

            // Actualizar paciente
            group.MapPut("/", async Task<IResult> (PacienteDTO dto, ClaimsPrincipal user, IPacienteService service) =>
            {
                if (!user.EsAdmin() && user.NroPaciente() != dto.NroPaciente)
                    return Results.Forbid();

                await service.ActualizarAsync(dto);
                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador", "Paciente"));

            // Eliminar paciente
            group.MapDelete("/{nroPaciente}", async (int nroPaciente, IPacienteService service) =>
            {
                await service.EliminarAsync(nroPaciente);

                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));
        }
    }
}