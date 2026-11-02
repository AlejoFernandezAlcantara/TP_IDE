using Applications.Services;
using Data;
using Domain.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
//swagger configuration to include JWT authentication
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingrese el token JWT (sin la palabra 'Bearer')"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
//hasta aca swagger configuration to include JWT authentication

const string BlazorCorsPolicy = "BlazorCorsPolicy"; //controla el acceso a la API desde el front-end Blazor
builder.Services.AddCors(options =>
{
    options.AddPolicy(BlazorCorsPolicy, policy =>
    {
        policy.WithOrigins("https://localhost:7282", "http://localhost:5114")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
//inyeccion de dependencias para los servicios y repositorios
// Repositorios
// Paciente, Odontologo, Administrador, Reserva y Turno ya usan EF Core 
builder.Services.AddScoped<AppDbContext>(sp =>
    new AppDbContext(builder.Configuration.GetConnectionString("Default")!));

builder.Services.AddScoped<IOdontologoRepository, OdontologoRepository>();
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<IAdministradorRepository, AdministradorRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<ITurnoRepository, TurnoRepository>();

// Servicios
builder.Services.AddScoped<IOdontologoService, OdontologoService>();
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddScoped<ITurnoService, TurnoService>();

// JWT , configuracion de autenticación y autorización
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });
builder.Services.AddAuthorization(); //autoriza a usuarios autenticados a acceder a los endpoints de la API

var app = builder.Build();
//arranca la base de datos y la llena con datos iniciales(dbseeder)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    DbSeeder.Seed(context);
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(BlazorCorsPolicy);
app.UseAuthentication(); 
app.UseAuthorization();
//endpoints mapping
app.MapOdontologoEndpoints();
app.MapPacienteEndpoints();
app.MapReservaEndpoints();
app.MapPracticaEndpoints();
app.MapMutualEndpoints();
app.MapTurnoEndpoints();
app.MapAuthEndpoints();

app.Run();
