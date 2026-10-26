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

            // Login -> Home del rol -> (Cerrar sesión) -> Login otra vez.
            // La aplicación termina si se cierra el login, o si se cierra el Home con la X sin cerrar sesión.
            // Todo corre en el hilo principal (STA), sin Task.Run.
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

                // "Cerrar sesión" borra el token: en ese caso se vuelve al login.
                // Si el token sigue vigente, el usuario cerró la ventana con la X: se sale.
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
