using Applications.Services;
using DTO;

namespace WebAPI
{
    public static class PracticaEndpoints
    {
        public static void MapPracticaEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/practicas").WithTags("Practicas");

            group.MapGet("/", async (IPracticaService service) =>
                Results.Ok(await service.GetAllAsync()))
                .RequireAuthorization();

            group.MapGet("/{codigo}", async (int codigo, IPracticaService service) =>
            {
                var practica = await service.GetByCodigoAsync(codigo);
                return practica is null ? Results.NotFound() : Results.Ok(practica);
            })
            .RequireAuthorization();

            group.MapPost("/", (PracticaDTO dto, IPracticaService service) =>
                Intentar(async () =>
                {
                    await service.CrearAsync(dto);
                    return Results.Created("/api/practicas", dto);
                }))
                .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            group.MapPut("/", (PracticaDTO dto, IPracticaService service) =>
                Intentar(async () =>
                {
                    await service.ActualizarAsync(dto);
                    return Results.NoContent();
                }))
                .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            group.MapDelete("/{codigo}", (int codigo, IPracticaService service) =>
                Intentar(async () =>
                {
                    await service.EliminarAsync(codigo);
                    return Results.NoContent();
                }))
                .RequireAuthorization(policy => policy.RequireRole("Administrador"));
        }

        private static async Task<IResult> Intentar(Func<Task<IResult>> accion)
        {
            try { return await accion(); }
            catch (ArgumentException ex) { return Results.BadRequest(new { error = ex.Message }); }
            catch (InvalidOperationException ex) { return Results.Conflict(new { error = ex.Message }); }
        }
    }
}