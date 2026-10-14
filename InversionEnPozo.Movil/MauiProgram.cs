using Microsoft.Extensions.Logging;
using InversionEnPozo.Movil.Servicios;
using InversionEnPozo.Movil.VistasModelos;
using InversionEnPozo.Movil.Vistas;
using InversionEnPozo.Movil.Servicios.Interfaces;

namespace InversionEnPozo.Movil
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Servicios (Singletons o Transients según convenga)
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IProyectoService, ProyectoService>();
            builder.Services.AddTransient<IContratoService, ContratoService>();

            // Vistas y ViewModels - Login
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginViewModel>();

            // Vistas y ViewModels - Panel Constructora
            builder.Services.AddTransient<ConstructoraMainPage>();
            builder.Services.AddTransient<ConstructoraViewModel>();

            // Vistas y ViewModels - Panel Inversionista
            builder.Services.AddTransient<InversionistaMainPage>();
            builder.Services.AddTransient<InversionistaViewModel>();

            // Vistas y ViewModels - Smart Contracts / Proyectos
            builder.Services.AddTransient<ContratoPage>();
            builder.Services.AddTransient<ContratoViewModel>();

            builder.Services.AddTransient<DetalleProyectoPage>();
            builder.Services.AddTransient<DetalleProyectoViewModel>();

            builder.Services.AddSingleton<INotificacionService, NotificacionService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}