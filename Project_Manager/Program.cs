using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Data;

var builder = WebApplication.CreateBuilder(args);

// Thêm Razor Pages nha các bé, để còn xài được cái UI mặc định của Identity. Xài chùa mà vẫn xịn 😎
builder.Services.AddRazorPages();

// Thêm MVC vô, chứ không là project nó méo biết controller là cái gì đâu nha mấy đứa
builder.Services.AddControllersWithViews();

// Kết nối database nè mấy má, nhớ chỉnh lại connection string chứ xài của tao là toang đấy 👻
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

// Bật Swagger lên nhen, test API cho sướng tay, khỏi phải đoán mò 🙌
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Lắng nghe tất cả IP luôn nha mấy ông. Không phải localhost nữa, IP nào cũng được, xịn chưa 😤
builder.WebHost.UseUrls("http://0.0.0.0:5293");

// Cấu hình Identity để đăng ký đăng nhập xài được role luôn nha, không là "tài khoản trần truồng" 😆
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI(); // Quan trọng lắm nhen, đừng xóa. Xóa là đi luôn cái giao diện login xịn xò

var app = builder.Build();

// Tạo sẵn mấy cái role "Admin" với "User" để sau này gán tài khoản cho lẹ 😎
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = new[] { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role)); // Có là chơi, chưa có là tạo luôn cho nóng 💥
        }
    }
}

// Middleware nè, phần xử lý các tầng trong web. Nhìn vậy thôi chứ cực kỳ quan trọng 😮‍💨
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Có lỗi là đẩy về trang Error cho đỡ bốc cháy 🤯
    app.UseHsts(); // Bảo mật nhen, không đùa đâu. Chuẩn https
}

app.UseSwagger(); // Bật Swagger API interface nè mấy má
app.UseSwaggerUI(); // Không có UI thì test API bằng niềm tin à?

app.UseHttpsRedirection(); // Bắt buộc phải xài HTTPS nha. Đừng xài HTTP chay, quê lắm 🤓
app.UseStaticFiles();      // Cho phép load ảnh, css, js linh tinh các kiểu con đà điểu

app.UseRouting();          // Cái này để định hướng request đi đâu. Kiểu như dẫn đường chứ không là đi lạc 😵‍💫
app.UseAuthentication();   // Đăng nhập vào đây mới có token, không có là bị đá ra ngoài 🥹
app.UseAuthorization();    // Check quyền, không là đứa nào cũng làm admin thì mệt lắm 😤

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Cấu hình định tuyến chính, Home là điểm xuất phát cho mọi cuộc hành trình 🏠

app.MapRazorPages(); // Không có cái này thì giao diện Identity toang. Thêm đi đừng hỏi!

app.Run(); // Chạy thôi mấy ông ơiiiii 🚀
