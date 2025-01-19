using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы
builder.Services.AddControllersWithViews();

// Настройка подключения к базе данных
builder.Services.AddDbContext<MiniBidlo.Models.FlowerMagazinContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"))
);

// Настройка сессий
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Установить тайм-аут сессии
});

// Добавление аутентификации через куки
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Autorization/Index";  // Путь к странице авторизации
        options.LogoutPath = "/Autorization/Logout";  // Путь к выходу
    });

// Добавление авторизации
builder.Services.AddAuthorization();

var app = builder.Build();

// Порядок использования middleware должен быть правильным
app.UseRouting();  // Распознавание маршрутов

// Использование аутентификации и авторизации
app.UseAuthentication();  // Подключаем аутентификацию
app.UseAuthorization();   // Подключаем авторизацию

// Использование сессий
app.UseSession();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Настройка маршрутов
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Autorization}/{action=Index}/{id?}");

app.Run();
