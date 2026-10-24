using System;
using System.Threading.Tasks;

namespace Applications.Services
{
    public interface IBlazorAuthService : IAuthService
    {
        event Action<bool>? AuthenticationStateChanged;   
        Task<bool> IsAuthenticatedAsync();     
        Task<string?> GetTokenAsync();     
        Task<string?> GetNombreAsync();        
        Task<string?> GetRolAsync();        
        Task<int?> GetNroPacienteAsync();
        Task<string?> GetMatriculaAsync();
        Task<bool> LoginAsync(string email, string password);
        Task LogoutAsync();   
        Task CheckTokenExpirationAsync();
    }
}