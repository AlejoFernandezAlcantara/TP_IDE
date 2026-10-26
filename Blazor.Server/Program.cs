using API.Auth.Blazor.Server;
using API.Clients;
using Applications.Services;
using Blazor.Server;
using Blazor.Server.Components;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Componentes Blazor (un solo registro)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options => options.DetailedErrors = true); // DIAGNÓSTICO: mostrar la excepción real en consola (PARA CHUEQUEAR ERROR SE HIZO)

// Autenticación
builder.Services.AddScoped<IBlazorAuthService, BlazorServerAuthService>();
builder.Services.AddScoped<IAuthService>(sp => sp.GetRequiredService<IBlazorAuthService>());
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();

// API Clients
builder.Services.AddHttpClient<AuthApiClient>();
builder.Services.AddHttpClient<OdontologoApiClient>();
builder.Services.AddHttpClient<PacienteApiClient>();
builder.Services.AddHttpClient<ReservaApiClient>();
builder.Services.AddHttpClient<PracticaApiClient>();
builder.Services.AddHttpClient<MutualApiClient>();
builder.Services.AddHttpClient<TurnoApiClient>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();