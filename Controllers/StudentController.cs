using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WEB.DTOs;
using WEB.Interface.IRepository;
using WEB.Models;

namespace WEB.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;

        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }


        [HttpGet("Display")]
        public async Task<IActionResult> Display()
        {
            var students = await _studentRepository.Display();
            return Ok(students);
        }


        [HttpPost("Create")]
        public async Task<IActionResult> CreateStudent(StudentDTO stuDTO)
        {
            await _studentRepository.Create(stuDTO); //var ma store garna mildaina cuz no return type when creating
            return Ok(new { Message = "Added new record", stuDTO.Name, stuDTO.Address });
        }


        [HttpGet("GetByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var student = await _studentRepository.GetByName(name);
            if (student != null)
            {
                return Ok(student);
            }
            return NotFound(new { Message = "Not Found user", name });
        }


        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var student = await _studentRepository.GetById(id);
            if (student != null && student.Id != Guid.Empty)
            {
                return Ok(student);
            }
            return NotFound(new { Message = "Not Found user", id });
        }
    }
}