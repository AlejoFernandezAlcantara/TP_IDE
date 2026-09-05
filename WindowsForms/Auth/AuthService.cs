using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using DTO;

namespace WindowsForms.Auth
{
    public class AuthService : IAuthService
    {
        private const string BaseUrl = "http://localhost:5232/api/";

        private static string? _currentToken;
        private static DateTime _tokenExpiration;
        private static string? _currentNombre;
        private static string? _currentRol;

        public event Action<bool>? AuthenticationStateChanged;

        public async Task<bool> IsAuthenticatedAsync()
        {
            return !string.IsNullOrEmpty(_currentToken) && DateTime.UtcNow < _tokenExpiration;
        }

        public async Task<string?> GetTokenAsync()
        {
            var autenticado = await IsAuthenticatedAsync();
            return autenticado ? _currentToken : null;
        }

        public async Task<string?> GetNombreAsync()
        {
            var autenticado = await IsAuthenticatedAsync();
            return autenticado ? _currentNombre : null;
        }

        public async Task<string?> GetRolAsync()
        {
            var autenticado = await IsAuthenticatedAsync();
            return autenticado ? _currentRol : null;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            using var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };

            var request = new LoginRequestDTO { Email = email, Password = password };
            var response = await client.PostAsJsonAsync("auth/login", request);

            if (!response.IsSuccessStatusCode)
                return false;

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
            if (loginResponse == null)
                return false;

            _currentToken = loginResponse.Token;
            _currentNombre = loginResponse.Nombre;
            _currentRol = loginResponse.Rol;

            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(_currentToken);
            _tokenExpiration = jwtToken.ValidTo;

            AuthenticationStateChanged?.Invoke(true);
            return true;
        }

        public async Task LogoutAsync()
        {
            _currentToken = null;
            _tokenExpiration = default;
            _currentNombre = null;
            _currentRol = null;
            AuthenticationStateChanged?.Invoke(false);
        }

        public async Task CheckTokenExpirationAsync()
        {
            if (!string.IsNullOrEmpty(_currentToken) && DateTime.UtcNow >= _tokenExpiration)
            {
                await LogoutAsync();
            }
        }
    }
}