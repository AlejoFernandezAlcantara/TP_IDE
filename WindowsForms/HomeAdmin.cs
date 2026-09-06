using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void lblBienvenida_Load(object sender, EventArgs e)
        {
            lblBienvenida.Text = $"Bienvenido/a, {Sesion.Nombre} ({Sesion.Rol})";
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
