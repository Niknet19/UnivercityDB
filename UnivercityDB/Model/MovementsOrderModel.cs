using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnivercityDB.Model
{
    public class MovementsOrderModel
    {
        private readonly string _connectionString;

        public MovementsOrderModel(string connectionString)
        {
            _connectionString = connectionString;
        }

        public MovementsOrderModel()
        {
            _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=ahubozq13q12;Database=University";
        }

        public List<MovementOrder> GetAllMovements()
        {
            string query = BuildBaseQuery();
            return ExecuteQuery(query);
        }

        public bool AddMovement(MovementOrder movement, int employeeID)
        {
            string query = @"
                INSERT INTO movements (reason, date, ordernumber)
                VALUES (@reason, @date, @ordernumber)
                RETURNING id";

            int movementId;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@reason", movement.Reason);
                    command.Parameters.AddWithValue("@date", movement.Date);
                    command.Parameters.AddWithValue("@ordernumber", movement.OrderNumber);

                    movementId = (int)command.ExecuteScalar();
                }
            }

            // Добавляем запись в промежуточную таблицу
            return LinkMovementToEmployee(movementId, employeeID);
        }

        public bool UpdateMovement(MovementOrder movement)
        {
            string query = @"
                UPDATE movements
                SET
                    reason = @reason,
                    date = @date,
                    ordernumber = @ordernumber
                WHERE id = @id";

            return ExecuteNonQuery(query, new Dictionary<string, object>
            {
                { "@id", movement.Id },
                { "@reason", movement.Reason },
                { "@date", movement.Date },
                { "@ordernumber", movement.OrderNumber },
            });
        }

        public bool DeleteMovement(int id)
        {
            // Удаляем связи с сотрудниками
            string deleteLinkQuery = "DELETE FROM movements_employee WHERE movementid = @id";

            if (!ExecuteNonQuery(deleteLinkQuery, new Dictionary<string, object> { { "@id", id } }))
                return false;

            // Удаляем сам приказ
            string deleteMovementQuery = "DELETE FROM movements WHERE id = @id";

            return ExecuteNonQuery(deleteMovementQuery, new Dictionary<string, object> { { "@id", id } });
        }

        public List<MovementOrder> SearchMovement(string searchTerm)
        {
            var res = Utils.ParseFullName(searchTerm);
            string query = BuildBaseQuery() + @"
                WHERE e.surname ILIKE @surname
                   AND e.firstname ILIKE @firstname
                   AND e.patronymic ILIKE @patronymic";

            return ExecuteQuery(query, new Dictionary<string, object>
            {
                { "@surname", $"%{res.Surname}%" },
                { "@firstname", $"%{res.FirstName}%" },
                { "@patronymic", $"%{res.Patronymic}%" }
            });
        }

        private string BuildBaseQuery()
        {
            return @"
            SELECT 
                m.id,
                e.surname || ' ' || e.firstname || ' ' || e.patronymic AS full_name,
                m.reason,
                m.date,
                m.ordernumber
            FROM movements m
            JOIN movements_employee em ON m.id = em.movementid
            JOIN employee e ON em.employeeid = e.id";
        }

        private bool LinkMovementToEmployee(int movementId, int employeeId)
        {
            string query = @"
                INSERT INTO movements_employee (employeeid, movementid)
                VALUES (@employeeid, @movementid)";

            return ExecuteNonQuery(query, new Dictionary<string, object>
            {
                { "@employeeid", employeeId },
                { "@movementid", movementId },
            });
        }

        private List<MovementOrder> ExecuteQuery(string query, Dictionary<string, object>? parameters = null)
        {
            var movements = new List<MovementOrder>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            movements.Add(new MovementOrder
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                EmployeeName = reader.GetString(reader.GetOrdinal("full_name")),
                                Reason = reader.GetString(reader.GetOrdinal("reason")),
                                Date = reader.GetDateTime(reader.GetOrdinal("date")),
                                OrderNumber = reader.GetString(reader.GetOrdinal("ordernumber")),
                            });
                        }
                    }
                }
            }

            return movements;
        }

        private bool ExecuteNonQuery(string query, Dictionary<string, object> parameters)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
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
