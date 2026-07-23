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

            // Público: cualquiera puede ver la lista (opcional restringir si preferís)
            group.MapGet("/", (IPacienteService service) =>
                Results.Ok(service.GetAll()))
                .RequireAuthorization();

            group.MapGet("/{nroPaciente}", (int nroPaciente, IPacienteService service) =>
            {
                var paciente = service.GetByNroPaciente(nroPaciente);
                return paciente is null ? Results.NotFound() : Results.Ok(paciente);
            })
            .RequireAuthorization();

            // Público: autorregistro. El admin también puede usarlo, sin restricción no hace falta distinguir.
            // PacienteEndpoints.cs
            group.MapPost("/", (PacienteDTO dto, IPacienteService service) =>
            {
                var paciente = new Paciente(
                    dto.NroPaciente,
                    dto.Nombre,
                    dto.Apellido,
                    dto.Direccion,
                    dto.Telefono,
                    dto.Email,
                    BCrypt.Net.BCrypt.HashPassword(dto.Password)
                );
                service.Crear(paciente);
                return Results.Created($"/api/pacientes/{paciente.NroPaciente}", paciente);
            });

            group.MapPut("/", (Paciente paciente, IPacienteService service) =>
            {
                service.Actualizar(paciente);
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