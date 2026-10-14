using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InversionEnPozo.Movil.Modelos;
using InversionEnPozo.Movil.Servicios.Interfaces;
using System.Collections.ObjectModel;

namespace InversionEnPozo.Movil.VistasModelos
{
    public partial class InversionistaViewModel : ObservableObject
    {
        private readonly IContratoService _contratoService;

        [ObservableProperty]
        private ObservableCollection<ProyectoResponseDto> proyectos = new();

        [ObservableProperty]
        private bool estaCargando;

        public InversionistaViewModel(IContratoService contratoService)
        {
            _contratoService = contratoService;
            _ = CargarProyectosAsync();
        }

        [RelayCommand]
        public async Task CargarProyectosAsync()
        {
            if (EstaCargando) return;

            try
            {
                EstaCargando = true;
                var lista = await _contratoService.ObtenerProyectosActivosAsync();

                proyectos.Clear();
                foreach (var p in lista)
                {
                    proyectos.Add(p);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"No se pudieron cargar los proyectos: {ex.Message}", "OK");
            }
            finally
            {
                EstaCargando = false;
            }
        }

        [RelayCommand]
        private async Task InvertirAsync(ProyectoResponseDto proyecto)
        {
            if (proyecto == null) return;

            string montoStr = await Shell.Current.DisplayPromptAsync(
                "Invertir en Pozo",
                $"Ingrese el monto a invertir en '{proyecto.Titulo}':",
                keyboard: Keyboard.Numeric,
                accept: "Invertir",
                cancel: "Cancelar");

            if (decimal.TryParse(montoStr, out decimal montoAporte) && montoAporte > 0)
            {
                await Shell.Current.DisplayAlert("Éxito", $"Preparando transferencia de {montoAporte:C} al contrato del proyecto...", "OK");
            }
        }
    }
}