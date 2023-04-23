using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	public class PersonListViewModel : ViewModelBase
	{
		private readonly ObservableCollection<PersonViewModel> _people;
		private readonly IPersonService _personService;
		public IEnumerable<PersonViewModel> People => _people;
		public ICommand AddPersonCommand { get; }
		public PersonListViewModel(IPersonService personService, INavigationService addBookNavigationService)
		{
			_personService = personService;
			_people = new ObservableCollection<PersonViewModel>();
			AddPersonCommand = new NavigateCommand(addBookNavigationService);
			
			//List<Person> people = _personService.GetAllGenericRepos();
			//_personService.Serialize(people);

			foreach (Person person in _personService.GetAll())
			{
				_people.Add(new PersonViewModel(person));
			}
		}
	}
}
