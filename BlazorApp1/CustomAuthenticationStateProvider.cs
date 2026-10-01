using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Applications.Services;

namespace Blazor.Server
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IBlazorAuthService _authService;

        public CustomAuthenticationStateProvider(IBlazorAuthService authService)
        {
            _authService = authService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var estaAutenticado = await _authService.IsAuthenticatedAsync();

            if (estaAutenticado)
            {
                var nombre = await _authService.GetNombreAsync() ?? "Usuario";
                var rol = await _authService.GetRolAsync() ?? "usuario";
                var token = await _authService.GetTokenAsync();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, nombre),
                    new Claim(ClaimTypes.Role, rol),
                    new Claim("Token", token ?? "")
                };

                var identity = new ClaimsIdentity(claims, "jwt");
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            else
            {
                var anonymousIdentity = new ClaimsIdentity();
                var anonymousUser = new ClaimsPrincipal(anonymousIdentity);
                return new AuthenticationState(anonymousUser);
            }
        }

        public async Task NotifyAuthenticationStateChangedAsync()
        {
            var authState = await GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }
    }
}