using System.Drawing;
using System.Windows.Forms;

namespace WindowsForms
{
    partial class MutualesForm
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
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            dgvMutuales = new DataGridView();
            colCuit = new DataGridViewTextBoxColumn();
            colNombre = new DataGridViewTextBoxColumn();
            btnNueva = new Button();
            btnEditar = new Button();
            btnEliminar = new Button();
            lblEstado = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvMutuales).BeginInit();
            SuspendLayout();
            //
            // lblBuscar
            //
            lblBuscar.AutoSize = true;
            lblBuscar.Location = new Point(12, 15);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(45, 15);
            lblBuscar.TabIndex = 0;
            lblBuscar.Text = "Buscar:";
            //
            // txtBuscar
            //
            txtBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtBuscar.Location = new Point(70, 12);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Nombre o CUIT...";
            txtBuscar.Size = new Size(502, 23);
            txtBuscar.TabIndex = 1;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            //
            // dgvMutuales
            //
            dgvMutuales.AllowUserToAddRows = false;
            dgvMutuales.AllowUserToDeleteRows = false;
            dgvMutuales.AllowUserToResizeRows = false;
            dgvMutuales.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMutuales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMutuales.BackgroundColor = SystemColors.Window;
            dgvMutuales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMutuales.Columns.AddRange(new DataGridViewColumn[] { colCuit, colNombre });
            dgvMutuales.Location = new Point(12, 45);
            dgvMutuales.MultiSelect = false;
            dgvMutuales.Name = "dgvMutuales";
            dgvMutuales.ReadOnly = true;
            dgvMutuales.RowHeadersVisible = false;
            dgvMutuales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMutuales.Size = new Size(560, 330);
            dgvMutuales.TabIndex = 2;
            dgvMutuales.CellDoubleClick += dgvMutuales_CellDoubleClick;
            dgvMutuales.SelectionChanged += dgvMutuales_SelectionChanged;
            //
            // colCuit
            //
            colCuit.DataPropertyName = "Cuit";
            colCuit.FillWeight = 35F;
            colCuit.HeaderText = "CUIT";
            colCuit.Name = "colCuit";
            colCuit.ReadOnly = true;
            //
            // colNombre
            //
            colNombre.DataPropertyName = "Nombre";
            colNombre.FillWeight = 65F;
            colNombre.HeaderText = "Nombre";
            colNombre.Name = "colNombre";
            colNombre.ReadOnly = true;
            //
            // btnNueva
            //
            btnNueva.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnNueva.Location = new Point(12, 385);
            btnNueva.Name = "btnNueva";
            btnNueva.Size = new Size(110, 30);
            btnNueva.TabIndex = 3;
            btnNueva.Text = "+ Nueva mutual";
            btnNueva.UseVisualStyleBackColor = true;
            btnNueva.Click += btnNueva_Click;
            //
            // btnEditar
            //
            btnEditar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnEditar.Location = new Point(128, 385);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(90, 30);
            btnEditar.TabIndex = 4;
            btnEditar.Text = "Editar";
            btnEditar.UseVisualStyleBackColor = true;
            btnEditar.Click += btnEditar_Click;
            //
            // btnEliminar
            //
            btnEliminar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnEliminar.Location = new Point(224, 385);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(90, 30);
            btnEliminar.TabIndex = 5;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += btnEliminar_Click;
            //
            // lblEstado
            //
            lblEstado.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblEstado.ForeColor = SystemColors.GrayText;
            lblEstado.Location = new Point(320, 390);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(252, 20);
            lblEstado.TabIndex = 6;
            lblEstado.TextAlign = ContentAlignment.MiddleRight;
            //
            // MutualesForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 427);
            Controls.Add(lblEstado);
            Controls.Add(btnEliminar);
            Controls.Add(btnEditar);
            Controls.Add(btnNueva);
            Controls.Add(dgvMutuales);
            Controls.Add(txtBuscar);
            Controls.Add(lblBuscar);
            MinimumSize = new Size(500, 350);
            Name = "MutualesForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Mutuales";
            Load += MutualesForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvMutuales).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBuscar;
        private TextBox txtBuscar;
        private DataGridView dgvMutuales;
        private DataGridViewTextBoxColumn colCuit;
        private DataGridViewTextBoxColumn colNombre;
        private Button btnNueva;
        private Button btnEditar;
        private Button btnEliminar;
        private Label lblEstado;
    }
}