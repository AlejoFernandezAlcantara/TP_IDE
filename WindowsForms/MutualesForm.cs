using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using API.Clients;
using DTO;
using WindowsForms.Auth;

namespace WindowsForms
{
    // Listado de mutuales para el Administrador: búsqueda, alta, edición y baja.
    public partial class MutualesForm : Form
    {
        private readonly MutualApiClient _client;
        private List<MutualDTO> _mutuales = new();
        private bool _ocupado = false;

        public MutualesForm()
        {
            _client = new MutualApiClient(new System.Net.Http.HttpClient());
            InitializeComponent();
            dgvMutuales.AutoGenerateColumns = false;
        }

        
        private async Task SetTokenAsync()
        {
            _client.SetToken(await AuthServiceProvider.Instance.GetTokenAsync());
        }

        private MutualDTO? Seleccionada => dgvMutuales.CurrentRow?.DataBoundItem as MutualDTO;

        private async void MutualesForm_Load(object sender, EventArgs e)
        {
            await CargarAsync();
        }

        private async Task CargarAsync()
        {
            try
            {
                SetOcupado(true);
                lblEstado.Text = "Cargando...";

                await SetTokenAsync();
                var resultado = await _client.GetAllAsync();
                if (resultado == null)
                {
                    _mutuales = new();
                    MessageBox.Show(this, "No se pudieron cargar las mutuales. Volvé a iniciar sesión.",
                        "Mutuales", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    _mutuales = resultado.OrderBy(m => m.Nombre).ToList();
                }

                AplicarFiltro();
            }
            catch (Exception ex)
            {
                lblEstado.Text = string.Empty;
                MessageBox.Show(this, ex.Message, "Mutuales", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetOcupado(false);
            }
        }

        private void AplicarFiltro()
        {
            var filtro = txtBuscar.Text.Trim();

            var lista = string.IsNullOrEmpty(filtro)
                ? _mutuales
                : _mutuales.Where(m =>
                        m.Nombre.Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                        m.Cuit.ToString().Contains(filtro))
                    .ToList();

            dgvMutuales.DataSource = null;
            dgvMutuales.DataSource = lista;

            if (lista.Count == 0)
                lblEstado.Text = string.IsNullOrEmpty(filtro)
                    ? "No hay mutuales cargadas."
                    : "No hay mutuales que coincidan con la búsqueda.";
            else
                lblEstado.Text = $"{lista.Count} mutual(es)";

            ActualizarBotones();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        private void dgvMutuales_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarBotones();
        }

        private async void dgvMutuales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !_ocupado)
                await EditarSeleccionadaAsync();
        }

        private async void btnNueva_Click(object sender, EventArgs e)
        {
            await SetTokenAsync();
            using var form = new MutualDetalleForm(_client, null);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                await CargarAsync();
                lblEstado.Text = "Mutual guardada correctamente.";
            }
        }

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            await EditarSeleccionadaAsync();
        }

        private async Task EditarSeleccionadaAsync()
        {
            var mutual = Seleccionada;
            if (mutual == null)
                return;

            await SetTokenAsync();
            using var form = new MutualDetalleForm(_client, mutual);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                await CargarAsync();
                lblEstado.Text = "Mutual guardada correctamente.";
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            var mutual = Seleccionada;
            if (mutual == null)
                return;

            var confirmacion = MessageBox.Show(this,
                $"¿Seguro que querés eliminar la mutual \"{mutual.Nombre}\" (CUIT {mutual.Cuit})?",
                "Eliminar mutual", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (confirmacion != DialogResult.Yes)
                return;

            try
            {
                SetOcupado(true);

                await SetTokenAsync();
                var (ok, error) = await _client.EliminarAsync(mutual.Cuit);
                if (!ok)
                {
                    MessageBox.Show(this, error ?? "No se pudo eliminar la mutual.",
                        "Eliminar mutual", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Eliminar mutual", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                SetOcupado(false);
            }

            await CargarAsync();
            lblEstado.Text = $"Mutual \"{mutual.Nombre}\" eliminada.";
        }

        private void SetOcupado(bool ocupado)
        {
            _ocupado = ocupado;
            UseWaitCursor = ocupado;
            txtBuscar.Enabled = !ocupado;
            btnNueva.Enabled = !ocupado;
            ActualizarBotones();
        }

        private void ActualizarBotones()
        {
            var haySeleccion = Seleccionada != null;
            btnEditar.Enabled = !_ocupado && haySeleccion;
            btnEliminar.Enabled = !_ocupado && haySeleccion;
        }
    }
}