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
        // Un email no puede repetirse entre administradores, odontólogos y pacientes.
        
        public static async Task<bool> EmailEnUsoAsync(
            AppDbContext db,
            string email,
            int? excluirNroPaciente = null,
            string? excluirMatricula = null)
        {
            var e = email.Trim().ToLower();

            if (await db.Administradores.AnyAsync(a => a.Email.ToLower() == e))
                return true;

            var odontologos = db.Odontologos.Where(o => o.Email.ToLower() == e);
            if (excluirMatricula != null)
                odontologos = odontologos.Where(o => o.Matricula != excluirMatricula);
            if (await odontologos.AnyAsync())
                return true;

            var pacientes = db.Pacientes.Where(p => p.Email.ToLower() == e);
            if (excluirNroPaciente.HasValue)
                pacientes = pacientes.Where(p => p.NroPaciente != excluirNroPaciente.Value);
            return await pacientes.AnyAsync();
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

            group.MapPut("/", async Task<IResult> (PacienteDTO dto, ClaimsPrincipal user, IPacienteService service, AppDbContext db) =>
            {
                if (!user.EsAdmin() && user.NroPaciente() != dto.NroPaciente)
                    return Results.Forbid();

                // El email no puede pertenecer a otra cuenta (la propia no cuenta)
                if (await EmailEnUsoAsync(db, dto.Email, excluirNroPaciente: dto.NroPaciente))
                    return Results.Conflict(new { error = "Ya existe una cuenta con ese email." });

                await service.ActualizarAsync(dto);
                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole("Administrador", "Paciente"));

            group.MapDelete("/{nroPaciente}", async (int nroPaciente, IPacienteService service) =>
            {
                try
                {
                    await service.EliminarAsync(nroPaciente);
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