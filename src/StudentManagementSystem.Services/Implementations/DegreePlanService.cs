using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using StudentManagementSystem.Domain;
using StudentManagementSystem.Services.Interfaces;

namespace StudentManagementSystem.Services.Implementations
{
    public class DegreePlanService : IDegreePlanService
    {
        // Database path relative to executable location (matches WinForms)
        private readonly string _connectionString = StudentService.GetConnectionString();

        public async Task<DegreePlan> GetDegreePlanByIdAsync(int degreePlanId)
        {
            return await Task.Run(() =>
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT DegreePlanID, StudentID FROM DegreePlan WHERE DegreePlanID = @id";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", degreePlanId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new DegreePlan
                                {
                                    DegreePlanID = reader.GetInt32(0),
                                    StudentID = reader.IsDBNull(1) ? 0 : reader.GetInt32(1)
                                };
                            }
                        }
                    }
                }
                return null;
            });
        }

        public async Task<IEnumerable<DegreePlan>> GetDegreePlansByStudentIdAsync(int studentId)
        {
            return await Task.Run(() =>
            {
                var degreePlans = new List<DegreePlan>();

                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT DegreePlanID, StudentID FROM DegreePlan WHERE StudentID = @sid";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@sid", studentId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                degreePlans.Add(new DegreePlan
                                {
                                    DegreePlanID = reader.GetInt32(0),
                                    StudentID = reader.IsDBNull(1) ? 0 : reader.GetInt32(1)
                                });
                            }
                        }
                    }
                }

                return degreePlans;
            });
        }

        public async Task<DegreePlan> CreateDegreePlanAsync(int studentId)
        {
            return await Task.Run(() =>
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string insertQuery = @"INSERT INTO DegreePlan (StudentID) VALUES (@StudentID);
                                          SELECT last_insert_rowid();";

                    using (var cmd = new SQLiteCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentID", studentId);
                        int newId = Convert.ToInt32(cmd.ExecuteScalar());

                        return new DegreePlan
                        {
                            DegreePlanID = newId,
                            StudentID = studentId
                        };
                    }
                }
            });
        }

        public async Task<DegreePlan> UpdateDegreePlanAsync(DegreePlan degreePlan)
        {
            return await Task.Run(() =>
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string updateQuery = @"UPDATE DegreePlan SET StudentID = @StudentID WHERE DegreePlanID = @DegreePlanID";

                    using (var cmd = new SQLiteCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentID", degreePlan.StudentID);
                        cmd.Parameters.AddWithValue("@DegreePlanID", degreePlan.DegreePlanID);
                        cmd.ExecuteNonQuery();
                    }
                }

                return degreePlan;
            });
        }

        public async Task<bool> DeleteDegreePlanAsync(int degreePlanId)
        {
            return await Task.Run(() =>
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();

                    // Get all semester IDs
                    var semesterIds = new List<int>();
                    string getSemestersQuery = "SELECT SemesterID FROM Semester WHERE DegreePlanID = @dpid";
                    using (var cmd = new SQLiteCommand(getSemestersQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@dpid", degreePlanId);
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
                        cmd.Parameters.AddWithValue("@dpid", degreePlanId);
                        cmd.ExecuteNonQuery();
                    }

                    // Delete from DegreePlan
                    string deleteDegreePlanQuery = "DELETE FROM DegreePlan WHERE DegreePlanID = @dpid";
                    using (var cmd = new SQLiteCommand(deleteDegreePlanQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@dpid", degreePlanId);
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            });
        }

        // Semester Management
        public async Task<IEnumerable<Semester>> GetSemestersByDegreePlanIdAsync(int degreePlanId)
        {
            return await Task.Run(() =>
            {
                var semesters = new List<Semester>();

                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string query = @"SELECT SemesterID, DegreePlanID, Semester, SchoolYear 
                                    FROM Semester 
                                    WHERE DegreePlanID = @dpid 
                                    ORDER BY SchoolYear ASC, Semester ASC";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@dpid", degreePlanId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                semesters.Add(new Semester
                                {
                                    SemesterID = reader.GetInt32(0),
                                    DegreePlanID = reader.GetInt32(1),
                                    SemesterValue = reader.GetBoolean(2),
                                    SchoolYear = reader.GetString(3)
                                });
                            }
                        }
                    }
                }

                return semesters;
            });
        }

        public async Task<Semester> AddSemesterAsync(int degreePlanId, bool isSpringSemester, string schoolYear)
        {
            return await Task.Run(() =>
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    
                    // Check for duplicate semester
                    string existsQuery = @"SELECT 1 FROM Semester 
                                         WHERE DegreePlanID = @dpid AND Semester = @sem AND SchoolYear = @year 
                                         LIMIT 1;";
                    using (var checkCmd = new SQLiteCommand(existsQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@dpid", degreePlanId);
                        checkCmd.Parameters.AddWithValue("@sem", isSpringSemester);
                        checkCmd.Parameters.AddWithValue("@year", schoolYear);
                        var exists = checkCmd.ExecuteScalar();
                        if (exists != null)
                        {
                            throw new InvalidOperationException("That semester already exists for this degree plan.");
                        }
                    }

                    string insertQuery = @"INSERT INTO Semester (DegreePlanID, Semester, SchoolYear) 
                                          VALUES (@DegreePlanID, @Semester, @SchoolYear);
                                          SELECT last_insert_rowid();";

                    using (var cmd = new SQLiteCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@DegreePlanID", degreePlanId);
                        cmd.Parameters.AddWithValue("@Semester", isSpringSemester);
                        cmd.Parameters.AddWithValue("@SchoolYear", schoolYear);

                        int newId = Convert.ToInt32(cmd.ExecuteScalar());

                        return new Semester
                        {
                            SemesterID = newId,
                            DegreePlanID = degreePlanId,
                            SemesterValue = isSpringSemester,
                            SchoolYear = schoolYear
                        };
                    }
                }
            });
        }

        public async Task<bool> RemoveSemesterAsync(int semesterId)
        {
            return await Task.Run(() =>
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();

                    // Delete from SemesterClass first (foreign key)
                    string deleteSemesterClassQuery = "DELETE FROM SemesterClass WHERE SemesterID = @semid";
                    using (var cmd = new SQLiteCommand(deleteSemesterClassQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@semid", semesterId);
                        cmd.ExecuteNonQuery();
                    }

                    // Delete the semester
                    string deleteSemesterQuery = "DELETE FROM Semester WHERE SemesterID = @semid";
                    using (var cmd = new SQLiteCommand(deleteSemesterQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@semid", semesterId);
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            });
        }

        // Class Assignment Management
        public async Task<IEnumerable<SemesterClass>> GetClassesBySemesterAsync(int semesterId)
        {
            return await Task.Run(() =>
            {
                var semesterClasses = new List<SemesterClass>();

                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string query = @"SELECT sc.SemesterClassID, sc.SemesterID, sc.ClassID, sc.Grade, 
                                           c.CourseNumber, c.ClassName, c.Label, c.Semester
                                    FROM SemesterClass sc
                                    JOIN Classes c ON sc.ClassID = c.ClassID
                                    WHERE sc.SemesterID = @semid
                                    ORDER BY c.CourseNumber ASC";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@semid", semesterId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                semesterClasses.Add(new SemesterClass
                                {
                                    SemesterClassID = reader.GetInt32(0),
                                    SemesterID = reader.GetInt32(1),
                                    ClassID = reader.GetInt32(2),
                                    Grade = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                    Course = new Course
                                    {
                                        ClassID = reader.GetInt32(2),
                                        CourseNumber = reader.GetString(4),
                                        CourseName = reader.GetString(5),
                                        Label = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                        Semester = reader.GetBoolean(7)
                                    }
                                });
                            }
                        }
                    }
                }

                return semesterClasses;
            });
        }

        public async Task<IEnumerable<SemesterClass>> GetClassesBySemesterAndYearAsync(int degreePlanId, bool isSpringSemester, string schoolYear)
        {
            return await Task.Run(() =>
            {
                var semesterClasses = new List<SemesterClass>();

                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string query = @"SELECT sc.SemesterClassID, sc.SemesterID, sc.ClassID, sc.Grade, 
                                           c.CourseNumber, c.ClassName, c.Label, c.Semester
                                    FROM SemesterClass sc
                                    JOIN Semester s ON sc.SemesterID = s.SemesterID
                                    JOIN Classes c ON sc.ClassID = c.ClassID
                                    WHERE s.DegreePlanID = @dpid AND s.Semester = @sem AND s.SchoolYear = @year
                                    ORDER BY c.CourseNumber ASC";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@dpid", degreePlanId);
                        cmd.Parameters.AddWithValue("@sem", isSpringSemester);
                        cmd.Parameters.AddWithValue("@year", schoolYear);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Handle Grade column - might be stored as different types
                                string grade = string.Empty;
                                if (!reader.IsDBNull(3))
                                {
                                    try
                                    {
                                        grade = reader.GetString(3);
                                    }
                                    catch
                                    {
                                        grade = reader[3]?.ToString() ?? string.Empty;
                                    }
                                }

                                // Handle CourseNumber and ClassName - might be stored as different types
                                string courseNumber = reader[4]?.ToString() ?? string.Empty;
                                string courseName = reader[5]?.ToString() ?? string.Empty;

                                // Handle Label column - might be stored as different types
                                string label = string.Empty;
                                if (!reader.IsDBNull(6))
                                {
                                    try
                                    {
                                        label = reader.GetString(6);
                                    }
                                    catch
                                    {
                                        label = reader[6]?.ToString() ?? string.Empty;
                                    }
                                }

                                // Handle Semester column - might be stored as boolean, integer, or string
                                bool semesterValue = false;
                                if (!reader.IsDBNull(7))
                                {
                                    try
                                    {
                                        semesterValue = reader.GetBoolean(7);
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            semesterValue = reader.GetInt32(7) != 0;
                                        }
                                        catch
                                        {
                                            string semText = reader[7]?.ToString() ?? "false";
                                            semesterValue = semText.Equals("Spring", StringComparison.OrdinalIgnoreCase) ||
                                                           semText.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                                                           semText == "1";
                                        }
                                    }
                                }

                                semesterClasses.Add(new SemesterClass
                                {
                                    SemesterClassID = reader.GetInt32(0),
                                    SemesterID = reader.GetInt32(1),
                                    ClassID = reader.GetInt32(2),
                                    Grade = grade,
                                    Course = new Course
                                    {
                                        ClassID = reader.GetInt32(2),
                                        CourseNumber = courseNumber,
                                        CourseName = courseName,
                                        Label = label,
                                        Semester = semesterValue
                                    }
                                });
                            }
                        }
                    }
                }

                return semesterClasses;
            });
        }

        public async Task<SemesterClass> AssignClassToSemesterAsync(int semesterId, int classId)
        {
            return await Task.Run(() =>
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();

                    // Check if already assigned
                    string checkQuery = "SELECT COUNT(*) FROM SemesterClass WHERE SemesterID = @semid AND ClassID = @classid";
                    using (var cmd = new SQLiteCommand(checkQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@semid", semesterId);
                        cmd.Parameters.AddWithValue("@classid", classId);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count > 0)
                        {
                            throw new InvalidOperationException("Class is already assigned to this semester");
                        }
                    }

                    // Insert
                    string insertQuery = @"INSERT INTO SemesterClass (SemesterID, ClassID, Grade) 
                                          VALUES (@SemesterID, @ClassID, @Grade);
                                          SELECT last_insert_rowid();";

                    int newId;
                    using (var cmd = new SQLiteCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@SemesterID", semesterId);
                        cmd.Parameters.AddWithValue("@ClassID", classId);
                        cmd.Parameters.AddWithValue("@Grade", string.Empty);
                        newId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Get the course info to return
                    string getCourseQuery = "SELECT CourseNumber, ClassName, Label, Semester FROM Classes WHERE ClassID = @classid";
                    using (var cmd = new SQLiteCommand(getCourseQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@classid", classId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Handle Label column - might be stored as different types
                                string label = string.Empty;
                                if (!reader.IsDBNull(2))
                                {
                                    try
                                    {
                                        label = reader.GetString(2);
                                    }
                                    catch
                                    {
                                        label = reader[2].ToString();
                                    }
                                }

                                // Handle Semester column - might be stored as boolean, integer, or string
                                bool semesterValue = false;
                                try
                                {
                                    semesterValue = reader.GetBoolean(3);
                                }
                                catch
                                {
                                    try
                                    {
                                        semesterValue = reader.GetInt32(3) != 0;
                                    }
                                    catch
                                    {
                                        string semText = reader.GetString(3);
                                        semesterValue = semText.Equals("Spring", StringComparison.OrdinalIgnoreCase) ||
                                                       semText.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                                                       semText == "1";
                                    }
                                }

                                // Handle CourseNumber and ClassName - might be stored as different types
                                string courseNumber = reader[0]?.ToString() ?? string.Empty;
                                string courseName = reader[1]?.ToString() ?? string.Empty;

                                return new SemesterClass
                                {
                                    SemesterClassID = newId,
                                    SemesterID = semesterId,
                                    ClassID = classId,
                                    Grade = string.Empty,
                                    Course = new Course
                                    {
                                        ClassID = classId,
                                        CourseNumber = courseNumber,
                                        CourseName = courseName,
                                        Label = label,
                                        Semester = semesterValue
                                    }
                                };
                            }
                        }
                    }
                }

                return null;
            });
        }

        public async Task<bool> RemoveClassFromSemesterAsync(int semesterClassId)
        {
            return await Task.Run(() =>
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();

                    string deleteQuery = "DELETE FROM SemesterClass WHERE SemesterClassID = @id";

                    using (var cmd = new SQLiteCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", semesterClassId);
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            });
        }

        public async Task<SemesterClass> UpdateGradeAsync(int semesterClassId, string grade)
        {
            return await Task.Run(() =>
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();

                    // Update the grade
                    string updateQuery = "UPDATE SemesterClass SET Grade = @Grade WHERE SemesterClassID = @id";

                    using (var cmd = new SQLiteCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Grade", grade);
                        cmd.Parameters.AddWithValue("@id", semesterClassId);
                        cmd.ExecuteNonQuery();
                    }

                    // Fetch and return the updated record with course info
                    string selectQuery = @"SELECT sc.SemesterClassID, sc.SemesterID, sc.ClassID, sc.Grade,
                                                 c.CourseNumber, c.ClassName, c.Label, c.Semester
                                          FROM SemesterClass sc
                                          JOIN Classes c ON sc.ClassID = c.ClassID
                                          WHERE sc.SemesterClassID = @id";

                    using (var cmd = new SQLiteCommand(selectQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", semesterClassId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new SemesterClass
                                {
                                    SemesterClassID = reader.GetInt32(0),
                                    SemesterID = reader.GetInt32(1),
                                    ClassID = reader.GetInt32(2),
                                    Grade = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                    Course = new Course
                                    {
                                        ClassID = reader.GetInt32(2),
                                        CourseNumber = reader.GetString(4),
                                        CourseName = reader.GetString(5),
                                        Label = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                        Semester = reader.GetBoolean(7)
                                    }
                                };
                            }
                        }
                    }
                }

                return null;
            });
        }
    }
}
