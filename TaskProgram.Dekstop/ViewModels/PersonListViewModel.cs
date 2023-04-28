using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
		private static ObservableCollection<PersonViewModel> _people;
		private static IPersonService _personService;
		private static INavigationService _addBookNavigationService;
		public static IEnumerable<PersonViewModel> People => _people;
		public ICommand AddPersonCommand { get; }
		public ICommand DeletePersonCommand { get; }

		
		private PersonViewModel selectedPerson;
		public PersonViewModel SelectedPerson
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
			AddPersonCommand = new DelegateCommand(GoToAddPersonPage);
			DeletePersonCommand = new DelegateCommand(DeletePerson, CanBeDeleted);
			List<Person> people = _personService.GetAllEF();
			if (people != null && people.Count !=0)
			{
				foreach (Person person in _personService.GetAllEF())
				{
					var personViewModel = new PersonViewModel(person);
					_people.Add(personViewModel);
				}
			}

		}
		private void UpdatePerson()
		{
			//var person = new Person(FirstName, LastName, Gender, Age, PhoneNumber,
			//	new Address(StreetAddress, City, State, PostalCode));
			//var person = new Person(SelectedPerson.FirstName, SelectedPerson.LastName, SelectedPerson.Gender, int.Parse(SelectedPerson.Age), SelectedPerson.PhoneNumber,
			//	new Address(SelectedPerson.StreetAddress, SelectedPerson.City, SelectedPerson.State, SelectedPerson.PostalCode));
			
			//_personService.UpdateEF(person);

		}
		private bool CanBeDeleted()
		{
			return SelectedPerson != null;
		}

		private void DeletePerson()
		{
			_personService.DeleteEF(SelectedPerson.Id);
			_people.Remove(SelectedPerson);
		}

		private void GoToAddPersonPage()
		{
			_addBookNavigationService.Navigate();
		}
	}
}
