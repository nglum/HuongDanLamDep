using HuongDanLamDep.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using QuestPDF.Infrastructure;
using LicenseType = QuestPDF.Infrastructure.LicenseType;
using HuongDanLamDep.Services;

QuestPDF.Settings.License = LicenseType.Community;
namespace HuongDanLamDep
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
			// DB
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
				?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(connectionString));

			builder.Services.AddDatabaseDeveloperPageExceptionFilter();

			// ✅ CHỈ GIỮ 1 Identity (AddDefaultIdentity) + Roles
			builder.Services.AddDefaultIdentity<IdentityUser>(options =>
			{
				options.SignIn.RequireConfirmedAccount = false;
			})
			.AddRoles<IdentityRole>()
			.AddEntityFrameworkStores<ApplicationDbContext>();

			builder.Services.AddControllersWithViews();
			builder.Services.AddRazorPages(); // ✅ để /Identity/... chạy

			var app = builder.Build();

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

			app.UseAuthentication(); // ✅ cực quan trọng
			app.UseAuthorization();

			// Areas
			app.MapControllerRoute(
				name: "areas",
				pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

			// Default
			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.MapRazorPages(); // ✅ Identity UI endpoints
			using (var scope = app.Services.CreateScope())
			{
				IdentitySeed.SeedAsync(scope.ServiceProvider).GetAwaiter().GetResult();
			}
			builder.Services.AddScoped<HuongDanLamDep.Services.ITutorialPdfService, HuongDanLamDep.Services.TutorialPdfService>();
			app.Run();

		}
	}
}