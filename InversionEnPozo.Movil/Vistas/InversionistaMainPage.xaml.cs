using InversionEnPozo.Movil.VistasModelos;

namespace InversionEnPozo.Movil.Vistas;

public partial class InversionistaMainPage : ContentPage
{
    public InversionistaMainPage(InversionistaViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}