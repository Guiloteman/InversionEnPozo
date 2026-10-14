using InversionEnPozo.Movil.Modelos;
using InversionEnPozo.Movil.Servicios.Interfaces;
using System.Net.Http.Json;


namespace InversionEnPozo.Movil.Servicios
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://empirical-pastel-galley.ngrok-free.dev/api/Auth";

        public AuthService()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            _httpClient = new HttpClient(handler);
        }

        public async Task<(bool Exito, string Mensaje)> RegistrarAsync(string email, string password, string nombre, string rol)
        {
            try
            {
                var modelo = new RegistroRequestDto
                {
                    Email = email,
                    Password = password,
                    NombreCompleto = nombre,
                    Rol = rol
                };

                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/registro", modelo);

                if (response.IsSuccessStatusCode)
                    return (true, "¡Usuario registrado con éxito! Revisa tu correo electrónico para confirmar la cuenta.");

                var errorContent = await response.Content.ReadAsStringAsync();
                return (false, $"Error en el registro: {errorContent}");
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión con la API: {ex.Message}");
            }
        }

        public async Task<(bool Exito, string Rol, string Mensaje)> LoginAsync(string email, string password)
        {
            try
            {
                var modelo = new LoginRequestDto { Email = email, Password = password };
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/login", modelo);

                if (response.IsSuccessStatusCode)
                {
    
                    var resultadoApi = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                    string rolObtenido = resultadoApi?.Rol ?? "Inversionista";

                    return (true, rolObtenido, "¡Bienvenido!");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return (false, string.Empty, $"No se pudo iniciar sesión: {errorContent}");
            }
            catch (Exception ex)
            {
                return (false, string.Empty, $"Error de conexión con la API: {ex.Message}");
            }
        }
    }
}