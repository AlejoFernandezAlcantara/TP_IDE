using System.Net.Http.Json;
using DTO;

namespace API.Clients
{
    public class OdontologoApiClient
    {
        private readonly HttpClient _client;

        public OdontologoApiClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("http://localhost:5232/api/");
        }

        public async Task<List<OdontologoDTO>?> GetAllAsync()
        {
            var response = await _client.GetAsync("odontologos");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<OdontologoDTO>>();
        }

        public async Task<OdontologoDTO?> GetByMatriculaAsync(string matricula)
        {
            var response = await _client.GetAsync($"odontologos/{matricula}");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<OdontologoDTO>();
        }

        public async Task<bool> CrearAsync(OdontologoDTO odontologo)
        {
            var response = await _client.PostAsJsonAsync("odontologos", odontologo);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarAsync(OdontologoDTO odontologo)
        {
            var response = await _client.PutAsJsonAsync($"odontologos/{odontologo.Matricula}", odontologo);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarAsync(string matricula)
        {
            var response = await _client.DeleteAsync($"odontologos/{matricula}");
            return response.IsSuccessStatusCode;
        }
    }
}
