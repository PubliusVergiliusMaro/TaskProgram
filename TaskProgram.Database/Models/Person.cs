using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskProgram.Database.Models
{
	public class Person
	{
		public Person() { }
		public Person(string firstName, string lastName, string gender, int age, string phoneNumber, Address address)
		{

			FirstName = firstName;
			LastName = lastName;
			Gender = gender;
			Age = age;
			PhoneNumber = phoneNumber;
			Address = address;
		}

		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Gender { get; set; }
		public int Age { get; set; }
		public string PhoneNumber { get; set; }
		public Address Address { get; set; }

		
	}
}
