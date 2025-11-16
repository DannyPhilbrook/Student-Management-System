using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using StudentManagementSystem.Domain;
using StudentManagementSystem.Services.Interfaces;

namespace StudentManagementSystem.Services.Implementations
{
    public class CourseService : ICourseService
    {
        // Database path relative to executable location (matches WinForms)
        private readonly string _connectionString = $"Data Source={System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "stdmngsys.db")};Version=3;";

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await Task.Run(() =>
            {
                var courses = new List<Course>();

                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT ClassID, CourseNumber, ClassName, Label, Semester FROM Classes ORDER BY CourseNumber ASC";

                    using (var cmd = new SQLiteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            courses.Add(MapCourse(reader));
                        }
                    }
                }

                return courses;
            });
        }

        public async Task<Course> GetCourseByIdAsync(int classID)
        {
            return await Task.Run(() =>
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT ClassID, CourseNumber, ClassName, Label, Semester FROM Classes WHERE ClassID = @id";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", classID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                                return MapCourse(reader);
                        }
                    }
                }
                return null;
            });
        }

        public async Task<Course> GetCourseByNumberAsync(string courseNumber)
        {
            return await Task.Run(() =>
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT ClassID, CourseNumber, ClassName, Label, Semester FROM Classes WHERE CourseNumber = @num";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@num", courseNumber);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                                return MapCourse(reader);
                        }
                    }
                }
                return null;
            });
        }

        public async Task<IEnumerable<string>> GetDistinctLabelsAsync()
        {
            return await Task.Run(() =>
            {
                var labels = new List<string>();

                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT DISTINCT Label FROM Classes ORDER BY Label ASC";

                    using (var cmd = new SQLiteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                                labels.Add(reader.GetString(0));
                        }
                    }
                }

                return labels;
            });
        }

        public async Task<Course> AddCourseAsync(Course course)
        {
            return await Task.Run(() =>
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string insertQuery = @"INSERT INTO Classes (CourseNumber, ClassName, Label, Semester)
                                          VALUES (@CourseNumber, @ClassName, @Label, @Semester);
                                          SELECT last_insert_rowid();";

                    using (var cmd = new SQLiteCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CourseNumber", course.CourseNumber);
                        cmd.Parameters.AddWithValue("@ClassName", course.CourseName);
                        cmd.Parameters.AddWithValue("@Label", course.Label ?? string.Empty);
                        cmd.Parameters.AddWithValue("@Semester", course.Semester);

                        course.ClassID = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }

                return course;
            });
        }

        public async Task<Course> UpdateCourseAsync(Course course)
        {
            return await Task.Run(() =>
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();

                    // Get current semester info to handle semester change
                    int oldSemesterID = -1;
                    int degreePlanID = -1;
                    bool oldSemesterValue = false;

                    string semesterInfoQuery = @"
                        SELECT s.SemesterID, s.DegreePlanID, s.Semester
                        FROM Semester s
                        JOIN SemesterClass sc ON sc.SemesterID = s.SemesterID
                        WHERE sc.ClassID = @ClassID";

                    using (var cmd = new SQLiteCommand(semesterInfoQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@ClassID", course.ClassID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                oldSemesterID = reader.GetInt32(0);
                                degreePlanID = reader.GetInt32(1);
                                oldSemesterValue = reader.GetBoolean(2);
                            }
                        }
                    }

                    // Update the Classes table
                    string updateQuery = @"UPDATE Classes 
                                          SET CourseNumber = @CourseNumber, 
                                              ClassName = @ClassName, 
                                              Label = @Label, 
                                              Semester = @Semester 
                                          WHERE ClassID = @ClassID";

                    using (var cmd = new SQLiteCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CourseNumber", course.CourseNumber);
                        cmd.Parameters.AddWithValue("@ClassName", course.CourseName);
                        cmd.Parameters.AddWithValue("@Label", course.Label ?? string.Empty);
                        cmd.Parameters.AddWithValue("@Semester", course.Semester);
                        cmd.Parameters.AddWithValue("@ClassID", course.ClassID);

                        cmd.ExecuteNonQuery();

                        // If semester changed, move to new semester
                        if (oldSemesterID != -1 && course.Semester != oldSemesterValue)
                        {
                            string findNewSemesterQuery = @"
                                SELECT SemesterID
                                FROM Semester
                                WHERE DegreePlanID = @DegreePlanID AND Semester = @NewSemester
                                ORDER BY SchoolYear ASC
                                LIMIT 1";

                            int newSemesterID = -1;
                            using (var findCmd = new SQLiteCommand(findNewSemesterQuery, conn))
                            {
                                findCmd.Parameters.AddWithValue("@DegreePlanID", degreePlanID);
                                findCmd.Parameters.AddWithValue("@NewSemester", course.Semester);
                                var resultObj = findCmd.ExecuteScalar();
                                if (resultObj != null)
                                    newSemesterID = Convert.ToInt32(resultObj);
                            }

                            if (newSemesterID != -1)
                            {
                                string updateSemesterClassQuery = @"
                                    UPDATE SemesterClass
                                    SET SemesterID = @NewSemesterID
                                    WHERE ClassID = @ClassID";

                                using (var updateSemCmd = new SQLiteCommand(updateSemesterClassQuery, conn))
                                {
                                    updateSemCmd.Parameters.AddWithValue("@NewSemesterID", newSemesterID);
                                    updateSemCmd.Parameters.AddWithValue("@ClassID", course.ClassID);
                                    updateSemCmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }

                return course;
            });
        }

        public async Task<bool> DeleteCourseAsync(int classID)
        {
            return await Task.Run(() =>
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();

                    // Delete from SemesterClass first (foreign key constraint)
                    string deleteSemesterClassQuery = "DELETE FROM SemesterClass WHERE ClassID = @ClassID";
                    using (var cmd = new SQLiteCommand(deleteSemesterClassQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@ClassID", classID);
                        cmd.ExecuteNonQuery();
                    }

                    // Delete the class
                    string deleteQuery = "DELETE FROM Classes WHERE ClassID = @ClassID";
                    using (var cmd = new SQLiteCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@ClassID", classID);
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            });
        }

        // Helper method to map SQLiteDataReader to Course object
        private Course MapCourse(SQLiteDataReader reader)
        {
            return new Course
            {
                ClassID = reader.GetInt32(reader.GetOrdinal("ClassID")),
                CourseNumber = reader.GetString(reader.GetOrdinal("CourseNumber")),
                CourseName = reader.GetString(reader.GetOrdinal("ClassName")),
                Label = reader.IsDBNull(reader.GetOrdinal("Label")) ? string.Empty : reader.GetString(reader.GetOrdinal("Label")),
                Semester = reader.GetBoolean(reader.GetOrdinal("Semester"))
            };
        }
    }
}
