using InversionEnPozo.Movil.VistasModelos;

namespace InversionEnPozo.Movil.Vistas;

public partial class DetalleProyectoPage : ContentPage
{
	public DetalleProyectoPage(DetalleProyectoViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}