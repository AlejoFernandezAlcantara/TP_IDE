using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsForms.Controles
{
    public class DienteControl : UserControl
    {
        public int NroDiente { get; private set; }

        private Label lblNumero = null!;
        private Button btnV = null!; // Vestibular (Arriba)
        private Button btnM = null!; // Mesial (Izquierda)
        private Button btnO = null!; // Oclusal (Centro)
        private Button btnD = null!; // Distal (Derecha)
        private Button btnL = null!; // Lingual / Palatina (Abajo)

        private readonly Dictionary<string, bool> carasSeleccionadas = new()
        {
            { "V", false },
            { "M", false },
            { "O", false },
            { "D", false },
            { "L", false }
        };

        private readonly Color colorDefault = Color.White;
        private readonly Color colorSeleccionado = Color.FromArgb(37, 99, 235); // Azul moderno
        private readonly Color colorCentroDefault = Color.FromArgb(241, 245, 249);

        public event EventHandler? SeleccionCambiada;

        public DienteControl() : this(11) { }

        public DienteControl(int nroDiente)
        {
            NroDiente = nroDiente;
            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.Size = new Size(58, 68);
            this.BorderStyle = BorderStyle.FixedSingle;
            this.BackColor = Color.White;

            lblNumero = new Label
            {
                Text = NroDiente.ToString(),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                Dock = DockStyle.Top,
                Height = 16
            };

            var panelCruz = new Panel
            {
                Size = new Size(42, 42),
                Location = new Point(7, 18),
                BackColor = Color.Transparent
            };

            int sz = 13;
            btnV = CrearBotonCara("V", new Point(14, 0), new Size(sz, sz), "Vestibular");
            btnM = CrearBotonCara("M", new Point(0, 14), new Size(sz, sz), "Mesial");
            btnO = CrearBotonCara("O", new Point(14, 14), new Size(sz, sz), "Oclusal");
            btnO.BackColor = colorCentroDefault;
            btnD = CrearBotonCara("D", new Point(28, 14), new Size(sz, sz), "Distal");
            btnL = CrearBotonCara("L", new Point(14, 28), new Size(sz, sz), "Lingual");

            panelCruz.Controls.AddRange(new Control[] { btnV, btnM, btnO, btnD, btnL });

            this.Controls.Add(panelCruz);
            this.Controls.Add(lblNumero);
        }

        private Button CrearBotonCara(string cara, Point loc, Size sz, string tooltip)
        {
            var btn = new Button
            {
                Text = cara,
                Location = loc,
                Size = sz,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 5.5f, FontStyle.Bold),
                Margin = new Padding(0),
                Padding = new Padding(0),
                BackColor = colorDefault,
                ForeColor = Color.FromArgb(51, 65, 85)
            };
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Color.FromArgb(148, 163, 184);

            btn.Click += (s, e) => ToggleCara(cara, btn);
            return btn;
        }

        private void ToggleCara(string cara, Button btn)
        {
            bool estadoActual = carasSeleccionadas[cara];
            bool nuevoEstado = !estadoActual;
            carasSeleccionadas[cara] = nuevoEstado;

            if (nuevoEstado)
            {
                btn.BackColor = colorSeleccionado;
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderColor = Color.FromArgb(29, 78, 216);
            }
            else
            {
                btn.BackColor = cara == "O" ? colorCentroDefault : colorDefault;
                btn.ForeColor = Color.FromArgb(51, 65, 85);
                btn.FlatAppearance.BorderColor = Color.FromArgb(148, 163, 184);
            }

            SeleccionCambiada?.Invoke(this, EventArgs.Empty);
        }

        public List<string> ObtenerCarasSeleccionadas()
        {
            var seleccionadas = new List<string>();
            foreach (var kvp in carasSeleccionadas)
            {
                if (kvp.Value)
                    seleccionadas.Add(kvp.Key);
            }
            return seleccionadas;
        }

        public void LimpiarSeleccion()
        {
            foreach (var key in new List<string>(carasSeleccionadas.Keys))
            {
                carasSeleccionadas[key] = false;
            }

            btnV.BackColor = colorDefault; btnV.ForeColor = Color.FromArgb(51, 65, 85);
            btnM.BackColor = colorDefault; btnM.ForeColor = Color.FromArgb(51, 65, 85);
            btnO.BackColor = colorCentroDefault; btnO.ForeColor = Color.FromArgb(51, 65, 85);
            btnD.BackColor = colorDefault; btnD.ForeColor = Color.FromArgb(51, 65, 85);
            btnL.BackColor = colorDefault; btnL.ForeColor = Color.FromArgb(51, 65, 85);
        }
    }
}