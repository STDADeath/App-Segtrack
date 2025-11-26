using MySql.Data.MySqlClient;
using segtrack.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace segtrack.Services
{
    public class FuncionarioService
    {
        private readonly DatabaseService _databaseService;

        public FuncionarioService()
        {
            _databaseService = new DatabaseService();
        }

        // ?? Obtener todos los funcionarios
        public async Task<List<Funcionario>> GetFuncionariosAsync()
        {
            List<Funcionario> lista = new List<Funcionario>();

            using (var conn = _databaseService.GetConnection())
            {
                await conn.OpenAsync();

                string query = "SELECT * FROM funcionario";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new Funcionario
                        {
                            IdFuncionario = reader.GetInt32("IdFuncionario"),
                            CargoFuncionario = reader.GetString("CargoFuncionario"),
                            QrCodigoFuncionario = reader.GetString("QrCodigoFuncionario"),
                            NombreFuncionario = reader.GetString("NombreFuncionario"),
                            TelefonoFuncionario = reader.GetString("TelefonoFuncionario"),
                            DocumentoFuncionario = reader.GetString("DocumentoFuncionario"),
                            CorreoFuncionario = reader.GetString("CorreoFuncionario"),
                            Estado = reader.GetString("Estado") == "Activo" ?
                                EstadoFuncionario.Activo : EstadoFuncionario.Inactivo,
                            IdSede = reader.GetInt32("IdSede")
                        });
                    }
                }
            }

            return lista;
        }

        // ?? Insertar funcionario
        public async Task<bool> InsertFuncionarioAsync(Funcionario f)
        {
            using (var conn = _databaseService.GetConnection())
            {
                await conn.OpenAsync();

                string query = @"INSERT INTO funcionario 
                (CargoFuncionario, QrCodigoFuncionario, NombreFuncionario, TelefonoFuncionario, DocumentoFuncionario, 
                 CorreoFuncionario, Estado, IdSede)
                 VALUES (@cargo, @qr, @nombre, @tel, @doc, @correo, @estado, @sede)";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@cargo", f.CargoFuncionario);
                    cmd.Parameters.AddWithValue("@qr", f.QrCodigoFuncionario);
                    cmd.Parameters.AddWithValue("@nombre", f.NombreFuncionario);
                    cmd.Parameters.AddWithValue("@tel", f.TelefonoFuncionario);
                    cmd.Parameters.AddWithValue("@doc", f.DocumentoFuncionario);
                    cmd.Parameters.AddWithValue("@correo", f.CorreoFuncionario);
                    cmd.Parameters.AddWithValue("@estado", f.Estado.ToString());
                    cmd.Parameters.AddWithValue("@sede", f.IdSede);

                    return await cmd.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        // ?? Actualizar funcionario
        public async Task<bool> UpdateFuncionarioAsync(Funcionario f)
        {
            using (var conn = _databaseService.GetConnection())
            {
                await conn.OpenAsync();

                string query = @"UPDATE funcionario SET
                    CargoFuncionario=@cargo,
                    QrCodigoFuncionario=@qr,
                    NombreFuncionario=@nombre,
                    TelefonoFuncionario=@tel,
                    DocumentoFuncionario=@doc,
                    CorreoFuncionario=@correo,
                    Estado=@estado,
                    IdSede=@sede
                WHERE IdFuncionario=@id";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@cargo", f.CargoFuncionario);
                    cmd.Parameters.AddWithValue("@qr", f.QrCodigoFuncionario);
                    cmd.Parameters.AddWithValue("@nombre", f.NombreFuncionario);
                    cmd.Parameters.AddWithValue("@tel", f.TelefonoFuncionario);
                    cmd.Parameters.AddWithValue("@doc", f.DocumentoFuncionario);
                    cmd.Parameters.AddWithValue("@correo", f.CorreoFuncionario);
                    cmd.Parameters.AddWithValue("@estado", f.Estado.ToString());
                    cmd.Parameters.AddWithValue("@sede", f.IdSede);
                    cmd.Parameters.AddWithValue("@id", f.IdFuncionario);

                    return await cmd.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        // ?? Eliminar funcionario
        public async Task<bool> DeleteFuncionarioAsync(int id)
        {
            using (var conn = _databaseService.GetConnection())
            {
                await conn.OpenAsync();

                string query = "DELETE FROM funcionario WHERE IdFuncionario=@id";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    return await cmd.ExecuteNonQueryAsync() > 0;
                }
            }
        }
    }
}
