using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Data;
using Domain.Model;

namespace Applications.Services
{
    public class AuthService : IAuthService
    {
        private readonly IOdontologoRepository _odontologoRepo;
        private readonly IPacienteRepository _pacienteRepo;
        private readonly IAdministradorRepository _administradorRepo;

        public AuthService(
            IOdontologoRepository odontologoRepo,
            IPacienteRepository pacienteRepo,
            IAdministradorRepository administradorRepo)
        {
            _odontologoRepo = odontologoRepo;
            _pacienteRepo = pacienteRepo;
            _administradorRepo = administradorRepo;
        }

        public async Task<Usuario?> ValidarCredencialesAsync(string email, string password)
        {
            Usuario? usuario =
                await _administradorRepo.GetByEmailAsync(email)
                ?? (Usuario?) (await _odontologoRepo.GetAllAsync()).FirstOrDefault(o => o.Email == email)
                ?? (await _pacienteRepo.GetAllAsync()).FirstOrDefault(p => p.Email == email);

            if (usuario == null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash))
                return null;

            return usuario;
        }
    }
}