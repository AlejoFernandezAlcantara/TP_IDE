
using Domain.Model;


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
                Form? home = Sesion.Rol switch
                {
                    "Administrador" => new HomeAdmin(),
                    "Paciente" => new HomePaciente(),
                    "Odontologo" => new HomeOdontologo(),
                    _ => null
                };

                if (home != null)
                {
                    Application.Run(home);
                }
                else
                    MessageBox.Show($"Rol no reconocido: {Sesion.Rol}");
            }
        }
    }
}