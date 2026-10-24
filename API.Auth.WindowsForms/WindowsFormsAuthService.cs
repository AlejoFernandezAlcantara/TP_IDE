using Applications.Services;
using Domain.Model;
using DTO;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
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
        private int? _nroPaciente;
        private string? _matricula;

        public event Action<bool>? AuthenticationStateChanged;

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

        public async Task<int?> GetNroPacienteAsync()
        {
            var autenticado = await IsAuthenticatedAsync();
            return autenticado ? _nroPaciente : null;
        }

        public async Task<string?> GetMatriculaAsync()
        {
            var autenticado = await IsAuthenticatedAsync();
            return autenticado ? _matricula : null;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            try
            {
                using var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };

                var request = new LoginRequestDTO { Email = email, Password = password };
                var response = await client.PostAsJsonAsync("auth/login", request);

                if (!response.IsSuccessStatusCode)
                    return false;

                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
                if (loginResponse == null)
                    return false;

                _token = loginResponse.Token;
                _nombre = loginResponse.Nombre;
                _rol = loginResponse.Rol;

                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(_token);
                _tokenExpiration = jwtToken.ValidTo;

                var nroClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "NroPaciente" || c.Type == "nroPaciente")?.Value;
                if (int.TryParse(nroClaim, out int nro))
                    _nroPaciente = nro;
                else
                    _nroPaciente = null;

                _matricula = jwtToken.Claims.FirstOrDefault(c => c.Type == "Matricula" || c.Type == "matricula")?.Value;

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
            _nroPaciente = null;
            _matricula = null;
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
            return Task.FromResult<Usuario?>(null);
        }
    }
}