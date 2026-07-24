using Applications.Services;
using Domain.Model;
using DTO;

namespace WebAPI
{
    public static class OdontologoEndpoints
    {
        public static void MapOdontologoEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/odontologos").WithTags("Odontologos");

            group.MapGet("/", (IOdontologoService service) =>
                Results.Ok(service.GetAll()))
                .RequireAuthorization();

            group.MapGet("/{matricula}", (string matricula, IOdontologoService service) =>
            {
                var odontologo = service.GetByMatricula(matricula);
                return odontologo is null ? Results.NotFound() : Results.Ok(odontologo);
            })
            .RequireAuthorization();

            group.MapPost("/", (OdontologoDTO dto, IOdontologoService service) =>
            {
                service.Crear(dto);
                return Results.Created($"/api/odontologos/{dto.Matricula}", dto);
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));   // 👈 solo admin

            group.MapPut("/", (OdontologoDTO    dto, IOdontologoService service) =>
            {
                service.Actualizar(dto);
                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            group.MapDelete("/{matricula}", (string matricula, IOdontologoService service) =>
            {
                service.Eliminar(matricula);
                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));
        }
    }
}