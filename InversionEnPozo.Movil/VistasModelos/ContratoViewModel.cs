using InversionEnPozo.Movil.Servicios;
using InversionEnPozo.Movil.Servicios.Interfaces;
using System.Windows.Input;


namespace InversionEnPozo.Movil.VistasModelos
{
    public class ContratoViewModel : BindableObject
    {
        private readonly IContratoService _contratoService;

        private string _nombreProyecto = string.Empty;
        public string NombreProyecto
        {
            get => _nombreProyecto;
            set { _nombreProyecto = value; OnPropertyChanged(); }
        }

        private string _descripcion = string.Empty;
        public string Descripcion
        {
            get => _descripcion;
            set { _descripcion = value; OnPropertyChanged(); }
        }

        private decimal _presupuestoTotal;
        public decimal PresupuestoTotal
        {
            get => _presupuestoTotal;
            set { _presupuestoTotal = value; OnPropertyChanged(); }
        }

        private string _resultadoHash = "Ningún contrato generado aún.";
        public string ResultadoHash
        {
            get => _resultadoHash;
            set { _resultadoHash = value; OnPropertyChanged(); }
        }

        private string _estadoHito = "Hitos pendientes de aprobación.";
        public string EstadoHito
        {
            get => _estadoHito;
            set { _estadoHito = value; OnPropertyChanged(); }
        }

        public ICommand CrearProyectoCommand { get; }
        public ICommand AprobarHitoCommand { get; }
        public ICommand VerTransaccionCommand { get; }

        public ContratoViewModel(IContratoService contratoService)
        {
            _contratoService = contratoService;

            CrearProyectoCommand = new Command(async () => await CrearProyectoAsync());
            AprobarHitoCommand = new Command(async () => await AprobarHitoAsync());
            VerTransaccionCommand = new Command<string>(async (hash) => await VerEnEtherscanAsync(hash));
        }

        private async Task VerEnEtherscanAsync(string hashTransaccion)
        {
            if (string.IsNullOrWhiteSpace(hashTransaccion))
            {
                await Shell.Current.DisplayAlert("Aviso", "No hay un hash de transacción disponible para este registro.", "OK");
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

        private async Task CrearProyectoAsync()
        {
            if (string.IsNullOrWhiteSpace(NombreProyecto) || PresupuestoTotal <= 0)
            {
                ResultadoHash = "Por favor ingresa un nombre y un presupuesto válido.";
                return;
            }

            ResultadoHash = "Generando contrato inteligente (Blockchain)...";

            var (exito, datos, mensaje) = await _contratoService.CrearProyectoAsync(NombreProyecto, Descripcion, PresupuestoTotal, "AdminConstructora");

            if (exito && datos != null)
            {
                ResultadoHash = $"¡Éxito!\nHash Génesis:\n{datos.HashContratoGenesis}";
            }
            else
            {
                ResultadoHash = $"Error: {mensaje}";
            }
        }

        private async Task AprobarHitoAsync()
        {
            EstadoHito = "Procesando validación del hito...";

            int hitoIdEjemplo = 1;
            var (exito, datos, mensaje) = await _contratoService.AprobarHitoAsync(hitoIdEjemplo);

            if (exito && datos != null)
            {
                EstadoHito = $"¡Hito #{datos.HitoId} Aprobado!\nHash Tx:\n{datos.HashTransaccionHito}";
            }
            else
            {
                EstadoHito = $"Error al aprobar hito: {mensaje}";
            }
        }
    }
}