using InversionEnPozo.Movil.Servicios.Interfaces;
using InversionEnPozo.Movil.Vistas;

namespace InversionEnPozo.Movil
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registro de rutas para la navegación condicional por roles
            Routing.RegisterRoute(nameof(InversionistaMainPage), typeof(InversionistaMainPage));
            Routing.RegisterRoute(nameof(ConstructoraMainPage), typeof(ConstructoraMainPage));
            Routing.RegisterRoute(nameof(ContratoPage), typeof(ContratoPage));
            Routing.RegisterRoute(nameof(DetalleProyectoPage), typeof(DetalleProyectoPage));
        }

        public void ConfigurarMenuPorRol(string rol)
        {
            FlyoutBehavior = FlyoutBehavior.Flyout;
            MenuLogin.IsVisible = false;

            if (rol == "Constructora")
            {
                MenuConstructora.IsVisible = true;
                MenuInversionista.IsVisible = false;
                MenuContrato.IsVisible = true;
            }
            else
            {
                MenuConstructora.IsVisible = false;
                MenuInversionista.IsVisible = true;
                MenuContrato.IsVisible = false;
            }
        }

        private void OnCerrarMenuClicked(object sender, EventArgs e)
        {
            // Cierra el menú lateral al hacer clic en el botón ✕
            FlyoutIsPresented = false;
        }

        private async void OnCerrarSesionClicked(object sender, EventArgs e)
        {
            bool confirmar = await DisplayAlert("Cerrar Sesión", "¿Estás seguro de que deseas salir?", "Sí", "No");

            if (confirmar)
            {
                FlyoutBehavior = FlyoutBehavior.Disabled;

                MenuLogin.IsVisible = true;
                MenuConstructora.IsVisible = false;
                MenuInversionista.IsVisible = false;
                MenuContrato.IsVisible = false;

                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
    }
}