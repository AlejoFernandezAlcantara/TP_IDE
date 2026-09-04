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

            group.MapGet("/", async (IOdontologoService service) =>
                Results.Ok(await service.GetAllAsync()))
                .RequireAuthorization();

            group.MapGet("/{matricula}", async (string matricula, IOdontologoService service) =>
            {
                var odontologo = await service.GetByMatriculaAsync(matricula);
                return odontologo is null ? Results.NotFound() : Results.Ok(odontologo);
            })
            .RequireAuthorization();

            group.MapPost("/", async (OdontologoDTO dto, IOdontologoService service) =>
            {
                await service.CrearAsync(dto);
                return Results.Created($"/api/odontologos/{dto.Matricula}", dto);
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            group.MapPut("/", async (OdontologoDTO dto, IOdontologoService service) =>
            {
                await service.ActualizarAsync(dto);
                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            group.MapDelete("/{matricula}", async (string matricula, IOdontologoService service) =>
            {
                await service.EliminarAsync(matricula);
                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));
        }
    }
}