using Applications.Services;
using Data;
using Domain.Model;
using DTO;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WebAPI
{
    public static class PacienteEndpoints
    {
        // CHEQUEAR QUE EL EMAIL NO PUEDA repetirse entre administradores, odontólogos y pacientes
        private static async Task<bool> EmailEnUsoAsync(AppDbContext db, string email)
        {
            var e = email.Trim().ToLower();

            if (await db.Administradores.AnyAsync(a => a.Email.ToLower() == e))
                return true;

            if (await db.Odontologos.AnyAsync(o => o.Email.ToLower() == e))
                return true;

            return await db.Pacientes.AnyAsync(p => p.Email.ToLower() == e);
        }

        public static void MapPacienteEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/pacientes").WithTags("Pacientes");

            group.MapGet("/", async (IPacienteService service) =>
                Results.Ok(await service.GetAllAsync()))
                .RequireAuthorization(policy => policy.RequireRole("Administrador", "Odontologo"));

            group.MapGet("/{nroPaciente}", async Task<IResult> (int nroPaciente, ClaimsPrincipal user, IPacienteService service) =>
            {
                if (user.IsInRole("Paciente") && user.NroPaciente() != nroPaciente)
                    return Results.Forbid();

                var paciente = await service.GetByNroPacienteAsync(nroPaciente);
                return paciente is null ? Results.NotFound() : Results.Ok(paciente);
            })
            .RequireAuthorization();

            // Alta de pacientes por Admin y odontologo
            group.MapPost("/", async Task<IResult> (PacienteDTO dto, IPacienteService service, AppDbContext db) =>
            {
                if (string.IsNullOrWhiteSpace(dto.Password))
                    return Results.BadRequest(new { error = "La contraseña es obligatoria." });

                if (await EmailEnUsoAsync(db, dto.Email))
                    return Results.Conflict(new { error = "Ya existe una cuenta con ese email." });

                try
                {
                    dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                    await service.CrearAsync(dto);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }

                // No devolvemos el hash en la respuesta
                dto.Password = string.Empty;
                return Results.Created($"/api/pacientes/{dto.NroPaciente}", dto);
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador", "Odontologo"));

            // Autoregistro del propio paciente: no requiere sesión (no hay token)
            group.MapPost("/registro", async Task<IResult> (PacienteDTO dto, IPacienteService service, AppDbContext db) =>
            {
                if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                    return Results.BadRequest(new { error = "La contraseña debe tener al menos 6 caracteres." });

                if (await EmailEnUsoAsync(db, dto.Email))
                    return Results.Conflict(new { error = "Ya existe una cuenta con ese email." });

                if (await db.Pacientes.AnyAsync(p => p.NroDni == dto.NroDni && p.TipoDni == dto.TipoDni))
                    return Results.Conflict(new { error = "Ya existe un paciente con ese documento." });

                try
                {
                    dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                    await service.CrearAsync(dto);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }

                dto.Password = string.Empty;
                return Results.Created($"/api/pacientes/{dto.NroPaciente}", dto);
            });

            group.MapPut("/", async Task<IResult> (PacienteDTO dto, ClaimsPrincipal user, IPacienteService service) =>
            {
                if (!user.EsAdmin() && user.NroPaciente() != dto.NroPaciente)
                    return Results.Forbid();

                await service.ActualizarAsync(dto);
                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador", "Paciente"));

            group.MapDelete("/{nroPaciente}", async (int nroPaciente, IPacienteService service) =>
            {
                await service.EliminarAsync(nroPaciente);
                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));
        }
    }
}