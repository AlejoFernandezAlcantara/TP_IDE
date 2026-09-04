using Applications.Services;
using Domain.Model;
using DTO;

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
            group.MapGet("/{nroPaciente}", async (int nroPaciente, IPacienteService service) =>
            {
                var paciente = await service.GetByNroPacienteAsync(nroPaciente);

                return paciente is null
                    ? Results.NotFound()
                    : Results.Ok(paciente);
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
            group.MapPut("/", async (PacienteDTO dto, IPacienteService service) =>
            {
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