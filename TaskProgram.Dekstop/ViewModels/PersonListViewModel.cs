using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using TaskProgram.Database.Models;
using TaskProgram.Dekstop.Commands;
using TaskProgram.Dekstop.Services.NavigationServices;
using TaskProgram.Services.PersonServices;

namespace TaskProgram.Dekstop.ViewModels
{
    public class PersonListViewModel : ViewModelBase
	{
		private readonly ObservableCollection<PersonViewModel> _people;
		private readonly IPersonService _personService;
		private readonly INavigationService _addBookNavigationService;
		public IEnumerable<PersonViewModel> People => _people;
		public ICommand AddPersonCommand { get; }

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
			get { return lastName; }
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

		private Person selectedPerson;
		public Person SelectedPerson
		{
			get { return selectedPerson; }
			set
			{
				selectedPerson = value;
				OnPropertyChanged(nameof(SelectedPerson));
			}
		}

		public PersonListViewModel(IPersonService personService, INavigationService addBookNavigationService)
		{
			_addBookNavigationService = addBookNavigationService;
			_personService = personService;
			_people = new ObservableCollection<PersonViewModel>();
			AddPersonCommand = new DelegateCommand(GoToAddPersonPage);//new NavigateCommand(addBookNavigationService);

			List<Person> people = _personService.GetAllEF();
			if (people != null && people.Count !=0)
			{
				foreach (Person person in _personService.GetAllEF())
				{
					var personViewModel = new PersonViewModel(person);
					personViewModel.PropertyChanged += PersonViewModel_PropertyChanged;
					_people.Add(personViewModel);
				}
			}
		}

		private void GoToAddPersonPage()
		{
			_addBookNavigationService.Navigate();
		}
		private void PersonViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			
		}
	}
}
