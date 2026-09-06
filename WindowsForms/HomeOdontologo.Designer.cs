namespace WindowsForms
{
    partial class HomeOdontologo
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
            lblBienvenida = new Label();
            SuspendLayout();
            // 
            // lblBienvenida
            // 
            lblBienvenida.AutoSize = true;
            lblBienvenida.Location = new Point(350, 66);
            lblBienvenida.Name = "lblBienvenida";
            lblBienvenida.Size = new Size(38, 15);
            lblBienvenida.TabIndex = 0;
            lblBienvenida.Text = "label1";
            // 
            // HomeOdontologo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblBienvenida);
            Name = "HomeOdontologo";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBienvenida;
    }
}