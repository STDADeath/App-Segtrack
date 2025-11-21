using QRMySqlApp.ViewModels;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace QRMySqlApp
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _viewModel;

        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        /// <summary>
        /// Se ejecuta cuando la página aparece en pantalla
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Solicitar permisos de cámara
            await SolicitarPermisoCamara();
        }

        /// <summary>
        /// Solicita permisos de cámara al usuario
        /// </summary>
        private async Task SolicitarPermisoCamara()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Camera>();
                }

                if (status == PermissionStatus.Granted)
                {
                    // Permiso concedido, activar la detección
                    cameraBarcodeReaderView.IsDetecting = true;
                }
                else
                {
                    // Permiso denegado
                    await DisplayAlert(
                        "Permiso Denegado",
                        "Se necesita acceso a la cámara para escanear códigos QR. " +
                        "Por favor, habilita el permiso en la configuración de la aplicación.",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al solicitar permisos: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Se ejecuta cuando la cámara detecta uno o más códigos de barras/QR
        /// </summary>
        private void OnBarcodesDetected(object sender, BarcodeDetectionEventArgs e)
        {
            var first = e.Results?.FirstOrDefault();

            if (first != null)
            {
                // Detener la detección temporalmente
                cameraBarcodeReaderView.IsDetecting = false;

                // Procesar el código QR en el hilo principal
                Dispatcher.Dispatch(async () =>
                {
                    // Vibración para feedback háptico (opcional)
                    try
                    {
                        var duration = TimeSpan.FromMilliseconds(100);
                        Vibration.Default.Vibrate(duration);
                    }
                    catch
                    {
                        // Ignorar si no está disponible
                    }

                    // Procesar el código QR
                    await _viewModel.ProcesarCodigoQRCommand.ExecuteAsync(first.Value);
                });
            }
        }

        /// <summary>
        /// Se ejecuta cuando la página desaparece de la pantalla
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Detener la cámara para ahorrar batería
            if (cameraBarcodeReaderView != null)
            {
                cameraBarcodeReaderView.IsDetecting = false;
            }
        }

        /// <summary>
        /// Maneja el evento de toque en el área de la cámara (opcional)
        /// Permite enfocar manualmente
        /// </summary>
        private void OnCameraTapped(object sender, TappedEventArgs e)
        {
            // Implementar enfoque manual si es necesario
            // cameraBarcodeReaderView.AutoFocus();
        }
    }
}

