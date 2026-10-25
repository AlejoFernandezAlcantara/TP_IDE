using System;
using System.Windows.Forms;
using WindowsForms.Auth;

namespace WindowsForms
{
    public partial class HomePaciente : Form
    {
        public HomePaciente()
        {
            InitializeComponent();
            Load += HomePaciente_Load;
        }

        private async void HomePaciente_Load(object? sender, EventArgs e)
        {
            var authService = AuthServiceProvider.Instance;
            var nombre = await authService.GetNombreAsync();
            var rol = await authService.GetRolAsync();

            Text = "Clínica Odontológica - Paciente";
            lblBienvenida.Text = $"Bienvenido/a, {nombre} ({rol})";

            var btnReservar = MenuHelper.Boton("Reservar turno", (s, ev) => { using var f = new FormReservar(); f.ShowDialog(); });
            var btnMis = MenuHelper.Boton("Mis Reservas", (s, ev) => { using var f = new FormMisReservas(); f.ShowDialog(); });
            var btnSalir = MenuHelper.Boton("Cerrar Sesión", async (s, ev) =>
            {
                await authService.LogoutAsync();
                Close();
            });

            MenuHelper.Apilar(this, lblBienvenida, btnReservar, btnMis, btnSalir);
        }
    }
}
