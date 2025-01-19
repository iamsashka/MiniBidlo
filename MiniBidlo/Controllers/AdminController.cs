using Microsoft.AspNetCore.Mvc;
using MiniBidlo.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MiniBidlo.Controllers
{
    public class AdminController : Controller
    {
        private readonly FlowerMagazinContext _context;

        public AdminController(FlowerMagazinContext context)
        {
            _context = context;
        }

        // Проверка на роль администратора
        private bool IsAdmin()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            return userRole != null && userRole.Equals("admin", System.StringComparison.OrdinalIgnoreCase);
        }

        // Главная страница панели администратора
        public IActionResult Index()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Catalog");  // Если не администратор, перенаправляем на каталог
            }

            var products = _context.Products.Include(p => p.Category).ToList();
            return View(products);
        }

        // Создание продукта
        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Catalog");  // Если не администратор, перенаправляем на каталог
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Catalog");  // Если не администратор, перенаправляем на каталог
            }

            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // Редактирование продукта
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Catalog");  // Если не администратор, перенаправляем на каталог
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Catalog");  // Если не администратор, перенаправляем на каталог
            }

            if (id != product.IdProduct)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(p => p.IdProduct == product.IdProduct))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // Удаление продукта
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Catalog");  // Если не администратор, перенаправляем на каталог
            }

            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.IdProduct == id.Value);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Catalog");  // Если не администратор, перенаправляем на каталог
            }

            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // Управление пользователями
        public IActionResult IndexUser()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Catalog");  // Если не администратор, перенаправляем на каталог
            }

            var users = _context.Users.ToList();
            return View(users);
        }

        // Создание пользователя
        [HttpGet]
        public IActionResult CreateUser()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Catalog");  // Если не администратор, перенаправляем на каталог
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Catalog");  // Если не администратор, перенаправляем на каталог
            }

            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("IndexUser");
            }
            return View(user);
        }

        // Редактирование пользователя
        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Catalog");  // Если не администратор, перенаправляем на каталог
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(int id, User user)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Catalog");  // Если не администратор, перенаправляем на каталог
            }

            if (id != user.IdUser)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Users.Any(u => u.IdUser == user.IdUser))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction("IndexUser");
            }
            return View(user);
        }

        // Удаление пользователя
        [HttpGet]
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Catalog");  // Если не администратор, перенаправляем на каталог
            }

            if (!id.HasValue)
            {
                return RedirectToAction("IndexUser");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdUser == id.Value);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        public async Task<IActionResult> DeleteUserConfirmed(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Catalog");  // Если не администратор, перенаправляем на каталог
            }

            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("IndexUser");
        }

        // Управление категориями
        public IActionResult ManageCategories()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Catalog");  // Если не администратор, перенаправляем на каталог
            }

            var categories = _context.Categories.ToList();
            return View(categories);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Catalog");  // Если не администратор, перенаправляем на каталог
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Catalog");  // Если не администратор, перенаправляем на каталог
            }

            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageCategories");
            }
            return View(category);
        }

        // Редактирование категории
        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Catalog");  // Если не администратор, перенаправляем на каталог
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(int id, Category category)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Catalog");  // Если не администратор, перенаправляем на каталог
            }

            if (id != category.IdCategory)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Categories.Any(c => c.IdCategory == category.IdCategory))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction("ManageCategories");
            }
            return View(category);
        }

        // Удаление категории
        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Catalog");  // Если не администратор, перенаправляем на каталог
            }

            if (!id.HasValue)
            {
                return RedirectToAction("ManageCategories");
            }

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.IdCategory == id.Value);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("DeleteCategory")]
        public async Task<IActionResult> DeleteCategoryConfirmed(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Catalog");  // Если не администратор, перенаправляем на каталог
            }

            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ManageCategories");
        }
    }
}
