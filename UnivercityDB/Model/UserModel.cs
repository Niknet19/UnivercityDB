using Npgsql;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace UnivercityDB.Model
{


    public class UserModel
    {
        private string _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=ahubozq13q12;Database=University";

     

        public UserPermissions GetMenuPermissions(string username)
        {
            var userPermissions = new UserPermissions();

            var query = @"
            SELECT f.Name, p.CanRead, p.CanWrite, p.CanEdit, p.CanDelete
            FROM Users u
            JOIN Permissions p ON u.RoleId = p.RoleId
            JOIN Forms f ON p.FormId = f.Id
            WHERE u.Username = @Username";

            using (var connection = new NpgsqlConnection(_connectionString))
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var formName = reader.GetString(reader.GetOrdinal("Name"));
                        var canRead = reader.GetBoolean(reader.GetOrdinal("CanRead"));
                        var canWrite = reader.GetBoolean(reader.GetOrdinal("CanWrite"));
                        var canEdit = reader.GetBoolean(reader.GetOrdinal("CanEdit"));
                        var canDelete = reader.GetBoolean(reader.GetOrdinal("CanDelete"));

                        switch (formName)
                        {
                            case "Orders":
                                userPermissions.OrdersPermission.CanRead = canRead;
                                userPermissions.OrdersPermission.CanWrite = canWrite;
                                userPermissions.OrdersPermission.CanEdit = canEdit;
                                userPermissions.OrdersPermission.CanDelete = canDelete;
                                break;
                            case "Documents":
                                userPermissions.DocumentsPermission.CanRead = canRead;
                                userPermissions.DocumentsPermission.CanWrite = canWrite;
                                userPermissions.DocumentsPermission.CanEdit = canEdit;
                                userPermissions.DocumentsPermission.CanDelete = canDelete;
                                break;
                            case "City":
                                userPermissions.CitiesPermission.CanRead = canRead;
                                userPermissions.CitiesPermission.CanWrite = canWrite;
                                userPermissions.CitiesPermission.CanEdit = canEdit;
                                userPermissions.CitiesPermission.CanDelete = canDelete;
                                break;
                            case "Street":
                                userPermissions.StreetsPermission.CanRead = canRead;
                                userPermissions.StreetsPermission.CanWrite = canWrite;
                                userPermissions.StreetsPermission.CanEdit = canEdit;
                                userPermissions.StreetsPermission.CanDelete = canDelete;
                                break;
                            case "Departments":
                                userPermissions.DepartmentsPermission.CanRead = canRead;
                                userPermissions.DepartmentsPermission.CanWrite = canWrite;
                                userPermissions.DepartmentsPermission.CanEdit = canEdit;
                                userPermissions.DepartmentsPermission.CanDelete = canDelete;
                                break;
                            case "Education":
                                userPermissions.EducationPermission.CanRead = canRead;
                                userPermissions.EducationPermission.CanWrite = canWrite;
                                userPermissions.EducationPermission.CanEdit = canEdit;
                                userPermissions.EducationPermission.CanDelete = canDelete;
                                break;
                            case "AcademicDegree":
                                userPermissions.AcademicDegreesPermission.CanRead = canRead;
                                userPermissions.AcademicDegreesPermission.CanWrite = canWrite;
                                userPermissions.AcademicDegreesPermission.CanEdit = canEdit;
                                userPermissions.AcademicDegreesPermission.CanDelete = canDelete;
                                break;
                            case "AcademicTitle":
                                userPermissions.AcademicTitlesPermission.CanRead = canRead;
                                userPermissions.AcademicTitlesPermission.CanWrite = canWrite;
                                userPermissions.AcademicTitlesPermission.CanEdit = canEdit;
                                userPermissions.AcademicTitlesPermission.CanDelete = canDelete;
                                break;
                            case "Jobtitles":
                                userPermissions.JobtitlesPermission.CanRead = canRead;
                                userPermissions.JobtitlesPermission.CanWrite = canWrite;
                                userPermissions.JobtitlesPermission.CanEdit = canEdit;
                                userPermissions.JobtitlesPermission.CanDelete = canDelete;
                                break;
                            case "Speciality":
                                userPermissions.SpecialityPermission.CanRead = canRead;
                                userPermissions.SpecialityPermission.CanWrite = canWrite;
                                userPermissions.SpecialityPermission.CanEdit = canEdit;
                                userPermissions.SpecialityPermission.CanDelete = canDelete;
                                break;
                            case "Employee":
                                userPermissions.EmployeePermission.CanRead = canRead;
                                userPermissions.EmployeePermission.CanWrite = canWrite;
                                userPermissions.EmployeePermission.CanEdit = canEdit;
                                userPermissions.EmployeePermission.CanDelete = canDelete;
                                break;
                        }

                    }
                }

            }

            return userPermissions;
        }

        public Permission GetPermissions(int userName, string formName)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
            SELECT p.CanRead, p.CanWrite, p.CanEdit, p.CanDelete
            FROM Users u
            JOIN Permissions p ON u.RoleId = p.RoleId
            JOIN Forms f ON p.FormId = f.Id
            WHERE u.Username = @Username AND f.Name = @FormName;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userName);
                    command.Parameters.AddWithValue("@FormName", formName);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Permission
                            {
                                CanRead = reader.GetBoolean(0),
                                CanWrite = reader.GetBoolean(1),
                                CanEdit = reader.GetBoolean(2),
                                CanDelete = reader.GetBoolean(3)
                            };
                        }
                    }
                }
            }

            return new Permission(); 
        }

        public bool AuthenticateUser(string username, string password)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT password FROM users WHERE username = @username";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("username", username);
                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            string storedHashedPassword = result.ToString();
                            return VerifyPassword(password, storedHashedPassword);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false;
        }

 
        public bool UpdateUser(string username, string newPassword)
        {
            string hashedPassword = HashPassword(newPassword);
            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
       
            string insertQuery = "UPDATE users SET password = @password WHERE username = @username";
            using (var cmd = new NpgsqlCommand(insertQuery, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", hashedPassword);
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Новый пароль установлен.", "Ок", MessageBoxButton.OK, MessageBoxImage.Information);
                    return true;
                }
                else return false;
            }
        }

       
        public bool AddUser(string username, string password)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();

              
                    string checkQuery = "SELECT COUNT(*) FROM users WHERE username = @username";
                    using (var checkCmd = new NpgsqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("username", username);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            Console.WriteLine("User with this username already exists.");
                            return false; 
                        }
                    }

                    string hashedPassword = HashPassword(password);

                  
                    string insertQuery = "INSERT INTO users (username, password) VALUES (@username, @password)";
                    using (var cmd = new NpgsqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("username", username);
                        cmd.Parameters.AddWithValue("password", hashedPassword);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("User added successfully.");
                            return true; 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false;
        }

       
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

      
        private bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            string enteredPasswordHash = HashPassword(enteredPassword);
            return enteredPasswordHash == storedHashedPassword;
        }
    }
}
