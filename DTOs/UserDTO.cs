using System.ComponentModel.DataAnnotations;

namespace WEB.DTOs
{
    public class UserDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class UserDisplayDTO
    {
        public string Username { get; set; }
    }
}
