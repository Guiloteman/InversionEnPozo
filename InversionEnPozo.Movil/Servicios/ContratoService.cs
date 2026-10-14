using System.Net.Http.Json;
using InversionEnPozo.Movil.Modelos;
using InversionEnPozo.Movil.Servicios.Interfaces;

namespace InversionEnPozo.Movil.Servicios
{
    public class ContratoService : IContratoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://empirical-pastel-galley.ngrok-free.dev/api/Contrato";

        public ContratoService()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            _httpClient = new HttpClient(handler);
        }

        public async Task<(bool Exito, CrearProyectoResponseDto? Datos, string Mensaje)> CrearProyectoAsync(string nombre, string descripcion, decimal presupuesto, string constructoraId)
        {
            try
            {
                var request = new CrearProyectoRequestDto
                {
                    NombreProyecto = nombre,
                    Descripcion = descripcion,
                    PresupuestoTotal = presupuesto,
                    ConstructoraId = constructoraId
                };

                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/crear-proyecto", request);

                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<CrearProyectoResponseDto>();
                    return (true, resultado, "Proyecto creado con contrato inteligente.");
                }

                var error = await response.Content.ReadAsStringAsync();
                return (false, null, $"Error: {error}");
            }
            catch (Exception ex)
            {
                return (false, null, $"Excepción de conexión: {ex.Message}");
            }
        }

        public async Task<(bool Exito, AprobarHitoResponseDto? Datos, string Mensaje)> AprobarHitoAsync(int hitoId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"{_baseUrl}/aprobar-hito/{hitoId}", null);

                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<AprobarHitoResponseDto>();
                    return (true, resultado, "Hito aprobado y fondos liberados.");
                }

                var error = await response.Content.ReadAsStringAsync();
                return (false, null, $"Error: {error}");
            }
            catch (Exception ex)
            {
                return (false, null, $"Excepción de conexión: {ex.Message}");
            }
        }

        public async Task<List<ProyectoResponseDto>> ObtenerProyectosAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/proyectos");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<ProyectoResponseDto>>() ?? new();
                }
                return new();
            }
            catch
            {
                return new();
            }
        }

        public async Task<List<ProyectoResponseDto>> ObtenerProyectosActivosAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/proyectos");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<ProyectoResponseDto>>() ?? new();
                }
                return new();
            }
            catch
            {
                return new();
            }
        }
    }
}