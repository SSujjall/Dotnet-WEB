using System.ComponentModel.DataAnnotations;

namespace WEB.Models
{
    public class Book
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
    }
}
