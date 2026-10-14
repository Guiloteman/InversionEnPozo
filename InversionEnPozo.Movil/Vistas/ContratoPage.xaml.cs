using InversionEnPozo.Movil.VistasModelos;

namespace InversionEnPozo.Movil.Vistas;

public partial class ContratoPage : ContentPage
{
	public ContratoPage(ContratoViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}