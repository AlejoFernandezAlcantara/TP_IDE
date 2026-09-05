using BCrypt.Net;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class AdministradorRepository : IAdministradorRepository
    {
        private readonly AppDbContext _context;

        public AdministradorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Administrador?> GetByEmailAsync(string email)
        {
            return await _context.Administradores
                .FirstOrDefaultAsync(a => a.Email == email);
        }
    }
}