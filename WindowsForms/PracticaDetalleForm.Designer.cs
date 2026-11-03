using System.Drawing;
using System.Windows.Forms;

namespace WindowsForms
{
    partial class PracticaDetalleForm
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
            lblCodigo = new Label();
            txtCodigo = new TextBox();
            lblDetalle = new Label();
            txtDetalle = new TextBox();
            lblPrecio = new Label();
            nudPrecio = new NumericUpDown();
            lblError = new Label();
            btnGuardar = new Button();
            btnCancelar = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)nudPrecio).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // lblCodigo
            //
            lblCodigo.AutoSize = true;
            lblCodigo.Location = new Point(15, 15);
            lblCodigo.Name = "lblCodigo";
            lblCodigo.Size = new Size(46, 15);
            lblCodigo.TabIndex = 0;
            lblCodigo.Text = "Código";
            //
            // txtCodigo
            //
            txtCodigo.Location = new Point(15, 35);
            txtCodigo.Name = "txtCodigo";
            txtCodigo.ReadOnly = true;
            txtCodigo.Size = new Size(120, 23);
            txtCodigo.TabIndex = 1;
            txtCodigo.TabStop = false;
            //
            // lblDetalle
            //
            lblDetalle.AutoSize = true;
            lblDetalle.Location = new Point(15, 68);
            lblDetalle.Name = "lblDetalle";
            lblDetalle.Size = new Size(55, 15);
            lblDetalle.TabIndex = 2;
            lblDetalle.Text = "Detalle *";
            //
            // txtDetalle
            //
            txtDetalle.Location = new Point(15, 88);
            txtDetalle.Name = "txtDetalle";
            txtDetalle.Size = new Size(350, 23);
            txtDetalle.TabIndex = 3;
            //
            // lblPrecio
            //
            lblPrecio.AutoSize = true;
            lblPrecio.Location = new Point(15, 121);
            lblPrecio.Name = "lblPrecio";
            lblPrecio.Size = new Size(48, 15);
            lblPrecio.TabIndex = 4;
            lblPrecio.Text = "Precio *";
            //
            // nudPrecio
            //
            nudPrecio.DecimalPlaces = 2;
            nudPrecio.Location = new Point(15, 141);
            nudPrecio.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            nudPrecio.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            nudPrecio.Name = "nudPrecio";
            nudPrecio.Size = new Size(150, 23);
            nudPrecio.TabIndex = 5;
            nudPrecio.TextAlign = HorizontalAlignment.Right;
            nudPrecio.ThousandsSeparator = true;
            //
            // lblError
            //
            lblError.ForeColor = Color.Firebrick;
            lblError.Location = new Point(15, 172);
            lblError.Name = "lblError";
            lblError.Size = new Size(370, 35);
            lblError.TabIndex = 6;
            lblError.Visible = false;
            //
            // btnGuardar
            //
            btnGuardar.Location = new Point(235, 212);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(130, 30);
            btnGuardar.TabIndex = 7;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(139, 212);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(90, 30);
            btnCancelar.TabIndex = 8;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // errorProvider
            //
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            //
            // PracticaDetalleForm
            //
            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(400, 255);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(lblError);
            Controls.Add(nudPrecio);
            Controls.Add(lblPrecio);
            Controls.Add(txtDetalle);
            Controls.Add(lblDetalle);
            Controls.Add(txtCodigo);
            Controls.Add(lblCodigo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "PracticaDetalleForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Práctica";
            ((System.ComponentModel.ISupportInitialize)nudPrecio).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCodigo;
        private TextBox txtCodigo;
        private Label lblDetalle;
        private TextBox txtDetalle;
        private Label lblPrecio;
        private NumericUpDown nudPrecio;
        private Label lblError;
        private Button btnGuardar;
        private Button btnCancelar;
        private ErrorProvider errorProvider;
    }
}