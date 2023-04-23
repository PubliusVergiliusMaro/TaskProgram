using Microsoft.EntityFrameworkCore;
using TaskProgram.Common;
using TaskProgram.Database.Configurations;
using TaskProgram.Database.Models;

namespace TaskProgram.Database
{
	public class ApplicationContext : DbContext
	{
		public DbSet<Person> People { get; set; }
		public DbSet<Address> Address { get; set; }
		public ApplicationContext()
		{
			Database.Migrate();
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(Constants.CONNECTION_STRING);
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new AddressConfiguration());
			modelBuilder.ApplyConfiguration(new PersonConfiguration());
		}
	}
}
