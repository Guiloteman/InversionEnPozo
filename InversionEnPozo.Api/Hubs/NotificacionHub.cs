using Microsoft.AspNetCore.SignalR;

namespace InversionEnPozo.Api.Hubs
{
    public class NotificacionHub : Hub
    {
        public async Task UnirseAGrupoProyecto(string proyectoId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Proyecto_{proyectoId}");
        }
    }
}