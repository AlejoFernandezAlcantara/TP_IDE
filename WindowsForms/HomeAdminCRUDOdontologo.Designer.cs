namespace WindowsForms
{
    partial class HomeAdminCRUDOdontologo
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
            components = new System.ComponentModel.Container();
            dataGridView1 = new DataGridView();
            odontologoDTOBindingSource = new BindingSource(components);
            buttonAdd = new Button();
            buttonEdit = new Button();
            buttonDelete = new Button();
            buttonReturn = new Button();
            textNroDocumento = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            textApellido = new TextBox();
            label4 = new Label();
            textNombre = new TextBox();
            label5 = new Label();
            textContraseña = new TextBox();
            label6 = new Label();
            textEmail = new TextBox();
            label7 = new Label();
            label8 = new Label();
            textMatricula = new TextBox();
            textEspecialidad = new TextBox();
            cmbTipoDocumento = new ComboBox();
            labelAction = new Label();
            matriculaDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            nroDocumentoDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            tipoDocumentoDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            especialidadDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            nombreDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            apellidoDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            emailDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)odontologoDTOBindingSource).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { matriculaDataGridViewTextBoxColumn, nroDocumentoDataGridViewTextBoxColumn, tipoDocumentoDataGridViewTextBoxColumn, especialidadDataGridViewTextBoxColumn, nombreDataGridViewTextBoxColumn, apellidoDataGridViewTextBoxColumn, emailDataGridViewTextBoxColumn });
            dataGridView1.DataSource = odontologoDTOBindingSource;
            dataGridView1.Location = new Point(0, 1);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1105, 601);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            // 
            // odontologoDTOBindingSource
            // 
            odontologoDTOBindingSource.DataSource = typeof(DTO.OdontologoDTO);
            // 
            // buttonAdd
            // 
            buttonAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonAdd.Location = new Point(1143, 391);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(192, 23);
            buttonAdd.TabIndex = 1;
            buttonAdd.Text = "Añadir nuevo odontólogo";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += buttonAdd_Click;
            // 
            // buttonEdit
            // 
            buttonEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonEdit.Location = new Point(1143, 420);
            buttonEdit.Name = "buttonEdit";
            buttonEdit.Size = new Size(192, 23);
            buttonEdit.TabIndex = 2;
            buttonEdit.Text = "Editar odontólogo";
            buttonEdit.UseVisualStyleBackColor = true;
            buttonEdit.Click += buttonEdit_Click;
            // 
            // buttonDelete
            // 
            buttonDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonDelete.Location = new Point(1143, 449);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(192, 23);
            buttonDelete.TabIndex = 3;
            buttonDelete.Text = "Eliminar odontólogo";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += buttonDelete_Click;
            // 
            // buttonReturn
            // 
            buttonReturn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonReturn.Location = new Point(1143, 506);
            buttonReturn.Name = "buttonReturn";
            buttonReturn.Size = new Size(192, 23);
            buttonReturn.TabIndex = 4;
            buttonReturn.Text = "Volver";
            buttonReturn.UseVisualStyleBackColor = true;
            buttonReturn.Click += buttonReturn_Click;
            // 
            // textNroDocumento
            // 
            textNroDocumento.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textNroDocumento.Location = new Point(1111, 69);
            textNroDocumento.Name = "textNroDocumento";
            textNroDocumento.Size = new Size(269, 23);
            textNroDocumento.TabIndex = 6;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(1111, 4);
            label1.Name = "label1";
            label1.Size = new Size(113, 15);
            label1.TabIndex = 7;
            label1.Text = "Tipo de Documento";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(1111, 51);
            label2.Name = "label2";
            label2.Size = new Size(108, 15);
            label2.TabIndex = 8;
            label2.Text = "Nro de documento";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(1111, 95);
            label3.Name = "label3";
            label3.Size = new Size(51, 15);
            label3.TabIndex = 10;
            label3.Text = "Apellido";
            // 
            // textApellido
            // 
            textApellido.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textApellido.Location = new Point(1111, 113);
            textApellido.Name = "textApellido";
            textApellido.Size = new Size(269, 23);
            textApellido.TabIndex = 9;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(1111, 139);
            label4.Name = "label4";
            label4.Size = new Size(51, 15);
            label4.TabIndex = 12;
            label4.Text = "Nombre";
            // 
            // textNombre
            // 
            textNombre.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textNombre.Location = new Point(1111, 157);
            textNombre.Name = "textNombre";
            textNombre.Size = new Size(269, 23);
            textNombre.TabIndex = 11;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Location = new Point(1111, 320);
            label5.Name = "label5";
            label5.Size = new Size(67, 15);
            label5.TabIndex = 20;
            label5.Text = "Contraseña";
            // 
            // textContraseña
            // 
            textContraseña.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textContraseña.Location = new Point(1111, 338);
            textContraseña.Name = "textContraseña";
            textContraseña.Size = new Size(269, 23);
            textContraseña.TabIndex = 19;
            textContraseña.UseSystemPasswordChar = true;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Location = new Point(1111, 276);
            label6.Name = "label6";
            label6.Size = new Size(36, 15);
            label6.TabIndex = 18;
            label6.Text = "Email";
            // 
            // textEmail
            // 
            textEmail.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textEmail.Location = new Point(1111, 294);
            textEmail.Name = "textEmail";
            textEmail.Size = new Size(269, 23);
            textEmail.TabIndex = 17;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label7.AutoSize = true;
            label7.Location = new Point(1111, 232);
            label7.Name = "label7";
            label7.Size = new Size(57, 15);
            label7.TabIndex = 16;
            label7.Text = "Matricula";
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label8.AutoSize = true;
            label8.Location = new Point(1111, 185);
            label8.Name = "label8";
            label8.Size = new Size(72, 15);
            label8.TabIndex = 15;
            label8.Text = "Especialidad";
            // 
            // textMatricula
            // 
            textMatricula.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textMatricula.Location = new Point(1111, 250);
            textMatricula.Name = "textMatricula";
            textMatricula.Size = new Size(269, 23);
            textMatricula.TabIndex = 14;
            // 
            // textEspecialidad
            // 
            textEspecialidad.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textEspecialidad.Location = new Point(1111, 203);
            textEspecialidad.Name = "textEspecialidad";
            textEspecialidad.Size = new Size(269, 23);
            textEspecialidad.TabIndex = 13;
            // 
            // cmbTipoDocumento
            // 
            cmbTipoDocumento.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbTipoDocumento.FormattingEnabled = true;
            cmbTipoDocumento.Location = new Point(1111, 22);
            cmbTipoDocumento.Name = "cmbTipoDocumento";
            cmbTipoDocumento.Size = new Size(269, 23);
            cmbTipoDocumento.TabIndex = 21;
            // 
            // labelAction
            // 
            labelAction.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelAction.AutoSize = true;
            labelAction.BackColor = SystemColors.ButtonHighlight;
            labelAction.Location = new Point(1236, 551);
            labelAction.Name = "labelAction";
            labelAction.Size = new Size(0, 15);
            labelAction.TabIndex = 22;
            // 
            // matriculaDataGridViewTextBoxColumn
            // 
            matriculaDataGridViewTextBoxColumn.DataPropertyName = "Matricula";
            matriculaDataGridViewTextBoxColumn.HeaderText = "Matricula";
            matriculaDataGridViewTextBoxColumn.MinimumWidth = 6;
            matriculaDataGridViewTextBoxColumn.Name = "matriculaDataGridViewTextBoxColumn";
            matriculaDataGridViewTextBoxColumn.Width = 125;
            // 
            // nroDocumentoDataGridViewTextBoxColumn
            // 
            nroDocumentoDataGridViewTextBoxColumn.DataPropertyName = "NroDocumento";
            nroDocumentoDataGridViewTextBoxColumn.HeaderText = "NroDocumento";
            nroDocumentoDataGridViewTextBoxColumn.MinimumWidth = 6;
            nroDocumentoDataGridViewTextBoxColumn.Name = "nroDocumentoDataGridViewTextBoxColumn";
            nroDocumentoDataGridViewTextBoxColumn.Width = 125;
            // 
            // tipoDocumentoDataGridViewTextBoxColumn
            // 
            tipoDocumentoDataGridViewTextBoxColumn.DataPropertyName = "TipoDocumento";
            tipoDocumentoDataGridViewTextBoxColumn.HeaderText = "TipoDocumento";
            tipoDocumentoDataGridViewTextBoxColumn.MinimumWidth = 6;
            tipoDocumentoDataGridViewTextBoxColumn.Name = "tipoDocumentoDataGridViewTextBoxColumn";
            tipoDocumentoDataGridViewTextBoxColumn.Width = 125;
            // 
            // especialidadDataGridViewTextBoxColumn
            // 
            especialidadDataGridViewTextBoxColumn.DataPropertyName = "Especialidad";
            especialidadDataGridViewTextBoxColumn.HeaderText = "Especialidad";
            especialidadDataGridViewTextBoxColumn.MinimumWidth = 6;
            especialidadDataGridViewTextBoxColumn.Name = "especialidadDataGridViewTextBoxColumn";
            especialidadDataGridViewTextBoxColumn.Width = 125;
            // 
            // nombreDataGridViewTextBoxColumn
            // 
            nombreDataGridViewTextBoxColumn.DataPropertyName = "Nombre";
            nombreDataGridViewTextBoxColumn.HeaderText = "Nombre";
            nombreDataGridViewTextBoxColumn.MinimumWidth = 6;
            nombreDataGridViewTextBoxColumn.Name = "nombreDataGridViewTextBoxColumn";
            nombreDataGridViewTextBoxColumn.Width = 125;
            // 
            // apellidoDataGridViewTextBoxColumn
            // 
            apellidoDataGridViewTextBoxColumn.DataPropertyName = "Apellido";
            apellidoDataGridViewTextBoxColumn.HeaderText = "Apellido";
            apellidoDataGridViewTextBoxColumn.MinimumWidth = 6;
            apellidoDataGridViewTextBoxColumn.Name = "apellidoDataGridViewTextBoxColumn";
            apellidoDataGridViewTextBoxColumn.Width = 125;
            // 
            // emailDataGridViewTextBoxColumn
            // 
            emailDataGridViewTextBoxColumn.DataPropertyName = "Email";
            emailDataGridViewTextBoxColumn.HeaderText = "Email";
            emailDataGridViewTextBoxColumn.MinimumWidth = 6;
            emailDataGridViewTextBoxColumn.Name = "emailDataGridViewTextBoxColumn";
            emailDataGridViewTextBoxColumn.Width = 125;
            // 
            // HomeAdminCRUDOdontologo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1387, 601);
            Controls.Add(labelAction);
            Controls.Add(cmbTipoDocumento);
            Controls.Add(label5);
            Controls.Add(textContraseña);
            Controls.Add(label6);
            Controls.Add(textEmail);
            Controls.Add(label7);
            Controls.Add(label8);
            Controls.Add(textMatricula);
            Controls.Add(textEspecialidad);
            Controls.Add(label4);
            Controls.Add(textNombre);
            Controls.Add(label3);
            Controls.Add(textApellido);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textNroDocumento);
            Controls.Add(buttonReturn);
            Controls.Add(buttonDelete);
            Controls.Add(buttonEdit);
            Controls.Add(buttonAdd);
            Controls.Add(dataGridView1);
            Name = "HomeAdminCRUDOdontologo";
            Text = "Gestion de odontologos";
            Load += FormCrudOdontologos_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)odontologoDTOBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private Button buttonAdd;
        private Button buttonEdit;
        private Button buttonDelete;
        private Button buttonReturn;
        private TextBox textNroDocumento;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox textApellido;
        private Label label4;
        private TextBox textNombre;
        private Label label5;
        private TextBox textContraseña;
        private Label label6;
        private TextBox textEmail;
        private Label label7;
        private Label label8;
        private TextBox textMatricula;
        private TextBox textEspecialidad;
        private BindingSource odontologoDTOBindingSource;
        private ComboBox cmbTipoDocumento;
        private Label labelAction;
        private DataGridViewTextBoxColumn matriculaDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn nroDocumentoDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn tipoDocumentoDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn especialidadDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn nombreDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn apellidoDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn emailDataGridViewTextBoxColumn;
    }
}