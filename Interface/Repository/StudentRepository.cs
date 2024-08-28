using Microsoft.Data.SqlClient;
using System.Data;
using WEB.DTOs;
using WEB.Interface.IRepository;
using WEB.Models;

namespace WEB.Interface.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public StudentRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("TestStudents");
        }

        public async Task<List<Student>> Display()
        {
            var students = new List<Student>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("selectStudent", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                var adapter = new SqlDataAdapter(command);
                var dtable = new DataTable();

                adapter.Fill(dtable);

                foreach (DataRow row in dtable.Rows)
                {
                    var student = new Student
                    {
                        Id = (Guid)row["Id"],
                        Name = (string)row["Name"],
                        Address = (string)row["Address"]
                    };
                    students.Add(student);
                }
            }

            return students;
        }

        public async Task Create(StudentDTO stuDTO)
        {
            var newId = Guid.NewGuid();

            var student = new Student
            {
                Id = newId,
                Name = stuDTO.Name,
                Address = stuDTO.Address
            };

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var cmd = new SqlCommand("insertStudent", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", student.Id);
                cmd.Parameters.AddWithValue("@Name", student.Name);
                cmd.Parameters.AddWithValue("@Address", student.Address);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<Student> GetByName(string name)
        {
            using (var conString = new SqlConnection(_connectionString))
            {
                await conString.OpenAsync();

                var cmd = new SqlCommand("selectStudentByName", conString)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Name", name);

                DataTable dtable = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtable);

                if (dtable.Rows.Count > 0)
                {
                    Student stuObject = new Student
                    {
                        Id = (Guid)dtable.Rows[0]["Id"],
                        Name = (string)dtable.Rows[0]["Name"],
                        Address = (string)dtable.Rows[0]["Address"]
                    };

                    return stuObject;
                }

                return null;
            }
        }

        public async Task<Student> GetById(Guid id)
        {
            using (var conString = new SqlConnection(_connectionString))
            {
                await conString.OpenAsync();

                var cmd = new SqlCommand("selectStudentById", conString)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);

                DataTable dtable = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtable);

                if (dtable.Rows.Count > 0)
                {
                    Student stuObject = new Student
                    {
                        Id = (Guid)dtable.Rows[0]["Id"],
                        Name = (string)dtable.Rows[0]["Name"],
                        Address = (string)dtable.Rows[0]["Address"]
                    };
                    await conString.CloseAsync();
                    return stuObject;
                }

                return null;
            }
        }
    }
}