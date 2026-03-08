using HuongDanLamDep.Models;
using Microsoft.EntityFrameworkCore;
namespace HuongDanLamDep.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<Tag> Tags { get; set; }
	}
}
