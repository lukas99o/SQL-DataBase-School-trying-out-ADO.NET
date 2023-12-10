using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_DataBase_School
{
    internal static class Functions
    {
        private static string _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=SchoolProject;Integrated Security=True";

        public static void ViewAllStudents(bool sort, bool ascDesc)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string orderByClause = sort ? (ascDesc ? "ORDER BY FirstName" : "ORDER BY FirstName DESC") :
                                              (ascDesc ? "ORDER BY LastName" : "ORDER BY LastName DESC");

                string sqlQuery = "SELECT * FROM Students " + orderByClause;

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("StudentID\tFirstName\tLastName\tAge\tClassID");

                        while (reader.Read())
                        {
                            int studentID = reader.GetInt32(reader.GetOrdinal("StudentID"));
                            string firstName = reader.GetString(reader.GetOrdinal("FirstName")).Trim();
                            string lastName = reader.GetString(reader.GetOrdinal("LastName")).Trim();
                            int age = reader.GetInt32(reader.GetOrdinal("Age"));
                            int classID = reader.GetInt32(reader.GetOrdinal("ClassID"));

                            Console.WriteLine($"{studentID}\t\t{firstName}\t\t{lastName}\t\t{age}\t\t{classID}");
                        }
                    }
                }
            }
        }

        public static void GetStudentsInClass(int classID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT * FROM Students WHERE ClassID = @ClassID", connection))
                {
                    command.Parameters.AddWithValue("@ClassID", classID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine($"Elever i KlassID {classID}:");
                        
                        while (reader.Read())
                        {
                            int studentID = reader.GetInt32(reader.GetOrdinal("StudentID"));
                            string firstName = reader.GetString(reader.GetOrdinal("FirstName")).Trim();
                            string lastName = reader.GetString(reader.GetOrdinal("LastName")).Trim();
                            int age = reader.GetInt32(reader.GetOrdinal("Age"));

                            Console.WriteLine($"{studentID}\t\t{firstName}\t\t{lastName}\t\t{age}");
                        }
                    }
                }
            }
        }

        public static void AddNewPersonnel(string firstName, string lastName, string category)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Personnel (FirstName, LastName, Category) " +
                    "VALUES (@FirstName, @LastName, @Category)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Category", category);

                    int rowsAffected = command.ExecuteNonQuery();

                    Console.WriteLine($"{rowsAffected} row's inserted.");
                }
            }
        }

        public static void ViewPersonnel(string category)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Personnel WHERE Category = @Category";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Category", category);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int personnelID = reader.GetInt32(reader.GetOrdinal("PersonnelID"));
                            string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                            string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                            string personnelCategory = reader.GetString(reader.GetOrdinal("Category"));

                            Console.WriteLine($"{personnelID}\t{firstName}\t{lastName}\t{personnelCategory}");
                        }
                    }
                }
            }
        }

        public static void GradesThisMonth()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT Students.FirstName, Students.LastName, Courses.CourseName, Grades.Grade, Grades.GradeDate
                    FROM Grades
                    JOIN Students ON Grades.StudentID = Students.StudentID
                    JOIN Courses ON Grades.CourseID = Courses.CourseID
                    WHERE Grades.GradeDate >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 1, 0)
                    AND Grades.GradeDate < DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string studentName = $"{reader["FirstName"]} {reader["LastName"]}";
                            string courseName = reader["CourseName"].ToString();
                            int gradeValue = Convert.ToInt32(reader["Grade"]);
                            DateTime gradeDate = Convert.ToDateTime(reader["GradeDate"]);

                            Console.WriteLine($"Student: {studentName}, Course: {courseName}, Grade: {gradeValue}, Date: {gradeDate.ToShortDateString()}");

                        }
                    }
                }
            }
        }

        public static void AverageGrade()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT CourseName, AVG(Grades.Grade) AS AverageGrade,
                                MAX(Grades.Grade) AS HighestGrade, MIN(Grades.Grade) AS
                                LowestGrade FROM Courses JOIN Grades ON Courses.CourseID =
                                Grades.CourseID GROUP BY Courses.CourseName;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string course = reader["CourseName"].ToString();
                            double averageGrade = Convert.ToDouble(reader["AverageGrade"]);
                            int highestGrade = Convert.ToInt32(reader["HighestGrade"]);
                            int lowestGrade = Convert.ToInt32(reader["LowestGrade"]);

                            Console.WriteLine($"Course: {course}");
                            Console.WriteLine($"Average Grade: {averageGrade}");
                            Console.WriteLine($"Highest Grade: {highestGrade}");
                            Console.WriteLine($"Lowest Grade: {lowestGrade}");
                            Console.WriteLine();
                        }
                    }
                }
            }
        }

        public static void AddNewStudents(string firstName, string lastName, DateTime birthDate, int classID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO Students (FirstName, LastName, BirthDate, ClassID)
                                VALUES (@FirstName, @LastName, @BirthDate, @ClassID);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("FirstName", firstName);
                    command.Parameters.AddWithValue("LastName", lastName);
                    command.Parameters.AddWithValue("BirthDate", birthDate);
                    command.Parameters.AddWithValue("ClassID", classID);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Den nya studenten lades till i databasen");
                    }
                    else
                    {
                        Console.WriteLine("Lyckades inte lägga in studenten i databasen");
                    }
                }
            }
        }
    }
}
