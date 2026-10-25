using Data;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace WebAPI
{
    public static class MutualEndpoints
    {
        public static void MapMutualEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/mutuales").WithTags("Mutuales");

            group.MapGet("/", async (AppDbContext db) =>
            {
                var mutuales = await db.Mutuales
                    .Select(m => new MutualDTO
                    {
                        Cuit = m.Cuit,
                        Nombre = m.Nombre
                    })
                    .ToListAsync();

                return Results.Ok(mutuales);
            })
            .RequireAuthorization();
        }
    }
}