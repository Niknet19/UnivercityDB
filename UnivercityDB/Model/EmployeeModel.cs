using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnivercityDB.Model
{
    internal class EmployeeModel
    {
        private readonly string _connectionString;

        public EmployeeModel(string connectionString)
        {
            _connectionString = connectionString;
        }

        public EmployeeModel()
        {
            _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=ahubozq13q12;Database=University";
        }

        public List<Employee> GetAllEmployees()
        {
            string query = BuildBaseQuery();
            return ExecuteQuery(query);
        }

        public int SearchCurrentEmployees(string searchTerm)
        {
            var fullName = Utils.ParseFullName(searchTerm);
            string query = BuildBaseQuery() + @"
                WHERE e.surname ILIKE @surname
                   AND e.firstname ILIKE @firstname
                   AND e.patronymic ILIKE @patronymic";

            return ExecuteQuery(query, new Dictionary<string, object>
            {
                { "@surname", $"%{fullName.Surname}%" },
                { "@firstname", $"%{fullName.FirstName}%" },
                { "@patronymic", $"%{fullName.Patronymic}%" }
            })[0].Id;
        }

        public List<string> SearchEmployeeJobtitles(Employee employee)
        {
            CatalogsModel catalogsModel = new CatalogsModel();
            string subquery = @$"JOIN jobtitles j ON jobtitleid = j.id
                                WHERE employeeid = {employee.Id}";
            return catalogsModel.GetNamesFromTable("jobtitles_employee", subquery);

        }


        public List<string> SearchEmployeeDepartments(Employee employee)
        {
            CatalogsModel catalogsModel = new CatalogsModel();
            string subquery = @$"JOIN departments d ON departmentid = d.id
                                WHERE employeeid = {employee.Id}";
            return catalogsModel.GetNamesFromTable("departments_employee", subquery);

        }


        public bool DeleteJobtitles(Employee employee, string jobtitleName)
        {
            string query = @"DELETE FROM jobtitles_employee
                            WHERE
                                employeeid = @employeeid AND
                                jobtitleid = (SELECT id FROM jobtitles WHERE name = @jobtitleName)";
            return ExecuteNonQuery(query, new Dictionary<string, object> {
                { "@employeeid", employee.Id },
                { "@jobtitleName", jobtitleName },
            });
        }

        public bool DeleteDepartments(Employee employee, string departmentName)
        {
            string query = @"DELETE FROM departments_employee
                            WHERE
                                employeeid = @employeeid AND
                                departmentid = (SELECT id FROM departments WHERE name = @departmentName)";
            return ExecuteNonQuery(query, new Dictionary<string, object> {
                { "@employeeid", employee.Id },
                { "@departmentName", departmentName },
            });
        }

        public bool AddDepartments(Employee employee, string departmentName)
        {
            string query = @"INSERT INTO departments_employee (employeeid, departmentid)
                            VALUES (
                                @employeeid,
                                (SELECT id FROM departments WHERE name = @departmentName))";
            return ExecuteNonQuery(query, new Dictionary<string, object> {
                { "@employeeid", employee.Id },
                { "@departmentName", departmentName },
            });
        }


        public bool AddJobtitles(Employee employee, string jobtitleName)
        {
            string query = @"INSERT INTO jobtitles_employee (employeeid, jobtitleid)
                            VALUES (
                                @employeeid,
                                (SELECT id FROM jobtitles WHERE name = @jobtitleName))";
            return ExecuteNonQuery(query, new Dictionary<string, object> { 
                { "@employeeid", employee.Id },
                { "@jobtitleName", jobtitleName },
            });
        }


        public List<Employee> SearchEmployees(string searchTerm)
        {
            string query = BuildBaseQuery() + @"
                WHERE e.surname ILIKE @searchTerm
                   OR e.firstname ILIKE @searchTerm
                   OR e.patronymic ILIKE @searchTerm";

            return ExecuteQuery(query, new Dictionary<string, object>
            {
                { "@searchTerm", $"%{searchTerm}%" }
            });
        }

        private string BuildBaseQuery()
        {
            return @"
                SELECT 
                    e.id,
                    e.surname,
                    e.firstname,
                    e.patronymic,
                    e.gender,
                    e.birthdate,
                    e.phone,
                    e.passportseries,
                    e.passportnumber,
                    e.passportissuedate,
                    e.issuedby,
                    ad.name AS academicdegree_name,
                    at.name AS academictitle_name,
                    ei.name AS educationalinstitution_name,
                    s.name AS street_name,
                    c.name AS city_name,
                    e.housenumber,
                    sp.name AS specialty_name,
                    dt.name AS documenttype_name,
                    e.employmentstartdate,
                    e.graduationyear,
                    e.educationdocumentdata
                FROM employee e
                LEFT JOIN academicdegree ad ON e.academicdegreeid = ad.id
                LEFT JOIN academictitle at ON e.academictitleid = at.id
                LEFT JOIN educationalinstitution ei ON e.educationalinstitutionid = ei.id
                LEFT JOIN street s ON e.streetid = s.id
                LEFT JOIN city c ON e.cityid = c.id
                LEFT JOIN speciality sp ON e.specialtyid = sp.id
                LEFT JOIN documenttype dt ON e.documenttypeid = dt.id";
        }

        private List<Employee> ExecuteQuery(string query, Dictionary<string, object>? parameters = null)
        {
            var employees = new List<Employee>();

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
                            employees.Add(new Employee
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Surname = reader.GetString(reader.GetOrdinal("surname")),
                                FirstName = reader.GetString(reader.GetOrdinal("firstname")),
                                Patronymic = reader.GetString(reader.GetOrdinal("patronymic")),
                                Gender = reader.GetChar(reader.GetOrdinal("gender")),
                                BirthDate = reader.GetDateTime(reader.GetOrdinal("birthdate")),
                                Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone")),
                                PassportSeries = reader.GetInt32(reader.GetOrdinal("passportseries")),
                                PassportNumber = reader.GetInt32(reader.GetOrdinal("passportnumber")),
                                PassportIssueDate = reader.GetDateTime(reader.GetOrdinal("passportissuedate")),
                                IssuedBy = reader.IsDBNull(reader.GetOrdinal("issuedby")) ? null : reader.GetString(reader.GetOrdinal("issuedby")),
                                AcademicDegreeName = reader.IsDBNull(reader.GetOrdinal("academicdegree_name")) ? null : reader.GetString(reader.GetOrdinal("academicdegree_name")),
                                AcademicTitleName = reader.IsDBNull(reader.GetOrdinal("academictitle_name")) ? null : reader.GetString(reader.GetOrdinal("academictitle_name")),
                                EducationalInstitutionName = reader.IsDBNull(reader.GetOrdinal("educationalinstitution_name")) ? null : reader.GetString(reader.GetOrdinal("educationalinstitution_name")),
                                StreetName = reader.IsDBNull(reader.GetOrdinal("street_name")) ? null : reader.GetString(reader.GetOrdinal("street_name")),
                                CityName = reader.IsDBNull(reader.GetOrdinal("city_name")) ? null : reader.GetString(reader.GetOrdinal("city_name")),
                                HouseNumber = reader.IsDBNull(reader.GetOrdinal("housenumber")) ? null : reader.GetString(reader.GetOrdinal("housenumber")),
                                SpecialtyName = reader.IsDBNull(reader.GetOrdinal("specialty_name")) ? null : reader.GetString(reader.GetOrdinal("specialty_name")),
                                DocumentTypeName = reader.IsDBNull(reader.GetOrdinal("documenttype_name")) ? null : reader.GetString(reader.GetOrdinal("documenttype_name")),
                                EmploymentStartDate = reader.GetDateTime(reader.GetOrdinal("employmentstartdate")),
                                GraduationYear = reader.GetInt32(reader.GetOrdinal("graduationyear")),
                                EducationDocumentData = reader.IsDBNull(reader.GetOrdinal("educationdocumentdata")) ? null : reader.GetString(reader.GetOrdinal("educationdocumentdata"))
                            }); ;
                        }
                    }
                }
            }

            return employees;
        }


        public bool AddEmployee(Employee employee)
        {
            const string query = @"
            INSERT INTO employee (
                surname, firstname, patronymic, gender, birthdate, phone,
                passportseries, passportnumber, passportissuedate, issuedby,
                academicdegreeid, academictitleid, educationalinstitutionid,
                streetid, cityid, housenumber, specialtyid, documenttypeid,
                employmentstartdate, graduationyear, educationdocumentdata
            ) VALUES (
                @surname, @firstname, @patronymic,@gender, @birthdate, @phone,
                @passportseries, @passportnumber, @passportissuedate, @issuedby,
                (SELECT id FROM academicdegree WHERE name = @academicdegree_name),
                (SELECT id FROM academictitle WHERE name = @academictitle_name),
                (SELECT id FROM educationalinstitution WHERE name = @educationalinstitution_name),
                (SELECT id FROM street WHERE name = @street_name),
                (SELECT id FROM city WHERE name = @city_name),
                @housenumber,
                (SELECT id FROM speciality WHERE name = @specialty_name),
                (SELECT id FROM documenttype WHERE name = @documenttype_name),
                @employmentstartdate, @graduationyear, @educationdocumentdata
            )";

            return ExecuteNonQuery(query, MapEmployeeToParameters(employee));
        }

        public bool UpdateEmployee(Employee employee)
        {
            const string query = @"
            UPDATE employee
            SET
                surname = @surname,
                firstname = @firstname,
                patronymic = @patronymic,
                birthdate = @birthdate,
                phone = @phone,
                passportseries = @passportseries,
                passportnumber = @passportnumber,
                passportissuedate = @passportissuedate,
                issuedby = @issuedby,
                academicdegreeid = (SELECT id FROM academicdegree WHERE name = @academicdegree_name),
                academictitleid = (SELECT id FROM academictitle WHERE name = @academictitle_name),
                educationalinstitutionid = (SELECT id FROM educationalinstitution WHERE name = @educationalinstitution_name),
                streetid = (SELECT id FROM street WHERE name = @street_name),
                cityid = (SELECT id FROM city WHERE name = @city_name),
                housenumber = @housenumber,
                specialtyid = (SELECT id FROM speciality WHERE name = @specialty_name),
                documenttypeid = (SELECT id FROM documenttype WHERE name = @documenttype_name),
                employmentstartdate = @employmentstartdate,
                graduationyear = @graduationyear,
                educationdocumentdata = @educationdocumentdata
            WHERE id = @id";

            return ExecuteNonQuery(query, MapEmployeeToParameters(employee));
        }

        public bool DeleteEmployee(int employeeId)
        {
            const string query = "DELETE FROM employee WHERE id = @id";
            return ExecuteNonQuery(query, new Dictionary<string, object> { { "@id", employeeId } });
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

        private Dictionary<string, object> MapEmployeeToParameters(Employee employee)
        {
            return new Dictionary<string, object>
        {
            { "@id", employee.Id },
            { "@surname", employee.Surname ?? (object)DBNull.Value },
            { "@firstname", employee.FirstName ?? (object)DBNull.Value },
            { "@patronymic", employee.Patronymic ?? (object)DBNull.Value },
                { "@gender", employee.Gender},
            { "@birthdate", employee.BirthDate },
            { "@phone", employee.Phone ?? (object)DBNull.Value },
            { "@passportseries", employee.PassportSeries },
            { "@passportnumber", employee.PassportNumber },
            { "@passportissuedate", employee.PassportIssueDate },
            { "@issuedby", employee.IssuedBy ?? (object)DBNull.Value },
            { "@academicdegree_name", employee.AcademicDegreeName ?? (object)DBNull.Value },
            { "@academictitle_name", employee.AcademicTitleName ?? (object)DBNull.Value },
            { "@educationalinstitution_name", employee.EducationalInstitutionName ?? (object)DBNull.Value },
            { "@street_name", employee.StreetName ?? (object)DBNull.Value },
            { "@city_name", employee.CityName ?? (object)DBNull.Value },
            { "@housenumber", employee.HouseNumber ?? (object)DBNull.Value },
            { "@specialty_name", employee.SpecialtyName ?? (object)DBNull.Value },
            { "@documenttype_name", employee.DocumentTypeName ?? (object)DBNull.Value },
            { "@employmentstartdate", employee.EmploymentStartDate },
            { "@graduationyear", employee.GraduationYear },
            { "@educationdocumentdata", employee.EducationDocumentData ?? (object)DBNull.Value }
        };
        }

    }


}
