using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
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
		public ICommand EditPersonCommand { get; }
		public ICommand DeletePersonCommand { get; }
		public ICommand UpdateTableCommand { get; }


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
			EditPersonCommand = new DelegateCommand(GoToAddPersonPage);
			DeletePersonCommand = new DelegateCommand(DeletePerson, CanBeDeleted);
			UpdateTableCommand = new DelegateCommand(UpdateTable);

			
			List<Person> people = _personService.GetAllEF();
			if (people != null && people.Count != 0)
			{
				foreach (Person person in _personService.GetAllEF())
				{
					var personViewModel = new PersonViewModel(person);
					_people.Add(personViewModel);
				}
			}
		}
		private void UpdateTable()
		{
			_people.Clear();
			List<Person> people = _personService.GetAllEF();
			if (people != null && people.Count != 0)
			{
				foreach (Person person in _personService.GetAllEF())
				{
					var personViewModel = new PersonViewModel(person);
					_people.Add(personViewModel);
				}
			}
			MessageBox.Show("Table was updated.", "Updated",
				  MessageBoxButton.OK, MessageBoxImage.Information);
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
