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

        public Usuario? ValidarCredenciales(string email, string password)
        {
            Usuario? usuario =
                _administradorRepo.GetByEmail(email)
                ?? (Usuario?)_odontologoRepo.GetAll().FirstOrDefault(o => o.Email == email)
                ?? _pacienteRepo.GetAll().FirstOrDefault(p => p.Email == email);

            if (usuario == null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash))
                return null;

            return usuario;
        }
    }
}