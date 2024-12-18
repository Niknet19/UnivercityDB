using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;

namespace UnivercityDB.Model
{
    public class CatalogsModel
    {
        string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=ahubozq13q12;Database=University";

        public bool AddToCatalog(string name, string tableName)
        {
            string query =
                $@"INSERT INTO {tableName}
                (name)
                VALUES
                (@name)";
            return ExecuteNonQuery(query, new Dictionary<string, object>
            {
                { "@name", name }
            });
        }

        public List<string> Search(string seacrhText, string tableName)
        {
            string query = $"SELECT name FROM {tableName} WHERE name ILIKE '%{seacrhText}%'";

            List<string> names = new List<string>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Создаем команду для выполнения запроса
                using (var command = new NpgsqlCommand(query, connection))
                {
                    // Выполняем запрос и читаем результаты
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            names.Add(reader.GetString(0));
                        }
                    }
                }
                connection.Close();
            }

            return names;

        }

        public bool UpdateCatalog(string name, string newName, string tableName)
        {
            string query =
                $@"UPDATE {tableName}
                SET name = @newName
                WHERE id = (SELECT id FROM {tableName} WHERE name = @name)";
            return ExecuteNonQuery(query, new Dictionary<string, object>
            {
                { "@name", name },
                { "@newName", newName }
            });
        }


        public bool DeleteFromCatalog(string name, string tableName)
        {
            string query =
                $@"DELETE FROM {tableName}
                WHERE name = @name";
            return ExecuteNonQuery(query, new Dictionary<string, object>
            {
                { "@name", name }
            });
        }

        public List<string> GetNamesFromTable(string tableName, string opntionalQuery = "")
        {

            //string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=ahubozq13q12;Database=University";
            // Список для хранения результатов
            List<string> names = new List<string>();

            try
            {
                // Создаем подключение к базе данных
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL-запрос с параметризированным именем таблицы
                    string query = $"SELECT name FROM {tableName} ";
                    query += opntionalQuery;

                    // Создаем команду для выполнения запроса
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        // Выполняем запрос и читаем результаты
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                names.Add(reader.GetString(0));
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }

            return names;
        }

        private bool ExecuteNonQuery(string query, Dictionary<string, object> parameters)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }

                    int affectedRows = command.ExecuteNonQuery();
                    return affectedRows > 0;
                }
            }
        }



    }
}
