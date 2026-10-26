using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using API.Clients;
using Domain.Model;
using DTO;
using WindowsForms.Auth;

namespace WindowsForms
{
    // Dos reportes: (1) evolución mensual de reservas, atenciones y dinero (igual que en Blazor) con el resumen por estado,
    // (2) atenciones e ingresos por odontólogo.
    // El gráfico se dibuja con GDI+ para no depender de librerías externas.
    public class FormReportes : Form
    {
        private const int MesesDelGrafico = 12;

        private readonly ReservaApiClient _reservaClient;
        private readonly OdontologoApiClient _odontologoClient;

        private EvolucionMensualPanel _chart = null!;
        private CheckBox _chkReservas = null!;
        private CheckBox _chkAtendidos = null!;
        private CheckBox _chkCancelaciones = null!;
        private CheckBox _chkDinero = null!;
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

            // ---- Reporte 1: evolución mensual + resumen por estado ----
            var tab1 = new TabPage("Reservas por estado");

            _chart = new EvolucionMensualPanel { Dock = DockStyle.Fill };

            _dgvEstados = CrearGrilla(new[]
            {
                ("Mes", "Mes"), ("Reservas", "Reservas"), ("Atendidos", "Turnos atendidos"),
                ("Cancelaciones", "Cancelaciones"), ("Dinero", "Dinero")
            });
            _dgvEstados.Dock = DockStyle.Bottom;
            _dgvEstados.Height = 210;

            _lblTotal = new Label { Dock = DockStyle.Top, Height = 28, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Padding = new Padding(10, 6, 0, 0) };

            _chkReservas = CrearCheck("Reservas por mes", true);
            _chkAtendidos = CrearCheck("Turnos atendidos por mes", true);
            _chkCancelaciones = CrearCheck("Cancelaciones por mes", false);
            _chkDinero = CrearCheck("Dinero por mes", true);

            var pnlOpciones = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(6, 2, 0, 0) };
            pnlOpciones.Controls.AddRange(new Control[] { _chkReservas, _chkAtendidos, _chkCancelaciones, _chkDinero });

            var pnlSuperior = new Panel { Dock = DockStyle.Top, Height = 62 };
            pnlSuperior.Controls.Add(pnlOpciones);
            pnlSuperior.Controls.Add(_lblTotal);

            tab1.Controls.Add(_chart);
            tab1.Controls.Add(_dgvEstados);
            tab1.Controls.Add(pnlSuperior);

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

        private CheckBox CrearCheck(string texto, bool marcado)
        {
            var chk = new CheckBox
            {
                Text = texto,
                Checked = marcado,
                AutoSize = true,
                Margin = new Padding(0, 4, 16, 0)
            };
            chk.CheckedChanged += (s, e) => ActualizarSeries();
            return chk;
        }

        private void ActualizarSeries()
        {
            _chart.MostrarReservas = _chkReservas.Checked;
            _chart.MostrarAtendidos = _chkAtendidos.Checked;
            _chart.MostrarCancelaciones = _chkCancelaciones.Checked;
            _chart.MostrarDinero = _chkDinero.Checked;
            _chart.Invalidate();
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

                // ---- Reporte 1: evolución mensual ----
                var meses = CalcularMeses(reservas, MesesDelGrafico);
                _chart.SetData(meses);

                int total = reservas.Count;
                _lblTotal.Text = $"Total de reservas: {total}";

                // ---- Reporte 1: tabla de totales por mes (igual que la tabla general de Blazor) ----
                var es = new CultureInfo("es-AR");
                _dgvEstados.Rows.Clear();
                foreach (var m in meses)
                    _dgvEstados.Rows.Add(m.Mes.ToString("MMMM yyyy", es), m.Reservas, m.Atendidos, m.Cancelaciones, $"$ {m.Dinero:N0}");

                _dgvEstados.Rows.Add("Total", meses.Sum(m => m.Reservas), meses.Sum(m => m.Atendidos),
                    meses.Sum(m => m.Cancelaciones), $"$ {meses.Sum(m => m.Dinero):N0}");
                _dgvEstados.Rows[_dgvEstados.Rows.Count - 1].DefaultCellStyle.Font = new Font(Font, FontStyle.Bold);

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

        // Agrupa cada reserva por el mes de su primer turno (o de creación si no tiene turnos).
        // Las reglas son las mismas que en Reportes.razor de Blazor.
        private static List<DatoMes> CalcularMeses(IEnumerable<ReservaDTO> reservas, int cantidadMeses)
        {
            var hoy = DateTime.Today;
            var primero = new DateTime(hoy.Year, hoy.Month, 1).AddMonths(-(cantidadMeses - 1));
            var lista = new List<DatoMes>();

            for (int i = 0; i < cantidadMeses; i++)
            {
                var mes = primero.AddMonths(i);
                var delMes = reservas
                    .Where(r =>
                    {
                        var fecha = r.PrimerTurno ?? r.FechaCreacion;
                        return fecha.Year == mes.Year && fecha.Month == mes.Month;
                    })
                    .ToList();

                var atendidas = delMes.Where(r => r.Estado == EstadoReserva.Completada).ToList();

                lista.Add(new DatoMes(
                    mes,
                    delMes.Count(r => r.Estado != EstadoReserva.Cancelada),
                    atendidas.Sum(r => r.Turnos.Count),
                    delMes.Count(r => r.Estado == EstadoReserva.Cancelada),
                    atendidas.Sum(r => (double)(r.Importe ?? 0f))));
            }

            return lista;
        }
    }

    // Datos de un mes del gráfico
    public record DatoMes(DateTime Mes, int Reservas, int Atendidos, int Cancelaciones, double Dinero);

