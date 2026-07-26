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

            // Público (todos ven la lista)
            group.MapGet("/", (IPacienteService service) =>
                Results.Ok(service.GetAll()))
                .RequireAuthorization();

            group.MapGet("/{nroPaciente}", (int nroPaciente, IPacienteService service) =>
            {
                var paciente = service.GetByNroPaciente(nroPaciente);
                return paciente is null ? Results.NotFound() : Results.Ok(paciente);
            })
            .RequireAuthorization();

            // Público
            // PacienteEndpoints.cs
            group.MapPost("/", (PacienteDTO dto, IPacienteService service) =>
            {
                // Hashear la contraseña antes de enviarla al servicio (o hacerlo dentro del servicio)??
                dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password ?? string.Empty);

                service.Crear(dto);

                return Results.Created($"/api/pacientes/{dto.NroPaciente}", dto);
            });

            group.MapPut("/", (PacienteDTO dto, IPacienteService service) =>
            {
                service.Actualizar(dto);
                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador", "Paciente"));

            group.MapDelete("/{nroPaciente}", (int nroPaciente, IPacienteService service) =>
            {
                service.Eliminar(nroPaciente);
                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));
        }
    }
}