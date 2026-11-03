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
    // Listado de prácticas para el Administrador: búsqueda, alta, edición y baja.
    public partial class PracticasForm : Form
    {
        private readonly PracticaApiClient _client;
        private List<PracticaDTO> _practicas = new();
        private bool _ocupado = false;

        public PracticasForm()
        {
            _client = new PracticaApiClient(new System.Net.Http.HttpClient());
            InitializeComponent();
            dgvPracticas.AutoGenerateColumns = false;
        }

        private PracticaDTO? Seleccionada => dgvPracticas.CurrentRow?.DataBoundItem as PracticaDTO;

        
        private async Task SetTokenAsync()
        {
            _client.SetToken(await AuthServiceProvider.Instance.GetTokenAsync());
        }

        private async void PracticasForm_Load(object sender, EventArgs e)
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
                    _practicas = new();
                    MessageBox.Show(this, "No se pudieron cargar las prácticas. Verificá que el WebAPI esté corriendo y que tu sesión siga activa.",
                        "Prácticas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    _practicas = resultado.OrderBy(p => p.Detalle).ToList();
                }

                AplicarFiltro();
            }
            catch (Exception ex)
            {
                lblEstado.Text = string.Empty;
                MessageBox.Show(this, ex.Message, "Prácticas", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                ? _practicas
                : _practicas.Where(p =>
                        p.Detalle.Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                        p.CodigoPractica.ToString().Contains(filtro))
                    .ToList();

            dgvPracticas.DataSource = null;
            dgvPracticas.DataSource = lista;

            if (lista.Count == 0)
                lblEstado.Text = string.IsNullOrEmpty(filtro)
                    ? "Todavía no hay prácticas cargadas."
                    : "No hay prácticas que coincidan con la búsqueda.";
            else
                lblEstado.Text = $"{lista.Count} práctica(s)";

            ActualizarBotones();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        private void dgvPracticas_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarBotones();
        }

        private async void dgvPracticas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !_ocupado)
                await EditarSeleccionadaAsync();
        }

        private async void btnNueva_Click(object sender, EventArgs e)
        {
            await SetTokenAsync();
            using var form = new PracticaDetalleForm(_client, null);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                await CargarAsync();
                lblEstado.Text = "Práctica guardada correctamente.";
            }
        }

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            await EditarSeleccionadaAsync();
        }

        private async Task EditarSeleccionadaAsync()
        {
            var practica = Seleccionada;
            if (practica == null)
                return;

            await SetTokenAsync();
            using var form = new PracticaDetalleForm(_client, practica);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                await CargarAsync();
                lblEstado.Text = "Práctica guardada correctamente.";
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            var practica = Seleccionada;
            if (practica == null)
                return;

            var confirmacion = MessageBox.Show(this,
                $"¿Eliminar la práctica \"{practica.Detalle}\"? Esta acción no se puede deshacer.",
                "Eliminar práctica", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (confirmacion != DialogResult.Yes)
                return;

            try
            {
                SetOcupado(true);

                await SetTokenAsync();
                var (ok, error) = await _client.EliminarAsync(practica.CodigoPractica);
                if (!ok)
                {
                    MessageBox.Show(this, error ?? "No se pudo eliminar la práctica.",
                        "Eliminar práctica", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Eliminar práctica", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                SetOcupado(false);
            }

            await CargarAsync();
            lblEstado.Text = $"Práctica \"{practica.Detalle}\" eliminada.";
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