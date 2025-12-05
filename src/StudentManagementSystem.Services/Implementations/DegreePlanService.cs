using StudentManagementSystem.Domain;
using StudentManagementSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace StudentManagementSystem.Services.Implementations
{
    public class DegreePlanService : IDegreePlanService
    {
        // Database path relative to executable location (matches WinForms)
        private readonly string _connectionString = StudentService.GetConnectionString();

        // ---- Helper readers: make SQLite type differences safe ----
        private static int ReadInt(SQLiteDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return 0;
            var v = reader.GetValue(index);
            // Handle long (Int64) or other numeric types
            return Convert.ToInt32(v);
        }

        private static string ReadString(SQLiteDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return string.Empty;
            var v = reader.GetValue(index);
            return v?.ToString() ?? string.Empty;
        }

        private static bool ReadBool(SQLiteDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return false;
            var v = reader.GetValue(index);
            if (v is bool b) return b;
            if (v is long l) return l != 0;
            if (v is int i) return i != 0;
            var s = v?.ToString() ?? string.Empty;
            return s.Equals("true", StringComparison.OrdinalIgnoreCase)
                || s.Equals("spring", StringComparison.OrdinalIgnoreCase)
                || s == "1";
        }
        // ------------------------------------------------------------

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
                                    DegreePlanID = ReadInt(reader, 0),
                                    StudentID = reader.IsDBNull(1) ? 0 : ReadInt(reader, 1)
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
                                    DegreePlanID = ReadInt(reader, 0),
                                    StudentID = reader.IsDBNull(1) ? 0 : ReadInt(reader, 1)
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
                                semesterIds.Add(ReadInt(reader, 0));
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
                                    SemesterID = ReadInt(reader, 0),
                                    DegreePlanID = ReadInt(reader, 1),
                                    SemesterValue = ReadBool(reader, 2),
                                    SchoolYear = ReadString(reader, 3)
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
                            // Return null to indicate the semester already exists.
                            return null;
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
                                // Read values defensively
                                int semClassId = ReadInt(reader, 0);
                                int semId = ReadInt(reader, 1);
                                int classId = ReadInt(reader, 2);
                                string grade = ReadString(reader, 3);
                                string courseNumber = ReadString(reader, 4);
                                string courseName = ReadString(reader, 5);
                                string label = ReadString(reader, 6);
                                bool semesterValue = ReadBool(reader, 7);

                                semesterClasses.Add(new SemesterClass
                                {
                                    SemesterClassID = semClassId,
                                    SemesterID = semId,
                                    ClassID = classId,
                                    Grade = grade,
                                    Course = new Course
                                    {
                                        ClassID = classId,
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
                                // Defensive reads
                                int semClassId = ReadInt(reader, 0);
                                int semId = ReadInt(reader, 1);
                                int classId = ReadInt(reader, 2);
                                string grade = ReadString(reader, 3);
                                string courseNumber = ReadString(reader, 4);
                                string courseName = ReadString(reader, 5);
                                string label = ReadString(reader, 6);
                                bool semesterValue = ReadBool(reader, 7);

                                semesterClasses.Add(new SemesterClass
                                {
                                    SemesterClassID = semClassId,
                                    SemesterID = semId,
                                    ClassID = classId,
                                    Grade = grade,
                                    Course = new Course
                                    {
                                        ClassID = classId,
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
                                string courseNumber = ReadString(reader, 0);
                                string courseName = ReadString(reader, 1);
                                string label = ReadString(reader, 2);
                                bool semesterValue = ReadBool(reader, 3);

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
                                int semClassId = ReadInt(reader, 0);
                                int semId = ReadInt(reader, 1);
                                int classId = ReadInt(reader, 2);
                                string gradeVal = ReadString(reader, 3);
                                string courseNumber = ReadString(reader, 4);
                                string courseName = ReadString(reader, 5);
                                string label = ReadString(reader, 6);
                                bool semesterValue = ReadBool(reader, 7);

                                return new SemesterClass
                                {
                                    SemesterClassID = semClassId,
                                    SemesterID = semId,
                                    ClassID = classId,
                                    Grade = gradeVal,
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
    }
}