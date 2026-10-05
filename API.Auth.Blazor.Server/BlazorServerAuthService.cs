using Applications.Services;
using Domain.Model;
using DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;

namespace API.Auth.Blazor.Server
{
	public class BlazorServerAuthService : IBlazorAuthService
    {
		private const string BaseUrl = "http://localhost:5232/api/";

		private string? _token;
		private DateTime _tokenExpiration;
		private string? _nombre;
		private string? _rol;

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

			_token = loginResponse.Token;
			_nombre = loginResponse.Nombre;
			_rol = loginResponse.Rol;

			var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(_token);
			_tokenExpiration = jwtToken.ValidTo;

			AuthenticationStateChanged?.Invoke(true);
			return true;
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
        public async Task<Usuario?> ValidarCredencialesAsync(string email, string password)
        {
            return null;
        }

    }
}