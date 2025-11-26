using segtrack.Models;
using segtrack.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace segtrack.ViewModels
{
    public class ViewModelFuncionario : BaseViewModel
    {
        private readonly DatabaseService _database;

        // Lista observable para mostrar en la UI (CollectionView)
        public ObservableCollection<Funcionario> Funcionarios { get; set; } =
            new ObservableCollection<Funcionario>();

        // Propiedades que se enlazan a la vista
        private Funcionario _funcionarioActual = new Funcionario();
        public Funcionario FuncionarioActual
        {
            get => _funcionarioActual;
            set
            {
                _funcionarioActual = value;
                OnPropertyChanged();
            }
        }

        // Comandos
        public ICommand CargarCommand { get; }
        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }

        public ViewModelFuncionario()
        {
            _database = new DatabaseService();

            CargarCommand = new Command(async () => await CargarFuncionarios());
            GuardarCommand = new Command(async () => await GuardarFuncionario());
            EliminarCommand = new Command<Funcionario>(async (f) => await EliminarFuncionario(f));
        }

        // ===============================
        // 1. Cargar funcionarios
        // ===============================
        public async Task CargarFuncionarios()
        {
            Funcionarios.Clear();

            using (var conn = _database.GetConnection())
            {
                await conn.OpenAsync();

                string query = "SELECT * FROM funcionario";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Funcionarios.Add(new Funcionario
                    {
                        IdFuncionario = reader.GetInt32("IdFuncionario"),
                        CargoFuncionario = reader.GetString("CargoFuncionario"),
                        QrCodigoFuncionario = reader.GetString("QrCodigoFuncionario"),
                        NombreFuncionario = reader.GetString("NombreFuncionario"),
                        TelefonoFuncionario = reader.GetString("TelefonoFuncionario"),
                        DocumentoFuncionario = reader.GetString("DocumentoFuncionario"),
                        CorreoFuncionario = reader.GetString("CorreoFuncionario"),
                        Estado = (EstadoFuncionario)Enum.Parse(typeof(EstadoFuncionario), reader.GetString("Estado")),
                        IdSede = reader.GetInt32("IdSede")
                    });
                }
            }
        }

        // ===============================
        // 2. Guardar (Insertar o actualizar)
        // ===============================
        public async Task GuardarFuncionario()
        {
            using (var conn = _database.GetConnection())
            {
                await conn.OpenAsync();

                string query = FuncionarioActual.IdFuncionario == 0
                    ? @"INSERT INTO funcionario 
                        (CargoFuncionario, QrCodigoFuncionario, NombreFuncionario, 
                        TelefonoFuncionario, DocumentoFuncionario, CorreoFuncionario, Estado, IdSede)
                        VALUES (@Cargo, @Qr, @Nombre, @Telefono, @Doc, @Correo, @Estado, @IdSede)"
                    : @"UPDATE funcionario SET
                        CargoFuncionario=@Cargo,
                        QrCodigoFuncionario=@Qr,
                        NombreFuncionario=@Nombre,
                        TelefonoFuncionario=@Telefono,
                        DocumentoFuncionario=@Doc,
                        CorreoFuncionario=@Correo,
                        Estado=@Estado,
                        IdSede=@IdSede
                        WHERE IdFuncionario=@Id";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Cargo", FuncionarioActual.CargoFuncionario);
                cmd.Parameters.AddWithValue("@Qr", FuncionarioActual.QrCodigoFuncionario);
                cmd.Parameters.AddWithValue("@Nombre", FuncionarioActual.NombreFuncionario);
                cmd.Parameters.AddWithValue("@Telefono", FuncionarioActual.TelefonoFuncionario);
                cmd.Parameters.AddWithValue("@Doc", FuncionarioActual.DocumentoFuncionario);
                cmd.Parameters.AddWithValue("@Correo", FuncionarioActual.CorreoFuncionario);
                cmd.Parameters.AddWithValue("@Estado", FuncionarioActual.Estado.ToString());
                cmd.Parameters.AddWithValue("@IdSede", FuncionarioActual.IdSede);

                if (FuncionarioActual.IdFuncionario != 0)
                    cmd.Parameters.AddWithValue("@Id", FuncionarioActual.IdFuncionario);

                await cmd.ExecuteNonQueryAsync();
            }

            await CargarFuncionarios();
            FuncionarioActual = new Funcionario();
        }

        // ===============================
        // 3. Eliminar funcionario
        // ===============================
        public async Task EliminarFuncionario(Funcionario f)
        {
            if (f == null) return;

            using (var conn = _database.GetConnection())
            {
                await conn.OpenAsync();
                string query = "DELETE FROM funcionario WHERE IdFuncionario=@Id";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", f.IdFuncionario);

                await cmd.ExecuteNonQueryAsync();
            }

            Funcionarios.Remove(f);
        }
    }
}
