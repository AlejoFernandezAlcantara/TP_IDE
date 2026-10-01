namespace Applications.Services
{
    
    public interface IBlazorAuthService : IAuthService
    {
        
        event Action<bool>? AuthenticationStateChanged;   
        Task<bool> IsAuthenticatedAsync();     
        Task<string?> GetTokenAsync();     
        Task<string?> GetNombreAsync();        
        Task<string?> GetRolAsync();        
        Task<bool> LoginAsync(string email, string password);
        Task LogoutAsync();   
        Task CheckTokenExpirationAsync();
    }
}