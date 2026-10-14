using InversionEnPozo.Movil.VistasModelos;

namespace InversionEnPozo.Movil.Vistas
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}