using Domain.Model;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForms
{
    public partial class HomeAdminCRUDPaciente : Form
    {
        private readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5232/api/")
        };
        private int _nroPacienteSeleccionado;

        public HomeAdminCRUDPaciente()
        {
            InitializeComponent();
            cmbTipoDocumento.DataSource = Enum.GetValues(typeof(tiposEnumerados));

            var token = WindowsForms.Auth.AuthServiceProvider.Instance.GetTokenAsync().Result;
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        private async void FormCrudPacientes_Load(object sender, EventArgs e)
        {
            await CargarPacientes();
        }

        private async Task CargarPacientes()
        {
            var lista = await _client.GetFromJsonAsync<List<PacienteDTO>>("pacientes");
            dataGridView1.DataSource = lista;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;

            if (dataGridView1.SelectedRows[0].DataBoundItem is not PacienteDTO paciente) return;

            _nroPacienteSeleccionado = paciente.NroPaciente;

            // Muestra el nro paciente en el label, no editable
            lblNroPaciente.Text = $"Nro Paciente: {paciente.NroPaciente}";

            textNombre.Text = paciente.Nombre;
            textApellido.Text = paciente.Apellido;
            textEmail.Text = paciente.Email;
            textDireccion.Text = paciente.Direccion;
            textTelefono.Text = paciente.Telefono;
            textNroDni.Text = paciente.NroDni.ToString();
            cmbTipoDocumento.SelectedItem = paciente.TipoDni;
        }

        // POST — no manda NroPaciente, lo genera el servidor
        private async void buttonAdd_Click(object sender, EventArgs e)
        {
            var nuevo = new PacienteDTO
            {
                Nombre = textNombre.Text,
                Apellido = textApellido.Text,
                Email = textEmail.Text,
                Direccion = textDireccion.Text,
                Telefono = textTelefono.Text,
                NroDni = Convert.ToInt32(textNroDni.Text),
                TipoDni = (tiposEnumerados)cmbTipoDocumento.SelectedItem,
                Password = textContraseña.Text
            };

            var response = await _client.PostAsJsonAsync("pacientes", nuevo);

            if (response.IsSuccessStatusCode)
            {
                await CargarPacientes();
                MostrarMensajeExito();
                LimpiarCampos();
            }
            else
            {
                var detalle = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Error al añadir el paciente.\n\nStatus: {response.StatusCode}\n\nDetalle: {detalle}");
            }
        }

        // PUT — manda NroPaciente para identificar cuál editar
        private async void buttonEdit_Click(object sender, EventArgs e)
        {
            if (_nroPacienteSeleccionado == 0)
            {
                MessageBox.Show("Seleccioná un paciente primero.");
                return;
            }

            var editado = new PacienteDTO
            {
                NroPaciente = _nroPacienteSeleccionado, // viene de la variable, no del TextBox
                Nombre = textNombre.Text,
                Apellido = textApellido.Text,
                Email = textEmail.Text,
                Direccion = textDireccion.Text,
                Telefono = textTelefono.Text,
                NroDni = Convert.ToInt32(textNroDni.Text),
                TipoDni = (tiposEnumerados)cmbTipoDocumento.SelectedItem,
                Password = textContraseña.Text
            };

            var response = await _client.PutAsJsonAsync("pacientes", editado);

            if (response.IsSuccessStatusCode)
            {
                await CargarPacientes();
                MostrarMensajeExito();
            }
            else
            {
                var detalle = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Error al editar el paciente.\n\nStatus: {response.StatusCode}\n\nDetalle: {detalle}");
            }
        }

        // DELETE — usa la variable _nroPacienteSeleccionado
        private async void buttonDelete_Click(object sender, EventArgs e)
        {
            if (_nroPacienteSeleccionado == 0)
            {
                MessageBox.Show("Seleccioná un paciente primero.");
                return;
            }

            var confirmar = MessageBox.Show(
                "¿Estás seguro que querés eliminar este paciente?",
                "Confirmar",
                MessageBoxButtons.YesNo
            );

            if (confirmar == DialogResult.Yes)
            {
                var response = await _client.DeleteAsync($"pacientes/{_nroPacienteSeleccionado}");

                if (response.IsSuccessStatusCode)
                {
                    await CargarPacientes();
                    MostrarMensajeExito();
                    LimpiarCampos();
                    _nroPacienteSeleccionado = 0;
                }
                else
                {
                    MessageBox.Show("Error al eliminar el paciente.");
                }
            }
        }

        private void buttonReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void MostrarMensajeExito()
        {
            labelAction.Text = "Acción ejecutada con éxito";
            labelAction.ForeColor = Color.Green;
            labelAction.Visible = true;

            await Task.Delay(3000);

            for (int i = 255; i >= 0; i -= 5)
            {
                labelAction.ForeColor = Color.FromArgb(i, 0, 128, 0);
                await Task.Delay(20);
            }

            labelAction.Visible = false;
        }

        private void LimpiarCampos()
        {
            lblNroPaciente.Text = "Nro Paciente: -";
            textNombre.Text = string.Empty;
            textApellido.Text = string.Empty;
            textEmail.Text = string.Empty;
            textDireccion.Text = string.Empty;
            textTelefono.Text = string.Empty;
            textNroDni.Text = string.Empty;
            textContraseña.Text = string.Empty;
            cmbTipoDocumento.SelectedIndex = -1;
        }

        private void labelAction_Click(object sender, EventArgs e)
        {
        }
    }
}