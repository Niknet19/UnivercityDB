using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnivercityDB.Model
{
    public class SanctionsOrderModel
    {
        private readonly string _connectionString;


        public SanctionsOrderModel(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SanctionsOrderModel()
        {
            _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=ahubozq13q12;Database=University";
        }

        public List<SanctionsOrder> GetAllOrders(string type)
        {
            string query = BuildBaseQuery(type);
            return ExecuteQuery(query);
        }

        public bool AddOrder(string tablename, SanctionsOrder order, int employeeID)
        {

            string query = "";
            if (tablename == "Penalties")
            {
                query += @"INSERT INTO penalties ";
                tablename = "penaltytype";
            }
            if (tablename == "Promotions")
            {
                query += @"INSERT INTO promotions ";
                tablename = "promotiontype";
            }

            query +=
                $@"(employeeid, typeid, date, ordernumber)
                VALUES
                (@employeeid,
                (SELECT id FROM {tablename} WHERE name = @type),
                @date,
                @ordernumber)";
            return ExecuteNonQuery(query, new Dictionary<string, object>
            {
                { "@ordernumber", order.OrderNumber },
                { "@date", order.Date },
                { "@type", order.SanctionType },
                { "@employeeid", employeeID },
            });
        }

        public bool UpdateOrder(string tablename, SanctionsOrder order)
        {
            string query = "";
            if (tablename == "Penalties")
            {
                query += @"UPDATE penalties ";
                tablename = "penaltytype";
            }
            if (tablename == "Promotions")
            {
                query += @"UPDATE promotions ";
                tablename = "promotiontype";
            }
            query +=
                @$"SET
                date = @date,
                ordernumber = @ordernumber,
                typeid = (SELECT id FROM {tablename} WHERE name ILIKE @type)
                WHERE id = @id";
            return ExecuteNonQuery(query, new Dictionary<string, object>
            {
                { "@id", order.Id },
                { "@ordernumber", order.OrderNumber },
                { "@date", order.Date },
                 { "@type", "%" + order.SanctionType + "%" }
            });
        }

        public bool DeleteOrder(string tablename, int id)
        {
            string query = "";
            if (tablename == "Penalties") query += @"DELETE FROM penalties ";
            if (tablename == "Promotions") query += @"DELETE FROM promotions ";
            query += "WHERE id = @id ";
            return ExecuteNonQuery(query, new Dictionary<string, object>
            {
                { "@id", id },
            });
        }


        public List<SanctionsOrder> SearchOrder(string searchTerm, string type)
        {
            var res = Utils.ParseFullName(searchTerm);
            string query = BuildBaseQuery(type) + @"
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

        private string BuildBaseQuery(string tablename)
        {
            string basestr = $@"
            SELECT 
                p.id,
                e.surname || ' ' || e.firstname || ' ' || e.patronymic AS full_name,
                pt.name AS penalty_type,
                p.date,
                p.ordernumber
            FROM {tablename} p
            JOIN employee e ON p.employeeid = e.id ";
            if (tablename == "Penalties") basestr += "JOIN penaltytype pt ON p.typeid = pt.id";
            if (tablename == "Promotions") basestr += "JOIN promotiontype pt ON p.typeid = pt.id";
            return basestr;
        }


        private List<SanctionsOrder> ExecuteQuery(string query, Dictionary<string, object>? parameters = null)
        {
            var orders = new List<SanctionsOrder>();

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
                            orders.Add(new SanctionsOrder
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                EmployeeName = reader.GetString(reader.GetOrdinal("full_name")),
                                SanctionType = reader.GetString(reader.GetOrdinal("penalty_type")),
                                Date = reader.GetDateTime(reader.GetOrdinal("date")),
                                OrderNumber = reader.GetString(reader.GetOrdinal("ordernumber")),
                            }); ;
                        }
                    }
                }
            }

            return orders;
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
