using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaskProgram.Database.Models
{
	public class Address
	{
		public Address() { }
		public Address( string streetAddress, string city, string state, string postalCode)
		{
			StreetAddress= streetAddress;
			City= city;
			State= state;
			PostalCode= postalCode;
		}
		public int Id { get; set; }
		public string StreetAddress { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string PostalCode { get; set; }
		public Person Person { get; set; }
		public int? Person_FK { get; set; }


	}
}
