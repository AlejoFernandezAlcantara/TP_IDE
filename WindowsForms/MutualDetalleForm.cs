using API.Clients;
using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Windows.Forms;

namespace WindowsForms
{
    
    public partial class MutualDetalleForm : Form
    {
        // Misma regla que Domain.Model.Mutual.NombreParticular
        private const string NombreParticular = "Particular";

        private readonly MutualApiClient _client;
        private readonly bool _isEdit;
        private readonly MutualDTO _mutual;

        public MutualDetalleForm(MutualApiClient client, MutualDTO? mutual)
        {
            _client = client;
            InitializeComponent();

            _isEdit = mutual != null;
            _mutual = mutual == null
                ? new MutualDTO()
                : new MutualDTO { Cuit = mutual.Cuit, Nombre = mutual.Nombre };

            Text = _isEdit ? "Editar mutual" : "Nueva mutual";
            btnGuardar.Text = _isEdit ? "Guardar cambios" : "Crear mutual";

            if (_isEdit)
            {
                txtCuit.Text = _mutual.Cuit.ToString();
                txtCuit.ReadOnly = true;
                txtCuit.TabStop = false;
                lblAyudaCuit.Text = "El CUIT identifica a la mutual y no se puede modificar.";
                txtNombre.Text = _mutual.Nombre;
                ActiveControl = txtNombre;
            }
            else
            {
                ActiveControl = txtCuit;
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Validar())
                return;

            try
            {
                SetGuardando(true);
                OcultarError();

                var dto = new MutualDTO
                {
                    Cuit = _isEdit ? _mutual.Cuit : int.Parse(txtCuit.Text.Trim()),
                    Nombre = txtNombre.Text.Trim()
                };

                var (ok, error) = _isEdit
                    ? await _client.ActualizarAsync(dto)
                    : await _client.CrearAsync(dto);

                if (!ok)
                {
                    MostrarError(error ?? "No se pudo guardar la mutual. Revisá los datos o volvé a iniciar sesión.");
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

            if (!_isEdit)
            {
                if (!int.TryParse(txtCuit.Text.Trim(), out var cuit) || cuit <= 0)
                {
                    errorProvider.SetError(txtCuit, "El CUIT debe ser un número positivo");
                    isValid = false;
                }
            }

            var nombre = txtNombre.Text.Trim();
            if (string.IsNullOrEmpty(nombre))
            {
                errorProvider.SetError(txtNombre, "El nombre es obligatorio");
                isValid = false;
            }
            else if (string.Equals(nombre, NombreParticular, StringComparison.OrdinalIgnoreCase))
            {
                errorProvider.SetError(txtNombre, $"\"{NombreParticular}\" es una cobertura reservada");
                isValid = false;
            }

            return isValid;
        }

        private void SetGuardando(bool guardando)
        {
            UseWaitCursor = guardando;
            btnGuardar.Enabled = !guardando;
            btnCancelar.Enabled = !guardando;
            txtNombre.Enabled = !guardando;
            if (!_isEdit)
                txtCuit.Enabled = !guardando;
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