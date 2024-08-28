using System.ComponentModel.DataAnnotations;

namespace WEB.Models
{
    public class Student
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}