    // Gráfico de evolución mensual dibujado con GDI+ (mismas series y colores que el gráfico de Blazor)
    public class EvolucionMensualPanel : Panel
    {
        private static readonly Color ColorReservas = Color.FromArgb(13, 110, 253);
        private static readonly Color ColorAtendidos = Color.FromArgb(25, 135, 84);
        private static readonly Color ColorCancelaciones = Color.FromArgb(220, 53, 69);
        private static readonly Color ColorDinero = Color.FromArgb(201, 162, 39);
        private static readonly CultureInfo Es = new CultureInfo("es-AR");

        private List<DatoMes> _datos = new();

        // Mismas series marcadas por defecto que en Blazor
        public bool MostrarReservas { get; set; } = true;
        public bool MostrarAtendidos { get; set; } = true;
        public bool MostrarCancelaciones { get; set; } = false;
        public bool MostrarDinero { get; set; } = true;

        public EvolucionMensualPanel()
        {
            DoubleBuffered = true;
            BackColor = Color.White;
            ResizeRedraw = true;
        }

        public void SetData(List<DatoMes> datos)
        {
            _datos = datos;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using var fTexto = new Font("Segoe UI", 9f);
            using var fChico = new Font("Segoe UI", 8f);
            using var brTexto = new SolidBrush(Color.FromArgb(73, 80, 87));
            using var brDinero = new SolidBrush(ColorDinero);

            // Leyenda
            var leyenda = new List<(string Nombre, Color Color)>();
            if (MostrarReservas) leyenda.Add(("Reservas", ColorReservas));
            if (MostrarAtendidos) leyenda.Add(("Turnos atendidos", ColorAtendidos));
            if (MostrarCancelaciones) leyenda.Add(("Cancelaciones", ColorCancelaciones));
            if (MostrarDinero) leyenda.Add(("Dinero (eje derecho)", ColorDinero));

            int xLeyenda = 12;
            foreach (var item in leyenda)
            {
                using var brItem = new SolidBrush(item.Color);
                g.FillRectangle(brItem, xLeyenda, 12, 10, 10);
                g.DrawString(item.Nombre, fTexto, brTexto, xLeyenda + 14, 8);
                xLeyenda += 14 + (int)g.MeasureString(item.Nombre, fTexto).Width + 18;
            }

            if (_datos.Count == 0)
                return;

            var area = new Rectangle(60, 40, Math.Max(10, Width - 130), Math.Max(10, Height - 85));
            int n = _datos.Count;

            int maxCuenta = Math.Max(1, _datos.Max(d => Math.Max(
                MostrarReservas ? d.Reservas : 0,
                Math.Max(MostrarAtendidos ? d.Atendidos : 0, MostrarCancelaciones ? d.Cancelaciones : 0))));
            double escIzq = maxCuenta * 1.15;
            double escDer = Math.Max(1, _datos.Max(d => d.Dinero)) * 1.15;

            // Grilla y marcas de los ejes
            using var penGrilla = new Pen(Color.FromArgb(233, 236, 239));
            for (int k = 0; k <= 4; k++)
            {
                float y = area.Bottom - area.Height * k / 4f;
                g.DrawLine(penGrilla, area.Left, y, area.Right, y);
                g.DrawString(Math.Round(escIzq * k / 4.0).ToString("0"), fChico, brTexto, 8, y - 8);
                if (MostrarDinero)
                    g.DrawString(Pesos(escDer * k / 4.0), fChico, brDinero, area.Right + 6, y - 8);
            }

            float grupo = area.Width / (float)n;

            // Barras de cantidades
            var barras = new List<(Color Color, Func<DatoMes, int> Valor)>();
            if (MostrarReservas) barras.Add((ColorReservas, d => d.Reservas));
            if (MostrarAtendidos) barras.Add((ColorAtendidos, d => d.Atendidos));
            if (MostrarCancelaciones) barras.Add((ColorCancelaciones, d => d.Cancelaciones));

            if (barras.Count > 0)
            {
                float anchoBarra = grupo * 0.7f / barras.Count;
                for (int i = 0; i < n; i++)
                {
                    float gx = area.Left + i * grupo + grupo * 0.15f;
                    for (int j = 0; j < barras.Count; j++)
                    {
                        float h = (float)(barras[j].Valor(_datos[i]) / escIzq * area.Height);
                        using var brBarra = new SolidBrush(barras[j].Color);
                        g.FillRectangle(brBarra, gx + j * anchoBarra, area.Bottom - h, Math.Max(1, anchoBarra - 3), h);
                    }
                }
            }

            // Línea de dinero (eje derecho)
            if (MostrarDinero)
            {
                var puntos = new PointF[n];
                for (int i = 0; i < n; i++)
                {
                    float px = area.Left + i * grupo + grupo / 2;
                    float py = area.Bottom - (float)(_datos[i].Dinero / escDer * area.Height);
                    puntos[i] = new PointF(px, py);
                }

                using var penDinero = new Pen(ColorDinero, 3f);
                if (n > 1)
                    g.DrawLines(penDinero, puntos);
                foreach (var p in puntos)
                    g.FillEllipse(brDinero, p.X - 4, p.Y - 4, 8, 8);
            }

            // Etiquetas del eje X (si hay más de 18 meses, se muestra uno de cada dos)
            int paso = n > 18 ? 2 : 1;
            for (int i = 0; i < n; i += paso)
            {
                string etiqueta = _datos[i].Mes.ToString("MMM yy", Es);
                float ancho = g.MeasureString(etiqueta, fChico).Width;
                float px = area.Left + i * grupo + grupo / 2 - ancho / 2;
                g.DrawString(etiqueta, fChico, brTexto, px, area.Bottom + 6);
            }
        }

        private static string Pesos(double v) => "$ " + v.ToString("N0", Es);
    }
}