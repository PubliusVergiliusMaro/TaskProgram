using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProgram.Database.Models;

namespace TaskProgram.Dekstop.ViewModels
{
	public class PersonViewModel : ViewModelBase
	{
		private readonly Person _person;
		public int Id => _person.Id;
		public string FirstName => _person.FirstName;
		public string LastName => _person.LastName;
		public string Gender => _person.Gender;
		public int Age => _person.Age;
		public string PhoneNumber => _person.PhoneNumber;
		public string StreetAddress => _person.Address.StreetAddress;
		public string City => _person.Address.City;
		public string State => _person.Address.State;
		public string PostalCode => _person.Address.PostalCode;
		public PersonViewModel(Person person)
		{
			_person = person;
		}
	}
}
