using InversionEnPozo.Api.DTOs;

namespace InversionEnPozo.Api.Servicios.Interfaces
{
    public interface IEmailService
    {
        Task EnviarEmailAsync(string destinatario, string asunto, string mensajeHtml);
    }
}
