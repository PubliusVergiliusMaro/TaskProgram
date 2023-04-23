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

			_personService.Serialize(_personService.GetAll());

			foreach (Person person in _personService.Deserialize())
			{
				_people.Add(new PersonViewModel(person));

			}
			//_people.Add(new PersonViewModel(new Person()
			//{
			//	FirstName = "Oleg",
			//	LastName = "Redko",
			//	Age = 18,
			//	Gender = "Male",
			//	PhoneNumber = "34315",
			//	Address = new Address()
			//	{
			//		City = "Rivne",
			//		PostalCode = "32141",
			//		State = "rv",
			//		StreetAddress = "Mlunivska"
			//	}
			//}));
		}
	}
}
