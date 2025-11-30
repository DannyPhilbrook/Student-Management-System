using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using StudentManagementSystem.Domain;
using StudentManagementSystem.Services.Interfaces;

namespace StudentManagementSystem.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly string _connectionString = "Data Source=Database/stdmngsys.db;Version=3;";

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await Task.Run(() =>
            {
                var students = new List<Student>();

                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT StudentID, FirstName, LastName, StartingSemester, Notes, StudentStatus, SchoolYear, DegreePlanID FROM Student";

                    using (var cmd = new SQLiteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(MapStudent(reader));
                        }
                    }
                }

                return students;
            });
        }

        public async Task<Student> GetStudentByIdAsync(int studentID)
        {
            return await Task.Run(() =>
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT StudentID, FirstName, LastName, StartingSemester, Notes, StudentStatus, SchoolYear, DegreePlanID FROM Student WHERE StudentID = @id";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", studentID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                                return MapStudent(reader);
                        }
                    }
                }
                return null;
            });
        }

        public async Task<IEnumerable<Student>> SearchStudentsAsync(string name, string studentId, StudentStatus? status, string semester)
        {
            return await Task.Run(() =>
            {
                var students = new List<Student>();

                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();

                    var query = "SELECT StudentID, FirstName, LastName, StartingSemester, Notes, StudentStatus, SchoolYear, DegreePlanID FROM Student WHERE 1=1";
                    var parameters = new List<SQLiteParameter>();

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        query += " AND (FirstName LIKE @name OR LastName LIKE @name)";
                        parameters.Add(new SQLiteParameter("@name", $"%{name}%"));
                    }

                    if (!string.IsNullOrWhiteSpace(studentId))
                    {
                        query += " AND CAST(StudentID AS TEXT) LIKE @studentId";
                        parameters.Add(new SQLiteParameter("@studentId", $"%{studentId}%"));
                    }

                    if (status.HasValue)
                    {
                        query += " AND StudentStatus = @status";
                        parameters.Add(new SQLiteParameter("@status", (int)status.Value));
                    }

                    if (!string.IsNullOrWhiteSpace(semester))
                    {
                        bool isSpringSemester = semester.Equals("Spring", StringComparison.OrdinalIgnoreCase);
                        query += " AND StartingSemester = @semester";
                        parameters.Add(new SQLiteParameter("@semester", isSpringSemester));
                    }

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                students.Add(MapStudent(reader));
                            }
                        }
                    }
                }

                return students;
            });
        }

        public async Task<Student> AddStudentAsync(Student student)
        {
            return await Task.Run(() =>
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();

                    // Create a degree plan for the student first
                    string createDegreePlanQuery = "INSERT INTO DegreePlan (StudentID) VALUES (NULL); SELECT last_insert_rowid();";
                    int degreePlanID;
                    using (var cmd = new SQLiteCommand(createDegreePlanQuery, conn))
                    {
                        degreePlanID = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Insert the student
                    string insertQuery = @"INSERT INTO Student (FirstName, LastName, StartingSemester, Notes, StudentStatus, SchoolYear, DegreePlanID)
                                          VALUES (@FirstName, @LastName, @StartingSemester, @Notes, @StudentStatus, @SchoolYear, @DegreePlanID);
                                          SELECT last_insert_rowid();";

                    using (var cmd = new SQLiteCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", student.LastName);
                        cmd.Parameters.AddWithValue("@StartingSemester", student.StartingSemester);
                        cmd.Parameters.AddWithValue("@Notes", student.Notes ?? string.Empty);
                        cmd.Parameters.AddWithValue("@StudentStatus", (int)student.StudentStatus);
                        cmd.Parameters.AddWithValue("@SchoolYear", student.SchoolYear);
                        cmd.Parameters.AddWithValue("@DegreePlanID", degreePlanID);

                        student.StudentID = Convert.ToInt32(cmd.ExecuteScalar());
                        student.DegreePlanID = degreePlanID;
                    }

                    // Update the DegreePlan with the StudentID
                    string updateDegreePlanQuery = "UPDATE DegreePlan SET StudentID = @StudentID WHERE DegreePlanID = @DegreePlanID";
                    using (var cmd = new SQLiteCommand(updateDegreePlanQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentID", student.StudentID);
                        cmd.Parameters.AddWithValue("@DegreePlanID", degreePlanID);
                        cmd.ExecuteNonQuery();
                    }
                }

                return student;
            });
        }

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            return await Task.Run(() =>
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string updateQuery = @"UPDATE Student 
                                          SET FirstName = @FirstName, 
                                              LastName = @LastName, 
                                              StartingSemester = @StartingSemester, 
                                              Notes = @Notes, 
                                              StudentStatus = @StudentStatus, 
                                              SchoolYear = @SchoolYear, 
                                              DegreePlanID = @DegreePlanID 
                                          WHERE StudentID = @StudentID";

                    using (var cmd = new SQLiteCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", student.LastName);
                        cmd.Parameters.AddWithValue("@StartingSemester", student.StartingSemester);
                        cmd.Parameters.AddWithValue("@Notes", student.Notes ?? string.Empty);
                        cmd.Parameters.AddWithValue("@StudentStatus", (int)student.StudentStatus);
                        cmd.Parameters.AddWithValue("@SchoolYear", student.SchoolYear);
                        cmd.Parameters.AddWithValue("@DegreePlanID", student.DegreePlanID ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@StudentID", student.StudentID);

                        cmd.ExecuteNonQuery();
                    }
                }

                return student;
            });
        }

        public async Task<bool> DeleteStudentAsync(int studentID)
        {
            return await Task.Run(() =>
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();

                    // Get all degree plan IDs for this student
                    var degreePlanIds = new List<int>();
                    string getDegreePlansQuery = "SELECT DegreePlanID FROM DegreePlan WHERE StudentID = @sid";
                    using (var cmd = new SQLiteCommand(getDegreePlansQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@sid", studentID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                degreePlanIds.Add(reader.GetInt32(0));
                        }
                    }

                    // For each degree plan, cascade delete
                    foreach (int dpId in degreePlanIds)
                    {
                        // Get all semester IDs
                        var semesterIds = new List<int>();
                        string getSemestersQuery = "SELECT SemesterID FROM Semester WHERE DegreePlanID = @dpid";
                        using (var cmd = new SQLiteCommand(getSemestersQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@dpid", dpId);
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                    semesterIds.Add(reader.GetInt32(0));
                            }
                        }

                        // Delete from SemesterClass for each semester
                        foreach (int semId in semesterIds)
                        {
                            string deleteSemesterClassQuery = "DELETE FROM SemesterClass WHERE SemesterID = @semid";
                            using (var cmd = new SQLiteCommand(deleteSemesterClassQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("@semid", semId);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        // Delete from Semester
                        string deleteSemesterQuery = "DELETE FROM Semester WHERE DegreePlanID = @dpid";
                        using (var cmd = new SQLiteCommand(deleteSemesterQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@dpid", dpId);
                            cmd.ExecuteNonQuery();
                        }

                        // Delete from DegreePlan
                        string deleteDegreePlanQuery = "DELETE FROM DegreePlan WHERE DegreePlanID = @dpid";
                        using (var cmd = new SQLiteCommand(deleteDegreePlanQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@dpid", dpId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Finally, delete the student
                    string deleteStudentQuery = "DELETE FROM Student WHERE StudentID = @sid";
                    using (var cmd = new SQLiteCommand(deleteStudentQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@sid", studentID);
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            });
        }

        // Helper method to map SQLiteDataReader to Student object
        private Student MapStudent(SQLiteDataReader reader)
        {
            return new Student
            {
                StudentID = reader.GetInt32(reader.GetOrdinal("StudentID")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                StartingSemester = reader.GetBoolean(reader.GetOrdinal("StartingSemester")),
                Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? string.Empty : reader.GetString(reader.GetOrdinal("Notes")),
                StudentStatus = (StudentStatus)reader.GetInt32(reader.GetOrdinal("StudentStatus")),
                SchoolYear = reader.IsDBNull(reader.GetOrdinal("SchoolYear")) ? string.Empty : reader.GetString(reader.GetOrdinal("SchoolYear")),
                DegreePlanID = reader.IsDBNull(reader.GetOrdinal("DegreePlanID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("DegreePlanID"))
            };
        }

        // Interface methods (keeping old signatures for backward compatibility)
        Task<Student> IStudentService.GetStudentByStudentIdAsync(string studentId)
        {
            return Task.FromResult<Student>(null); // Not used in new implementation
        }
    }
}