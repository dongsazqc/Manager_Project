using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.Data;
using Project_Manager.Models;
using System.Linq;

namespace Project_Manager.Controllers
{
    public class ProductsViewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsViewController(ApplicationDbContext context)
        {
            _context = context;
        }
        // Action để hiển thị tất cả các sản phẩm
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }
        public IActionResult Details( int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            return View(product);
        }

        // Action để hiển thị form tạo sản phẩm mới
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Product());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
         // Kiểm tra nếu người dùng có chọn file ảnh
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Lưu ảnh vào thư mục wwwroot/images
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imageFile.FileName);

                    // Copy ảnh vào thư mục
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn vào thuộc tính ImageUrl của sản phẩm
                    product.ImageUrl = "/images/" + imageFile.FileName;
                }       

                // Thêm sản phẩm vào database
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(product);
        }

        // Action để hiển thị form chỉnh sửa sản phẩm
        public IActionResult Edit(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        // Action xử lý POST yêu cầu chỉnh sửa sản phẩm
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = _context.Products.FirstOrDefault(p => p.Id == product.Id);
                if (existingProduct == null)
                    return NotFound();

                // Cập nhật thông tin sản phẩm
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;

                _context.SaveChanges(); // Lưu thay đổi vào database

                return RedirectToAction("Index");
            }
            return View(product);
        }

        // Action để xóa sản phẩm
        
        public IActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        // Action xử lý POST yêu cầu xóa sản phẩm
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteC(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges(); // Lưu thay đổi vào database
            }

            return RedirectToAction("Index");
        }
    }
}
