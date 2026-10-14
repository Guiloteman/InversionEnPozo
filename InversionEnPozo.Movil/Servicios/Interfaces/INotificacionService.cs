namespace InversionEnPozo.Movil.Servicios.Interfaces
{
    public interface INotificacionService
    {
        event Action<string>? OnMensajeRecibido;
        Task InicializarAsync();
    }
}