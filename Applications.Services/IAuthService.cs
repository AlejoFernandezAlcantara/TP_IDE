using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;

namespace Applications.Services
{
    public interface IAuthService
    {
        Usuario? ValidarCredenciales(string email, string password);
    }
}