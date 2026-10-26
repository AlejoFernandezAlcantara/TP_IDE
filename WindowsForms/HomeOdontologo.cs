using System;
using System.Windows.Forms;
using WindowsForms.Auth;

namespace WindowsForms
{
    public partial class HomeOdontologo : Form
    {
        public HomeOdontologo()
        {
            InitializeComponent();
            Load += HomeOdontologo_Load;
        }

        private async void HomeOdontologo_Load(object? sender, EventArgs e)
        {
            var authService = AuthServiceProvider.Instance;
            var nombre = await authService.GetNombreAsync();
            var rol = await authService.GetRolAsync();

            Text = "Clínica Odontológica - Odontólogo";
            lblBienvenida.Text = $"Bienvenido/a, {nombre} ({rol})";

            var btnTurnos = MenuHelper.Boton("Mis Turnos", (s, ev) => { using var f = new FormTurnos(); f.ShowDialog(); });
            var btnPacientes = MenuHelper.Boton("Registrar paciente", (s, ev) => { using var f = new FormAltaPaciente(); f.ShowDialog(); });
            var btnReservar = MenuHelper.Boton("Reservar turno a un paciente", (s, ev) => { using var f = new FormReservar(); f.ShowDialog(); });
            var btnReservas = MenuHelper.Boton("Mis Reservas", (s, ev) => { using var f = new FormReservas(); f.ShowDialog(); });
            var btnAtender = MenuHelper.Boton("Atender Reservas", (s, ev) => { using var f = new FormReservas(modoAtencion: true); f.ShowDialog(); });
            var btnReportes = MenuHelper.Boton("Reportes", (s, ev) => { using var f = new FormReportes(); f.ShowDialog(); });
            var btnSalir = MenuHelper.Boton("Cerrar Sesión", cerrarSesiónToolStripMenuItem_Click);

            MenuHelper.Apilar(this, lblBienvenida, btnTurnos, btnPacientes, btnReservar, btnReservas, btnAtender, btnReportes, btnSalir);
        }

        private async void cerrarSesiónToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            var authService = AuthServiceProvider.Instance;
            await authService.LogoutAsync();
            Close();
        }
    }
}
