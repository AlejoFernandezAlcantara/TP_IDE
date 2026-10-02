using System.Security.Claims;

namespace WebAPI
{
    public static class UserExtensions
    {
        public static bool EsAdmin(this ClaimsPrincipal u) => u.IsInRole("Administrador");

        public static string? Matricula(this ClaimsPrincipal u) => u.FindFirst("matricula")?.Value;

        public static int? NroPaciente(this ClaimsPrincipal u)
            => int.TryParse(u.FindFirst("nroPaciente")?.Value, out var n) ? n : null;

      
        public static bool PuedeGestionarTurnosDe(this ClaimsPrincipal u, string? matricula)
            => u.EsAdmin() ||
               (u.IsInRole("Odontologo") && !string.IsNullOrEmpty(matricula) && u.Matricula() == matricula);
    }
}