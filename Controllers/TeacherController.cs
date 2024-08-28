using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WEB.DTOs;
using WEB.Models;

namespace WEB.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        public IConfiguration _configuration;

        public TeacherController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("GetTeacher")]
        public async Task<IActionResult> GetTeacher()
        {
            List<Teacher> teacherList = new List<Teacher>();

            using (var con = new SqlConnection(this._configuration.GetConnectionString("TestStudents")))
            {
                con.Open();

                using (var command = new SqlCommand("selectTeacher", con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Teacher teacher = new Teacher
                            {
                                Id = (Guid)reader["Id"],
                                Name = (string)reader["Name"],
                                Age = (int)reader["Age"]
                            };

                            teacherList.Add(teacher);
                        }
                    }
                }
                return Ok(teacherList);
            }
        }

        [HttpPost("AddTeacher")]
        public async Task<IActionResult> AddTeacher(TeacherDTO teaDTO)
        {
            Guid newId = Guid.NewGuid();

            var teacher = new Teacher
            {
                Id = newId,
                Name = teaDTO.Name,
                Age = teaDTO.Age
            };

            using (var con = new SqlConnection(this._configuration.GetConnectionString("TestStudents")))
            {
                await con.OpenAsync();

                using (var command = new SqlCommand())
                {
                    command.Connection = con;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"Insert into Teachers (Id, Name, Age) Values (@Id, @Name, @Age)";

                    command.Parameters.AddWithValue("@Id", teacher.Id);
                    command.Parameters.AddWithValue("@Name", teacher.Name);
                    command.Parameters.AddWithValue("@Age", teacher.Age);

                    await command.ExecuteNonQueryAsync();
                }
            }
            return Ok(new { Message = "Added new teacher", teacher.Id, teacher.Name, teacher.Age });
        }
    }
}
