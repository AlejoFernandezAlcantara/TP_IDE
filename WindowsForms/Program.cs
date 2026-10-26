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

            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            AuthServiceProvider.Register(new AuthService());
            var auth = AuthServiceProvider.Instance;

            
            while (true)
            {
                using (var loginForm = new LoginForm())
                {
                    if (loginForm.ShowDialog() != DialogResult.OK)
                        break;
                }

                Form? home = Sesion.Rol switch
                {
                    "Administrador" => new HomeAdmin(),
                    "Paciente" => new HomePaciente(),
                    "Odontologo" => new HomeOdontologo(),
                    _ => null
                };

                if (home == null)
                {
                    MessageBox.Show($"Rol no reconocido: {Sesion.Rol}");
                    auth.LogoutAsync().GetAwaiter().GetResult();
                    LimpiarSesion();
                    continue;
                }

                using (home)
                {
                    Application.Run(home);
                }

               
                bool sigueLogueado = auth.IsAuthenticatedAsync().GetAwaiter().GetResult();
                LimpiarSesion();
                if (sigueLogueado)
                    break;
            }
        }

        private static void LimpiarSesion()
        {
            Sesion.Email = null;
            Sesion.Rol = null;
            Sesion.Nombre = null;
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show(
                $"Ocurrió un error inesperado:\n\n{e.Exception.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
