using System.Drawing;
using System.Windows.Forms;

namespace WindowsForms
{
    partial class PracticasForm
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
            DataGridViewCellStyle estiloPrecio = new DataGridViewCellStyle();
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            dgvPracticas = new DataGridView();
            colCodigo = new DataGridViewTextBoxColumn();
            colDetalle = new DataGridViewTextBoxColumn();
            colPrecio = new DataGridViewTextBoxColumn();
            btnNueva = new Button();
            btnEditar = new Button();
            btnEliminar = new Button();
            lblEstado = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvPracticas).BeginInit();
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
            txtBuscar.PlaceholderText = "Detalle o código...";
            txtBuscar.Size = new Size(602, 23);
            txtBuscar.TabIndex = 1;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            //
            // dgvPracticas
            //
            dgvPracticas.AllowUserToAddRows = false;
            dgvPracticas.AllowUserToDeleteRows = false;
            dgvPracticas.AllowUserToResizeRows = false;
            dgvPracticas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPracticas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPracticas.BackgroundColor = SystemColors.Window;
            dgvPracticas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPracticas.Columns.AddRange(new DataGridViewColumn[] { colCodigo, colDetalle, colPrecio });
            dgvPracticas.Location = new Point(12, 45);
            dgvPracticas.MultiSelect = false;
            dgvPracticas.Name = "dgvPracticas";
            dgvPracticas.ReadOnly = true;
            dgvPracticas.RowHeadersVisible = false;
            dgvPracticas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPracticas.Size = new Size(660, 330);
            dgvPracticas.TabIndex = 2;
            dgvPracticas.CellDoubleClick += dgvPracticas_CellDoubleClick;
            dgvPracticas.SelectionChanged += dgvPracticas_SelectionChanged;
            //
            // colCodigo
            //
            colCodigo.DataPropertyName = "CodigoPractica";
            colCodigo.FillWeight = 20F;
            colCodigo.HeaderText = "Código";
            colCodigo.Name = "colCodigo";
            colCodigo.ReadOnly = true;
            //
            // colDetalle
            //
            colDetalle.DataPropertyName = "Detalle";
            colDetalle.FillWeight = 60F;
            colDetalle.HeaderText = "Detalle";
            colDetalle.Name = "colDetalle";
            colDetalle.ReadOnly = true;
            //
            // colPrecio
            //
            estiloPrecio.Alignment = DataGridViewContentAlignment.MiddleRight;
            estiloPrecio.Format = "N2";
            colPrecio.DataPropertyName = "Precio";
            colPrecio.DefaultCellStyle = estiloPrecio;
            colPrecio.FillWeight = 25F;
            colPrecio.HeaderText = "Precio";
            colPrecio.Name = "colPrecio";
            colPrecio.ReadOnly = true;
            //
            // btnNueva
            //
            btnNueva.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnNueva.Location = new Point(12, 385);
            btnNueva.Name = "btnNueva";
            btnNueva.Size = new Size(120, 30);
            btnNueva.TabIndex = 3;
            btnNueva.Text = "+ Nueva práctica";
            btnNueva.UseVisualStyleBackColor = true;
            btnNueva.Click += btnNueva_Click;
            //
            // btnEditar
            //
            btnEditar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnEditar.Location = new Point(138, 385);
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
            btnEliminar.Location = new Point(234, 385);
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
            lblEstado.Location = new Point(330, 390);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(342, 20);
            lblEstado.TabIndex = 6;
            lblEstado.TextAlign = ContentAlignment.MiddleRight;
            //
            // PracticasForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(684, 427);
            Controls.Add(lblEstado);
            Controls.Add(btnEliminar);
            Controls.Add(btnEditar);
            Controls.Add(btnNueva);
            Controls.Add(dgvPracticas);
            Controls.Add(txtBuscar);
            Controls.Add(lblBuscar);
            MinimumSize = new Size(560, 350);
            Name = "PracticasForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Prácticas";
            Load += PracticasForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvPracticas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBuscar;
        private TextBox txtBuscar;
        private DataGridView dgvPracticas;
        private DataGridViewTextBoxColumn colCodigo;
        private DataGridViewTextBoxColumn colDetalle;
        private DataGridViewTextBoxColumn colPrecio;
        private Button btnNueva;
        private Button btnEditar;
        private Button btnEliminar;
        private Label lblEstado;
    }
}