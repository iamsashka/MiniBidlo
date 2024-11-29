

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

var app = builder.Build();

// ������������� ������
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

// ��������� ���������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Autorization}/{action=Index}/{id?}");

app.Run();

