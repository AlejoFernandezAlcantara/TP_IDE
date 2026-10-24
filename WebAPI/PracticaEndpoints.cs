using Data;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace WebAPI
{
    public static class PracticaEndpoints
    {
        public static void MapPracticaEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/practicas").WithTags("Practicas");

            group.MapGet("/", async (AppDbContext db) =>
            {
                var practicas = await db.Practicas
                    .Select(p => new PracticaDTO
                    {
                        CodigoPractica = p.CodigoPractica,
                        Detalle = p.Detalle,
                        Precio = p.Precio
                    })
                    .ToListAsync();

                return Results.Ok(practicas);
            })
            .RequireAuthorization();
        }
    }
}