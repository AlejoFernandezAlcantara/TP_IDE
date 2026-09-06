
using Microsoft.Data.SqlClient;
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
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                lblError.Text = "Email o contraseña incorrectos.";
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}