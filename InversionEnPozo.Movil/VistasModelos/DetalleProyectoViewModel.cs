using System.Windows.Input;
using InversionEnPozo.Movil.Modelos;
using InversionEnPozo.Movil.Servicios.Interfaces;

namespace InversionEnPozo.Movil.VistasModelos
{
    public class DetalleProyectoViewModel : BindableObject
    {
        private readonly IContratoService _contratoService;
        private readonly INotificacionService _notificacionService;

        private ProyectoResponseDto _proyecto = new();
        public ProyectoResponseDto Proyecto
        {
            get => _proyecto;
            set { _proyecto = value; OnPropertyChanged(); }
        }

        private string _estadoHito = "Hitos pendientes de solicitud.";
        public string EstadoHito
        {
            get => _estadoHito;
            set { _estadoHito = value; OnPropertyChanged(); }
        }

        private string _hashTransaccionHito = string.Empty;
        public string HashTransaccionHito
        {
            get => _hashTransaccionHito;
            set { _hashTransaccionHito = value; OnPropertyChanged(); }
        }

        public ICommand SolicitarDesembolsoCommand { get; }
        public ICommand VerTransaccionCommand { get; }

        public DetalleProyectoViewModel(IContratoService contratoService, INotificacionService notificacionService)
        {
            _contratoService = contratoService;
            _notificacionService = notificacionService;
            _notificacionService.OnMensajeRecibido += (mensaje) =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    EstadoHito = mensaje;
                    await Shell.Current.DisplayAlert("¡Aviso en Tiempo Real!", mensaje, "OK");
                });
            };

            SolicitarDesembolsoCommand = new Command(async () => await SolicitarDesembolsoAsync());
            VerTransaccionCommand = new Command<string>(async (hash) => await VerEnEtherscanAsync(hash));
        }

        private async Task SolicitarDesembolsoAsync()
        {
            EstadoHito = "Procesando desembolso en la blockchain...";

            int hitoIdEjemplo = 1;
            var (exito, datos, mensaje) = await _contratoService.AprobarHitoAsync(hitoIdEjemplo);

            if (exito && datos != null)
            {
                EstadoHito = $"¡Desembolso Exitoso!\nHito #{datos.HitoId} liberado.";
                HashTransaccionHito = datos.HashTransaccionHito;
            }
            else
            {
                EstadoHito = $"Error al solicitar desembolso: {mensaje}";
            }
        }

        private async Task VerEnEtherscanAsync(string hashTransaccion)
        {
            if (string.IsNullOrWhiteSpace(hashTransaccion))
            {
                await Shell.Current.DisplayAlert("Aviso", "Primero debes completar una transacción para obtener el hash.", "OK");
                return;
            }

            string urlEtherscan = $"https://sepolia.etherscan.io/tx/{hashTransaccion}";

            try
            {
                await Launcher.Default.OpenAsync(urlEtherscan);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"No se pudo abrir el navegador: {ex.Message}", "OK");
            }
        }
    }
}