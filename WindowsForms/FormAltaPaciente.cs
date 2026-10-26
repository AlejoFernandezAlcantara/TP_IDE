using System;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Domain.Model;
using DTO;
using WindowsForms.Auth;

namespace WindowsForms
{
    // Alta de paciente hecha por el odontólogo (autenticado con token).
    // Usa POST /api/pacientes. El autoregistro público es RegistroPacienteForm.
    public class FormAltaPaciente : Form
    {
        private readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5232/api/")
        };

        private TextBox txtNombre = null!;
        private TextBox txtApellido = null!;
        private ComboBox cmbTipoDni = null!;
        private TextBox txtNroDni = null!;
        private TextBox txtTelefono = null!;
        private TextBox txtDireccion = null!;
        private TextBox txtEmail = null!;
        private TextBox txtPassword = null!;
        private Label lblError = null!;
        private Button btnGuardar = null!;
        private Button btnCancelar = null!;

        public FormAltaPaciente()
        {
            Inicializar();
        }

        private void Inicializar()
        {
            Text = "Registrar paciente";
            ClientSize = new Size(480, 500);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Font = new Font("Segoe UI", 9f);
            BackColor = Color.FromArgb(248, 250, 252);

            // ---------- Encabezado ----------
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.White };
            pnlHeader.Controls.Add(new Label
            {
                Text = "Registrar paciente",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 10)
            });
            pnlHeader.Controls.Add(new Label
            {
                Text = "El paciente podrá iniciar sesión con el email y la contraseña indicados.",
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(21, 38)
            });

            // ---------- Campos ----------
            var tabla = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 8, 20, 0),
                ColumnCount = 2
            };
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            txtNombre = new TextBox();
            txtApellido = new TextBox();
            cmbTipoDni = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTipoDni.Items.AddRange(new object[] { "DNI", "Pasaporte" });
            cmbTipoDni.SelectedIndex = 0;
            txtNroDni = new TextBox();
            txtTelefono = new TextBox();
            txtDireccion = new TextBox();
            txtEmail = new TextBox();
            txtPassword = new TextBox { UseSystemPasswordChar = true };

            AgregarFila(tabla, "Nombre *", txtNombre);
            AgregarFila(tabla, "Apellido *", txtApellido);
            AgregarFila(tabla, "Tipo de documento *", cmbTipoDni);
            AgregarFila(tabla, "Nº de documento *", txtNroDni);
            AgregarFila(tabla, "Teléfono *", txtTelefono);
            AgregarFila(tabla, "Dirección *", txtDireccion);
            AgregarFila(tabla, "Email *", txtEmail);
            AgregarFila(tabla, "Contraseña inicial *", txtPassword);

            // ---------- Pie: ayuda, error y botones ----------
            var pnlPie = new Panel { Dock = DockStyle.Bottom, Height = 120 };

            var lblAyuda = new Label
            {
                Text = "La contraseña debe tener al menos 6 caracteres.",
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(20, 4)
            };

            lblError = new Label
            {
                ForeColor = Color.Red,
                AutoSize = false,
                Size = new Size(440, 36),
                Location = new Point(20, 28)
            };

            btnGuardar = new Button
            {
                Text = "Registrar paciente",
                Size = new Size(150, 34),
                Location = new Point(310, 72),
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            btnGuardar.Click += async (s, e) => await GuardarAsync();

            btnCancelar = new Button
            {
                Text = "Cancelar",
                Size = new Size(100, 34),
                Location = new Point(200, 72),
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };

            pnlPie.Controls.AddRange(new Control[] { lblAyuda, lblError, btnGuardar, btnCancelar });

            // El control con Dock.Fill se agrega primero
            Controls.Add(tabla);
            Controls.Add(pnlPie);
            Controls.Add(pnlHeader);

            AcceptButton = btnGuardar;
            CancelButton = btnCancelar;
        }

        private static void AgregarFila(TableLayoutPanel tabla, string texto, Control control)
        {
            int fila = tabla.RowCount;
            tabla.RowCount = fila + 1;
            tabla.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            tabla.Controls.Add(new Label
            {
                Text = texto,
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(0, 8, 8, 8)
            }, 0, fila);

            control.Dock = DockStyle.Fill;
            control.Margin = new Padding(0, 4, 0, 4);
            tabla.Controls.Add(control, 1, fila);
        }

        private string? Validar()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtDireccion.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
                return "Completá todos los campos obligatorios.";

            if (!int.TryParse(txtNroDni.Text.Trim(), out var dni) || dni <= 0)
                return "Ingresá un número de documento válido.";

            if (!txtEmail.Text.Contains('@'))
                return "Ingresá un email válido.";

            if (txtPassword.Text.Length < 6)
                return "La contraseña debe tener al menos 6 caracteres.";

            return null;
        }

        private async Task GuardarAsync()
        {
            lblError.Text = string.Empty;

            var problema = Validar();
            if (problema is not null)
            {
                lblError.Text = problema;
                return;
            }

            btnGuardar.Enabled = false;
            btnGuardar.Text = "Guardando...";

            try
            {
                var token = await AuthServiceProvider.Instance.GetTokenAsync();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var dto = new PacienteDTO
                {
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Direccion = txtDireccion.Text.Trim(),
                    Telefono = txtTelefono.Text.Trim(),
                    NroDni = int.Parse(txtNroDni.Text.Trim()),
                    TipoDni = (tiposEnumerados)cmbTipoDni.SelectedIndex,
                    Email = txtEmail.Text.Trim(),
                    Password = txtPassword.Text
                };

                var response = await _client.PostAsJsonAsync("pacientes", dto);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Paciente registrado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                    return;
                }

                lblError.Text = await ApiErrorHelper.LeerErrorAsync(response);
            }
            catch (HttpRequestException)
            {
                lblError.Text = "No se pudo conectar con el servidor. Verificá que la API esté funcionando.";
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
            finally
            {
                if (!IsDisposed)
                {
                    btnGuardar.Enabled = true;
                    btnGuardar.Text = "Registrar paciente";
                }
            }
        }
    }
}
