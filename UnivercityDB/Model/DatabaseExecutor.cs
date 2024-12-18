using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnivercityDB.Model
{
    using Npgsql;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Windows;

    namespace UnivercityDB.Helpers
    {
        internal class DatabaseExecutor
        {
            private readonly string _connectionString;

            private ObservableCollection<DataRow> _queryResults;

            public DatabaseExecutor(string connectionString)
            {
                _connectionString = connectionString;
            }

            public DatabaseExecutor()
            {
                _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=ahubozq13q12;Database=University";
            }

            public Dictionary<string,string> GenerateQueryTemplates()
            {
                var dict = new Dictionary<string, string>
                {
                    { "Промежуточная таблица сотрудники - должности", "SELECT * FROM jobtitles_employee" },
                    { "Промежуточная таблица сотрудники - подразделения", "SELECT * FROM departments_employee"},
                    { "Промежуточная таблица сотрудники - перемещения", "SELECT * FROM movements_employee" }
                };
                return dict;
            }

            public string ExecuteNonQuery(string query)
            {
                try
                {
                    using (var connection = new NpgsqlConnection(_connectionString))
                    {
                        connection.Open();

                        using (var command = new NpgsqlCommand(query, connection))
                        {
                            
                            int rowsAffected = command.ExecuteNonQuery();
                            return $"{rowsAffected} row(s) affected."; // Возвращаем количество затронутых строк
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Возвращаем сообщение об ошибке
                    MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return $"Error: {ex.Message}";
                }
            }

            public async Task<DataTable> ExecuteSelectQueryAsync(string query)
                {
                    var dataTable = new DataTable();

                try { 
                    using (var connection = new NpgsqlConnection(_connectionString))
                    {
                        await connection.OpenAsync();

                        using (var command = new NpgsqlCommand(query, connection))
                        {
                            using (var reader = await command.ExecuteReaderAsync())
                            {
                                // Загружаем данные из reader в DataTable
                                dataTable.Load(reader);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}","Ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
                    //throw;
                }

                return dataTable;
            }
        }
    }
}