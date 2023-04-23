using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using TaskProgram.Database.Models;
using TaskProgram.Dekstop.Commands;
using TaskProgram.Dekstop.NavigationServices;
using TaskProgram.Services.PersonServices;

namespace TaskProgram.Dekstop.ViewModels
{
	public class AddPersonViewModel : ViewModelBase
	{
		private readonly INavigationService _personListMavigationService;
		public AddPersonViewModel(IPersonService personService,
			INavigationService personListMavigationService)
		{
			_personListMavigationService = personListMavigationService;
			SaveCommand = new SaveCommand(this, personService, _personListMavigationService);
			CancelCommand = new NavigateCommand(_personListMavigationService);
		}
		public ICommand SaveCommand { get; }
		public ICommand CancelCommand { get; }

		private string firstName;
		public string FirstName
		{
			get { return firstName; }
			set
			{
				firstName = value;
				OnPropertyChanged(nameof(FirstName));
			}
		}
		private string lastName;
		public string LastName
		{
			get { return firstName; }
			set
			{
				lastName = value;
				OnPropertyChanged(nameof(LastName));
			}
		}
		private string gender;
		public string Gender
		{
			get { return gender; }
			set
			{
				gender = value;
				OnPropertyChanged(nameof(Gender));
			}
		}
		private int age;
		public int Age
		{
			get { return age; }
			set
			{
				age = value;
				OnPropertyChanged(nameof(Age));
			}
		}
		private string phoneNumber;
		public string PhoneNumber
		{
			get { return phoneNumber; }
			set
			{
				phoneNumber = value;
				OnPropertyChanged(nameof(PhoneNumber));
			}
		}
		private string streetAddress;
		public string StreetAddress
		{
			get { return streetAddress; }
			set
			{
				streetAddress = value;
				OnPropertyChanged(nameof(StreetAddress));
			}
		}
		private string city;
		public string City
		{
			get { return city; }
			set
			{
				city = value;
				OnPropertyChanged(nameof(City));
			}
		}
		private string state;
		public string State
		{
			get { return state; }
			set
			{
				state = value;
				OnPropertyChanged(nameof(State));
			}
		}
		private string postalCode;
		public string PostalCode
		{
			get { return postalCode; }
			set
			{
				postalCode = value;
				OnPropertyChanged(nameof(PostalCode));
			}
		}

		public Action CloseAction { get; set; }
	}
}
