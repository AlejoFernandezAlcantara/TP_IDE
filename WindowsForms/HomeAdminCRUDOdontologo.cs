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
    public partial class HomeAdminCRUDOdontologo : Form
    {
        private readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5232/api/")
        };
        private string? _matriculaSeleccionada;

        public HomeAdminCRUDOdontologo()
        {
            InitializeComponent();
            MinimumSize = new Size(1000, 750);
            cmbTipoDocumento.DataSource = Enum.GetValues(typeof(tiposEnumerados));

            var token = WindowsForms.Auth.AuthServiceProvider.Instance.GetTokenAsync().Result;
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        private async void FormCrudOdontologos_Load(object sender, EventArgs e)
        {
            await CargarOdontologos();
        }

        private async Task CargarOdontologos()
        {
            try
            {
                var lista = await _client.GetFromJsonAsync<List<OdontologoDTO>>("odontologos");
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
                    $"No se pudo cargar la lista de odontólogos.\n\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0) return;

                if (dataGridView1.SelectedRows[0].DataBoundItem is not OdontologoDTO odontologo) return;

                _matriculaSeleccionada = odontologo.Matricula;

                textNombre.Text = odontologo.Nombre;
                textApellido.Text = odontologo.Apellido;
                textEmail.Text = odontologo.Email;
                textMatricula.Text = odontologo.Matricula;
                textEspecialidad.Text = odontologo.Especialidad;
                textNroDocumento.Text = odontologo.NroDocumento.ToString();
                cmbTipoDocumento.SelectedItem = odontologo.TipoDocumento;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo mostrar el odontólogo seleccionado.\n\n{ex.Message}",
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

                var nuevo = new OdontologoDTO
                {
                    Nombre = textNombre.Text,
                    Apellido = textApellido.Text,
                    Email = textEmail.Text,
                    Matricula = textMatricula.Text,
                    Especialidad = textEspecialidad.Text,
                    NroDocumento = Convert.ToInt32(textNroDocumento.Text),
                    TipoDocumento = (tiposEnumerados)cmbTipoDocumento.SelectedItem,
                    Password = textContraseña.Text
                };

                var response = await _client.PostAsJsonAsync("odontologos", nuevo);

                if (response.IsSuccessStatusCode)
                {
                    await CargarOdontologos();
                    MostrarMensajeExito();
                    LimpiarCampos();
                }
                else
                {
                    var detalle = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al añadir el odontólogo.\n\nStatus: {response.StatusCode}\n\nDetalle: {detalle}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show(
                    "El número de documento debe ser un valor numérico válido.",
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
                    $"Ocurrió un error inesperado al añadir el odontólogo.\n\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_matriculaSeleccionada))
                {
                    MessageBox.Show("Seleccioná un odontólogo primero.", "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (cmbTipoDocumento.SelectedItem == null)
                {
                    MessageBox.Show("Seleccioná un tipo de documento.", "Datos incompletos",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var editado = new OdontologoDTO
                {
                    Nombre = textNombre.Text,
                    Apellido = textApellido.Text,
                    Email = textEmail.Text,
                    Matricula = textMatricula.Text,
                    Especialidad = textEspecialidad.Text,
                    NroDocumento = Convert.ToInt32(textNroDocumento.Text),
                    TipoDocumento = (tiposEnumerados)cmbTipoDocumento.SelectedItem,
                    Password = textContraseña.Text
                };

                var response = await _client.PutAsJsonAsync("odontologos", editado);

                if (response.IsSuccessStatusCode)
                {
                    await CargarOdontologos();
                    MostrarMensajeExito();
                }
                else
                {
                    var detalle = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al editar el odontólogo.\n\nStatus: {response.StatusCode}\n\nDetalle: {detalle}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show(
                    "El número de documento debe ser un valor numérico válido.",
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
                    $"Ocurrió un error inesperado al editar el odontólogo.\n\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_matriculaSeleccionada))
                {
                    MessageBox.Show("Seleccioná un odontólogo primero.", "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var confirmar = MessageBox.Show(
                    "¿Estás seguro que querés eliminar este odontólogo?",
                    "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmar == DialogResult.Yes)
                {
                    var response = await _client.DeleteAsync($"odontologos/{_matriculaSeleccionada}");

                    if (response.IsSuccessStatusCode)
                    {
                        await CargarOdontologos();
                        MostrarMensajeExito();
                        LimpiarCampos();
                        _matriculaSeleccionada = null;
                    }
                    else
                    {
                        var detalle = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Error al eliminar el odontólogo.\n\nStatus: {response.StatusCode}\n\nDetalle: {detalle}",
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
                    $"Ocurrió un error inesperado al eliminar el odontólogo.\n\n{ex.Message}",
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
            textNombre.Text = string.Empty;
            textApellido.Text = string.Empty;
            textEmail.Text = string.Empty;
            textMatricula.Text = string.Empty;
            textEspecialidad.Text = string.Empty;
            textNroDocumento.Text = string.Empty;
            textContraseña.Text = string.Empty;
            cmbTipoDocumento.SelectedIndex = -1;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}