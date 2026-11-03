using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DTO;

namespace API.Clients
{
    public class MutualApiClient
    {
        private readonly HttpClient _client;

        public MutualApiClient(HttpClient client)
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

        public async Task<List<MutualDTO>?> GetAllAsync()
        {
            var r = await _client.GetAsync("mutuales");
            return r.IsSuccessStatusCode ? await r.Content.ReadFromJsonAsync<List<MutualDTO>>() : null;
        }

        
        public async Task<MutualDTO?> GetByCuitAsync(int cuit)
        {
            var r = await _client.GetAsync($"mutuales/{cuit}");
            if (r.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            r.EnsureSuccessStatusCode();
            return await r.Content.ReadFromJsonAsync<MutualDTO>();
        }

        public async Task<(bool Ok, string? Error)> CrearAsync(MutualDTO dto)
            => await ResultadoAsync(await _client.PostAsJsonAsync("mutuales", dto), "No se pudo crear la mutual.");

        public async Task<(bool Ok, string? Error)> ActualizarAsync(MutualDTO dto)
            => await ResultadoAsync(await _client.PutAsJsonAsync("mutuales", dto), "No se pudo actualizar la mutual.");

        public async Task<(bool Ok, string? Error)> EliminarAsync(int cuit)
            => await ResultadoAsync(await _client.DeleteAsync($"mutuales/{cuit}"), "No se pudo eliminar la mutual.");

        private static async Task<(bool Ok, string? Error)> ResultadoAsync(HttpResponseMessage r, string mensajePorDefecto)
        {
            if (r.IsSuccessStatusCode)
                return (true, null);

            if (r.StatusCode == System.Net.HttpStatusCode.Forbidden)
                return (false, "No tenés permiso para realizar esta acción.");

            if (r.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return (false, "Tu sesión expiró. Volvé a iniciar sesión.");

            if (r.StatusCode == System.Net.HttpStatusCode.NotFound)
                return (false, "La mutual no existe.");

            try
            {
                var body = await r.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                if (body != null && body.TryGetValue("error", out var msg) && !string.IsNullOrWhiteSpace(msg))
                    return (false, msg);
            }
            catch
            {
            }

            return (false, mensajePorDefecto);
        }
    }
}