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
    // Equivalente WinForms de MisReservas.razor (vista del paciente)
    public class FormMisReservas : Form
    {
        private const int HorasLimiteCancelacion = 12;

        private readonly ReservaApiClient _reservaClient;
        private readonly TurnoApiClient _turnoClient;
        private readonly OdontologoApiClient _odontologoClient;

        private List<ReservaDTO> _reservas = new();
        private Dictionary<string, string> _odontologos = new();
        private Dictionary<int, TurnoDTO> _primerTurno = new();
        private int? _nroPaciente;

        private DataGridView dgv = null!;
        private CheckBox chkCanceladas = null!;
        private Button btnCancelar = null!;

        public FormMisReservas()
        {
            var http = new System.Net.Http.HttpClient();
            _reservaClient = new ReservaApiClient(http);
            _turnoClient = new TurnoApiClient(http);
            _odontologoClient = new OdontologoApiClient(http);

            Text = "Mis Reservas";
            Size = new Size(1000, 600);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 9f);
            BackColor = Color.FromArgb(248, 250, 252);

            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 75, BackColor = Color.White, Padding = new Padding(15) };
            pnlTop.Controls.Add(new Label { Text = "Mis Reservas", Font = new Font("Segoe UI", 13f, FontStyle.Bold), Location = new Point(15, 10), AutoSize = true });

            chkCanceladas = new CheckBox { Text = "Mostrar canceladas", Location = new Point(15, 45), AutoSize = true };
            chkCanceladas.CheckedChanged += (s, e) => Pintar();

            var btnNueva = new Button { Text = "+ Reservar turno", Location = new Point(620, 25), Size = new Size(140, 35), Anchor = AnchorStyles.Top | AnchorStyles.Right, BackColor = Color.FromArgb(37, 99, 235), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnNueva.Click += async (s, e) =>
            {
                using var f = new FormReservar();
                if (f.ShowDialog() == DialogResult.OK)
                    await CargarAsync();
            };

            btnCancelar = new Button { Text = "Cancelar reserva", Location = new Point(770, 25), Size = new Size(140, 35), Anchor = AnchorStyles.Top | AnchorStyles.Right, BackColor = Color.FromArgb(245, 158, 11), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnCancelar.Click += async (s, e) => await CancelarAsync();

            pnlTop.Controls.AddRange(new Control[] { chkCanceladas, btnNueva, btnCancelar });

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            dgv.Columns.Add("Id", "ID");
            dgv.Columns.Add("Turnos", "Fecha y hora del turno");
            dgv.Columns.Add("Odontologo", "Odontólogo");
            dgv.Columns.Add("Estado", "Estado");
            dgv.Columns.Add("Resultado", "Resultado");
            dgv.Columns["Id"].Width = 50;

            Controls.Add(dgv);
            Controls.Add(pnlTop);

            Load += async (s, e) => await CargarAsync();
        }

        private async Task CargarAsync()
        {
            try
            {
                var token = await AuthServiceProvider.Instance.GetTokenAsync();
                _nroPaciente = TokenHelper.NroPaciente(token);
                if (_nroPaciente is null)
                {
                    MessageBox.Show("No se pudo identificar tu número de paciente. Cerrá sesión y volvé a ingresar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _reservaClient.SetToken(token);
                _turnoClient.SetToken(token);
                _odontologoClient.SetToken(token);

                var r = await _reservaClient.GetByPacienteAsync(_nroPaciente.Value);
                if (r is null)
                {
                    MessageBox.Show("No se pudieron cargar tus reservas. Verificá que el WebAPI esté corriendo y que tu sesión siga activa.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                _reservas = r;

                var ids = _reservas.Select(x => x.Id).ToHashSet();
                var turnos = await _turnoClient.GetAllAsync() ?? new();
                _primerTurno = turnos
                    .Where(t => t.ReservaId.HasValue && ids.Contains(t.ReservaId.Value))
                    .GroupBy(t => t.ReservaId!.Value)
                    .ToDictionary(g => g.Key, g => g.OrderBy(t => t.FechaHoraInicio).First());

                var odos = await _odontologoClient.GetAllAsync() ?? new();
                _odontologos = odos.ToDictionary(o => o.Matricula, o => $"{o.Apellido}, {o.Nombre}");

                Pintar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar reservas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Pintar()
        {
            dgv.Rows.Clear();
            var lista = _reservas
                .Where(r => chkCanceladas.Checked || r.Estado != EstadoReserva.Cancelada)
                .OrderByDescending(r => r.PrimerTurno ?? r.FechaCreacion);

            foreach (var r in lista)
            {
                string odo = _odontologos.TryGetValue(r._odontologoMatricula, out var n) ? n : r._odontologoMatricula;
                string turnos = r.Turnos.Count > 0 ? r.TurnosTexto : "-";
                string res = string.IsNullOrWhiteSpace(r.Resultado) ? "-" : r.Resultado!;
                dgv.Rows.Add(r.Id, turnos, odo, r.Estado, res);
            }
        }

        private async Task CancelarAsync()
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccioná una reserva.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int id = Convert.ToInt32(dgv.SelectedRows[0].Cells["Id"].Value);
            var reserva = _reservas.FirstOrDefault(r => r.Id == id);
            if (reserva == null || !_primerTurno.TryGetValue(id, out var turno))
            {
                MessageBox.Show("Esta reserva no se puede cancelar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (reserva.Estado != EstadoReserva.Pendiente && reserva.Estado != EstadoReserva.Confirmada)
            {
                MessageBox.Show("Solo se pueden cancelar reservas pendientes o confirmadas.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (turno.FechaHoraInicio - DateTime.Now < TimeSpan.FromHours(HorasLimiteCancelacion))
            {
                MessageBox.Show($"Solo podés cancelar hasta {HorasLimiteCancelacion} horas antes del turno.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"¿Cancelar tu reserva del {turno.FechaHoraInicio:dd/MM/yyyy} a las {turno.FechaHoraInicio:HH:mm}?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            var (ok, error) = await _turnoClient.CancelarReservaAsync(turno.Codigo);
            if (!ok)
                MessageBox.Show(error ?? "No se pudo cancelar la reserva.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            await CargarAsync();
        }
    }
}
