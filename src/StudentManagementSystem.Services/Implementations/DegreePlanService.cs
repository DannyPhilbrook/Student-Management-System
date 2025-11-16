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
        private readonly string _connectionString = $"Data Source={System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "stdmngsys.db")};Version=3;";

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
                    string query = @"SELECT sc.SemesterID, sc.ClassID, sc.Grade, 
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
                                    SemesterID = reader.GetInt32(0),
                                    ClassID = reader.GetInt32(1),
                                    Grade = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    Course = new Course
                                    {
                                        ClassID = reader.GetInt32(1),
                                        CourseNumber = reader.GetString(3),
                                        CourseName = reader.GetString(4),
                                        Label = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                        Semester = reader.GetBoolean(6)
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
                                return new SemesterClass
                                {
                                    SemesterClassID = newId,
                                    SemesterID = semesterId,
                                    ClassID = classId,
                                    Grade = string.Empty,
                                    Course = new Course
                                    {
                                        ClassID = classId,
                                        CourseNumber = reader.GetString(0),
                                        CourseName = reader.GetString(1),
                                        Label = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                        Semester = reader.GetBoolean(3)
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
