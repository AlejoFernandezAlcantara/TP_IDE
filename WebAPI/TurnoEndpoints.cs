using Applications.Services;
using DTO;

namespace WebAPI
{
    public static class TurnoEndpoints
    {
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

            group.MapPost("/", async (TurnoDTO dto, ITurnoService service) =>
            {
                await service.CrearAsync(dto);
                return Results.Created("/api/turnos", dto);
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador", "Odontologo"));

            group.MapPut("/", async (TurnoDTO dto, ITurnoService service) =>
            {
                await service.ActualizarAsync(dto);
                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador", "Odontologo"));

            group.MapDelete("/{codigo}", async (int codigo, ITurnoService service) =>
            {
                await service.EliminarAsync(codigo);
                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));
        }
    }
}