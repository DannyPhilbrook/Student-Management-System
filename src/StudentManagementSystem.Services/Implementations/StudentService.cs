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
        // Database path relative to executable location (matches WinForms)
        private readonly string _connectionString = GetConnectionString();

        // Shared helper method for getting connection string (used by all services)
        public static string GetConnectionString()
        {
            // Try executable directory first (for deployed app)
            string exePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "stdmngsys.db");
            if (System.IO.File.Exists(exePath))
            {
                return $"Data Source={exePath};Version=3;";
            }

            // Try project root directory (for development, matches WinForms)
            string projectRoot = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Database", "stdmngsys.db");
            string projectRootFull = System.IO.Path.GetFullPath(projectRoot);
            if (System.IO.File.Exists(projectRootFull))
            {
                return $"Data Source={projectRootFull};Version=3;";
            }

            // Try relative path like WinForms (from executable)
            string relativePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "Database", "stdmngsys.db");
            string relativePathFull = System.IO.Path.GetFullPath(relativePath);
            if (System.IO.File.Exists(relativePathFull))
            {
                return $"Data Source={relativePathFull};Version=3;";
            }

            // Fallback to executable directory (will create if doesn't exist)
            return $"Data Source={exePath};Version=3;";
        }

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

        public async Task<IEnumerable<Student>> SearchStudentsAsync(string name, string studentId, IEnumerable<StudentStatus> statuses, string semester)
        {
            return await Task.Run(() =>
            {
                var students = new List<Student>();

                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();

                    // Build base query - match WinForms: separate FirstName and LastName filters
                    var query = "SELECT StudentID, FirstName, LastName, StartingSemester, Notes, StudentStatus, SchoolYear, DegreePlanID FROM Student WHERE 1=1";
                    var parameters = new List<SQLiteParameter>();

                    // Split name into first and last name for separate filtering (matching WinForms)
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        // WinForms uses LOWER() for case-insensitive search
                        query += " AND (LOWER(FirstName) LIKE @firstName OR LOWER(LastName) LIKE @lastName)";
                        parameters.Add(new SQLiteParameter("@firstName", $"%{name.Trim().ToLower()}%"));
                        parameters.Add(new SQLiteParameter("@lastName", $"%{name.Trim().ToLower()}%"));
                    }

                    if (!string.IsNullOrWhiteSpace(studentId))
                    {
                        query += " AND StudentID LIKE @studentId";
                        parameters.Add(new SQLiteParameter("@studentId", $"%{studentId.Trim()}%"));
                    }

                    // Handle multiple status selections (matching WinForms checkbox logic)
                    if (statuses != null && statuses.Any())
                    {
                        var statusList = statuses.ToList();
                        var statusConditions = string.Join(" OR ", statusList.Select((s, i) => $"StudentStatus = @status{i}"));
                        query += " AND (" + statusConditions + ")";

                        for (int i = 0; i < statusList.Count; i++)
                        {
                            parameters.Add(new SQLiteParameter($"@status{i}", (int)statusList[i]));
                        }
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

        public async Task<Student> AddStudentAsync(Student student, string schoolYear)
        {
            return await Task.Run(() =>
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();

                    // NEW: Check for duplicate StudentID before mutating DB.
                    // This prevents UNIQUE constraint exceptions and avoids creating orphan DegreePlan/Semester records.
                    using (var checkCmd = new SQLiteCommand("SELECT COUNT(1) FROM Student WHERE StudentID = @sid;", conn))
                    {
                        checkCmd.Parameters.AddWithValue("@sid", student.StudentID);
                        var existingCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (existingCount > 0)
                        {
                            // Service layer raises a clear exception; UI should catch and show a warning popup.
                            // throw new InvalidOperationException($"A student with ID {student.StudentID} already exists.");
                            return null; // Indicate duplicate found without exception
                        }
                    }

                    // Determine boolean value based on starting semester
                    // false = Fall (0), true = Spring (1)
                    bool semesterValue = student.StartingSemester;
                    int startSemesterIndex = semesterValue ? 1 : 0;

                    // STEP 1: Create a new DegreePlan (matching WinForms logic)
                    // Note: WinForms includes SchoolYear and StartSemester in DegreePlan, but we'll use minimal schema
                    // If those columns don't exist, this will still work
                    int degreePlanId;
                    try
                    {
                        // Try with SchoolYear and StartSemester (WinForms schema)
                        string createDegreePlanQuery = "INSERT INTO DegreePlan (StudentID, SchoolYear, StartSemester) VALUES (@sid, @year, @sem);";
                        using (var cmd = new SQLiteCommand(createDegreePlanQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@sid", 0); // Temporary placeholder
                            cmd.Parameters.AddWithValue("@year", schoolYear);
                            cmd.Parameters.AddWithValue("@sem", semesterValue);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch
                    {
                        // Fallback to minimal schema if columns don't exist
                        string createDegreePlanQuery = "INSERT INTO DegreePlan (StudentID) VALUES (NULL);";
                        using (var cmd = new SQLiteCommand(createDegreePlanQuery, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Get DegreePlanID
                    using (var cmd = new SQLiteCommand("SELECT last_insert_rowid();", conn))
                    {
                        degreePlanId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // STEP 2: Generate 4 semesters for this degree plan
                    GenerateSemesters(conn, degreePlanId, schoolYear, startSemesterIndex);

                    // STEP 3: Generate default classes for each semester
                    GenerateDefaultClasses(conn, degreePlanId, startSemesterIndex);

                    // STEP 4: Create the Student record (StudentID is user-entered, not auto-increment)
                    string insertQuery = @"INSERT INTO Student (FirstName, LastName, StudentID, StartingSemester, Notes, SchoolYear, DegreePlanID, StudentStatus)
                                          VALUES (@FirstName, @LastName, @StudentID, @StartingSemester, @Notes, @SchoolYear, @DegreePlanID, @StudentStatus);";

                    using (var cmd = new SQLiteCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", student.LastName);
                        cmd.Parameters.AddWithValue("@StudentID", student.StudentID);
                        cmd.Parameters.AddWithValue("@StartingSemester", semesterValue);
                        cmd.Parameters.AddWithValue("@Notes", student.Notes ?? string.Empty);
                        cmd.Parameters.AddWithValue("@SchoolYear", schoolYear);
                        cmd.Parameters.AddWithValue("@DegreePlanID", degreePlanId);
                        cmd.Parameters.AddWithValue("@StudentStatus", (int)student.StudentStatus);

                        cmd.ExecuteNonQuery();
                    }

                    // Get the primary key (last_insert_rowid returns the rowid, which is StudentID if it's the primary key)
                    using (var cmd = new SQLiteCommand("SELECT last_insert_rowid();", conn))
                    {
                        int studentPrimaryId = Convert.ToInt32(cmd.ExecuteScalar());
                        
                        // STEP 5: Update DegreePlan with actual StudentID (primary key)
                        using (var updateCmd = new SQLiteCommand("UPDATE DegreePlan SET StudentID = @sid WHERE DegreePlanID = @dpid;", conn))
                        {
                            updateCmd.Parameters.AddWithValue("@sid", studentPrimaryId);
                            updateCmd.Parameters.AddWithValue("@dpid", degreePlanId);
                            updateCmd.ExecuteNonQuery();
                        }

                        // Note: student.StudentID already contains the user-entered value
                        // If the database uses StudentID as primary key, last_insert_rowid() returns it
                        // Otherwise, we might need to query it back
                        student.DegreePlanID = degreePlanId;
                    }
                }

                return student;
            });
        }

        // Generate 4 semesters for a degree plan (matching WinForms GenerateSemesters)
        private void GenerateSemesters(SQLiteConnection conn, int degreePlanId, string startYear, int startSemester)
        {
            // Convert start year string to an integer (e.g., "2025" → 2025)
            int currentYear = int.Parse(startYear);
            int currentSemester = startSemester; // 0 = Fall, 1 = Spring

            for (int i = 0; i < 4; i++)
            {
                string insertSemester = "INSERT INTO Semester (DegreePlanID, Semester, SchoolYear) VALUES (@dpid, @sem, @year)";
                using (var cmd = new SQLiteCommand(insertSemester, conn))
                {
                    cmd.Parameters.AddWithValue("@dpid", degreePlanId);
                    bool semester = currentSemester == 1;
                    cmd.Parameters.AddWithValue("@sem", semester);
                    cmd.Parameters.AddWithValue("@year", $"{currentYear}");
                    cmd.ExecuteNonQuery();
                }

                // Toggle semester: 0 ↔ 1
                currentSemester = (currentSemester == 0) ? 1 : 0;

                // Increment year *after* Fall (0) → Spring (1) transition
                if (currentSemester == 1)
                    currentYear++;
            }
        }

        // Generate default classes for each semester (matching WinForms GenerateDefaultClasses)
        private void GenerateDefaultClasses(SQLiteConnection conn, int degreePlanId, int startSemester)
        {
            // startSemester: 0 = Fall, 1 = Spring

            // Adjust order depending on starting semester
            List<(bool semester, int yearOffset, List<(string Label, int CourseNumber, string ClassName)> classes)> semesterCurriculum;

            if (startSemester == 1) // if Spring start
            {
                semesterCurriculum = new List<(bool semester, int yearOffset, List<(string Label, int CourseNumber, string ClassName)> classes)>
                {
                    (true, 0, new List<(string,int,string)> { // SPRING
                        ("CSCI", 5348, "Digital Forensics"),
                        ("CSCI", 5360, "Computer Networking")
                    }),
                    (false, 0, new List<(string,int,string)> { // FALL
                        ("CSCI", 5312, "Web Security"),
                        ("CSCI", 5322, "Cryptography"),
                        ("CSCI", 5313, "Software Dev Principles")
                    }),
                    (true, 1, new List<(string,int,string)> { // SPRING
                        ("CSCI", 5345, "Malware Analysis"),
                        ("CSCI", 5347, "Cyber Security Concepts"),
                        ("CSCI", 5363, "Computer Net and Dist Systems")
                    }),
                    (false, 1, new List<(string,int,string)> { // FALL
                        ("CSCI", 5362, "Penetration Testing"),
                        ("CSCI", 5320, "Database Management Systems")
                    }),
                };
            }
            else // if Fall start
            {
                semesterCurriculum = new List<(bool semester, int yearOffset, List<(string Label, int CourseNumber, string ClassName)> classes)>
                {
                    (false, 0, new List<(string,int,string)> { // FALL
                        ("CSCI", 5312, "Web Security"),
                        ("CSCI", 5322, "Cryptography"),
                        ("CSCI", 5313, "Software Dev Principles")
                    }),
                    (true, 1, new List<(string,int,string)> { // SPRING
                        ("CSCI", 5348, "Digital Forensics"),
                        ("CSCI", 5360, "Computer Networking")
                    }),
                    (false, 1, new List<(string,int,string)> { // FALL
                        ("CSCI", 5362, "Penetration Testing"),
                        ("CSCI", 5320, "Database Management Systems")
                    }),
                    (true, 2, new List<(string,int,string)> { // SPRING
                        ("CSCI", 5345, "Malware Analysis"),
                        ("CSCI", 5347, "Cyber Security Concepts"),
                        ("CSCI", 5363, "Computer Net and Dist Systems")
                    }),
                };
            }

            // Get all semesters for this degree plan
            var semesterQuery = "SELECT SemesterID, Semester, SchoolYear FROM Semester WHERE DegreePlanID = @dpid ORDER BY SchoolYear, Semester;";
            var semesterMap = new List<(int SemesterID, bool Semester, string Year)>();
            using (var cmd = new SQLiteCommand(semesterQuery, conn))
            {
                cmd.Parameters.AddWithValue("@dpid", degreePlanId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        semesterMap.Add((reader.GetInt32(0), reader.GetBoolean(1), reader.GetString(2)));
                    }
                }
            }

            // Insert classes per semester
            foreach (var sem in semesterMap)
            {
                int baseYear = int.Parse(semesterMap.First().Year);
                int offset = int.Parse(sem.Year) - baseYear;

                var match = semesterCurriculum.FirstOrDefault(d => d.semester == sem.Semester && d.yearOffset == offset);
                if (match.classes != null)
                {
                    foreach (var c in match.classes)
                    {
                        // Find ClassID from Classes table
                        int classId;
                        using (var cmd = new SQLiteCommand("SELECT ClassID FROM Classes WHERE Label=@lbl AND CourseNumber=@num;", conn))
                        {
                            cmd.Parameters.AddWithValue("@lbl", c.Label);
                            cmd.Parameters.AddWithValue("@num", c.CourseNumber.ToString());
                            var result = cmd.ExecuteScalar();
                            if (result == null)
                            {
                                // If not found, insert new class
                                using (var insert = new SQLiteCommand("INSERT INTO Classes (CourseNumber, ClassName, Label, Semester) VALUES (@num, @name, @lbl, @sem);", conn))
                                {
                                    insert.Parameters.AddWithValue("@num", c.CourseNumber.ToString());
                                    insert.Parameters.AddWithValue("@name", c.ClassName);
                                    insert.Parameters.AddWithValue("@lbl", c.Label);
                                    insert.Parameters.AddWithValue("@sem", sem.Semester);
                                    insert.ExecuteNonQuery();
                                }
                                using (var getIdCmd = new SQLiteCommand("SELECT last_insert_rowid();", conn))
                                {
                                    classId = Convert.ToInt32(getIdCmd.ExecuteScalar());
                                }
                            }
                            else
                            {
                                classId = Convert.ToInt32(result);
                            }
                        }

                        // Link class to semester
                        using (var cmd = new SQLiteCommand("INSERT INTO SemesterClass (SemesterID, ClassID) VALUES (@sid, @cid);", conn))
                        {
                            cmd.Parameters.AddWithValue("@sid", sem.SemesterID);
                            cmd.Parameters.AddWithValue("@cid", classId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            return await Task.Run(() =>
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    // Match WinForms logic: Update all fields including StudentID if it's changeable
                    // Note: In WinForms, StudentID appears to be updatable, but we'll use it as the WHERE clause identifier
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
                        cmd.Parameters.AddWithValue("@SchoolYear", student.SchoolYear ?? "2024");
                        cmd.Parameters.AddWithValue("@DegreePlanID", student.DegreePlanID ?? 1); // Default DegreePlanID like WinForms
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