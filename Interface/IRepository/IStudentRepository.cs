using WEB.DTOs;
using WEB.Models;

namespace WEB.Interface.IRepository
{
    public interface IStudentRepository
    {
        Task<List<Student>> Display();
        Task Create(StudentDTO stuDTO);
        Task<Student> GetByName(string name);
        Task<Student> GetById(Guid id);
    }
}
