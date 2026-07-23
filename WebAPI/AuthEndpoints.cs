
    using Applications.Services;
    using DTO;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Domain.Model;

namespace WebAPI
    {
        public static class AuthEndpoints
        {
            public static void MapAuthEndpoints(this WebApplication app)
            {
                var group = app.MapGroup("/api/auth").WithTags("Auth");

                group.MapPost("/login", (LoginRequestDTO request, IAuthService authService, IConfiguration config) =>
                {
                    var usuario = authService.ValidarCredenciales(request.Email, request.Password);
                    if (usuario == null)
                        return Results.Unauthorized();

                    var jwtKey = config["Jwt:Key"]!;
                    var claims = new[]
                    {
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.Rol)
                };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: config["Jwt:Issuer"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(double.Parse(config["Jwt:ExpiresMinutes"]!)),
                        signingCredentials: creds);

                    var response = new LoginResponseDTO
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Nombre = usuario.Rol switch
                        {
                            "Administrador" => ((Administrador)usuario).Nombre,
                            "Odontologo" => ((Odontologo)usuario).Nombre,
                            "Paciente" => ((Paciente)usuario).Nombre,
                            _ => ""
                        },
                        Rol = usuario.Rol
                    };

                    return Results.Ok(response);
                });
            }
        }
    }