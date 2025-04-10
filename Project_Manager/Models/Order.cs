using Microsoft.AspNetCore.Identity;

namespace Project_Manager.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        public string UserId { get; set; } 
        public IdentityUser? User { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } = new();
    }
}
