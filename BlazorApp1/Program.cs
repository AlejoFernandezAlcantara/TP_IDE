using API.Auth.Blazor.Server;
using API.Clients;
using Applications.Services;
using Blazor.Server;
using Blazor.Server.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
.AddInteractiveServerComponents();

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
builder.Services.AddHttpClient<TurnoApiClient>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

builder.Services.AddRazorComponents()
.AddInteractiveServerComponents();
app.MapRazorComponents<App>()
.AddInteractiveServerRenderMode();

app.Run();