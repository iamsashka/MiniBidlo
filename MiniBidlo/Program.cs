using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ��������� �������
builder.Services.AddControllersWithViews();

// ��������� ����������� � ���� ������
builder.Services.AddDbContext<MiniBidlo.Models.FlowerMagazinContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"))
);

// ��������� ������
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // ���������� ����-��� ������
});

// ���������� �������������� ����� ����
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Autorization/Index";  // ���� � �������� �����������
        options.LogoutPath = "/Autorization/Logout";  // ���� � ������
    });

// ���������� �����������
builder.Services.AddAuthorization();

var app = builder.Build();

// ������� ������������� middleware ������ ���� ����������
app.UseRouting();  // ������������� ���������

// ������������� �������������� � �����������
app.UseAuthentication();  // ���������� ��������������
app.UseAuthorization();   // ���������� �����������

// ������������� ������
app.UseSession();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// ��������� ���������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Autorization}/{action=Index}/{id?}");

app.Run();
