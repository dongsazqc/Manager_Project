using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore; 
using Project_Manager.Data; 
using Project_Manager.Models; 

namespace Project_Manager.Controllers
{
    [Route("api/[controller]")] // Đoạn này phải như này nè mấy ní, route nó mới hiểu đường API là /api/product
    [ApiController] // Không có dòng này là mất hết mấy tính năng xịn như validate model, parse json các thứ
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context; // Phải có context thì mới nói chuyện được với DB chứ mấy bố
        }

        // GET: api/product
        [HttpGet]
        [AllowAnonymous] // Nhớ thêm dòng này mấy ní, không có là người lạ không xem được gì đâu nha
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
          return await _context.Products.ToListAsync();
        }

        // GET: api/product/5
        [HttpGet("{id}")] 
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
           
            var product = await _context.Products.Include(p => p.Category)
                                                 .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            return product;
        }

        // POST: api/product
        [HttpPost] 
        public async Task<ActionResult<ProductCategoryDTO>>reateProduct(ProductCategoryDTO ProCateDTO)
        {
            var crePro = new Product
            {
                 Name = ProCateDTO.Name,
                 Price = ProCateDTO.Price,
                 Description = ProCateDTO.Description,
                 ImageUrl = ProCateDTO.ImageUrl,
                 CategoryId = ProCateDTO.CategoryId,
            };
             _context.Products.Add(crePro);
            await  _context.SaveChangesAsync();
            return Ok();
        }

        // PUT: api/product/5
        [HttpPut("{id}")] 
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
                return BadRequest(); // ID trên URL với trong body mà lệch là tao cho lỗi liền nha

            _context.Entry(product).State = EntityState.Modified; // Gắn cờ update, không là nó tưởng đang thêm

            try
            {
                await _context.SaveChangesAsync(); 
            }
            catch (DbUpdateConcurrencyException) // Khi có drama chỉnh cùng lúc (concurrent update)
            {
                if (!_context.Products.Any(p => p.Id == id))
                    return NotFound();
                else
                    throw; 
            }

            return NoContent(); 
        }

        // DELETE: api/product/5
        [HttpDelete("{id}")] 
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id); // Kiểm tra sản phẩm có tồn tại không
            if (product == null)
                return NotFound(); 

            _context.Products.Remove(product); 
            await _context.SaveChangesAsync(); 

            return NoContent(); 
        }
    }
}
