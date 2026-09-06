namespace WindowsForms
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            label1 = new Label();
            txtEmail = new TextBox();
            label2 = new Label();
            txtPassword = new TextBox();
            btnIngresar = new Button();
            label3 = new Label();
            lblError = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(91, 110);
            label1.Name = "label1";
            label1.Size = new Size(36, 15);
            label1.TabIndex = 0;
            label1.Text = "Email";
            // 
            // txtEmail
            // 
            txtEmail.AccessibleName = "txtEmail";
            txtEmail.Location = new Point(133, 102);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(182, 23);
            txtEmail.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(60, 134);
            label2.Name = "label2";
            label2.Size = new Size(67, 15);
            label2.TabIndex = 2;
            label2.Text = "Contraseña";
            // 
            // txtPassword
            // 
            txtPassword.AccessibleName = "txtPassword";
            txtPassword.Location = new Point(133, 131);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(182, 23);
            txtPassword.TabIndex = 3;
            // 
            // btnIngresar
            // 
            btnIngresar.AccessibleName = "btnIngresar";
            btnIngresar.Location = new Point(240, 170);
            btnIngresar.Name = "btnIngresar";
            btnIngresar.Size = new Size(75, 23);
            btnIngresar.TabIndex = 4;
            btnIngresar.Text = "Ingresar";
            btnIngresar.UseVisualStyleBackColor = true;
            btnIngresar.Click += button1_Click;
            // 
            // label3
            // 
            label3.Location = new Point(0, 0);
            label3.Name = "label3";
            label3.Size = new Size(100, 23);
            label3.TabIndex = 7;
            // 
            // lblError
            // 
            lblError.AccessibleName = "lblError";
            lblError.AutoSize = true;
            lblError.ForeColor = Color.Red;
            lblError.Location = new Point(200, 209);
            lblError.Name = "lblError";
            lblError.Size = new Size(0, 15);
            lblError.TabIndex = 6;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(431, 313);
            Controls.Add(lblError);
            Controls.Add(label3);
            Controls.Add(btnIngresar);
            Controls.Add(txtPassword);
            Controls.Add(label2);
            Controls.Add(txtEmail);
            Controls.Add(label1);
            Name = "LoginForm";
            Text = "LoginForm";
            Load += LoginForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtEmail;
        private Label label2;
        private TextBox txtPassword;
        private Button btnIngresar;
        private Label label3;
        private Label lblError;
    }
}