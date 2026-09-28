nusing System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using DTO;

namespace BlazorApp1.Auth
{
	public class AuthService : IAuthService
	{
		private const string BaseUrl = "http://localhost:5232/api/";

		// OJO: a propósito NO son "static". Este servicio se registra como "Scoped"
		// en Program.cs, así que Blazor crea una instancia nueva por cada usuario
		// conectado (por cada "circuito"). Sin static, cada uno tiene su propia sesión.
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
	}
}