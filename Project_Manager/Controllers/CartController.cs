using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Data;
using Project_Manager.Extensions;
using Project_Manager.Models;
using System.Linq;

namespace Project_Manager.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor để inject DbContext vào controller
        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }
        // Giả sử bạn đã có CartItem
        public class CartItem
        {
            public Product Product { get; set; }
            public int Quantity { get; set; }
        }

        private List<CartItem> _cartItems
        {
            get
            {
                var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
                return cart;
            }
            set
            {
                HttpContext.Session.SetObjectAsJson("Cart", value);
            }
        }

        // Action để hiển thị giỏ hàng
        public IActionResult Index()
        {
            return View(_cartItems);  // Trả về danh sách CartItem
        }

        // Thêm sản phẩm vào giỏ hàng
        public IActionResult AddToCart(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
                return NotFound();

            var cart = _cartItems;
            var existingItem = cart.FirstOrDefault(item => item.Product.Id == productId);

            if (existingItem == null)
            {
                cart.Add(new CartItem { Product = product, Quantity = 1 });
            }
            else
            {
                existingItem.Quantity++;
            }

            _cartItems = cart;
            return RedirectToAction("Index");
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = _cartItems;
            var itemToRemove = cart.FirstOrDefault(item => item.Product.Id == productId);

            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                _cartItems = cart;
            }

            return RedirectToAction("Index");
        }
    }
}
