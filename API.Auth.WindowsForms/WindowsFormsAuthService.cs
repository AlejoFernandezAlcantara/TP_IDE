using Domain.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using DTO;
using Applications.Services;
using System.Windows.Forms;

namespace API.Auth.WindowsForms
{
    public class WindowsFormsAuthService : IBlazorAuthService
    {
        private const string BaseUrl = "http://localhost:5232/api/";

        private string? _token;
        private DateTime _tokenExpiration;
        private string? _nombre;
        private string? _rol;

        public event Action<bool>? AuthenticationStateChanged;

        
        public string? UsuarioActual => _nombre;
        public string? RolActual => _rol;

        public Task<bool> IsAuthenticatedAsync()
        {
            var autenticado = !string.IsNullOrEmpty(_token) && DateTime.UtcNow < _tokenExpiration;
            return Task.FromResult(autenticado);
        }

        public async Task<string?> GetTokenAsync()
        {
            var autenticado = await IsAuthenticatedAsync();
            return autenticado ? _token : null;
        }

        public async Task<string?> GetNombreAsync()
        {
            var autenticado = await IsAuthenticatedAsync();
            return autenticado ? _nombre : null;
        }

        public async Task<string?> GetRolAsync()
        {
            var autenticado = await IsAuthenticatedAsync();
            return autenticado ? _rol : null;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            using var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };

            var request = new LoginRequestDTO { Email = email, Password = password };

            try
            {
                var response = await client.PostAsJsonAsync("auth/login", request);

                if (!response.IsSuccessStatusCode)
                {
                    
                    MessageBox.Show("Email o contraseña incorrectos", "Error de Login",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
                if (loginResponse == null)
                    return false;

                _token = loginResponse.Token;
                _nombre = loginResponse.Nombre;
                _rol = loginResponse.Rol;

                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(_token);
                _tokenExpiration = jwtToken.ValidTo;

                AuthenticationStateChanged?.Invoke(true);
                return true;
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("No se pudo conectar al servidor", "Error de Conexión",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public Task LogoutAsync()
        {
            _token = null;
            _tokenExpiration = default;
            _nombre = null;
            _rol = null;
            AuthenticationStateChanged?.Invoke(false);
            return Task.CompletedTask;
        }

        public async Task CheckTokenExpirationAsync()
        {
            if (!await IsAuthenticatedAsync())
                await LogoutAsync();
        }
        public Task<Usuario?> ValidarCredencialesAsync(string email, string password)
        {
            // Este método es más para compatibilidad con IAuthService.
            // La validación real se hace en LoginAsync(), contra la WebAPI.
            return Task.FromResult<Usuario?>(null);
        }
    }
}