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
    // Dos reportes: (1) reservas por estado con gráfico de barras, (2) atenciones e ingresos por odontólogo.
    // El gráfico se dibuja con GDI+ para no depender de librerías externas.
    public class FormReportes : Form
    {
        private readonly ReservaApiClient _reservaClient;
        private readonly OdontologoApiClient _odontologoClient;

        private BarChartPanel _chart = null!;
        private DataGridView _dgvEstados = null!;
        private DataGridView _dgvOdontologos = null!;
        private Label _lblTotal = null!;

        public FormReportes()
        {
            var http = new System.Net.Http.HttpClient();
            _reservaClient = new ReservaApiClient(http);
            _odontologoClient = new OdontologoApiClient(http);

            Text = "Reportes";
            Size = new Size(950, 650);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 9f);
            BackColor = Color.FromArgb(248, 250, 252);

            var tabs = new TabControl { Dock = DockStyle.Fill };

            // ---- Reporte 1 ----
            var tab1 = new TabPage("Reservas por estado");
            _lblTotal = new Label { Dock = DockStyle.Top, Height = 30, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Padding = new Padding(10, 6, 0, 0) };
            _chart = new BarChartPanel { Dock = DockStyle.Fill };
            _dgvEstados = CrearGrilla(new[] { ("Estado", "Estado"), ("Cantidad", "Cantidad"), ("Porcentaje", "% del total") });
            _dgvEstados.Dock = DockStyle.Bottom;
            _dgvEstados.Height = 150;
            tab1.Controls.Add(_chart);
            tab1.Controls.Add(_dgvEstados);
            tab1.Controls.Add(_lblTotal);

            // ---- Reporte 2 ----
            var tab2 = new TabPage("Atenciones e ingresos por odontólogo");
            _dgvOdontologos = CrearGrilla(new[]
            {
                ("Odontologo", "Odontólogo"), ("Total", "Reservas"), ("Completadas", "Atendidas"),
                ("Canceladas", "Canceladas"), ("Ingresos", "Ingresos (atendidas)")
            });
            _dgvOdontologos.Dock = DockStyle.Fill;
            tab2.Controls.Add(_dgvOdontologos);

            tabs.TabPages.Add(tab1);
            tabs.TabPages.Add(tab2);

            var btnRef = new Button { Text = "Actualizar", Dock = DockStyle.Bottom, Height = 32, FlatStyle = FlatStyle.Flat };
            btnRef.Click += async (s, e) => await CargarAsync();

            Controls.Add(tabs);
            Controls.Add(btnRef);

            Load += async (s, e) => await CargarAsync();
        }

        private static DataGridView CrearGrilla(IEnumerable<(string name, string header)> columnas)
        {
            var g = new DataGridView
            {
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            foreach (var (n, h) in columnas)
                g.Columns.Add(n, h);
            return g;
        }

        private async Task CargarAsync()
        {
            try
            {
                var token = await AuthServiceProvider.Instance.GetTokenAsync();
                _reservaClient.SetToken(token);
                _odontologoClient.SetToken(token);

                var reservas = await _reservaClient.GetAllAsync();
                if (reservas is null)
                {
                    MessageBox.Show("No se pudieron cargar las reservas para el reporte.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var odos = (await _odontologoClient.GetAllAsync() ?? new())
                    .ToDictionary(o => o.Matricula, o => $"{o.Apellido}, {o.Nombre}");

                // ---- Reporte 1: por estado ----
                var porEstado = Enum.GetValues<EstadoReserva>()
                    .Select(e => (Estado: e, Cant: reservas.Count(r => r.Estado == e)))
                    .ToList();
                int total = reservas.Count;
                _lblTotal.Text = $"Total de reservas: {total}";

                _chart.SetData("Reservas por estado", porEstado.Select(x => (x.Estado.ToString(), (double)x.Cant)));

                _dgvEstados.Rows.Clear();
                foreach (var x in porEstado)
                    _dgvEstados.Rows.Add(x.Estado, x.Cant, total == 0 ? "0%" : $"{x.Cant * 100.0 / total:N1}%");

                // ---- Reporte 2: por odontólogo ----
                _dgvOdontologos.Rows.Clear();
                foreach (var g in reservas.GroupBy(r => r._odontologoMatricula).OrderByDescending(g => g.Count()))
                {
                    string nombre = odos.TryGetValue(g.Key, out var n) ? n : g.Key;
                    var completadas = g.Where(r => r.Estado == EstadoReserva.Completada).ToList();
                    double ingresos = completadas.Sum(r => (double)(r.Importe ?? 0));
                    _dgvOdontologos.Rows.Add(nombre, g.Count(), completadas.Count,
                        g.Count(r => r.Estado == EstadoReserva.Cancelada), $"$ {ingresos:N2}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar reportes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // Gráfico de barras simple dibujado con GDI+
    public class BarChartPanel : Panel
    {
        private string _titulo = string.Empty;
        private List<(string Etiqueta, double Valor)> _datos = new();

        private static readonly Color[] Colores =
        {
            Color.FromArgb(245, 158, 11), Color.FromArgb(37, 99, 235),
            Color.FromArgb(22, 163, 74), Color.FromArgb(107, 114, 128)
        };

        public BarChartPanel()
        {
            DoubleBuffered = true;
            BackColor = Color.White;
            ResizeRedraw = true;
        }

        public void SetData(string titulo, IEnumerable<(string, double)> datos)
        {
            _titulo = titulo;
            _datos = datos.ToList();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using var fTitulo = new Font("Segoe UI", 11f, FontStyle.Bold);
            using var fTexto = new Font("Segoe UI", 9f);
            g.DrawString(_titulo, fTitulo, Brushes.Black, 12, 8);

            if (_datos.Count == 0)
                return;

            var area = new Rectangle(50, 45, Math.Max(10, Width - 80), Math.Max(10, Height - 95));
            double max = Math.Max(1, _datos.Max(d => d.Valor));

            using var pen = new Pen(Color.LightGray);
            for (int i = 0; i <= 4; i++)
            {
                int y = area.Bottom - (int)(area.Height * i / 4.0);
                g.DrawLine(pen, area.Left, y, area.Right, y);
                g.DrawString(Math.Round(max * i / 4.0).ToString(), fTexto, Brushes.Gray, 8, y - 8);
            }

            float slot = (float)area.Width / _datos.Count;
            float barW = slot * 0.55f;

            for (int i = 0; i < _datos.Count; i++)
            {
                var (etiqueta, valor) = _datos[i];
                float h = (float)(area.Height * valor / max);
                float x = area.Left + slot * i + (slot - barW) / 2;
                float y = area.Bottom - h;

                using var br = new SolidBrush(Colores[i % Colores.Length]);
                g.FillRectangle(br, x, y, barW, h);
                g.DrawString(valor.ToString("0"), fTexto, Brushes.Black, x + barW / 2 - 8, y - 18);

                var sz = g.MeasureString(etiqueta, fTexto);
                g.DrawString(etiqueta, fTexto, Brushes.Black, x + barW / 2 - sz.Width / 2, area.Bottom + 6);
            }
        }
    }
}
