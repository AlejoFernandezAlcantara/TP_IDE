namespace WindowsForms
{
    partial class HomeAdmin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ButtonPacientes = new Button();
            buttonOdontologos = new Button();
            buttonCerrarSesion = new Button();
            lblBienvenida = new Label();
            SuspendLayout();
            // 
            // ButtonPacientes
            // 
            ButtonPacientes.AutoSize = true;
            ButtonPacientes.Location = new Point(204, 261);
            ButtonPacientes.Name = "ButtonPacientes";
            ButtonPacientes.Size = new Size(161, 25);
            ButtonPacientes.TabIndex = 0;
            ButtonPacientes.Text = "Administrar Pacientes";
            ButtonPacientes.UseVisualStyleBackColor = true;
            ButtonPacientes.Click += ButtonPacientes_Click;
            // 
            // buttonOdontologos
            // 
            buttonOdontologos.AutoSize = true;
            buttonOdontologos.Location = new Point(204, 290);
            buttonOdontologos.Name = "buttonOdontologos";
            buttonOdontologos.Size = new Size(161, 25);
            buttonOdontologos.TabIndex = 1;
            buttonOdontologos.Text = "Administrar Odontólogos";
            buttonOdontologos.UseVisualStyleBackColor = true;
            buttonOdontologos.Click += buttonOdontologos_Click;
            // 
            // buttonCerrarSesion
            // 
            buttonCerrarSesion.AutoSize = true;
            buttonCerrarSesion.Location = new Point(204, 348);
            buttonCerrarSesion.Name = "buttonCerrarSesion";
            buttonCerrarSesion.Size = new Size(161, 25);
            buttonCerrarSesion.TabIndex = 2;
            buttonCerrarSesion.Text = "Cerrar Sesión";
            buttonCerrarSesion.UseVisualStyleBackColor = true;
            buttonCerrarSesion.Click += buttonCerrarSesion_Click;
            // 
            // lblBienvenida
            // 
            lblBienvenida.AutoSize = true;
            lblBienvenida.Location = new Point(248, 172);
            lblBienvenida.Name = "lblBienvenida";
            lblBienvenida.Size = new Size(77, 15);
            lblBienvenida.TabIndex = 3;
            lblBienvenida.Text = "Bienvenido/a";
            lblBienvenida.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // HomeAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(577, 516);
            Controls.Add(lblBienvenida);
            Controls.Add(buttonCerrarSesion);
            Controls.Add(buttonOdontologos);
            Controls.Add(ButtonPacientes);
            Name = "HomeAdmin";
            Text = "Clínica Odontológica - Administrador";
            this.Load += new EventHandler(this.HomeAdmin_Load);
            this.Resize += new EventHandler(this.HomeAdmin_Resize);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button ButtonPacientes;
        private Button buttonOdontologos;
        private Button buttonCerrarSesion;
        private Label lblBienvenida;
    }
}