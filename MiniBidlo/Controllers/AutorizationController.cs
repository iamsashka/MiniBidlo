using Microsoft.AspNetCore.Mvc;
using MiniBidlo.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace MiniBidlo.Controllers
{
    public class AutorizationController : Controller
    {
        private readonly FlowerMagazinContext _context;

        // Конструктор для инжекции зависимости контекста базы данных
        public AutorizationController(FlowerMagazinContext context)
        {
            _context = context;
        }

        // Страница авторизации
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Страница регистрации
        [HttpGet]
        public IActionResult Registry()
        {
            return View();
        }

        // Обработка формы регистрации
        [HttpPost]
        public async Task<IActionResult> Registry(string login, string password, string email, string role, string name, string phone)
        {
            // Проверка на наличие пользователя с таким логином или email
            if (_context.Users.Any(u => u.Login == login || u.Email == email))
            {
                ViewData["Error"] = "Пользователь с таким логином или email уже существует.";
                return View();
            }

            // Хэшируем пароль
            string hashedPassword = HashPassword(password);

            // Создание нового пользователя
            var newUser = new User
            {
                Login = login,
                Password = hashedPassword,
                Email = email,
                Role = role,  // Роль по умолчанию
                Name = name, // Имя по умолчанию
                Phone = phone // Телефон по умолчанию
            };

            // Сохранение нового пользователя в базе данных
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Перенаправляем на страницу авторизации
            return RedirectToAction("Index");
        }

        // Метод для хэширования пароля
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        // Метод для проверки пароля (для входа)
        private bool VerifyPassword(string password, string storedHash)
        {
            string hashedPassword = HashPassword(password);
            return hashedPassword == storedHash;
        }
       
        // Обработка формы авторизации (POST)
        [HttpPost]
        public IActionResult Index(string login, string password)
        {
            // Поиск пользователя в базе данных по логину
            var user = _context.Users.FirstOrDefault(u => u.Login == login);

            if (user == null || user.Password != password)  // Простой пример, лучше использовать хэширование паролей
            {
                ViewData["Error"] = "Неверный логин или пароль.";
                return View();  // Если не найден или неверный пароль, возвращаем форму с ошибкой
            }

            // Успешная авторизация
            HttpContext.Session.SetString("UserName", user.Name);  // Сохраняем имя пользователя в сессии
            return RedirectToAction("Profile");  // Перенаправление на страницу профиля
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();  // Очистить сессию
            TempData["Message"] = "Вы успешно вышли из системы.";  // Установка временного сообщения
            return RedirectToAction("Index");  // Перенаправить на страницу авторизации
        }
        public IActionResult Profile()
        {
            var userName = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Index");  // Перенаправляем на страницу авторизации
            }

            var user = _context.Users.FirstOrDefault(u => u.Name == userName);

            if (user == null)
            {
                return RedirectToAction("Index");  // Если пользователя нет, перенаправляем на страницу авторизации
            }

            return View(user);  // Возвращаем представление с информацией о пользователе
        }

    }
}

