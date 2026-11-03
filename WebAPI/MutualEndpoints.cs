using Applications.Services;
using DTO;
//validan entrada y reglas http, llaman al servicio y traducen el resultado a codigo http
namespace WebAPI
{
    public static class MutualEndpoints
    {
        public static void MapMutualEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/mutuales").WithTags("Mutuales");

            // Listadoo
            group.MapGet("/", async (IMutualService service) =>
            {
                var mutuales = await service.GetAllAsync();
                return Results.Ok(mutuales);
            })
            .RequireAuthorization();

            // Obtener mutual por CUIT
            group.MapGet("/{cuit:int}", async Task<IResult> (int cuit, IMutualService service) =>
            {
                var mutual = await service.GetByCuitAsync(cuit);
                return mutual == null ? Results.NotFound() : Results.Ok(mutual);
            })
            .RequireAuthorization();

            // Alta ---> solo Administrador
            group.MapPost("/", async Task<IResult> (MutualDTO dto, IMutualService service) =>
            {
                try
                {
                    await service.CrearAsync(dto);
                    return Results.Created($"/api/mutuales/{dto.Cuit}", dto);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Conflict(new { error = ex.Message });
                }
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            
            group.MapPut("/", async Task<IResult> (MutualDTO dto, IMutualService service) =>
            {
                try
                {
                    await service.ActualizarAsync(dto);
                    return Results.NoContent();
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Conflict(new { error = ex.Message });
                }
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            // no se permite si la mutual tiene reservas asociadas..
            group.MapDelete("/{cuit:int}", async Task<IResult> (int cuit, IMutualService service) =>
            {
                try
                {
                    await service.EliminarAsync(cuit);
                    return Results.NoContent();
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Conflict(new { error = ex.Message });
                }
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));
        }
    }
}