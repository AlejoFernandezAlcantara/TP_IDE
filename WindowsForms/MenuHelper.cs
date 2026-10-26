using System.Drawing;
using System.Windows.Forms;

namespace WindowsForms
{
    // Arma el menú principal de cada rol como una columna de botones centrada//////
    public static class MenuHelper
    {
        public static Button Boton(string texto, EventHandler onClick)
        {
            var b = new Button
            {
                Text = texto,
                AutoSize = false,
                Size = new Size(240, 32),
                UseVisualStyleBackColor = true
            };
            b.Click += onClick;
            return b;
        }

        public static void Apilar(Form form, Label bienvenida, params Button[] botones)
        {
            foreach (var b in botones)
                if (!form.Controls.Contains(b))
                    form.Controls.Add(b);

            void Distribuir()
            {
                const int separacion = 8;
                int alto = bienvenida.Height + 20 + botones.Sum(b => b.Height + separacion);
                int y = Math.Max(20, (form.ClientSize.Height - alto) / 2);

                bienvenida.Location = new Point((form.ClientSize.Width - bienvenida.Width) / 2, y);
                y = bienvenida.Bottom + 20;

                foreach (var b in botones)
                {
                    b.Location = new Point((form.ClientSize.Width - b.Width) / 2, y);
                    y += b.Height + separacion;
                }
            }

            form.Resize += (s, e) => Distribuir();
            Distribuir();
        }
    }
}
