// DTO này được tạo ra để sử dụng khi POST dữ liệu lên Swagger.
// Lý do: Entity Product có liên kết 1-n với Category, nên khi POST trực tiếp Product, Swagger yêu cầu cung cấp thêm dữ liệu Category.
// DTO này giúp tách biệt dữ liệu cần thiết khi tạo mới Product, chỉ yêu cầu CategoryId thay vì toàn bộ Category object.
public class ProductCategoryDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    public int CategoryId { get; set; }
}
