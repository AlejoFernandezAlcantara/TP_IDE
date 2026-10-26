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
using WindowsForms.Controles;

namespace WindowsForms
{
    public class FormAtenderReserva : Form
    {
        private readonly int _reservaId;
        private readonly ReservaApiClient _reservaClient;
        private readonly PracticaApiClient _practicaClient;

        private ReservaDTO? _reserva;
        private List<PracticaDTO> _practicas = new();
        private readonly List<DienteControl> _dientesControles = new();
        private readonly List<ReservaPracticaItemDTO> _practicasCargadas = new();

        // Controles de UI
        private Label lblTitulo = null!;
        private Label lblInfoReserva = null!;
        private ComboBox cmbPracticas = null!;
        private NumericUpDown nudPrecio = null!;
        private TextBox txtObservacionesPractica = null!;
        private Label lblPiezasSeleccionadas = null!;
        private Button btnAgregarPractica = null!;
        private Button btnLimpiarDientes = null!;
        private DataGridView dgvPracticas = null!;
        private TextBox txtResultadoFinal = null!;
        private Label lblTotal = null!;
        private Button btnFinalizar = null!;

        public FormAtenderReserva(int reservaId)
        {
            _reservaId = reservaId;
            var http = new System.Net.Http.HttpClient();
            _reservaClient = new ReservaApiClient(http);
            _practicaClient = new PracticaApiClient(http);

            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.Text = "Atender Reserva - Odontograma";
            this.Size = new Size(1100, 780);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            this.BackColor = Color.FromArgb(248, 250, 252);

            // Panel Superior: Encabezado
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 80, Padding = new Padding(15), BackColor = Color.White };
            lblTitulo = new Label
            {
                Text = $"Atención Odontológica - Reserva #{_reservaId}",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(15, 10)
            };
            lblInfoReserva = new Label
            {
                Text = "Cargando datos de la cita...",
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(16, 42)
            };
            pnlTop.Controls.AddRange(new Control[] { lblTitulo, lblInfoReserva });

            // Panel Central Dividido
            var splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                IsSplitterFixed = true,
                BackColor = Color.FromArgb(226, 232, 240)
            };

