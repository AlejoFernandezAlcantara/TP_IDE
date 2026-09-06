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
    public partial class HomePaciente : Form
    {
        public HomePaciente()
        {
            InitializeComponent();
        }
        private async void HomePaciente_Load(object sender, EventArgs e)
        {
            var authService = AuthServiceProvider.Instance;
            var nombre = await authService.GetNombreAsync();
            var rol = await authService.GetRolAsync();

            lblBienvenida.Text = $"Bienvenido/a 2, {nombre} ({rol})";
        }
        private async void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var authService = AuthServiceProvider.Instance;
            await authService.LogoutAsync();
            Close();
        }
    }
}
