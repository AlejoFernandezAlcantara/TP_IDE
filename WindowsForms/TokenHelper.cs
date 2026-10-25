using System.IdentityModel.Tokens.Jwt;

namespace WindowsForms
{
    // Lee los datos del usuario logueado directamente del JWT
    public static class TokenHelper
    {
        public static string? Claim(string? token, string tipo)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            try
            {
                return new JwtSecurityTokenHandler().ReadJwtToken(token)
                    .Claims.FirstOrDefault(c => c.Type == tipo)?.Value;
            }
            catch
            {
                return null;
            }
        }

        public static string? Matricula(string? token) => Claim(token, "matricula");

        public static int? NroPaciente(string? token)
            => int.TryParse(Claim(token, "nroPaciente"), out var n) ? n : null;
    }
}
