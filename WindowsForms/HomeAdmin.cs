using System.Windows.Forms;
using WindowsForms.Auth;

namespace WindowsForms
{
    public partial class HomeAdmin : Form
    {
        public HomeAdmin()
        {
            InitializeComponent();
        }

        private void HomeAdmin_Load(object sender, EventArgs e)
        {
            lblBienvenida.Text = $"Bienvenido/a, {Sesion.Nombre} ({Sesion.Rol})";

            CentrarControl(lblBienvenida);
            CentrarControl(ButtonPacientes);
            CentrarControl(buttonOdontologos);
            CentrarControl(buttonCerrarSesion);

        }
        private void HomeAdmin_Resize(object sender, EventArgs e)
        {
            CentrarControles();
        }

        private void CentrarControles()
        {
            CentrarControl(lblBienvenida);
            CentrarControl(ButtonPacientes);
            CentrarControl(buttonOdontologos);
            CentrarControl(buttonCerrarSesion);

            int totalAltura = buttonCerrarSesion.Bottom - lblBienvenida.Top;
            int topInicio = (this.ClientSize.Height - totalAltura) / 2;

            lblBienvenida.Top = topInicio;
            ButtonPacientes.Top = lblBienvenida.Bottom + 20;
            buttonOdontologos.Top = ButtonPacientes.Bottom + 5;
            buttonCerrarSesion.Top = buttonOdontologos.Bottom + 20;
        }
        private void CentrarControl(Control control)
        {
            control.Left = (this.ClientSize.Width - control.Width) / 2;
        }

        private void ButtonPacientes_Click(object sender, EventArgs e)
        {
            var form = new HomeAdminCRUDPaciente();
            form.ShowDialog();
        }

        private void buttonOdontologos_Click(object sender, EventArgs e)
        {
            var form = new HomeAdminCRUDOdontologo();
            form.ShowDialog();
        }

        private void buttonCerrarSesion_Click(object sender, EventArgs e)
        {
            var authService = AuthServiceProvider.Instance;
            _ = authService.LogoutAsync();

            Sesion.Nombre = null;
            Sesion.Rol = null;
            Sesion.Email = null;

            var login = new LoginForm();
            login.Show();
            this.Close();
        }
    }
}