            // Lado Izquierdo: Odontograma y Asignación
            var pnlIzquierdo = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.FromArgb(248, 250, 252), Padding = new Padding(15) };
            
            var lblOdontoTitulo = new Label
            {
                Text = "Odontograma Anatómico (32 Dientes)",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(15, 10),
                AutoSize = true
            };

            btnLimpiarDientes = new Button
            {
                Text = "Desmarcar Todo",
                Location = new Point(560, 8),
                Size = new Size(130, 28),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(254, 242, 242),
                ForeColor = Color.FromArgb(185, 28, 28)
            };
            btnLimpiarDientes.Click += (s, e) => LimpiarDientes();

            var pnlArcadas = new Panel
            {
                Location = new Point(15, 45),
                Size = new Size(680, 180),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            // Renderizar Arcada Superior (16 dientes: 18 al 28)
            var arcadaSup = OdontogramaConstantes.Dientes.Take(16).ToArray();
            for (int i = 0; i < arcadaSup.Length; i++)
            {
                var dc = new DienteControl(arcadaSup[i]) { Location = new Point(10 + (i * 41), 10) };
                dc.SeleccionCambiada += (s, e) => ActualizarTextoPiezas();
                _dientesControles.Add(dc);
                pnlArcadas.Controls.Add(dc);
            }

            // Renderizar Arcada Inferior (16 dientes: 48 al 38)
            var arcadaInf = OdontogramaConstantes.Dientes.Skip(16).ToArray();
            for (int i = 0; i < arcadaInf.Length; i++)
            {
                var dc = new DienteControl(arcadaInf[i]) { Location = new Point(10 + (i * 41), 90) };
                dc.SeleccionCambiada += (s, e) => ActualizarTextoPiezas();
                _dientesControles.Add(dc);
                pnlArcadas.Controls.Add(dc);
            }

            // Panel de selección de práctica
            var grpPractica = new GroupBox
            {
                Text = "Asignar Práctica a las Piezas Marcadas",
                Location = new Point(15, 235),
                Size = new Size(680, 200),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42)
            };

            var lblP = new Label { Text = "Práctica:", Location = new Point(15, 28), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            cmbPracticas = new ComboBox { Location = new Point(15, 48), Size = new Size(340, 25), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9f) };
            cmbPracticas.SelectedIndexChanged += (s, e) => OnPracticaSeleccionada();

            var lblPre = new Label { Text = "Precio ($):", Location = new Point(370, 28), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            nudPrecio = new NumericUpDown { Location = new Point(370, 48), Size = new Size(120, 25), Maximum = 1000000, DecimalPlaces = 2, Font = new Font("Segoe UI", 9f) };

            lblPiezasSeleccionadas = new Label
            {
                Text = "Piezas afectadas: Ninguna",
                Location = new Point(15, 85),
                Size = new Size(640, 22),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(37, 99, 235)
            };

            var lblObs = new Label { Text = "Observaciones de la práctica (opcional):", Location = new Point(15, 110), AutoSize = true, Font = new Font("Segoe UI", 8.5f) };
            txtObservacionesPractica = new TextBox { Location = new Point(15, 130), Size = new Size(475, 23), Font = new Font("Segoe UI", 9f) };

            btnAgregarPractica = new Button
            {
                Text = "+ Agregar Práctica",
                Location = new Point(505, 126),
                Size = new Size(155, 32),
                BackColor = Color.FromArgb(22, 163, 74),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            btnAgregarPractica.Click += (s, e) => AgregarPractica();

            grpPractica.Controls.AddRange(new Control[] { lblP, cmbPracticas, lblPre, nudPrecio, lblPiezasSeleccionadas, lblObs, txtObservacionesPractica, btnAgregarPractica });

            pnlIzquierdo.Controls.AddRange(new Control[] { lblOdontoTitulo, btnLimpiarDientes, pnlArcadas, grpPractica });

            // Lado Derecho: Lista de Prácticas de la Sesión y Finalizar
            var pnlDerecho = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(15) };
            var lblResumenTitulo = new Label
            {
                Text = "Prácticas en la Sesión",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                Location = new Point(15, 10),
                AutoSize = true
            };

            dgvPracticas = new DataGridView
            {
                Location = new Point(15, 40),
                Size = new Size(330, 280),
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D
            };
            dgvPracticas.Columns.Add("Practica", "Práctica");
            dgvPracticas.Columns.Add("Piezas", "Piezas");
            dgvPracticas.Columns.Add("Precio", "Precio");

            lblTotal = new Label
            {
                Text = "Total: .00",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 163, 74),
                Location = new Point(15, 330),
                AutoSize = true
            };

            var lblResultado = new Label
            {
                Text = "Diagnóstico / Resultado clínico *:",
                Location = new Point(15, 365),
                AutoSize = true,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            txtResultadoFinal = new TextBox
            {
                Location = new Point(15, 390),
                Size = new Size(330, 80),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            btnFinalizar = new Button
            {
                Text = "Finalizar Atención y Guardar",
                Location = new Point(15, 485),
                Size = new Size(330, 42),
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold)
            };
            btnFinalizar.Click += async (s, e) => await FinalizarAtencionAsync();

            pnlDerecho.Controls.AddRange(new Control[] { lblResumenTitulo, dgvPracticas, lblTotal, lblResultado, txtResultadoFinal, btnFinalizar });

            splitContainer.Panel1.Controls.Add(pnlIzquierdo);
            splitContainer.Panel2.Controls.Add(pnlDerecho);

            this.Controls.Add(splitContainer);
            this.Controls.Add(pnlTop);

            // El SplitterDistance se fija recién con el formulario ya dimensionado
            this.Load += async (s, e) =>
            {
                splitContainer.SplitterDistance = 720;
                ActualizarTotal();
                await CargarDatosAsync();
            };
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var token = await AuthServiceProvider.Instance.GetTokenAsync();
                _reservaClient.SetToken(token);
                _practicaClient.SetToken(token);

                _reserva = await _reservaClient.GetByIdAsync(_reservaId);
                _practicas = await _practicaClient.GetAllAsync() ?? new();

                if (_reserva != null)
                {
                    lblInfoReserva.Text = $"Paciente: #{_reserva._pacienteId} | Cobertura: {_reserva.ObraSocial} {(!string.IsNullOrEmpty(_reserva.NroAfiliado) ? $"(Af: {_reserva.NroAfiliado})" : "")} | Estado: {_reserva.Estado} | Turno(s): {_reserva.TurnosTexto}";

                    if (_reserva.Estado == EstadoReserva.Completada || _reserva.Estado == EstadoReserva.Cancelada)
                    {
                        btnFinalizar.Enabled = false;
                        MessageBox.Show($"Esta reserva está {_reserva.Estado} y no se puede volver a atender ni modificar.",
                            "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                cmbPracticas.DataSource = _practicas;
                cmbPracticas.DisplayMember = "Detalle";
                cmbPracticas.ValueMember = "CodigoPractica";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // El API responde {"error":"..."}: mostrar solo el texto
        private static string? ExtraerMensaje(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;
            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(json);
                if (doc.RootElement.ValueKind == System.Text.Json.JsonValueKind.Object &&
                    doc.RootElement.TryGetProperty("error", out var e))
                    return e.GetString();
            }
            catch { }
            return json;
        }

        private void OnPracticaSeleccionada()
        {
            if (cmbPracticas.SelectedItem is PracticaDTO p)
            {
                nudPrecio.Value = (decimal)p.Precio;
            }
        }

        private string ObtenerStringFormateadoPiezas()
        {
            var piezas = new List<string>();
            foreach (var dc in _dientesControles)
            {
                var caras = dc.ObtenerCarasSeleccionadas();
                foreach (var cara in caras)
                {
                    piezas.Add($"{dc.NroDiente}-{cara}");
                }
            }
            return piezas.Count > 0 ? string.Join(", ", piezas) : "General";
        }

        private void ActualizarTextoPiezas()
        {
            var str = ObtenerStringFormateadoPiezas();
            lblPiezasSeleccionadas.Text = $"Piezas afectadas: {str}";
        }

        private void LimpiarDientes()
        {
            foreach (var dc in _dientesControles)
            {
                dc.LimpiarSeleccion();
            }
            ActualizarTextoPiezas();
        }

        private void ActualizarTotal()
        {
            lblTotal.Text = $"Total: $ {_practicasCargadas.Sum(p => p.Precio):N2}";
        }

        private void AgregarPractica()
        {
            if (cmbPracticas.SelectedItem is not PracticaDTO practicaSel)
            {
                MessageBox.Show("Seleccioná una práctica del listado.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string piezasFormateadas = ObtenerStringFormateadoPiezas();

            var item = new ReservaPracticaItemDTO
            {
                PracticaCodigo = practicaSel.CodigoPractica,
                DientesCaras = piezasFormateadas,
                Precio = (float)nudPrecio.Value,
                Observaciones = txtObservacionesPractica.Text
            };

            _practicasCargadas.Add(item);

            dgvPracticas.Rows.Add(practicaSel.Detalle, piezasFormateadas, $"$ {item.Precio:N2}");
            ActualizarTotal();

            // Limpiar selección para la próxima práctica
            LimpiarDientes();
            txtObservacionesPractica.Clear();
        }

        private async Task FinalizarAtencionAsync()
        {
            if (_practicasCargadas.Count == 0)
            {
                MessageBox.Show("Debés agregar al menos una práctica a la atención.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtResultadoFinal.Text))
            {
                MessageBox.Show("Ingresá el resultado clínico de la sesión.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnFinalizar.Enabled = false;
            btnFinalizar.Text = "Guardando...";

            try
            {
                var dto = new AtenderReservaDTO
                {
                    ReservaId = _reservaId,
                    Resultado = txtResultadoFinal.Text,
                    ImporteTotal = _practicasCargadas.Sum(p => p.Precio),
                    Practicas = _practicasCargadas
                };

                var (ok, error) = await _reservaClient.AtenderReservaAsync(dto);
                if (ok)
                {
                    MessageBox.Show("¡Reserva atendida y completada con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(ExtraerMensaje(error) ?? "No se pudo finalizar la atención.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnFinalizar.Enabled = true;
                btnFinalizar.Text = "Finalizar Atención y Guardar";
            }
        }
    }
}