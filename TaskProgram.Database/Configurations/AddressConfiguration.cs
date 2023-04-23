using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProgram.Database.Models;

namespace TaskProgram.Database.Configurations
{
	public class AddressConfiguration : IEntityTypeConfiguration<Address>
	{
		public void Configure(EntityTypeBuilder<Address> builder)
		{
				builder.ToTable("Addresses").HasKey(address => address.Id);

			builder
				.HasOne<Person>(address => address.Person)
				.WithOne(person => person.Address)
				.HasForeignKey<Address>(address => address.Person_FK)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
