using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using API.Clients;
using Domain.Model;
using DTO;

namespace WindowsForms
{
    // Equivalente WinForms de RegistroPaciente.razor: autoregistro público de un paciente.
    // No usa token: el endpoint POST /api/pacientes/registro es anónimo.
    public class RegistroPacienteForm : Form
    {
        private readonly PacienteApiClient _pacienteClient;

        private TextBox txtNombre = null!;
        private TextBox txtApellido = null!;
        private ComboBox cmbTipoDni = null!;
        private TextBox txtNroDni = null!;
        private TextBox txtTelefono = null!;
        private TextBox txtDireccion = null!;
        private TextBox txtEmail = null!;
        private TextBox txtPassword = null!;
        private TextBox txtConfirmar = null!;
        private Label lblError = null!;
        private Button btnCrear = null!;
        private Button btnCancelar = null!;

        // Lo usa el LoginForm para dejar el email ya escrito después de registrarse
        public string? EmailRegistrado { get; private set; }

        public RegistroPacienteForm()
        {
            _pacienteClient = new PacienteApiClient(new HttpClient());
            Inicializar();
        }

        private void Inicializar()
        {
            Text = "Crear cuenta de paciente";
            ClientSize = new Size(480, 540);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Font = new Font("Segoe UI", 9f);
            BackColor = Color.FromArgb(248, 250, 252);

            // ---------- Encabezado ----------
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 75, BackColor = Color.White };
            pnlHeader.Controls.Add(new Label
            {
                Text = "Crear cuenta de paciente",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 12)
            });
            pnlHeader.Controls.Add(new Label
            {
                Text = "Completá tus datos para poder reservar turnos.",
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(21, 44)
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
            txtConfirmar = new TextBox { UseSystemPasswordChar = true };

            var filas = new (string Texto, Control Control)[]
            {
                ("Nombre *", txtNombre),
                ("Apellido *", txtApellido),
                ("Tipo de documento *", cmbTipoDni),
                ("Nº de documento *", txtNroDni),
                ("Teléfono *", txtTelefono),
                ("Dirección *", txtDireccion),
                ("Email *", txtEmail),
                ("Contraseña *", txtPassword),
                ("Repetir contraseña *", txtConfirmar)
            };

            // Se fija la cantidad de filas una sola vez y después se ubica cada control en su fila
            tabla.RowStyles.Clear();
            for (int i = 0; i < filas.Length; i++)
                tabla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tabla.RowCount = filas.Length;

            for (int i = 0; i < filas.Length; i++)
                AgregarFila(tabla, i, filas[i].Texto, filas[i].Control);

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

            btnCrear = new Button
            {
                Text = "Crear cuenta",
                Size = new Size(150, 34),
                Location = new Point(310, 72),
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            btnCrear.Click += async (s, e) => await RegistrarAsync();

            btnCancelar = new Button
            {
                Text = "Cancelar",
                Size = new Size(100, 34),
                Location = new Point(200, 72),
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };

            pnlPie.Controls.AddRange(new Control[] { lblAyuda, lblError, btnCrear, btnCancelar });

            // El control con Dock.Fill se agrega primero (igual que en FormTurnos)
            Controls.Add(tabla);
            Controls.Add(pnlPie);
            Controls.Add(pnlHeader);

            AcceptButton = btnCrear;
            CancelButton = btnCancelar;
        }

        private static void AgregarFila(TableLayoutPanel tabla, int fila, string texto, Control control)
        {
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

        // Mismas reglas que Validar() de RegistroPaciente.razor
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

            if (txtPassword.Text != txtConfirmar.Text)
                return "Las contraseñas no coinciden.";

            return null;
        }

        private async Task RegistrarAsync()
        {
            lblError.Text = string.Empty;

            var problema = Validar();
            if (problema is not null)
            {
                lblError.Text = problema;
                return;
            }

            btnCrear.Enabled = false;
            btnCrear.Text = "Creando cuenta...";

            try
            {
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

                var (ok, error) = await _pacienteClient.RegistrarAsync(dto);

                if (ok)
                {
                    EmailRegistrado = dto.Email;
                    MessageBox.Show("¡Cuenta creada! Ya podés iniciar sesión con tu email y contraseña.",
                        "Registro exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    lblError.Text = error ?? "No se pudo crear la cuenta.";
                }
            }
            catch (HttpRequestException)
            {
                lblError.Text = "No se pudo conectar al servidor. Verificá que el WebAPI esté corriendo.";
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
            finally
            {
                if (!IsDisposed)
                {
                    btnCrear.Enabled = true;
                    btnCrear.Text = "Crear cuenta";
                }
            }
        }
    }
}