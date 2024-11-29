

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

var app = builder.Build();

// Использование сессий
app.UseSession();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Настройка маршрутов
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Autorization}/{action=Index}/{id?}");

app.Run();

