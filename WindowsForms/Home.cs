using WindowsForms.Auth;

namespace WindowsForms
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private async void Home_Load(object sender, EventArgs e)
        {
            var authService = AuthServiceProvider.Instance;
            var nombre = await authService.GetNombreAsync();
            var rol = await authService.GetRolAsync();

            lblBienvenida.Text = $"Bienvenido/a, {nombre} ({rol})";
        }

        private async void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var authService = AuthServiceProvider.Instance;
            await authService.LogoutAsync();
            Close();
        }
    }
}