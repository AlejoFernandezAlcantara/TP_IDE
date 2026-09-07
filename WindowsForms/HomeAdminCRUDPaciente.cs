using Domain.Model;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
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
            MinimumSize = new Size(1000, 750);
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
            try
            {
                var lista = await _client.GetFromJsonAsync<List<PacienteDTO>>("pacientes");
                dataGridView1.DataSource = lista;
            }
            catch (HttpRequestException)
            {
                MessageBox.Show(
                    "No se pudo conectar con el servidor. Verificá que la API esté funcionando.",
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo cargar la lista de pacientes.\n\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0) return;

                if (dataGridView1.SelectedRows[0].DataBoundItem is not PacienteDTO paciente) return;

                _nroPacienteSeleccionado = paciente.NroPaciente;

                lblNroPaciente.Text = $"Nro Paciente: {paciente.NroPaciente}";

                textNombre.Text = paciente.Nombre;
                textApellido.Text = paciente.Apellido;
                textEmail.Text = paciente.Email;
                textDireccion.Text = paciente.Direccion;
                textTelefono.Text = paciente.Telefono;
                textNroDni.Text = paciente.NroDni.ToString();
                cmbTipoDocumento.SelectedItem = paciente.TipoDni;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo mostrar el paciente seleccionado.\n\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbTipoDocumento.SelectedItem == null)
                {
                    MessageBox.Show("Seleccioná un tipo de documento.", "Datos incompletos",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

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
                    MessageBox.Show($"Error al añadir el paciente.\n\nStatus: {response.StatusCode}\n\nDetalle: {detalle}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show(
                    "El número de DNI debe ser un valor numérico válido.",
                    "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (HttpRequestException)
            {
                MessageBox.Show(
                    "No se pudo conectar con el servidor. Verificá que la API esté funcionando.",
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error inesperado al añadir el paciente.\n\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (_nroPacienteSeleccionado == 0)
                {
                    MessageBox.Show("Seleccioná un paciente primero.", "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (cmbTipoDocumento.SelectedItem == null)
                {
                    MessageBox.Show("Seleccioná un tipo de documento.", "Datos incompletos",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var editado = new PacienteDTO
                {
                    NroPaciente = _nroPacienteSeleccionado,
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
                    MessageBox.Show($"Error al editar el paciente.\n\nStatus: {response.StatusCode}\n\nDetalle: {detalle}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show(
                    "El número de DNI debe ser un valor numérico válido.",
                    "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (HttpRequestException)
            {
                MessageBox.Show(
                    "No se pudo conectar con el servidor. Verificá que la API esté funcionando.",
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error inesperado al editar el paciente.\n\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (_nroPacienteSeleccionado == 0)
                {
                    MessageBox.Show("Seleccioná un paciente primero.", "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var confirmar = MessageBox.Show(
                    "¿Estás seguro que querés eliminar este paciente?",
                    "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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
                        var detalle = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Error al eliminar el paciente.\n\nStatus: {response.StatusCode}\n\nDetalle: {detalle}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (HttpRequestException)
            {
                MessageBox.Show(
                    "No se pudo conectar con el servidor. Verificá que la API esté funcionando.",
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error inesperado al eliminar el paciente.\n\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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