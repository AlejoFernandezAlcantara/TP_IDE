using API.Clients;
using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Windows.Forms;

namespace WindowsForms
{
    
    public partial class PracticaDetalleForm : Form
    {
        private readonly PracticaApiClient _client;
        private readonly bool _isEdit;
        private readonly PracticaDTO _practica;

        public PracticaDetalleForm(PracticaApiClient client, PracticaDTO? practica)
        {
            _client = client;
            InitializeComponent();

            _isEdit = practica != null;
            _practica = practica == null
                ? new PracticaDTO()
                : new PracticaDTO
                {
                    CodigoPractica = practica.CodigoPractica,
                    Detalle = practica.Detalle,
                    Precio = practica.Precio
                };

            Text = _isEdit ? "Editar práctica" : "Nueva práctica";
            btnGuardar.Text = _isEdit ? "Guardar cambios" : "Crear práctica";

            if (_isEdit)
            {
                txtCodigo.Text = _practica.CodigoPractica.ToString();
                txtDetalle.Text = _practica.Detalle;
                nudPrecio.Value = Math.Clamp((decimal)_practica.Precio, nudPrecio.Minimum, nudPrecio.Maximum);
            }
            else
            {
                // En el alta no hay código todavía
                lblCodigo.Visible = false;
                txtCodigo.Visible = false;
            }

            ActiveControl = txtDetalle;
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Validar())
                return;

            try
            {
                SetGuardando(true);
                OcultarError();

                var dto = new PracticaDTO
                {
                    CodigoPractica = _practica.CodigoPractica,
                    Detalle = txtDetalle.Text.Trim(),
                    Precio = (float)nudPrecio.Value
                };

                var (ok, error) = _isEdit
                    ? await _client.ActualizarAsync(dto)
                    : await _client.CrearAsync(dto);

                if (!ok)
                {
                    MostrarError(error ?? "No se pudo guardar la práctica. Revisá los datos o volvé a iniciar sesión.");
                    return;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
            finally
            {
                SetGuardando(false);
            }
        }

        private bool Validar()
        {
            errorProvider.Clear();
            OcultarError();
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtDetalle.Text))
            {
                errorProvider.SetError(txtDetalle, "El detalle de la práctica es obligatorio");
                isValid = false;
            }

            if (nudPrecio.Value < 0)
            {
                errorProvider.SetError(nudPrecio, "El precio no puede ser negativo");
                isValid = false;
            }

            return isValid;
        }

        private void SetGuardando(bool guardando)
        {
            UseWaitCursor = guardando;
            btnGuardar.Enabled = !guardando;
            btnCancelar.Enabled = !guardando;
            txtDetalle.Enabled = !guardando;
            nudPrecio.Enabled = !guardando;
        }

        private void MostrarError(string mensaje)
        {
            lblError.Text = mensaje;
            lblError.Visible = true;
        }

        private void OcultarError()
        {
            lblError.Text = string.Empty;
            lblError.Visible = false;
        }
    }
}