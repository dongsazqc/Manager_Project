using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Project_Manager.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }

        [RegularExpression(@"^/images/.*\.(jpg|jpeg|png|gif)$", ErrorMessage = "Image URL must be a valid image file (e.g., .jpg, .png, .gif).")]
        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
