using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.Models;

public class UserController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public class UserWithRoleViewModel
    {
        public IdentityUser User { get; set; }
        public IList<string> Roles { get; set; }
    }

    public async Task<IActionResult> Index()
    {
        var users = _userManager.Users.ToList();
        var userRoles = new List<UserWithRoleViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userRoles.Add(new UserWithRoleViewModel
            {
                User = user,
                Roles = roles
            });
        }

        return View(userRoles); // View: Views/User/Index.cshtml
    }

    // 🛠️ Sửa người dùng
    public async Task<IActionResult> Edit(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var userRoles = await _userManager.GetRolesAsync(user);
        var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

        var model = new EditUserViewModel
        {
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            SelectedRoles = userRoles,
            AvailableRoles = allRoles
        };

        return View(model); // Views/User/Edit.cshtml
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditUserViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null) return NotFound();

        user.UserName = model.UserName;
        user.Email = model.Email;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            model.AvailableRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            return View(model);
        }

        // Xoá hết các roles hiện tại
        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);

        // Thêm các roles mới
        if (model.SelectedRoles != null && model.SelectedRoles.Any())
        {
            await _userManager.AddToRolesAsync(user, model.SelectedRoles);
        }

        return RedirectToAction("Index");
    }

    // ❌ Xoá người dùng
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        await _userManager.DeleteAsync(user);
        return RedirectToAction("Index");
    }
    // Hiển thị form thêm user
    public async Task<IActionResult> Create()
    {
        var model = new CreateUserViewModel
        {
            AvailableRoles = _roleManager.Roles.Select(r => r.Name).ToList()
        };

        return View(model); // Views/User/Create.cshtml
    }

    // Xử lý form thêm user
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.AvailableRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            return View(model);
        }

        var user = new IdentityUser
        {
            Email = model.Email,
            UserName = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            model.AvailableRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            return View(model);
        }

        if (model.SelectedRoles != null && model.SelectedRoles.Any())
        {
            await _userManager.AddToRolesAsync(user, model.SelectedRoles);
        }

        return RedirectToAction("Index");
    }

}
