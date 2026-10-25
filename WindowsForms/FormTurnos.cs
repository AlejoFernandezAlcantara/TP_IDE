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
    // Equivalente WinForms de Turnos.razor: alta individual, carga masiva, baja y liberación de turnos
    public class FormTurnos : Form
    {
        private const int MaxTurnosPorCarga = 500;
        private const int MaxDiasPorCarga = 120;

        private readonly TurnoApiClient _turnoClient;
        private readonly OdontologoApiClient _odontologoClient;
        private readonly PacienteApiClient _pacienteClient;

        private List<TurnoDTO> _turnos = new();
        private Dictionary<int, string> _pacientes = new();
        private string _rol = string.Empty;
        private string _matricula = string.Empty;

        private Label lblOdo = null!;
        private ComboBox cmbOdontologo = null!;
        private DataGridView dgv = null!;
        private ComboBox cmbEstado = null!;

        // Alta individual
        private DateTimePicker dtpIndFecha = null!;
        private DateTimePicker dtpIndHora = null!;
        private NumericUpDown nudIndDuracion = null!;

        // Carga masiva
        private DateTimePicker dtpMasDesde = null!;
        private DateTimePicker dtpMasHasta = null!;
        private DateTimePicker dtpHoraDesde = null!;
        private DateTimePicker dtpHoraHasta = null!;
        private NumericUpDown nudMasDuracion = null!;
        private readonly Dictionary<DayOfWeek, CheckBox> _chkDias = new();

        public FormTurnos()
        {
            var http = new System.Net.Http.HttpClient();
            _turnoClient = new TurnoApiClient(http);
            _odontologoClient = new OdontologoApiClient(http);
            _pacienteClient = new PacienteApiClient(http);

            Inicializar();
        }

        private void Inicializar()
        {
            Text = "Gestión de Turnos";
            Size = new Size(1100, 760);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 9f);
            BackColor = Color.FromArgb(248, 250, 252);

            var bold = new Font("Segoe UI", 9f, FontStyle.Bold);

            // ---------- Panel superior ----------
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 215, BackColor = Color.White, Padding = new Padding(15) };

            pnlTop.Controls.Add(new Label { Text = "Gestión de Turnos", Font = new Font("Segoe UI", 13f, FontStyle.Bold), Location = new Point(15, 10), AutoSize = true });

            lblOdo = new Label { Text = "Odontólogo:", Location = new Point(15, 48), AutoSize = true, Font = bold };
            cmbOdontologo = new ComboBox { Location = new Point(95, 44), Size = new Size(300, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbOdontologo.SelectionChangeCommitted += async (s, e) =>
            {
                _matricula = cmbOdontologo.SelectedValue as string ?? string.Empty;
                await CargarTurnosAsync();
            };
            pnlTop.Controls.AddRange(new Control[] { lblOdo, cmbOdontologo });

            // Alta individual
            var grpInd = new GroupBox { Text = "Turno individual", Location = new Point(15, 80), Size = new Size(330, 125) };
            grpInd.Controls.Add(new Label { Text = "Fecha:", Location = new Point(10, 28), AutoSize = true });
            dtpIndFecha = new DateTimePicker { Location = new Point(70, 24), Size = new Size(240, 25), Format = DateTimePickerFormat.Short, MinDate = DateTime.Today };
            grpInd.Controls.Add(dtpIndFecha);
            grpInd.Controls.Add(new Label { Text = "Hora:", Location = new Point(10, 58), AutoSize = true });
            dtpIndHora = new DateTimePicker { Location = new Point(70, 54), Size = new Size(90, 25), Format = DateTimePickerFormat.Custom, CustomFormat = "HH:mm", ShowUpDown = true, Value = DateTime.Today.AddHours(9) };
            grpInd.Controls.Add(dtpIndHora);
            grpInd.Controls.Add(new Label { Text = "Min:", Location = new Point(175, 58), AutoSize = true });
            nudIndDuracion = new NumericUpDown { Location = new Point(210, 54), Size = new Size(100, 25), Minimum = 5, Maximum = 240, Increment = 5, Value = 30 };
            grpInd.Controls.Add(nudIndDuracion);
            var btnInd = new Button { Text = "Cargar turno", Location = new Point(70, 88), Size = new Size(240, 28), BackColor = Color.FromArgb(37, 99, 235), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnInd.Click += async (s, e) => await CrearIndividualAsync();
            grpInd.Controls.Add(btnInd);
            pnlTop.Controls.Add(grpInd);

            // Carga masiva
            var grpMas = new GroupBox { Text = "Carga masiva de turnos", Location = new Point(360, 80), Size = new Size(700, 125) };
            grpMas.Controls.Add(new Label { Text = "Desde:", Location = new Point(10, 28), AutoSize = true });
            dtpMasDesde = new DateTimePicker { Location = new Point(60, 24), Size = new Size(110, 25), Format = DateTimePickerFormat.Short, MinDate = DateTime.Today };
            grpMas.Controls.Add(dtpMasDesde);
            grpMas.Controls.Add(new Label { Text = "Hasta:", Location = new Point(185, 28), AutoSize = true });
            dtpMasHasta = new DateTimePicker { Location = new Point(235, 24), Size = new Size(110, 25), Format = DateTimePickerFormat.Short, MinDate = DateTime.Today };
            grpMas.Controls.Add(dtpMasHasta);
            grpMas.Controls.Add(new Label { Text = "Horario:", Location = new Point(365, 28), AutoSize = true });
            dtpHoraDesde = new DateTimePicker { Location = new Point(420, 24), Size = new Size(70, 25), Format = DateTimePickerFormat.Custom, CustomFormat = "HH:mm", ShowUpDown = true, Value = DateTime.Today.AddHours(9) };
            grpMas.Controls.Add(dtpHoraDesde);
            grpMas.Controls.Add(new Label { Text = "a", Location = new Point(495, 28), AutoSize = true });
            dtpHoraHasta = new DateTimePicker { Location = new Point(510, 24), Size = new Size(70, 25), Format = DateTimePickerFormat.Custom, CustomFormat = "HH:mm", ShowUpDown = true, Value = DateTime.Today.AddHours(13) };
            grpMas.Controls.Add(dtpHoraHasta);
            grpMas.Controls.Add(new Label { Text = "Min:", Location = new Point(595, 28), AutoSize = true });
            nudMasDuracion = new NumericUpDown { Location = new Point(630, 24), Size = new Size(60, 25), Minimum = 5, Maximum = 240, Increment = 5, Value = 30 };
            grpMas.Controls.Add(nudMasDuracion);

            var dias = new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };
            int x = 10;
            foreach (var d in dias)
            {
                var chk = new CheckBox { Text = NombreDia(d), Location = new Point(x, 60), AutoSize = true, Checked = d != DayOfWeek.Saturday && d != DayOfWeek.Sunday };
                _chkDias[d] = chk;
                grpMas.Controls.Add(chk);
                x += 62;
            }
            var btnMas = new Button { Text = "Generar turnos", Location = new Point(10, 90), Size = new Size(200, 28), BackColor = Color.FromArgb(22, 163, 74), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnMas.Click += async (s, e) => await CrearMasivoAsync();
            grpMas.Controls.Add(btnMas);
            pnlTop.Controls.Add(grpMas);

            // ---------- Barra de acciones ----------
            var pnlAcc = new Panel { Dock = DockStyle.Top, Height = 45, Padding = new Padding(15, 8, 15, 5) };
            pnlAcc.Controls.Add(new Label { Text = "Estado:", Location = new Point(15, 14), AutoSize = true });
            cmbEstado = new ComboBox { Location = new Point(65, 10), Size = new Size(140, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbEstado.Items.AddRange(new object[] { "Todos", "Disponible", "Reservado", "Cancelado" });
            cmbEstado.SelectedIndex = 0;
            cmbEstado.SelectedIndexChanged += (s, e) => Pintar();
            pnlAcc.Controls.Add(cmbEstado);

            var btnLiberar = new Button { Text = "Cancelar reserva del turno", Location = new Point(230, 8), Size = new Size(190, 28), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(245, 158, 11), ForeColor = Color.White };
            btnLiberar.Click += async (s, e) => await LiberarAsync();
            var btnEliminar = new Button { Text = "Eliminar turno", Location = new Point(430, 8), Size = new Size(130, 28), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(220, 38, 38), ForeColor = Color.White };
            btnEliminar.Click += async (s, e) => await EliminarAsync();
            var btnRef = new Button { Text = "Refrescar", Location = new Point(570, 8), Size = new Size(90, 28), FlatStyle = FlatStyle.Flat };
            btnRef.Click += async (s, e) => await CargarTurnosAsync();
            pnlAcc.Controls.AddRange(new Control[] { btnLiberar, btnEliminar, btnRef });

            // ---------- Grilla ----------
            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            dgv.Columns.Add("Codigo", "Código");
            dgv.Columns.Add("Fecha", "Fecha y hora");
            dgv.Columns.Add("Duracion", "Duración (min)");
            dgv.Columns.Add("Estado", "Estado");
            dgv.Columns.Add("Paciente", "Paciente (si está reservado)");

            Controls.Add(dgv);
            Controls.Add(pnlAcc);
            Controls.Add(pnlTop);

            Load += async (s, e) => await InicializarDatosAsync();
        }

        private async Task InicializarDatosAsync()
        {
            try
            {
                var auth = AuthServiceProvider.Instance;
                var token = await auth.GetTokenAsync();
                _rol = await auth.GetRolAsync() ?? string.Empty;

                _turnoClient.SetToken(token);
                _odontologoClient.SetToken(token);
                _pacienteClient.SetToken(token);

                var pacientes = await _pacienteClient.GetAllAsync() ?? new();
                _pacientes = pacientes.ToDictionary(p => p.NroPaciente, p => $"{p.Apellido}, {p.Nombre}");

                if (_rol == "Administrador")
                {
                    var odos = (await _odontologoClient.GetAllAsync() ?? new()).OrderBy(o => o.Apellido).ThenBy(o => o.Nombre).ToList();
                    cmbOdontologo.DataSource = odos.Select(o => new KeyValuePair<string, string>(o.Matricula, $"{o.Apellido}, {o.Nombre} ({o.Matricula})")).ToList();
                    cmbOdontologo.DisplayMember = "Value";
                    cmbOdontologo.ValueMember = "Key";
                    _matricula = cmbOdontologo.SelectedValue as string ?? string.Empty;
                }
                else
                {
                    // El odontólogo solo gestiona sus propios turnos
                    lblOdo.Visible = false;
                    cmbOdontologo.Visible = false;
                    _matricula = TokenHelper.Matricula(token) ?? string.Empty;
                    if (string.IsNullOrEmpty(_matricula))
                        MessageBox.Show("No se pudo identificar tu matrícula. Cerrá sesión y volvé a ingresar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                await CargarTurnosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarTurnosAsync()
        {
            if (string.IsNullOrEmpty(_matricula))
            {
                _turnos = new();
                Pintar();
                return;
            }

            try
            {
                var r = await _turnoClient.GetByOdontologoAsync(_matricula);
                if (r is null)
                {
                    MessageBox.Show("No se pudieron cargar los turnos. Verificá que el WebAPI esté corriendo y que tu sesión siga activa.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                _turnos = r;
                Pintar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar turnos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Pintar()
        {
            dgv.Rows.Clear();
            string filtro = cmbEstado.SelectedItem?.ToString() ?? "Todos";

            foreach (var t in _turnos.OrderBy(t => t.FechaHoraInicio))
            {
                if (filtro != "Todos" && t.Estado.ToString() != filtro)
                    continue;

                string pac = t.Estado == EstadoTurno.Reservado
                    ? (_pacientes.TryGetValue(t._reservaPacienteId, out var n) ? n : $"Paciente Nº {t._reservaPacienteId}")
                    : "-";

                dgv.Rows.Add(t.Codigo, t.FechaHoraInicio.ToString("dd/MM/yyyy HH:mm"), t.Duracion, t.Estado, pac);
            }
        }

        private TurnoDTO? Seleccionado()
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccioná un turno de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            int codigo = Convert.ToInt32(dgv.SelectedRows[0].Cells["Codigo"].Value);
            return _turnos.FirstOrDefault(t => t.Codigo == codigo);
        }

        private TurnoDTO NuevoTurno(DateTime inicio, int duracion) => new TurnoDTO
        {
            FechaHoraInicio = inicio,
            Duracion = duracion,
            Estado = EstadoTurno.Disponible,
            _odontologoMatricula = _matricula,
            _reservaOdontologoMatricula = string.Empty
        };

        private async Task CrearIndividualAsync()
        {
            if (string.IsNullOrEmpty(_matricula))
            {
                MessageBox.Show("Elegí un odontólogo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var inicio = dtpIndFecha.Value.Date + dtpIndHora.Value.TimeOfDay;
            if (inicio < DateTime.Now)
            {
                MessageBox.Show("No se puede cargar un turno en el pasado.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var (ok, error) = await _turnoClient.CrearAsync(NuevoTurno(inicio, (int)nudIndDuracion.Value));
            if (!ok)
                MessageBox.Show(error ?? "No se pudo crear el turno.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            await CargarTurnosAsync();
        }

        private List<DateTime> GenerarSlots()
        {
            var resultado = new List<DateTime>();
            var desde = dtpHoraDesde.Value.TimeOfDay;
            var hasta = dtpHoraHasta.Value.TimeOfDay;
            int duracion = (int)nudMasDuracion.Value;

            if (hasta <= desde || dtpMasHasta.Value.Date < dtpMasDesde.Value.Date)
                return resultado;

            if ((dtpMasHasta.Value.Date - dtpMasDesde.Value.Date).TotalDays > MaxDiasPorCarga)
                return resultado;

            var paso = TimeSpan.FromMinutes(duracion);
            for (var dia = dtpMasDesde.Value.Date; dia <= dtpMasHasta.Value.Date; dia = dia.AddDays(1))
            {
                if (!_chkDias[dia.DayOfWeek].Checked)
                    continue;

                for (var h = desde; h + paso <= hasta; h += paso)
                    resultado.Add(dia + h);
            }
            return resultado;
        }

        private async Task CrearMasivoAsync()
        {
            if (string.IsNullOrEmpty(_matricula))
            {
                MessageBox.Show("Elegí un odontólogo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if ((dtpMasHasta.Value.Date - dtpMasDesde.Value.Date).TotalDays > MaxDiasPorCarga)
            {
                MessageBox.Show($"El rango no puede superar los {MaxDiasPorCarga} días.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var slots = GenerarSlots();
            if (slots.Count == 0)
            {
                MessageBox.Show("Revisá los datos: no se generaría ningún turno (fechas, horario, duración o días elegidos).", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (slots.Count > MaxTurnosPorCarga)
            {
                MessageBox.Show($"Se generarían {slots.Count} turnos y el máximo por carga es {MaxTurnosPorCarga}. Achicá el rango.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Se van a generar {slots.Count} turnos. ¿Continuar?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            Cursor = Cursors.WaitCursor;
            int creados = 0, omitidos = 0;
            string? primerError = null;

            try
            {
                foreach (var inicio in slots)
                {
                    if (inicio < DateTime.Now)
                    {
                        omitidos++;
                        primerError ??= "algunos horarios ya pasaron";
                        continue;
                    }

                    var (ok, error) = await _turnoClient.CrearAsync(NuevoTurno(inicio, (int)nudMasDuracion.Value));
                    if (ok) creados++;
                    else
                    {
                        omitidos++;
                        primerError ??= error;
                    }
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            string msg = $"Se cargaron {creados} turnos.";
            if (omitidos > 0)
                msg += $"\nSe omitieron {omitidos} ({primerError}).";
            MessageBox.Show(msg, "Carga masiva", MessageBoxButtons.OK, omitidos > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

            await CargarTurnosAsync();
        }

        private async Task EliminarAsync()
        {
            var t = Seleccionado();
            if (t == null) return;

            if (MessageBox.Show($"¿Eliminar el turno del {t.FechaHoraInicio:dd/MM/yyyy} a las {t.FechaHoraInicio:HH:mm}?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            var (ok, error) = await _turnoClient.EliminarAsync(t.Codigo);
            if (!ok)
                MessageBox.Show(error ?? "No se pudo eliminar el turno.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            await CargarTurnosAsync();
        }

        private async Task LiberarAsync()
        {
            var t = Seleccionado();
            if (t == null) return;

            if (t.Estado != EstadoTurno.Reservado)
            {
                MessageBox.Show("El turno no tiene una reserva activa.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("¿Cancelar la reserva de este turno? Los turnos de esa reserva volverán a quedar disponibles.", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            var (ok, error) = await _turnoClient.CancelarReservaAsync(t.Codigo);
            if (!ok)
                MessageBox.Show(error ?? "No se pudo cancelar la reserva.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            await CargarTurnosAsync();
        }

        private static string NombreDia(DayOfWeek d) => d switch
        {
            DayOfWeek.Monday => "Lun",
            DayOfWeek.Tuesday => "Mar",
            DayOfWeek.Wednesday => "Mié",
            DayOfWeek.Thursday => "Jue",
            DayOfWeek.Friday => "Vie",
            DayOfWeek.Saturday => "Sáb",
            _ => "Dom"
        };
    }
}
