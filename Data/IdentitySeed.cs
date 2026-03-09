using Microsoft.AspNetCore.Identity;

namespace HuongDanLamDep.Data
{
	public static class IdentitySeed
	{
		public static async Task SeedAsync(IServiceProvider services)
		{
			var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
			var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

			// Create roles
			string[] roles = { "Admin", "User" };
			foreach (var role in roles)
				if (!await roleManager.RoleExistsAsync(role))
					await roleManager.CreateAsync(new IdentityRole(role));

			// Create admin account
			string adminEmail = "admin@demo.com";
			string adminPass = "Admin123!";

			var admin = await userManager.FindByEmailAsync(adminEmail);
			if (admin == null)
			{
				admin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
				var result = await userManager.CreateAsync(admin, adminPass);
				if (result.Succeeded)
					await userManager.AddToRoleAsync(admin, "Admin");
			}
			else
			{
				if (!await userManager.IsInRoleAsync(admin, "Admin"))
					await userManager.AddToRoleAsync(admin, "Admin");
			}
		}
	}
}