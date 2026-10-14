using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InversionEnPozo.Movil.Servicios;
using InversionEnPozo.Movil.Servicios.Interfaces;
using Plugin.Maui.Biometric;

namespace InversionEnPozo.Movil.VistasModelos
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string nombreCompleto = string.Empty;

        [ObservableProperty]
        private bool esRegistro = false;

        [ObservableProperty]
        private string textoBotonPrincipal = "Iniciar Sesión";

        [ObservableProperty]
        private string textoAlternativo = "¿No tienes cuenta? Regístrate aquí";

        [ObservableProperty]
        private List<string> listaPerfiles = new() { "Inversionista", "Constructora" };

        [ObservableProperty]
        private string perfilSeleccionado = "Inversionista";

        [ObservableProperty]
        private bool isBusy = false;

        public bool IsNotBusy => !IsBusy;

        partial void OnEsRegistroChanged(bool value)
        {
            TextoBotonPrincipal = value ? "Registrarse" : "Iniciar Sesión";
            TextoAlternativo = value ? "¿Ya tienes cuenta? Inicia sesión" : "¿No tienes cuenta? Regístrate aquí";
        }

        [ObservableProperty]
        private bool ocultarPassword = true;

        [ObservableProperty]
        private string iconoOjo = "👁️";

        [RelayCommand]
        void AlternarVerPassword()
        {
            OcultarPassword = !OcultarPassword;
            IconoOjo = OcultarPassword ? "👁️" : "🙈";
        }

        [RelayCommand]
        private void CambiarModo()
        {
            EsRegistro = !EsRegistro;
        }

        [RelayCommand]
        private async Task EjecutarAccionAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Atención", "Por favor completa todos los campos obligatorios.", "OK");
                return;
            }

            IsBusy = true;
            OnPropertyChanged(nameof(IsNotBusy));

            try
            {
                if (EsRegistro)
                {
                    if (string.IsNullOrWhiteSpace(NombreCompleto))
                    {
                        await Shell.Current.DisplayAlert("Atención", "El nombre completo es obligatorio para registrarse.", "OK");
                        return;
                    }

                    var resultado = await _authService.RegistrarAsync(Email, Password, NombreCompleto, PerfilSeleccionado);
                    await Shell.Current.DisplayAlert("Registro", resultado.Mensaje, "OK");
                    if (resultado.Exito)
                    {
                        EsRegistro = false;
                    }
                }
                else
                {
                    var resultado = await _authService.LoginAsync(Email, Password);
                    await Shell.Current.DisplayAlert("Acceso", resultado.Mensaje, "OK");

                    if (resultado.Exito)
                    {
                        await SecureStorage.SetAsync("rol_usuario", resultado.Rol);
                        await ProcesarIngresoExitosoAsync(resultado.Rol);
                    }
                }
            }
            finally
            {
                IsBusy = false;
                OnPropertyChanged(nameof(IsNotBusy));
            }
        }

        [RelayCommand]
        private async Task AutenticarBiometricoAsync()
        {
            try
            {
                IsBusy = true;
                OnPropertyChanged(nameof(IsNotBusy));

                var request = new AuthenticationRequest
                {
                    Title = "Acceso Biométrico",
                    Subtitle = "Coloca tu huella para ingresar a Inversión en Pozo",
                    Description = "Verificación de identidad",
                    NegativeText = "Cancelar"
                };

                var result = await BiometricAuthenticationService.Default.AuthenticateAsync(request, CancellationToken.None);

                // Validación corregida usando BiometricResponseStatus.Success
                if (result.Status == BiometricResponseStatus.Success)
                {
                    string rolGuardado = await SecureStorage.GetAsync("rol_usuario") ?? "Inversionista";
                    await ProcesarIngresoExitosoAsync(rolGuardado);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Falló", "No se pudo verificar la huella digital.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Ocurrió un error biométrico: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
                OnPropertyChanged(nameof(IsNotBusy));
            }
        }

        private async Task ProcesarIngresoExitosoAsync(string rol)
        {
            if (Shell.Current is AppShell appShell)
            {
                appShell.ConfigurarMenuPorRol(rol);
            }

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                if (rol == "Constructora")
                {
                    await Shell.Current.GoToAsync("///ConstructoraMainPage");
                }
                else
                {
                    await Shell.Current.GoToAsync("///InversionistaMainPage");
                }
            });
        }
    }
}