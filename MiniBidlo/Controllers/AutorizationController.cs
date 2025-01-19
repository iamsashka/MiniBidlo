using Microsoft.AspNetCore.Mvc;
using MiniBidlo.Models;
using System.Security.Cryptography;
using System.Text;

public class AutorizationController : Controller
{
    private readonly FlowerMagazinContext _context;

    public AutorizationController(FlowerMagazinContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Registry()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Registry(string login, string password, string email, string role, string name, string phone)
    {
        if (_context.Users.Any(u => u.Login == login || u.Email == email))
        {
            ViewData["Error"] = "Пользователь с таким логином или email уже существует.";
            return View();
        }

        string hashedPassword = HashPassword(password);

        var newUser = new User
        {
            Login = login,
            Password = hashedPassword,
            Email = email,
            Role = "customer",  // Роль по умолчанию
            Name = name,
            Phone = phone
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Index(string login, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Login == login);
        var hashedPassword = HashPassword(password);

        if (user == null || user.Password != hashedPassword)
        {
            ViewData["Error"] = "Неверный логин или пароль.";
            return View();
        }

        // Сохраняем данные пользователя в сессии
        HttpContext.Session.SetInt32("UserId", user.IdUser);
        HttpContext.Session.SetString("UserName", user.Name);
        HttpContext.Session.SetString("UserRole", user.Role);

        return RedirectToAction("Profile");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        TempData["Message"] = "Вы успешно вышли из системы.";
        return RedirectToAction("Index");
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }

    public IActionResult Profile()
    {
        var userName = HttpContext.Session.GetString("UserName");

        if (string.IsNullOrEmpty(userName))
        {
            return RedirectToAction("Index");
        }

        var user = _context.Users.FirstOrDefault(u => u.Name == userName);

        if (user == null)
        {
            return RedirectToAction("Index");
        }

        return View(user);
    }

    // Страница администрирования (доступна только админам)
    [HttpGet]
    public IActionResult AdminPanel()
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        if (userRole != "Admin")
        {
            return RedirectToAction("Index");  // Если роль не Admin, перенаправляем на страницу авторизации
        }

        return View();  // Страница доступна только для администраторов
    }
}
