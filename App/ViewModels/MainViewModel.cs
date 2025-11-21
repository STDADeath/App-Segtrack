using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QRMySqlApp.Models;
using QRMySqlApp.Services;
using System.Diagnostics;

namespace QRMySqlApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        // Propiedades del código QR
        [ObservableProperty]
        private string datosQR = "";

        [ObservableProperty]
        private string serial = "";

        [ObservableProperty]
        private string campo1 = "";

        [ObservableProperty]
        private string campo2 = "";

        [ObservableProperty]
        private string campo3 = "";

        // Propiedades del producto desde BD
        [ObservableProperty]
        private string nombreProducto = "";

        [ObservableProperty]
        private string descripcionProducto = "";

        [ObservableProperty]
        private decimal precioProducto = 0;

        [ObservableProperty]
        private int stockProducto = 0;

        // Propiedades de estado de la UI
        [ObservableProperty]
        private string mensajeEstado = "Esperando escaneo...";

        [ObservableProperty]
        private bool isScanning = true;

        [ObservableProperty]
        private bool productFound = false;

        [ObservableProperty]
        private bool showNotFoundMessage = false;

        [ObservableProperty]
        private bool isLoading = false;

        public MainViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        /// <summary>
        /// Procesa el código QR escaneado
        /// </summary>
        [RelayCommand]
        public async Task ProcesarCodigoQR(string codigoQR)
        {
            try
            {
                IsScanning = false;
                IsLoading = true;
                DatosQR = codigoQR;
                MensajeEstado = "Procesando código QR...";

                // Limpiar datos anteriores
                LimpiarDatosProducto();

                // Parsear el código QR
                // Formato esperado: "SERIAL|CAMPO1|CAMPO2|CAMPO3"
                ParsearCodigoQR(codigoQR);

                // Buscar en la base de datos si hay serial
                if (!string.IsNullOrEmpty(Serial))
                {
                    MensajeEstado = "Buscando en base de datos...";
                    await BuscarProducto();
                }
                else
                {
                    MensajeEstado = "QR escaneado - Sin serial para buscar";
                    ShowNotFoundMessage = false;
                }
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error: {ex.Message}";
                Debug.WriteLine($"✗ Error al procesar QR: {ex.Message}");
                ShowNotFoundMessage = true;
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Parsea el código QR según el formato esperado
        /// </summary>
        private void ParsearCodigoQR(string codigoQR)
        {
            try
            {
                // Formato: "SERIAL|CAMPO1|CAMPO2|CAMPO3"
                var partes = codigoQR.Split('|');

                Serial = partes.Length > 0 ? partes[0].Trim() : "";
                Campo1 = partes.Length > 1 ? partes[1].Trim() : "";
                Campo2 = partes.Length > 2 ? partes[2].Trim() : "";
                Campo3 = partes.Length > 3 ? partes[3].Trim() : "";

                Debug.WriteLine($"✓ QR parseado - Serial: {Serial}, Campos: {Campo1}, {Campo2}, {Campo3}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"✗ Error al parsear QR: {ex.Message}");
            }
        }

        /// <summary>
        /// Busca el producto en la base de datos por serial
        /// </summary>
        [RelayCommand]
        private async Task BuscarProducto()
        {
            try
            {
                if (string.IsNullOrEmpty(Serial))
                {
                    MensajeEstado = "No hay serial para buscar";
                    ShowNotFoundMessage = false;
                    return;
                }

                var producto = await _databaseService.BuscarProductoPorSerialAsync(Serial);

                if (producto != null)
                {
                    // Producto encontrado
                    NombreProducto = producto.Nombre;
                    DescripcionProducto = producto.Descripcion;
                    PrecioProducto = producto.Precio;
                    StockProducto = producto.Stock;
                    ProductFound = true;
                    ShowNotFoundMessage = false;
                    MensajeEstado = "✓ Producto encontrado";

                    Debug.WriteLine($"✓ Producto encontrado: {producto.Nombre}");
                }
                else
                {
                    // Producto no encontrado
                    LimpiarDatosProducto();
                    ProductFound = false;
                    ShowNotFoundMessage = true;
                    MensajeEstado = "✗ Producto no encontrado";

                    Debug.WriteLine($"✗ Producto no encontrado con serial: {Serial}");
                }
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error en búsqueda: {ex.Message}";
                ProductFound = false;
                ShowNotFoundMessage = true;
                Debug.WriteLine($"✗ Error en BuscarProducto: {ex.Message}");
            }
        }

        /// <summary>
        /// Reinicia el escaneo y limpia todos los datos
        /// </summary>
        [RelayCommand]
        private void ReiniciarEscaneo()
        {
            DatosQR = "";
            Serial = "";
            Campo1 = "";
            Campo2 = "";
            Campo3 = "";
            LimpiarDatosProducto();
            IsScanning = true;
            MensajeEstado = "Esperando escaneo...";

            Debug.WriteLine("✓ Escaneo reiniciado");
        }

        /// <summary>
        /// Prueba la conexión con la base de datos
        /// </summary>
        [RelayCommand]
        private async Task TestConexion()
        {
            try
            {
                IsLoading = true;
                MensajeEstado = "Probando conexión...";

                bool conectado = await _databaseService.TestConnectionAsync();

                if (conectado)
                {
                    MensajeEstado = "✓ Conexión exitosa a MySQL";
                    await Task.Delay(2000); // Mostrar mensaje por 2 segundos
                    MensajeEstado = IsScanning ? "Esperando escaneo..." : "Listo para escanear";
                }
                else
                {
                    MensajeEstado = "✗ Error de conexión a MySQL";
                }
            }
            catch (Exception ex)
            {
                MensajeEstado = $"✗ Error: {ex.Message}";
                Debug.WriteLine($"✗ Error en TestConexion: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Limpia los datos del producto
        /// </summary>
        private void LimpiarDatosProducto()
        {
            NombreProducto = "";
            DescripcionProducto = "";
            PrecioProducto = 0;
            StockProducto = 0;
            ProductFound = false;
            ShowNotFoundMessage = false;
        }

        /// <summary>
        /// Busca manualmente un producto por serial (útil para testing)
        /// </summary>
        [RelayCommand]
        private async Task BuscarManual(string serialBuscar)
        {
            if (string.IsNullOrWhiteSpace(serialBuscar))
                return;

            Serial = serialBuscar;
            await BuscarProducto();
        }
    }
}