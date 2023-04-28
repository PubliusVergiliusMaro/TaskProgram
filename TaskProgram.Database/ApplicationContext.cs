﻿using Microsoft.EntityFrameworkCore;
using TaskProgram.Common;
using TaskProgram.Database.Configurations;

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
			optionsBuilder.UseSqlServer(DbConfiguration.CONNECTION_STRING);
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Address>()
				.HasOne<Person>(address => address.Person)
				.WithOne(person => person.Address)
				.HasForeignKey<Address>(address => address.Person_FK)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
