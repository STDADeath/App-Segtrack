using MySql.Data.MySqlClient;
using QRMySqlApp.Models;
using System.Data;
using System.Diagnostics;

namespace QRMySqlApp.Services
{
    public class DatabaseService
    {
        // ⚠️ IMPORTANTE: REEMPLAZAR CON TUS DATOS REALES DE CONEXIÓN
        private const string SERVER = "tu-servidor.com"; // Ejemplo: "mysql.mihost.com" o "192.168.1.100"
        private const string DATABASE = "nombre_base_datos";
        private const string USER = "usuario_mysql";
        private const string PASSWORD = "contraseña_mysql";
        private const string PORT = "3306";

        private string GetConnectionString()
        {
            return $"Server={SERVER};Port={PORT};Database={DATABASE};Uid={USER};Pwd={PASSWORD};SslMode=Required;";
        }

        /// <summary>
        /// Prueba la conexión a la base de datos
        /// </summary>
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var connection = new MySqlConnection(GetConnectionString());
                await connection.OpenAsync();
                Debug.WriteLine("✓ Conexión a MySQL exitosa");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"✗ Error de conexión a MySQL: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Busca un producto por su serial en la base de datos
        /// </summary>
        public async Task<ProductData> BuscarProductoPorSerialAsync(string serial)
        {
            try
            {
                using var connection = new MySqlConnection(GetConnectionString());
                await connection.OpenAsync();

                string query = @"SELECT serial, nombre, descripcion, precio, stock, fecha_registro 
                               FROM productos 
                               WHERE serial = @Serial 
                               LIMIT 1";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Serial", serial);

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new ProductData
                    {
                        Serial = reader.GetString("serial"),
                        Nombre = reader.GetString("nombre"),
                        Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion"))
                            ? "" : reader.GetString("descripcion"),
                        Precio = reader.GetDecimal("precio"),
                        Stock = reader.GetInt32("stock"),
                        FechaRegistro = reader.GetDateTime("fecha_registro")
                    };
                }

                Debug.WriteLine($"✗ No se encontró producto con serial: {serial}");
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"✗ Error al buscar producto: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los productos de la base de datos (limitado a 100)
        /// </summary>
        public async Task<List<ProductData>> ObtenerTodosProductosAsync()
        {
            var productos = new List<ProductData>();

            try
            {
                using var connection = new MySqlConnection(GetConnectionString());
                await connection.OpenAsync();

                string query = @"SELECT serial, nombre, descripcion, precio, stock, fecha_registro 
                               FROM productos 
                               ORDER BY fecha_registro DESC
                               LIMIT 100";

                using var command = new MySqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    productos.Add(new ProductData
                    {
                        Serial = reader.GetString("serial"),
                        Nombre = reader.GetString("nombre"),
                        Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion"))
                            ? "" : reader.GetString("descripcion"),
                        Precio = reader.GetDecimal("precio"),
                        Stock = reader.GetInt32("stock"),
                        FechaRegistro = reader.GetDateTime("fecha_registro")
                    });
                }

                Debug.WriteLine($"✓ Se obtuvieron {productos.Count} productos");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"✗ Error al obtener productos: {ex.Message}");
            }

            return productos;
        }

        /// <summary>
        /// Inserta un nuevo producto en la base de datos
        /// </summary>
        public async Task<bool> InsertarProductoAsync(ProductData producto)
        {
            try
            {
                using var connection = new MySqlConnection(GetConnectionString());
                await connection.OpenAsync();

                string query = @"INSERT INTO productos (serial, nombre, descripcion, precio, stock) 
                               VALUES (@Serial, @Nombre, @Descripcion, @Precio, @Stock)";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Serial", producto.Serial);
                command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                command.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                command.Parameters.AddWithValue("@Precio", producto.Precio);
                command.Parameters.AddWithValue("@Stock", producto.Stock);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                Debug.WriteLine($"✓ Producto insertado: {producto.Serial}");
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"✗ Error al insertar producto: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Actualiza el stock de un producto
        /// </summary>
        public async Task<bool> ActualizarStockAsync(string serial, int nuevoStock)
        {
            try
            {
                using var connection = new MySqlConnection(GetConnectionString());
                await connection.OpenAsync();

                string query = @"UPDATE productos 
                               SET stock = @Stock 
                               WHERE serial = @Serial";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Stock", nuevoStock);
                command.Parameters.AddWithValue("@Serial", serial);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                Debug.WriteLine($"✓ Stock actualizado para serial: {serial}");
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"✗ Error al actualizar stock: {ex.Message}");
                return false;
            }
        }
    }
}
