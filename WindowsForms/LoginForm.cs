

using WindowsForms.Auth;

namespace WindowsForms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
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

            int gapEmailFila = txtEmail.Top - label1.Top;
            int gapLabel2 = label2.Top - label1.Top;
            int gapPasswordFila = txtPassword.Top - label2.Top;
            int gapBoton = btnIngresar.Top - label2.Top;
            int gapError = lblError.Top - label2.Top;

            int totalAltura = lblError.Bottom - label1.Top;
            int topInicio = (ClientSize.Height - totalAltura) / 2;

            label1.Top = topInicio;
            txtEmail.Top = label1.Top + gapEmailFila;
            label2.Top = label1.Top + gapLabel2;
            txtPassword.Top = label2.Top + gapPasswordFila;
            btnIngresar.Top = label2.Top + gapBoton;
            lblError.Top = label2.Top + gapError;
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