using System.Drawing;
using System.Windows.Forms;

namespace WindowsForms
{
    partial class MutualDetalleForm
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
            components = new System.ComponentModel.Container();
            lblCuit = new Label();
            txtCuit = new TextBox();
            lblAyudaCuit = new Label();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblError = new Label();
            btnGuardar = new Button();
            btnCancelar = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // lblCuit
            //
            lblCuit.AutoSize = true;
            lblCuit.Location = new Point(15, 15);
            lblCuit.Name = "lblCuit";
            lblCuit.Size = new Size(45, 15);
            lblCuit.TabIndex = 0;
            lblCuit.Text = "CUIT *";
            //
            // txtCuit
            //
            txtCuit.Location = new Point(15, 35);
            txtCuit.MaxLength = 10;
            txtCuit.Name = "txtCuit";
            txtCuit.Size = new Size(350, 23);
            txtCuit.TabIndex = 1;
            //
            // lblAyudaCuit
            //
            lblAyudaCuit.AutoSize = true;
            lblAyudaCuit.ForeColor = SystemColors.GrayText;
            lblAyudaCuit.Location = new Point(15, 61);
            lblAyudaCuit.Name = "lblAyudaCuit";
            lblAyudaCuit.Size = new Size(70, 15);
            lblAyudaCuit.TabIndex = 2;
            lblAyudaCuit.Text = "Sin guiones.";
            //
            // lblNombre
            //
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(15, 88);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(61, 15);
            lblNombre.TabIndex = 3;
            lblNombre.Text = "Nombre *";
            //
            // txtNombre
            //
            txtNombre.Location = new Point(15, 108);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(350, 23);
            txtNombre.TabIndex = 4;
            //
            // lblError
            //
            lblError.ForeColor = Color.Firebrick;
            lblError.Location = new Point(15, 140);
            lblError.Name = "lblError";
            lblError.Size = new Size(370, 35);
            lblError.TabIndex = 5;
            lblError.Visible = false;
            //
            // btnGuardar
            //
            btnGuardar.Location = new Point(235, 182);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(130, 30);
            btnGuardar.TabIndex = 6;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(139, 182);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(90, 30);
            btnCancelar.TabIndex = 7;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // errorProvider
            //
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            //
            // MutualDetalleForm
            //
            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(400, 225);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(lblError);
            Controls.Add(txtNombre);
            Controls.Add(lblNombre);
            Controls.Add(lblAyudaCuit);
            Controls.Add(txtCuit);
            Controls.Add(lblCuit);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MutualDetalleForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Mutual";
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCuit;
        private TextBox txtCuit;
        private Label lblAyudaCuit;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblError;
        private Button btnGuardar;
        private Button btnCancelar;
        private ErrorProvider errorProvider;
    }
}