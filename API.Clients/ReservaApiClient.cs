using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DTO;

namespace API.Clients
{
    public class ReservaApiClient
    {
        private readonly HttpClient _client;

        public ReservaApiClient(HttpClient client)
        {
            _client = client;
            if (_client.BaseAddress == null)
            {
                _client.BaseAddress = new Uri("http://localhost:5232/api/");
            }
        }

        public void SetToken(string? token)
        {
            _client.DefaultRequestHeaders.Authorization = string.IsNullOrEmpty(token)
                ? null
                : new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<ReservaDTO>?> GetAllAsync()
        {
            var r = await _client.GetAsync("reservas");
            return r.IsSuccessStatusCode ? await r.Content.ReadFromJsonAsync<List<ReservaDTO>>() : null;
        }

        public async Task<ReservaDTO?> GetByIdAsync(int id)
        {
            var r = await _client.GetAsync($"reservas/{id}");
            return r.IsSuccessStatusCode ? await r.Content.ReadFromJsonAsync<ReservaDTO>() : null;
        }

        public async Task<List<ReservaDTO>?> GetByPacienteAsync(int pacienteId)
        {
            var r = await _client.GetAsync($"reservas/paciente/{pacienteId}");
            return r.IsSuccessStatusCode ? await r.Content.ReadFromJsonAsync<List<ReservaDTO>>() : null;
        }

        public async Task<bool> CrearAsync(ReservaDTO dto)
            => (await _client.PostAsJsonAsync("reservas", dto)).IsSuccessStatusCode;

        public async Task<bool> ActualizarAsync(ReservaDTO dto)
            => (await _client.PutAsJsonAsync("reservas", dto)).IsSuccessStatusCode;

        public async Task<(bool ok, string? error)> AtenderReservaAsync(AtenderReservaDTO dto)
        {
            var response = await _client.PostAsJsonAsync("reservas/atender", dto);
            if (response.IsSuccessStatusCode)
                return (true, null);

            var error = await response.Content.ReadAsStringAsync();
            return (false, string.IsNullOrWhiteSpace(error) ? "Error al atender la reserva" : error);
        }

        public async Task<bool> EliminarAsync(int id)
            => (await _client.DeleteAsync($"reservas/{id}")).IsSuccessStatusCode;
    }
}