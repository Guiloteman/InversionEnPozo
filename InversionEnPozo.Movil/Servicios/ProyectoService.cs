using System.Net.Http.Json;
using InversionEnPozo.Movil.Modelos;
using InversionEnPozo.Movil.Servicios.Interfaces;

namespace InversionEnPozo.Movil.Servicios
{
    public class ProyectoService : IProyectoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://empirical-pastel-galley.ngrok-free.dev/api/Proyectos";

        public ProyectoService()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            _httpClient = new HttpClient(handler);
        }

        public async Task<List<ProyectoInmobiliario>> ObtenerProyectosAsync()
        {
            try
            {
                var proyectos = await _httpClient.GetFromJsonAsync<List<ProyectoInmobiliario>>(_baseUrl);
                return proyectos ?? new List<ProyectoInmobiliario>();
            }
            catch
            {
                return new List<ProyectoInmobiliario>();
            }
        }
    }
}