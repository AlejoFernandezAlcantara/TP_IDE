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
    public class FormReservas : Form
    {
        private readonly ReservaApiClient _reservaClient;
        private readonly PacienteApiClient _pacienteClient;
        private readonly OdontologoApiClient _odontologoClient;

        private List<ReservaDTO> _reservas = new();
        private Dictionary<int, string> _pacientes = new();
        private Dictionary<string, string> _odontologos = new();

        private DataGridView dgvReservas = null!;
        private Button btnNuevaReserva = null!;
        private Button btnAtender = null!;
        private Button btnRefrescar = null!;
        private Button btnCancelar = null!;
        private Button btnEliminar = null!;
        private string _rol = string.Empty;
        private readonly bool _modoAtencion;
        private ComboBox cmbFiltroEstado = null!;

        public FormReservas(bool modoAtencion = false)
        {
            _modoAtencion = modoAtencion;
            var http = new System.Net.Http.HttpClient();
            _reservaClient = new ReservaApiClient(http);
            _pacienteClient = new PacienteApiClient(http);
            _odontologoClient = new OdontologoApiClient(http);

            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.Text = "Gestión de Reservas e Historial Clínico";
            this.Size = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            this.BackColor = Color.FromArgb(248, 250, 252);

            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 75, BackColor = Color.White, Padding = new Padding(15) };
            var lblTitulo = new Label
            {
                Text = "Listado de Reservas e Historial Odontológico",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                Location = new Point(15, 12),
                AutoSize = true
            };

            var lblFiltro = new Label { Text = "Estado:", Location = new Point(15, 45), AutoSize = true };
            cmbFiltroEstado = new ComboBox
            {
                Location = new Point(65, 42),
                Size = new Size(160, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            if (_modoAtencion)
            {
                this.Text = "Atender Reservas";
                lblTitulo.Text = "Reservas pendientes de atención";
                cmbFiltroEstado.Items.AddRange(new object[] { "Por atender", "Pendiente", "Confirmada" });
            }
            else
            {
                cmbFiltroEstado.Items.AddRange(new object[] { "Todos", "Pendiente", "Confirmada", "Completada", "Cancelada" });
            }
            cmbFiltroEstado.SelectedIndex = 0;
            cmbFiltroEstado.SelectedIndexChanged += (s, e) => FiltrarReservas();

            btnNuevaReserva = new Button
            {
                Text = "+ Nueva Reserva",
                Location = new Point(800, 25),
                Size = new Size(130, 35),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            btnNuevaReserva.Click += (s, e) =>
            {
                var f = new FormReservar();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    _ = CargarDatosAsync();
                }
            };

            btnAtender = new Button
            {
                Text = "Atender Reserva (Odontograma)",
                Location = new Point(620, 25),
                Size = new Size(170, 35),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.FromArgb(22, 163, 74),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            btnAtender.Click += (s, e) => AtenderSeleccionada();

            btnRefrescar = new Button
            {
                Text = "Refrescar",
                Location = new Point(940, 25),
                Size = new Size(90, 35),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FlatStyle = FlatStyle.Flat
            };
            btnRefrescar.Click += async (s, e) => await CargarDatosAsync();

            btnCancelar = new Button
            {
                Text = "Cancelar Reserva",
                Location = new Point(480, 25),
                Size = new Size(130, 35),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.FromArgb(245, 158, 11),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            btnCancelar.Click += async (s, e) => await CancelarSeleccionadaAsync();

            btnEliminar = new Button
            {
                Text = "Eliminar",
                Location = new Point(390, 25),
                Size = new Size(80, 35),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.FromArgb(220, 38, 38),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Visible = false // solo administradores
            };
            btnEliminar.Click += async (s, e) => await EliminarSeleccionadaAsync();

            pnlTop.Controls.AddRange(new Control[] { lblTitulo, lblFiltro, cmbFiltroEstado, btnEliminar, btnCancelar, btnAtender, btnNuevaReserva, btnRefrescar });

            dgvReservas = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                RowTemplate = { Height = 32 }
            };

            dgvReservas.Columns.Add("Id", "ID");
            dgvReservas.Columns.Add("Fecha", "Fecha y hora del turno");
            dgvReservas.Columns.Add("Paciente", "Paciente");
            dgvReservas.Columns.Add("Odontologo", "Odontólogo");
            dgvReservas.Columns.Add("Cobertura", "Cobertura");
            dgvReservas.Columns.Add("Estado", "Estado");
            dgvReservas.Columns.Add("Importe", "Importe");
            dgvReservas.Columns.Add("Historial", "Prácticas Realizadas (Piezas)");

            dgvReservas.Columns["Id"].Width = 50;
            dgvReservas.Columns["Fecha"].Width = 150;
            dgvReservas.Columns["Estado"].Width = 95;
            dgvReservas.Columns["Historial"].FillWeight = 220;

            this.Controls.Add(dgvReservas);
            this.Controls.Add(pnlTop);

            this.Load += async (s, e) => await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var token = await AuthServiceProvider.Instance.GetTokenAsync();
                _rol = await AuthServiceProvider.Instance.GetRolAsync() ?? string.Empty;
                btnEliminar.Visible = _rol == "Administrador";
                _reservaClient.SetToken(token);
                _pacienteClient.SetToken(token);
                _odontologoClient.SetToken(token);

                var rTask = _reservaClient.GetAllAsync();
                var pTask = _pacienteClient.GetAllAsync();
                var oTask = _odontologoClient.GetAllAsync();

                await Task.WhenAll(rTask, pTask, oTask);

                _reservas = (rTask.Result ?? new()).OrderByDescending(r => r.PrimerTurno ?? r.FechaCreacion).ToList();
                _pacientes = (pTask.Result ?? new()).ToDictionary(p => p.NroPaciente, p => $"{p.Apellido}, {p.Nombre}");
                _odontologos = (oTask.Result ?? new()).ToDictionary(o => o.Matricula, o => $"{o.Apellido}, {o.Nombre}");

                FiltrarReservas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar reservas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FiltrarReservas()
        {
            dgvReservas.Rows.Clear();
            string filtro = cmbFiltroEstado.SelectedItem?.ToString() ?? "Todos";

            var filtradas = _reservas;
            if (filtro == "Por atender")
            {
                filtradas = _reservas.Where(r => r.Estado == EstadoReserva.Pendiente || r.Estado == EstadoReserva.Confirmada).ToList();
            }
            else if (filtro != "Todos")
            {
                filtradas = _reservas.Where(r => r.Estado.ToString() == filtro).ToList();
            }

            foreach (var r in filtradas)
            {
                string pac = _pacientes.TryGetValue(r._pacienteId, out var p) ? p : $"#{r._pacienteId}";
                string odo = _odontologos.TryGetValue(r._odontologoMatricula, out var o) ? o : r._odontologoMatricula;
                string cob = r.ObraSocial == "Particular" ? "Particular" : $"{r.ObraSocial} ({r.NroAfiliado})";
                string imp = r.Importe.HasValue ? $"$ {r.Importe.Value:N2}" : "-";

                string historial = "-";
                if (r.Estado == EstadoReserva.Completada && r.PracticasRealizadas?.Count > 0)
                {
                    var items = r.PracticasRealizadas.Select(pr => $"{pr.PracticaDetalle ?? $"Práctica #{pr.PracticaCodigo}"} [Piezas: {pr.DientesCaras}]");
                    historial = string.Join(" | ", items);
                }
                else if (!string.IsNullOrEmpty(r.Resultado))
                {
                    historial = r.Resultado;
                }

                string turnos = r.Turnos.Count > 0 ? r.TurnosTexto : "Sin turno asignado";
                dgvReservas.Rows.Add(r.Id, turnos, pac, odo, cob, r.Estado, imp, historial);
            }
        }

        private ReservaDTO? ReservaSeleccionada()
        {
            if (dgvReservas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccioná una reserva de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            int id = Convert.ToInt32(dgvReservas.SelectedRows[0].Cells["Id"].Value);
            return _reservas.FirstOrDefault(r => r.Id == id);
        }

        // Una reserva atendida (Completada) no se puede editar ni eliminar
        private async Task CancelarSeleccionadaAsync()
        {
            var res = ReservaSeleccionada();
            if (res == null) return;

            if (res.Estado == EstadoReserva.Completada)
            {
                MessageBox.Show("Una reserva atendida no se puede editar ni eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (res.Estado == EstadoReserva.Cancelada)
            {
                MessageBox.Show("La reserva ya está cancelada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show($"¿Cancelar la reserva #{res.Id}? Sus turnos quedarán disponibles.", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            res.Estado = EstadoReserva.Cancelada;
            var (ok, error) = await _reservaClient.ActualizarAsync(res);
            if (!ok)
                MessageBox.Show(error ?? "No se pudo cancelar la reserva.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            await CargarDatosAsync();
        }

        private async Task EliminarSeleccionadaAsync()
        {
            var res = ReservaSeleccionada();
            if (res == null) return;

            if (res.Estado == EstadoReserva.Completada)
            {
                MessageBox.Show("Una reserva atendida no se puede editar ni eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"¿Eliminar definitivamente la reserva #{res.Id}?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            var (ok, error) = await _reservaClient.EliminarAsync(res.Id);
            if (!ok)
                MessageBox.Show(error ?? "No se pudo eliminar la reserva.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            await CargarDatosAsync();
        }

        private void AtenderSeleccionada()
        {
            if (dgvReservas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccioná una reserva de la lista para atenderla.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int id = Convert.ToInt32(dgvReservas.SelectedRows[0].Cells["Id"].Value);
            var res = _reservas.FirstOrDefault(r => r.Id == id);
            if (res == null) return;

            if (res.Estado == EstadoReserva.Cancelada)
            {
                MessageBox.Show("No se puede atender una reserva que fue cancelada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (res.Estado == EstadoReserva.Completada)
            {
                MessageBox.Show("Esta reserva ya fue atendida y no se puede modificar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var fAtender = new FormAtenderReserva(id);
            if (fAtender.ShowDialog() == DialogResult.OK)
            {
                _ = CargarDatosAsync();
            }
        }
    }
}