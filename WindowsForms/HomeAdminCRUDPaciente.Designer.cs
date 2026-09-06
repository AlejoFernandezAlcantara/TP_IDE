namespace WindowsForms
{
    partial class HomeAdminCRUDPaciente
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

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            labelAction = new Label();
            cmbTipoDocumento = new ComboBox();
            label5 = new Label();
            textContraseña = new TextBox();
            label6 = new Label();
            textEmail = new TextBox();
            label7 = new Label();
            label8 = new Label();
            textTelefono = new TextBox();
            textDireccion = new TextBox();
            label4 = new Label();
            textNombre = new TextBox();
            label3 = new Label();
            textApellido = new TextBox();
            label2 = new Label();
            label1 = new Label();
            textNroDni = new TextBox();
            buttonReturn = new Button();
            buttonDelete = new Button();
            buttonEdit = new Button();
            buttonAdd = new Button();
            dataGridView1 = new DataGridView();
            nroPacienteDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            nombreDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            apellidoDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            nroDniDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            tipoDniDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            direccionDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            telefonoDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            emailDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            passwordDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            pacienteDTOBindingSource = new BindingSource(components);
            lblNroPaciente = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pacienteDTOBindingSource).BeginInit();
            SuspendLayout();

            // labelAction
            labelAction.AutoSize = true;
            labelAction.BackColor = SystemColors.ButtonHighlight;
            labelAction.Location = new Point(1019, 555);
            labelAction.Name = "labelAction";
            labelAction.Size = new Size(0, 15);
            labelAction.TabIndex = 44;
            labelAction.Click += new EventHandler(labelAction_Click);

            // cmbTipoDocumento
            cmbTipoDocumento.FormattingEnabled = true;
            cmbTipoDocumento.Location = new Point(948, 19);
            cmbTipoDocumento.Name = "cmbTipoDocumento";
            cmbTipoDocumento.Size = new Size(269, 23);
            cmbTipoDocumento.TabIndex = 43;

            // label5
            label5.AutoSize = true;
            label5.Location = new Point(948, 317);
            label5.Name = "label5";
            label5.Size = new Size(67, 15);
            label5.TabIndex = 42;
            label5.Text = "Contraseña";

            // textContraseña
            textContraseña.Location = new Point(948, 335);
            textContraseña.Name = "textContraseña";
            textContraseña.Size = new Size(269, 23);
            textContraseña.TabIndex = 41;
            textContraseña.UseSystemPasswordChar = true;

            // label6
            label6.AutoSize = true;
            label6.Location = new Point(948, 273);
            label6.Name = "label6";
            label6.Size = new Size(36, 15);
            label6.TabIndex = 40;
            label6.Text = "Email";

            // textEmail
            textEmail.Location = new Point(948, 291);
            textEmail.Name = "textEmail";
            textEmail.Size = new Size(269, 23);
            textEmail.TabIndex = 39;

            // label7
            label7.AutoSize = true;
            label7.Location = new Point(948, 229);
            label7.Name = "label7";
            label7.Size = new Size(53, 15);
            label7.TabIndex = 38;
            label7.Text = "Teléfono";

            // label8
            label8.AutoSize = true;
            label8.Location = new Point(948, 182);
            label8.Name = "label8";
            label8.Size = new Size(57, 15);
            label8.TabIndex = 37;
            label8.Text = "Dirección";

            // textTelefono
            textTelefono.Location = new Point(948, 247);
            textTelefono.Name = "textTelefono";
            textTelefono.Size = new Size(269, 23);
            textTelefono.TabIndex = 36;

            // textDireccion
            textDireccion.Location = new Point(948, 200);
            textDireccion.Name = "textDireccion";
            textDireccion.Size = new Size(269, 23);
            textDireccion.TabIndex = 35;

            // label4
            label4.AutoSize = true;
            label4.Location = new Point(948, 136);
            label4.Name = "label4";
            label4.Size = new Size(51, 15);
            label4.TabIndex = 34;
            label4.Text = "Nombre";

            // textNombre
            textNombre.Location = new Point(948, 154);
            textNombre.Name = "textNombre";
            textNombre.Size = new Size(269, 23);
            textNombre.TabIndex = 33;

            // label3
            label3.AutoSize = true;
            label3.Location = new Point(948, 92);
            label3.Name = "label3";
            label3.Size = new Size(51, 15);
            label3.TabIndex = 32;
            label3.Text = "Apellido";

            // textApellido
            textApellido.Location = new Point(948, 110);
            textApellido.Name = "textApellido";
            textApellido.Size = new Size(269, 23);
            textApellido.TabIndex = 31;

            // label2
            label2.AutoSize = true;
            label2.Location = new Point(948, 48);
            label2.Name = "label2";
            label2.Size = new Size(108, 15);
            label2.TabIndex = 30;
            label2.Text = "Nro de documento";

            // label1
            label1.AutoSize = true;
            label1.Location = new Point(948, 1);
            label1.Name = "label1";
            label1.Size = new Size(113, 15);
            label1.TabIndex = 29;
            label1.Text = "Tipo de Documento";

            // textNroDni
            textNroDni.Location = new Point(948, 66);
            textNroDni.Name = "textNroDni";
            textNroDni.Size = new Size(269, 23);
            textNroDni.TabIndex = 28;

            // buttonReturn
            buttonReturn.Location = new Point(980, 503);
            buttonReturn.Name = "buttonReturn";
            buttonReturn.Size = new Size(192, 23);
            buttonReturn.TabIndex = 27;
            buttonReturn.Text = "Volver";
            buttonReturn.UseVisualStyleBackColor = true;
            buttonReturn.Click += new EventHandler(buttonReturn_Click);

            // buttonDelete
            buttonDelete.Location = new Point(980, 446);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(192, 23);
            buttonDelete.TabIndex = 26;
            buttonDelete.Text = "Eliminar paciente";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += new EventHandler(buttonDelete_Click);

            // buttonEdit
            buttonEdit.Location = new Point(980, 417);
            buttonEdit.Name = "buttonEdit";
            buttonEdit.Size = new Size(192, 23);
            buttonEdit.TabIndex = 25;
            buttonEdit.Text = "Editar paciente";
            buttonEdit.UseVisualStyleBackColor = true;
            buttonEdit.Click += new EventHandler(buttonEdit_Click);

            // buttonAdd
            buttonAdd.Location = new Point(980, 388);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(192, 23);
            buttonAdd.TabIndex = 24;
            buttonAdd.Text = "Añadir nuevo paciente";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += new EventHandler(buttonAdd_Click);

            // dataGridView1
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] {
                nroPacienteDataGridViewTextBoxColumn,
                nombreDataGridViewTextBoxColumn,
                apellidoDataGridViewTextBoxColumn,
                nroDniDataGridViewTextBoxColumn,
                tipoDniDataGridViewTextBoxColumn,
                direccionDataGridViewTextBoxColumn,
                telefonoDataGridViewTextBoxColumn,
                emailDataGridViewTextBoxColumn,
                passwordDataGridViewTextBoxColumn
            });
            dataGridView1.DataSource = pacienteDTOBindingSource;
            dataGridView1.Location = new Point(-1, -1);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(943, 607);
            dataGridView1.TabIndex = 23;
            dataGridView1.SelectionChanged += new EventHandler(dataGridView1_SelectionChanged);

            // columnas
            nroPacienteDataGridViewTextBoxColumn.DataPropertyName = "NroPaciente";
            nroPacienteDataGridViewTextBoxColumn.HeaderText = "NroPaciente";
            nroPacienteDataGridViewTextBoxColumn.Name = "nroPacienteDataGridViewTextBoxColumn";

            nombreDataGridViewTextBoxColumn.DataPropertyName = "Nombre";
            nombreDataGridViewTextBoxColumn.HeaderText = "Nombre";
            nombreDataGridViewTextBoxColumn.Name = "nombreDataGridViewTextBoxColumn";

            apellidoDataGridViewTextBoxColumn.DataPropertyName = "Apellido";
            apellidoDataGridViewTextBoxColumn.HeaderText = "Apellido";
            apellidoDataGridViewTextBoxColumn.Name = "apellidoDataGridViewTextBoxColumn";

            nroDniDataGridViewTextBoxColumn.DataPropertyName = "NroDni";
            nroDniDataGridViewTextBoxColumn.HeaderText = "NroDni";
            nroDniDataGridViewTextBoxColumn.Name = "nroDniDataGridViewTextBoxColumn";

            tipoDniDataGridViewTextBoxColumn.DataPropertyName = "TipoDni";
            tipoDniDataGridViewTextBoxColumn.HeaderText = "TipoDni";
            tipoDniDataGridViewTextBoxColumn.Name = "tipoDniDataGridViewTextBoxColumn";

            direccionDataGridViewTextBoxColumn.DataPropertyName = "Direccion";
            direccionDataGridViewTextBoxColumn.HeaderText = "Direccion";
            direccionDataGridViewTextBoxColumn.Name = "direccionDataGridViewTextBoxColumn";

            telefonoDataGridViewTextBoxColumn.DataPropertyName = "Telefono";
            telefonoDataGridViewTextBoxColumn.HeaderText = "Telefono";
            telefonoDataGridViewTextBoxColumn.Name = "telefonoDataGridViewTextBoxColumn";

            emailDataGridViewTextBoxColumn.DataPropertyName = "Email";
            emailDataGridViewTextBoxColumn.HeaderText = "Email";
            emailDataGridViewTextBoxColumn.Name = "emailDataGridViewTextBoxColumn";

            passwordDataGridViewTextBoxColumn.DataPropertyName = "Password";
            passwordDataGridViewTextBoxColumn.HeaderText = "Password";
            passwordDataGridViewTextBoxColumn.Name = "passwordDataGridViewTextBoxColumn";

            // pacienteDTOBindingSource
            pacienteDTOBindingSource.DataSource = typeof(DTO.PacienteDTO);

            // lblNroPaciente
            lblNroPaciente.AutoSize = true;
            lblNroPaciente.Location = new Point(948, 361);
            lblNroPaciente.Name = "lblNroPaciente";
            lblNroPaciente.Size = new Size(89, 15);
            lblNroPaciente.TabIndex = 45;
            lblNroPaciente.Text = "Nro Paciente: -";

            // HomeAdminCRUDPaciente
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1231, 610);
            Controls.Add(lblNroPaciente);
            Controls.Add(labelAction);
            Controls.Add(cmbTipoDocumento);
            Controls.Add(label5);
            Controls.Add(textContraseña);
            Controls.Add(label6);
            Controls.Add(textEmail);
            Controls.Add(label7);
            Controls.Add(label8);
            Controls.Add(textTelefono);
            Controls.Add(textDireccion);
            Controls.Add(label4);
            Controls.Add(textNombre);
            Controls.Add(label3);
            Controls.Add(textApellido);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textNroDni);
            Controls.Add(buttonReturn);
            Controls.Add(buttonDelete);
            Controls.Add(buttonEdit);
            Controls.Add(buttonAdd);
            Controls.Add(dataGridView1);
            Name = "HomeAdminCRUDPaciente";
            Text = "Clínica Odontológica - Administrador";
            Load += new EventHandler(FormCrudPacientes_Load);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pacienteDTOBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label labelAction;
        private ComboBox cmbTipoDocumento;
        private Label label5;
        private TextBox textContraseña;
        private Label label6;
        private TextBox textEmail;
        private Label label7;
        private Label label8;
        private TextBox textTelefono;
        private TextBox textDireccion;
        private Label label4;
        private TextBox textNombre;
        private Label label3;
        private TextBox textApellido;
        private Label label2;
        private Label label1;
        private TextBox textNroDni;
        private Button buttonReturn;
        private Button buttonDelete;
        private Button buttonEdit;
        private Button buttonAdd;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn nroPacienteDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn nombreDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn apellidoDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn nroDniDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn tipoDniDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn direccionDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn telefonoDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn emailDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn passwordDataGridViewTextBoxColumn;
        private BindingSource pacienteDTOBindingSource;
        private Label lblNroPaciente;
    }
}