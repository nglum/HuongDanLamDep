using HuongDanLamDep.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HuongDanLamDep
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// DB
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
				?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(connectionString));

			builder.Services.AddDatabaseDeveloperPageExceptionFilter();

			// ✅ Identity UI (có trang /Identity/Account/Login)
			builder.Services.AddDefaultIdentity<IdentityUser>(options =>
			{
				// đơn giản cho sinh viên
				options.SignIn.RequireConfirmedAccount = false;
			})
			.AddEntityFrameworkStores<ApplicationDbContext>();

			// MVC + RazorPages
			builder.Services.AddControllersWithViews();
			builder.Services.AddRazorPages(); // ✅ THIẾU CÁI NÀY SẼ 404 /Identity/...

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
			app.UseStaticFiles(); // ✅ để CSS/JS chạy

			app.UseRouting();

			app.UseAuthentication(); // ✅ THIẾU CÁI NÀY Identity không hoạt động đúng
			app.UseAuthorization();

			// Areas
			app.MapControllerRoute(
				name: "areas",
				pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

			// Default
			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			// ✅ Map Identity UI
			app.MapRazorPages(); // ✅ THIẾU CÁI NÀY THÌ /Identity/... 404

			app.Run();
		}
	}
}