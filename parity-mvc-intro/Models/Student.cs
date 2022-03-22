using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace parity_mvc_intro.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RegistryId { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public List<Course> Courses { get; set; }

        public static (bool success, int id) LogIn(string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StudentRegistryContext"].ToString()))
            {
                using (SqlCommand command = new SqlCommand("dbo.LogIn", connection))
                {
                    connection.Open();

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        switch (reader.HasRows)
                        {
                            case false:
                                return (false, -1);
                            case true:
                                reader.Read();
                                return (true, (int)reader["Id"]);
                                // else: We're gonna assume that there is no case where multiple accounts return due to constraints in account creation
                        }
                    }
                }
            }

            return (false, -1);
        }

        public static Student GetStudent(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StudentRegistryContext"].ToString()))
            {
                connection.Open();
                Student student = null;

                // Get base student data
                using (SqlCommand command = new SqlCommand("dbo.GetStudent", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        // Account exists
                        if (reader.HasRows)
                        {
                            reader.Read();

                            student = new Student()
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"],
                                RegistryId = (int)reader["RegistryId"],
                                Email = (string)reader["Email"],
                                Password = (string)reader["Password"]
                            };
                        }
                    }
                }

                // Failed to read base user data, cut off the rest of the query
                if (student == null)
                    return null;

                // Get student course information
                using (SqlCommand command = new SqlCommand("dbo.GetStudentCourses", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            student.Courses = new List<Course>();

                            while (reader.Read())
                            {
                                student.Courses.Add(new Course()
                                {
                                    Id = (int)reader["Id"],
                                    Name = (string)reader["Name"],
                                    Category = (string)reader["Category"],
                                    Code = (int)reader["Code"]
                                });
                            }
                        }
                    }
                }

                return student;
            }
        }
    }
}