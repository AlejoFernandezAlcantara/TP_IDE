using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace WindowsForms
{
    // Convierte la respuesta de error de la API en un texto legible para el usuario. (NODEVUELVE EL JASON)
    public static class ApiErrorHelper
    {
        public static async Task<string> LeerErrorAsync(HttpResponseMessage response)
        {
            var texto = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrWhiteSpace(texto))
            {
                try
                {
                    using var doc = JsonDocument.Parse(texto);
                    var raiz = doc.RootElement;

                    if (raiz.ValueKind == JsonValueKind.Object &&
                        raiz.TryGetProperty("error", out var error) &&
                        error.ValueKind == JsonValueKind.String)
                    {
                        return error.GetString() ?? "Ocurrió un error.";
                    }
                }
                catch (JsonException)
                {
                    // No es JSON: se usa el mensaje según el código HTTP
                }
            }

            return response.StatusCode switch
            {
                HttpStatusCode.Unauthorized => "Tu sesión expiró. Volvé a iniciar sesión.",
                HttpStatusCode.Forbidden => "No tenés permiso para realizar esta acción.",
                HttpStatusCode.NotFound => "No se encontró el registro.",
                HttpStatusCode.Conflict => "La operación no se puede realizar con los datos actuales.",
                HttpStatusCode.BadRequest => "Revisá los datos ingresados.",
                _ => "Ocurrió un error en el servidor."
            };
        }
    }
}