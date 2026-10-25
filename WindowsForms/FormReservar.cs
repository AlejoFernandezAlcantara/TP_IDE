using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using API.Clients;
using Domain.Model;
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

        private List<TurnoDTO> _turnos = new();
        private List<OdontologoDTO> _odontologos = new();
        private List<PacienteDTO> _pacientes = new();
        private List<MutualDTO> _mutuales = new();

        private string _rol = string.Empty;
        private string? _matriculaPropia;
        private int? _nroPacientePropio;

        private Label lblPac = null!;
        private ComboBox cmbPaciente = null!;
        private ComboBox cmbObraSocial = null!;
        private TextBox txtNroAfiliado = null!;
        private Label lblOdo = null!;
        private ComboBox cmbOdontologo = null!;
        private DateTimePicker dtpDesde = null!;
        private ErrorProvider errorProvider = null!;
        private DataGridView dgvTurnos = null!;
        private Label lblResumen = null!;
        private Button btnConfirmar = null!;
        private bool _cargando;

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
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            this.BackColor = Color.FromArgb(248, 250, 252);

            errorProvider = new ErrorProvider { BlinkStyle = ErrorBlinkStyle.NeverBlink };

            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 190, BackColor = Color.White, Padding = new Padding(20) };

            var lblTitulo = new Label
            {
                Text = "Registrar Nueva Reserva",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                Location = new Point(20, 15),
                AutoSize = true
            };

            var bold = new Font("Segoe UI", 9f, FontStyle.Bold);

            lblPac = new Label { Text = "Paciente *:", Location = new Point(20, 55), AutoSize = true, Font = bold };
            cmbPaciente = new ComboBox { Location = new Point(20, 75), Size = new Size(260, 25), DropDownStyle = ComboBoxStyle.DropDownList };

            var lblOS = new Label { Text = "Obra Social / Cobertura *:", Location = new Point(300, 55), AutoSize = true, Font = bold };
            cmbObraSocial = new ComboBox { Location = new Point(300, 75), Size = new Size(240, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbObraSocial.SelectedIndexChanged += OnObraSocialChanged;

            var lblAfil = new Label { Text = "N° de Afiliado *:", Location = new Point(560, 55), AutoSize = true, Font = bold };
            txtNroAfiliado = new TextBox { Location = new Point(560, 75), Size = new Size(240, 23), Enabled = false };

            lblOdo = new Label { Text = "Odontólogo:", Location = new Point(20, 115), AutoSize = true, Font = bold };
            cmbOdontologo = new ComboBox { Location = new Point(20, 135), Size = new Size(260, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbOdontologo.SelectedIndexChanged += (s, e) => AplicarFiltros();

            var lblDesde = new Label { Text = "Turnos desde:", Location = new Point(300, 115), AutoSize = true, Font = bold };
            dtpDesde = new DateTimePicker { Location = new Point(300, 135), Size = new Size(240, 25), Format = DateTimePickerFormat.Short, Value = DateTime.Today };
            dtpDesde.ValueChanged += (s, e) => AplicarFiltros();

            pnlTop.Controls.AddRange(new Control[] { lblTitulo, lblPac, cmbPaciente, lblOS, cmbObraSocial, lblAfil, txtNroAfiliado, lblOdo, cmbOdontologo, lblDesde, dtpDesde });

            var pnlCentral = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };
            var lblSub = new Label
            {
                Text = "Seleccioná uno o más turnos disponibles (Ctrl/Shift para varios, del mismo odontólogo):",
                Font = bold,
                Location = new Point(20, 10),
                AutoSize = true
            };

            dgvTurnos = new DataGridView
            {
                Location = new Point(20, 35),
                Size = new Size(840, 300),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AllowUserToAddRows = false,
                ReadOnly = true,
                MultiSelect = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            dgvTurnos.Columns.Add("Codigo", "Código");
            dgvTurnos.Columns.Add("Fecha", "Fecha y Hora");
            dgvTurnos.Columns.Add("Odontologo", "Odontólogo");
            dgvTurnos.Columns.Add("Duracion", "Duración (min)");
            dgvTurnos.SelectionChanged += (s, e) => ActualizarResumen();

            lblResumen = new Label
            {
                Text = "Ningún turno seleccionado.",
                Location = new Point(20, 345),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                AutoSize = true,
                ForeColor = Color.FromArgb(100, 116, 139)
            };

            btnConfirmar = new Button
            {
                Text = "Confirmar Reserva",
                Location = new Point(680, 342),
                Size = new Size(180, 38),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };
            btnConfirmar.Click += async (s, e) => await ConfirmarReservaAsync();

            pnlCentral.Controls.AddRange(new Control[] { lblSub, dgvTurnos, lblResumen, btnConfirmar });

            this.Controls.Add(pnlCentral);
            this.Controls.Add(pnlTop);

            this.Load += async (s, e) => await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                _cargando = true;

                var auth = AuthServiceProvider.Instance;
                var token = await auth.GetTokenAsync();
                _rol = await auth.GetRolAsync() ?? string.Empty;
                _matriculaPropia = TokenHelper.Matricula(token);
                _nroPacientePropio = TokenHelper.NroPaciente(token);

                _turnoClient.SetToken(token);
                _odontologoClient.SetToken(token);
                _pacienteClient.SetToken(token);
                _mutualClient.SetToken(token);

                var tTask = _turnoClient.GetAllAsync();
                var oTask = _odontologoClient.GetAllAsync();
                var mTask = _mutualClient.GetAllAsync();
                var pTask = _rol == "Paciente" ? Task.FromResult<List<PacienteDTO>?>(null) : _pacienteClient.GetAllAsync();

                await Task.WhenAll(tTask, oTask, mTask, pTask);

                _turnos = tTask.Result ?? new();
                _odontologos = oTask.Result ?? new();
                _mutuales = mTask.Result ?? new();
                _pacientes = pTask.Result ?? new();

                // Paciente: reserva siempre para sí mismo
                if (_rol == "Paciente")
                {
                    lblPac.Visible = false;
                    cmbPaciente.Visible = false;
                }
                else
                {
                    var items = _pacientes
                        .OrderBy(p => p.Apellido).ThenBy(p => p.Nombre)
                        .Select(p => new KeyValuePair<int, string>(p.NroPaciente, $"{p.Apellido}, {p.Nombre} (DNI {p.NroDni})"))
                        .ToList();
                    cmbPaciente.DataSource = items;
                    cmbPaciente.DisplayMember = "Value";
                    cmbPaciente.ValueMember = "Key";
                }

                // Odontólogo: solo puede reservar sus propios turnos
                if (_rol == "Odontologo")
                {
                    lblOdo.Visible = false;
                    cmbOdontologo.Visible = false;
                }
                else
                {
                    var odos = new List<KeyValuePair<string, string>> { new("", "Todos") };
                    odos.AddRange(_odontologos
                        .OrderBy(o => o.Apellido)
                        .Select(o => new KeyValuePair<string, string>(o.Matricula, $"{o.Apellido}, {o.Nombre} - {o.Especialidad}")));
                    cmbOdontologo.DataSource = odos;
                    cmbOdontologo.DisplayMember = "Value";
                    cmbOdontologo.ValueMember = "Key";
                }

                var listaOS = new List<string> { "Particular" };
                listaOS.AddRange(_mutuales.Select(m => m.Nombre));
                cmbObraSocial.DataSource = listaOS;

                _cargando = false;
                AplicarFiltros();
            }
            catch (Exception ex)
            {
                _cargando = false;
                MessageBox.Show("Error al cargar datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Solo turnos DISPONIBLES y futuros (los ya reservados no se muestran).
        // El odontólogo solo ve los suyos.
        private void AplicarFiltros()
        {
            if (_cargando)
                return;

            IEnumerable<TurnoDTO> q = _turnos
                .Where(t => t.Estado == EstadoTurno.Disponible)
                .Where(t => t.FechaHoraInicio > DateTime.Now)
                .Where(t => t.FechaHoraInicio.Date >= dtpDesde.Value.Date);

            if (_rol == "Odontologo")
                q = q.Where(t => string.Equals(t._odontologoMatricula, _matriculaPropia, StringComparison.OrdinalIgnoreCase));
            else if (cmbOdontologo.SelectedValue is string m && !string.IsNullOrEmpty(m))
                q = q.Where(t => t._odontologoMatricula == m);

            var diccOdo = _odontologos.ToDictionary(o => o.Matricula, o => $"{o.Apellido}, {o.Nombre}");

            dgvTurnos.Rows.Clear();
            foreach (var t in q.OrderBy(t => t.FechaHoraInicio))
            {
                string nombreOdo = diccOdo.TryGetValue(t._odontologoMatricula, out var n) ? n : t._odontologoMatricula;
                dgvTurnos.Rows.Add(t.Codigo, t.FechaHoraInicio.ToString("dd/MM/yyyy HH:mm"), nombreOdo, t.Duracion);
            }
            dgvTurnos.ClearSelection();
            ActualizarResumen();
        }

        private List<TurnoDTO> TurnosSeleccionados()
        {
            var codigos = dgvTurnos.SelectedRows.Cast<DataGridViewRow>()
                .Select(r => Convert.ToInt32(r.Cells["Codigo"].Value))
                .ToHashSet();
            return _turnos.Where(t => codigos.Contains(t.Codigo)).OrderBy(t => t.FechaHoraInicio).ToList();
        }

        private void ActualizarResumen()
        {
            var sel = TurnosSeleccionados();
            lblResumen.Text = sel.Count == 0
                ? "Ningún turno seleccionado."
                : $"{sel.Count} turno(s): " + string.Join(" | ", sel.Select(t => t.FechaHoraInicio.ToString("dd/MM HH:mm")));
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

            int pacienteId;
            if (_rol == "Paciente")
            {
                if (_nroPacientePropio is null)
                {
                    MessageBox.Show("No se pudo identificar tu número de paciente. Cerrá sesión y volvé a ingresar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                pacienteId = _nroPacientePropio.Value;
            }
            else
            {
                if (cmbPaciente.SelectedValue is not int sel || sel <= 0)
                {
                    errorProvider.SetError(cmbPaciente, "Seleccioná un paciente.");
                    return;
                }
                pacienteId = sel;
            }

            string os = cmbObraSocial.SelectedItem?.ToString() ?? "Particular";
            if (os != "Particular" && string.IsNullOrWhiteSpace(txtNroAfiliado.Text))
            {
                errorProvider.SetError(txtNroAfiliado, $"El número de afiliado es obligatorio para {os}.");
                return;
            }

            var seleccionados = TurnosSeleccionados();
            if (seleccionados.Count == 0)
            {
                MessageBox.Show("Seleccioná al menos un turno disponible de la grilla.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (seleccionados.Select(t => t._odontologoMatricula).Distinct().Count() > 1)
            {
                MessageBox.Show("Una reserva solo puede incluir turnos de un mismo odontólogo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnConfirmar.Enabled = false;
            btnConfirmar.Text = "Reservando...";

            try
            {
                var request = new ReservarTurnosDTO
                {
                    PacienteId = pacienteId,
                    TurnoCodigos = seleccionados.Select(t => t.Codigo).ToList(),
                    ObraSocial = os,
                    NroAfiliado = os == "Particular" ? null : txtNroAfiliado.Text.Trim()
                };

                var (ok, error) = await _turnoClient.ReservarVariosAsync(request);
                if (ok)
                {
                    MessageBox.Show(
                        seleccionados.Count == 1 ? "¡Turno reservado con éxito!" : $"¡Reserva creada con {seleccionados.Count} turnos!",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(error ?? "No se pudo reservar el turno.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    await CargarDatosAsync();
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
