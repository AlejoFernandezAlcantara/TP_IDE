using WindowsForms.Auth;

namespace WindowsForms
{
    public partial class LoginForm : Form
    {
        // Enlace "Registrate como paciente": se crea acá para no tocar el Designer.
        // Se inicializa en el campo (y no en el constructor) porque InitializeComponent()
        // dispara Resize, y CentrarControles() ya necesita que exista.
        private readonly LinkLabel lnkRegistro = new LinkLabel
        {
            Text = "¿No tenés cuenta? Registrate como paciente",
            AutoSize = true,
            Name = "lnkRegistro"
        };

        public LoginForm()
        {
            InitializeComponent();

            lnkRegistro.Location = new Point(0, lblError.Bottom + 12);
            lnkRegistro.LinkClicked += lnkRegistro_LinkClicked;
            Controls.Add(lnkRegistro);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var email = txtEmail.Text.Trim();
            var password = txtPassword.Text;
            var authService = AuthServiceProvider.Instance;

            var exito = await authService.LoginAsync(email, password);

            if (exito)
            {
                //aca guardo los datos para acceso a los formularios especificos para
                Sesion.Email = email;
                Sesion.Rol = await authService.GetRolAsync();
                Sesion.Nombre = await authService.GetNombreAsync();

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                lblError.Text = "Email o contrasenia incorrectos.";
            }
        }

        private void lnkRegistro_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            using var registro = new RegistroPacienteForm();
            if (registro.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(registro.EmailRegistrado))
            {
                // Después de registrarse deja el email puesto para ingresar directo
                txtEmail.Text = registro.EmailRegistrado;
                txtPassword.Clear();
                lblError.Text = string.Empty;
                txtPassword.Focus();
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            CentrarControles();
        }

        private void LoginForm_Resize(object sender, EventArgs e)
        {
            CentrarControles();
        }

        private void CentrarControles()
        {
            // Los pares label+textbox se centran como fila (mantienen su alineación interna)
            CentrarFila(label1, txtEmail);
            CentrarFila(label2, txtPassword);

            // Los controles sueltos se centran individualmente
            CentrarControl(btnIngresar);
            CentrarControl(lblError);
            CentrarControl(lnkRegistro);

            int gapEmailFila = txtEmail.Top - label1.Top;
            int gapLabel2 = label2.Top - label1.Top;
            int gapPasswordFila = txtPassword.Top - label2.Top;
            int gapBoton = btnIngresar.Top - label2.Top;
            int gapError = lblError.Top - label2.Top;
            int gapRegistro = lnkRegistro.Top - label2.Top;

            int totalAltura = lnkRegistro.Bottom - label1.Top;
            int topInicio = (ClientSize.Height - totalAltura) / 2;

            label1.Top = topInicio;
            txtEmail.Top = label1.Top + gapEmailFila;
            label2.Top = label1.Top + gapLabel2;
            txtPassword.Top = label2.Top + gapPasswordFila;
            btnIngresar.Top = label2.Top + gapBoton;
            lblError.Top = label2.Top + gapError;
            lnkRegistro.Top = label2.Top + gapRegistro;
        }

        private void CentrarControl(Control control)
        {
            control.Left = (ClientSize.Width - control.Width) / 2;
        }

        private void CentrarFila(Control izquierda, Control derecha)
        {
            int gap = derecha.Left - izquierda.Left;
            int anchoFila = (derecha.Left + derecha.Width) - izquierda.Left;

            izquierda.Left = (ClientSize.Width - anchoFila) / 2;
            derecha.Left = izquierda.Left + gap;
        }
    }
}
