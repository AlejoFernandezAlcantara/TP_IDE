
using Domain.Model;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
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
        private int _idSeleccionado;
        public HomeAdminCRUDOdontologo()
        {
            InitializeComponent();
        }

        private async void FormCrudOdontologos_Load(object sender, EventArgs e)
        {
            await CargarOdontologos();
            cmbTipoDocumento.DataSource = Enum.GetValues(typeof(tiposEnumerados));
        }
        private async Task CargarOdontologos()
        {
            var lista = await _client.GetFromJsonAsync<List<OdontologoDTO>>("odontologos");
            dataGridView1.DataSource = lista;
            dataGridView1.Columns["Id"].Visible = false; // ocultás el id
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;

            var fila = dataGridView1.SelectedRows[0];

            _idSeleccionado = Convert.ToInt32(fila.Cells["Id"].Value);

            textNombre.Text = fila.Cells["Nombre"].Value?.ToString();
            textApellido.Text = fila.Cells["Apellido"].Value?.ToString();
            textEmail.Text = fila.Cells["Email"].Value?.ToString();
            textMatricula.Text = fila.Cells["Matricula"].Value?.ToString();
            textEspecialidad.Text = fila.Cells["Especialidad"].Value?.ToString();
            textNroDocumento.Text = fila.Cells["NroDocumento"].Value?.ToString();
            cmbTipoDocumento.SelectedItem = fila.Cells["TipoDocumento"].Value;
        }

        private async void buttonAdd_Click(object sender, EventArgs e)
        {
            var nuevo = new OdontologoDTO
            {
                Nombre = textNombre.Text,
                Apellido = textApellido.Text,
                Email = textEmail.Text,
                Matricula = textMatricula.Text,
                Especialidad = textEspecialidad.Text,
                NroDocumento = Convert.ToInt32(textNroDocumento.Text),
                TipoDocumento = (tiposEnumerados)cmbTipoDocumento.SelectedItem
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
                MessageBox.Show("Error al añadir el odontólogo.");
            }
        }

        private async void buttonEdit_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado == 0)
            {
                MessageBox.Show("Seleccioná un odontólogo primero.");
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
                TipoDocumento = (tiposEnumerados)cmbTipoDocumento.SelectedItem
            };

            var response = await _client.PutAsJsonAsync($"odontologos/{_idSeleccionado}", editado);

            if (response.IsSuccessStatusCode)
            {
                await CargarOdontologos();
                MostrarMensajeExito();
            }
            else
            {
                MessageBox.Show("Error al editar el odontólogo.");
            }
        }

        private async void buttonDelete_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado == 0)
            {
                MessageBox.Show("Seleccioná un odontólogo primero.");
                return;
            }

            var confirmar = MessageBox.Show(
                "¿Estás seguro que querés eliminar este odontólogo?",
                "Confirmar",
                MessageBoxButtons.YesNo
            );

            if (confirmar == DialogResult.Yes)
            {
                var response = await _client.DeleteAsync($"odontologos/{_idSeleccionado}");

                if (response.IsSuccessStatusCode)
                {
                    await CargarOdontologos();
                    MostrarMensajeExito();
                    LimpiarCampos();
                    _idSeleccionado = 0;
                }
                else
                {
                    MessageBox.Show("Error al eliminar el odontólogo.");
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
            textNombre.Text = string.Empty;
            textApellido.Text = string.Empty;
            textEmail.Text = string.Empty;
            textMatricula.Text = string.Empty;
            textEspecialidad.Text = string.Empty;
            textNroDocumento.Text = string.Empty;
            cmbTipoDocumento.SelectedIndex = -1;
        }

        private void labelAction_Click(object sender, EventArgs e)
        {

        }
    }
}
