using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DTO;

namespace API.Clients
{
    public class PracticaApiClient
    {
        private readonly HttpClient _client;

        public PracticaApiClient(HttpClient client)
        {
            _client = client;
            if (_client.BaseAddress == null)
            {
                _client.BaseAddress = new Uri("http://localhost:5232/api/");
            }
        }

        public void SetToken(string? token) //guarda en el header de la petición el token de autenticación para que la API lo reconozca
        {
            _client.DefaultRequestHeaders.Authorization = string.IsNullOrEmpty(token)
                ? null
                : new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<PracticaDTO>?> GetAllAsync() //si es exitoso, devuelve la lista de prácticas, si no, devuelve null
        {
            var r = await _client.GetAsync("practicas");
            return r.IsSuccessStatusCode ? await r.Content.ReadFromJsonAsync<List<PracticaDTO>>() : null;
        }
    }
}