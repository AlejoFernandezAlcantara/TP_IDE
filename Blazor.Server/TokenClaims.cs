using System.IdentityModel.Tokens.Jwt;

namespace Blazor.Server
{
    /// <summary>
    /// Lee del JWT los datos que el WebAPI agrega al hacer login:
    /// "matricula" (odontólogos) y "nroPaciente" (pacientes).
    /// </summary>
    public static class TokenClaims
    {
        public static string? Matricula(string? token) => Obtener(token, "matricula");

        public static int? NroPaciente(string? token)
            => int.TryParse(Obtener(token, "nroPaciente"), out var n) ? n : null;

        private static string? Obtener(string? token, string tipo)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            try
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                return jwt.Claims.FirstOrDefault(c => c.Type == tipo)?.Value;
            }
            catch
            {
                return null;
            }
        }
    }
}
