using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using InversionEnPozo.Movil.Servicios.Interfaces;

namespace InversionEnPozo.Movil.Servicios
{
    public class NotificacionService : INotificacionService
    {
        private HubConnection? _hubConnection;

        public event Action<string>? OnMensajeRecibido;

        public async Task InicializarAsync()
        {
            string urlHub = "https://empirical-pastel-galley.ngrok-free.dev/notificacionHub";

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(urlHub)
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<object>("RecibirNotificacionDesembolso", (data) =>
            {
                OnMensajeRecibido?.Invoke("¡Actualización en la Red Sepolia! Un hito ha sido liberado.");
            });

            try
            {
                await _hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al conectar con SignalR: {ex.Message}");
            }
        }
    }
}