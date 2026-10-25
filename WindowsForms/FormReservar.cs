using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using API.Clients;
using DTO;
using WindowsForms.Auth;

namespace WindowsForms
{
    public class FormReservar : Form
    {
        private readonly TurnoApiClient _turnoClient;
        private readonly OdontologoApiClient _odontologoClient;
        private readonly PacienteApiClient _pacienteClient;
        private readonly MutualApiClient _mutualClient;

        private List<TurnoDTO> _turnosDisponibles = new();
        private List<OdontologoDTO> _odontologos = new();
        private List<PacienteDTO> _pacientes = new();
        private List<MutualDTO> _mutuales = new();

        private ComboBox cmbPaciente = null!;
        private ComboBox cmbObraSocial = null!;
        private TextBox txtNroAfiliado = null!;
        private ErrorProvider errorProvider = null!;
        private DataGridView dgvTurnos = null!;
        private Button btnConfirmar = null!;

        public FormReservar()
        {
            var http = new System.Net.Http.HttpClient();
            _turnoClient = new TurnoApiClient(http);
            _odontologoClient = new OdontologoApiClient(http);
            _pacienteClient = new PacienteApiClient(http);
            _mutualClient = new MutualApiClient(http);

            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.Text = "Nueva Reserva de Turno";
            this.Size = new Size(880, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            this.BackColor = Color.FromArgb(248, 250, 252);

            errorProvider = new ErrorProvider { BlinkStyle = ErrorBlinkStyle.NeverBlink };

            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 170, BackColor = Color.White, Padding = new Padding(20) };
            
            var lblTitulo = new Label
            {
                Text = "Registrar Nueva Reserva",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                Location = new Point(20, 15),
                AutoSize = true
            };

            var lblPac = new Label { Text = "Paciente *:", Location = new Point(20, 55), AutoSize = true, Font = new Font("Segoe UI", 9f, FontStyle.Bold) };
            cmbPaciente = new ComboBox { Location = new Point(20, 75), Size = new Size(260, 25), DropDownStyle = ComboBoxStyle.DropDownList };

            var lblOS = new Label { Text = "Obra Social / Cobertura *:", Location = new Point(300, 55), AutoSize = true, Font = new Font("Segoe UI", 9f, FontStyle.Bold) };
            cmbObraSocial = new ComboBox { Location = new Point(300, 75), Size = new Size(240, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbObraSocial.SelectedIndexChanged += OnObraSocialChanged;

            var lblAfil = new Label { Text = "N° de Afiliado *:", Location = new Point(560, 55), AutoSize = true, Font = new Font("Segoe UI", 9f, FontStyle.Bold) };
            txtNroAfiliado = new TextBox { Location = new Point(560, 75), Size = new Size(240, 23), Enabled = false };

            pnlTop.Controls.AddRange(new Control[] { lblTitulo, lblPac, cmbPaciente, lblOS, cmbObraSocial, lblAfil, txtNroAfiliado });

            var pnlCentral = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };
            var lblSub = new Label { Text = "Seleccioná un Turno Disponible:", Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(20, 10), AutoSize = true };

            dgvTurnos = new DataGridView
            {
                Location = new Point(20, 35),
                Size = new Size(820, 280),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            dgvTurnos.Columns.Add("Codigo", "Código");
            dgvTurnos.Columns.Add("Fecha", "Fecha y Hora");
            dgvTurnos.Columns.Add("Odontologo", "Odontólogo");
            dgvTurnos.Columns.Add("Duracion", "Duración (min)");

            btnConfirmar = new Button
            {
                Text = "Confirmar Reserva",
                Location = new Point(660, 325),
                Size = new Size(180, 38),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };
            btnConfirmar.Click += async (s, e) => await ConfirmarReservaAsync();

            pnlCentral.Controls.AddRange(new Control[] { lblSub, dgvTurnos, btnConfirmar });

            this.Controls.Add(pnlCentral);
            this.Controls.Add(pnlTop);

            this.Load += async (s, e) => await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var token = await AuthServiceProvider.Instance.GetTokenAsync();
                _turnoClient.SetToken(token);
                _odontologoClient.SetToken(token);
                _pacienteClient.SetToken(token);
                _mutualClient.SetToken(token);

                var tTask = _turnoClient.GetAllAsync();
                var oTask = _odontologoClient.GetAllAsync();
                var pTask = _pacienteClient.GetAllAsync();
                var mTask = _mutualClient.GetAllAsync();

                await Task.WhenAll(tTask, oTask, pTask, mTask);

                _turnosDisponibles = (tTask.Result ?? new()).Where(t => t.Estado == Domain.Model.EstadoTurno.Disponible).OrderBy(t => t.FechaHoraInicio).ToList();
                _odontologos = oTask.Result ?? new();
                _pacientes = pTask.Result ?? new();
                _mutuales = mTask.Result ?? new();

                // Cargar Pacientes
                cmbPaciente.DataSource = _pacientes;
                cmbPaciente.DisplayMember = "Apellido";
                cmbPaciente.ValueMember = "NroPaciente";

                // Cargar Obras Sociales con opción Particular
                var listaOS = new List<string> { "Particular" };
                listaOS.AddRange(_mutuales.Select(m => m.Nombre));
                cmbObraSocial.DataSource = listaOS;

                // Llenar grilla de turnos
                dgvTurnos.Rows.Clear();
                var diccOdo = _odontologos.ToDictionary(o => o.Matricula, o => $"{o.Apellido}, {o.Nombre}");
                foreach (var t in _turnosDisponibles)
                {
                    string nombreOdo = diccOdo.TryGetValue(t._odontologoMatricula, out var n) ? n : t._odontologoMatricula;
                    dgvTurnos.Rows.Add(t.Codigo, t.FechaHoraInicio.ToString("dd/MM/yyyy HH:mm"), nombreOdo, t.Duracion);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnObraSocialChanged(object? sender, EventArgs e)
        {
            string seleccion = cmbObraSocial.SelectedItem?.ToString() ?? "Particular";
            if (seleccion == "Particular")
            {
                txtNroAfiliado.Enabled = false;
                txtNroAfiliado.Clear();
                errorProvider.SetError(txtNroAfiliado, string.Empty);
            }
            else
            {
                txtNroAfiliado.Enabled = true;
            }
        }

        private async Task ConfirmarReservaAsync()
        {
            errorProvider.Clear();

            if (cmbPaciente.SelectedValue is not int pacienteId || pacienteId <= 0)
            {
                errorProvider.SetError(cmbPaciente, "Seleccioná un paciente.");
                return;
            }

            string os = cmbObraSocial.SelectedItem?.ToString() ?? "Particular";
            if (os != "Particular" && string.IsNullOrWhiteSpace(txtNroAfiliado.Text))
            {
                errorProvider.SetError(txtNroAfiliado, $"El número de afiliado es obligatorio para {os}.");
                return;
            }

            if (dgvTurnos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccioná un turno disponible de la grilla.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int codigoTurno = Convert.ToInt32(dgvTurnos.SelectedRows[0].Cells["Codigo"].Value);

            btnConfirmar.Enabled = false;
            btnConfirmar.Text = "Reservando...";

            try
            {
                var (ok, error) = await _turnoClient.ReservarAsync(codigoTurno, pacienteId);
                if (ok)
                {
                    MessageBox.Show("¡Turno reservado con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(error ?? "No se pudo reservar el turno.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnConfirmar.Enabled = true;
                btnConfirmar.Text = "Confirmar Reserva";
            }
        }
    }
}