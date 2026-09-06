using Data;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

using WindowsForms.Auth;

namespace WindowsForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            AuthServiceProvider.Register(new AuthService());

            Task.Run(async () => await MainAsync()).GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var authService = AuthServiceProvider.Instance;

            using var loginForm = new LoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new Home());
            }
        }
    }
}