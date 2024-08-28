using System.ComponentModel.DataAnnotations;

namespace WEB.DTOs
{
    public class StudentDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
