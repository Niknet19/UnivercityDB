using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UnivercityDB.Model
{
    internal class PrevEmploymentsModel
    {
        private readonly string _connectionString;

        public PrevEmploymentsModel(string connectionString)
        {
            _connectionString = connectionString;
        }

        public PrevEmploymentsModel()
        {
            _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=ahubozq13q12;Database=University";
        }

        private string BuildBaseQuery()
        {
            return @"
            SELECT 
            pe.id,
            pe.employeeid,
            pe.companyname,
            jt.name AS job_title_name,
            pe.startdate,
            pe.enddate,
            pe.termination_reason,
            pe.phonenumber,
            st.name AS street_name,
            c.name AS city_name,
            pe.housenumber
        FROM 
            previousemployments pe
        LEFT JOIN 
            jobtitles jt ON pe.jobtitleid = jt.id
        LEFT JOIN 
            street st ON pe.streetid = st.id
        LEFT JOIN 
            city c ON pe.cityid = c.id";
        }


        public List<PreviousEmployment> SearchEmployemntsByCompanyName(string searchTerm, int employeeId)
        {
            string query = BuildBaseQuery() + " WHERE pe.companyname ILIKE @companyname AND pe.employeeid = @employeeid";
            return ExecuteQuery(query, new Dictionary<string, object> { { "@companyname", "%"+searchTerm +"%" },
            { "@employeeid", employeeId }});
        }
        public List<PreviousEmployment> GetAllPreviousEmployments()
        {
            string query = BuildBaseQuery();
            return ExecuteQuery(query);
        }

        public List<PreviousEmployment> GetPreviousEmploymentsByEmployeeId(int employeeId)
        {
            
            string query = BuildBaseQuery() + " WHERE pe.employeeid = @employeeid";
            return ExecuteQuery(query, new Dictionary<string, object> { { "@employeeid", employeeId } });
        }

        public bool AddPreviousEmployment(PreviousEmployment employment)
        {
            const string query = @"
        INSERT INTO previousemployments (
            employeeid,
            companyname,
            jobtitleid,
            startdate,
            enddate,
            termination_reason,
            phonenumber,
            streetid,
            cityid,
            housenumber
        ) VALUES (
            @employeeid,
            @companyname,
            (SELECT id FROM jobtitles WHERE name = @position),
            @startdate,
            @enddate,
            @reasonforleaving,
            @companyphone,
            (SELECT id FROM street WHERE name = @street),
            (SELECT id FROM city WHERE name = @city),
            @housenumber)";
            return ExecuteNonQuery(query, MapEmploymentToParameters(employment));
        }

        public bool UpdatePreviousEmployment(PreviousEmployment employment)
        {
            const string query = @"
            UPDATE previousemployments
            SET
                employeeid = @employeeid,
                companyname = @companyname,
                jobtitleid = (SELECT id FROM jobtitles WHERE name = @position),
                startdate = @startdate,
                enddate = @enddate,
                termination_reason = @reasonforleaving,
                phonenumber = @companyphone,
                streetid = @streetid,
                cityid = @cityid,
                housenumber = @housenumber
            WHERE id = @id";
            return ExecuteNonQuery(query, MapEmploymentToParameters(employment));
        }

        public bool DeletereviousEmployment(PreviousEmployment employment)
        {
            const string query = @"
            DELETE FROM previousemployments
            WHERE id = @id ";
            return ExecuteNonQuery(query, MapEmploymentToParameters(employment));
        }


        private List<PreviousEmployment> ExecuteQuery(string query, Dictionary<string, object>? parameters = null)
        {

           
                var employments = new List<PreviousEmployment>();

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
                                employments.Add(new PreviousEmployment
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    EmployeeId = reader.GetInt32(reader.GetOrdinal("employeeid")),
                                    CompanyName = reader.GetString(reader.GetOrdinal("companyname")),
                                    Position = reader.GetString(reader.GetOrdinal("job_title_name")),
                                    StartDate = reader.GetDateTime(reader.GetOrdinal("startdate")),
                                    EndDate = reader.GetDateTime(reader.GetOrdinal("enddate")),
                                    ReasonForLeaving = reader.IsDBNull(reader.GetOrdinal("termination_reason")) ? null : reader.GetString(reader.GetOrdinal("termination_reason")),
                                    CompanyPhone = reader.IsDBNull(reader.GetOrdinal("phonenumber")) ? null : reader.GetString(reader.GetOrdinal("phonenumber")),
                                    Street = reader.IsDBNull(reader.GetOrdinal("street_name")) ? null : reader.GetString(reader.GetOrdinal("street_name")),
                                    City = reader.IsDBNull(reader.GetOrdinal("city_name")) ? null : reader.GetString(reader.GetOrdinal("city_name")),
                                    HouseNumber = reader.IsDBNull(reader.GetOrdinal("housenumber")) ? null : reader.GetString(reader.GetOrdinal("housenumber"))
                                });
                            }
                        }
                    }
                }
            

            return employments;
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

        private Dictionary<string, object> MapEmploymentToParameters(PreviousEmployment employment)
        {
            return new Dictionary<string, object>
        {
            { "@id", employment.Id },
            { "@employeeid", employment.EmployeeId },
            { "@companyname", employment.CompanyName ?? (object)DBNull.Value },
            { "@position", employment.Position ?? (object)DBNull.Value },
            { "@startdate", employment.StartDate },
            { "@enddate", employment.EndDate },
            { "@reasonforleaving", employment.ReasonForLeaving ?? (object)DBNull.Value },
            { "@companyphone", employment.CompanyPhone ?? (object)DBNull.Value },
            { "@street", employment.Street },
            { "@city", employment.City },
            { "@housenumber", employment.HouseNumber ?? (object)DBNull.Value }
        };
        }

    }
}
