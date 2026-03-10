using HuongDanLamDep.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
var builder = WebApplication.CreateBuilder(args);

// DB
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
	?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Identity + Roles (đơn giản cho sinh viên)
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
	options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// MVC + Razor Pages (Identity UI)
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// (KHI NÀO LÀM PDF/EXCEL THÌ ĐĂNG KÝ SERVICES Ở ĐÂY - TRƯỚC Build)
// builder.Services.AddScoped<...>();

var app = builder.Build();
builder.Services.AddScoped<HuongDanLamDep.Services.ITutorialReportPdfService, HuongDanLamDep.Services.TutorialReportPdfService>();
// Pipeline
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Routes
app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// Seed roles/admin (nếu bạn có file Data/IdentitySeed.cs)
using (var scope = app.Services.CreateScope())
{
	IdentitySeed.SeedAsync(scope.ServiceProvider).GetAwaiter().GetResult();
}

app.Run();