using InversionEnPozo.Movil.VistasModelos;

namespace InversionEnPozo.Movil.Vistas;

public partial class ConstructoraMainPage : ContentPage
{
	public ConstructoraMainPage(ConstructoraViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}