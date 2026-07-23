using Applications.Services;
using Domain.Model;

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

            group.MapPost("/", (Odontologo odontologo, IOdontologoService service) =>
            {
                service.Crear(odontologo);
                return Results.Created($"/api/odontologos/{odontologo.Matricula}", odontologo);
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));   // 👈 solo admin

            group.MapPut("/", (Odontologo odontologo, IOdontologoService service) =>
            {
                service.Actualizar(odontologo);
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