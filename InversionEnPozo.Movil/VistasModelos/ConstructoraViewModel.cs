using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InversionEnPozo.Movil.Vistas;

namespace InversionEnPozo.Movil.VistasModelos
{
    public partial class ConstructoraViewModel : ObservableObject
    {
        public ConstructoraViewModel()
        {
        }

        [RelayCommand]
        private async Task IrANuevoProyectoAsync()
        {
            await Shell.Current.GoToAsync(nameof(ContratoPage));
        }

        [RelayCommand]
        private async Task IrADetalleProyectoAsync()
        {
            await Shell.Current.GoToAsync(nameof(DetalleProyectoPage));
        }
    }
}