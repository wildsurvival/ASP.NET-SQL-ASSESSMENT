using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace parity_mvc_intro.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Code { get; set; }

        public static List<Course> GetAvailableCourses(int studentId)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StudentRegistryContext"].ToString()))
            {
                connection.Open();

                // Get base student data
                using (SqlCommand command = new SqlCommand("dbo.GetAvailableCourses", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudentId", studentId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<Course> courses = new List<Course>();

                        while (reader.Read())
                        {
                            courses.Add(new Course() { 
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"],
                                Category = (string)reader["Category"],
                                Code = (int)reader["Code"]
                            });
                        }

                        return courses;
                    }
                }
            }
        }

        public static bool RegisterCourse(int studentId, int courseId)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StudentRegistryContext"].ToString()))
            {
                connection.Open();

                // Get base student data
                using (SqlCommand command = new SqlCommand("dbo.RegisterCourse", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CourseId", courseId);
                    command.Parameters.AddWithValue("@StudentId", studentId);

                    switch (command.ExecuteNonQuery())
                    {
                        case 0:
                            return false;
                        default:
                            return true;
                    }
                }
            }
        }

        public static bool DropCourse(int studentId, int courseId)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StudentRegistryContext"].ToString()))
            {
                connection.Open();

                // Get base student data
                using (SqlCommand command = new SqlCommand("dbo.DropCourse", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CourseId", courseId);
                    command.Parameters.AddWithValue("@StudentId", studentId);

                    switch (command.ExecuteNonQuery())
                    {
                        case 0:
                            return false;
                        default:
                            return true;
                    }
                }
            }
        }
    }
}