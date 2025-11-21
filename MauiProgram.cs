using Microsoft.Extensions.Logging;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;
using QRMySqlApp.Services;
using QRMySqlApp.ViewModels;

namespace QRMySqlApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseBarcodeReader() // ⭐ IMPORTANTE: Habilitar el lector de códigos QR/Barras
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // ⭐ REGISTRO DE SERVICIOS
            // Singleton: Una sola instancia para toda la aplicación
            builder.Services.AddSingleton<DatabaseService>();

            // Transient: Nueva instancia cada vez que se solicita
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainViewModel>();

            // Puedes agregar más servicios aquí:
            // builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
            // builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

