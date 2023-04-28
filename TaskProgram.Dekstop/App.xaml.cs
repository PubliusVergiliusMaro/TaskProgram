using System.Windows;
using TaskProgram.Database;
using TaskProgram.Database.Models;
using TaskProgram.Database.Repository;
using TaskProgram.Dekstop.Services.NavigationServices;
using TaskProgram.Dekstop.Services.NavigationServices.Stores;
using TaskProgram.Dekstop.ViewModels;
using TaskProgram.Services.PersonServices;

namespace TaskProgram.Dekstop
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private readonly ApplicationContext dbcontext;
		private readonly IGenericRepository<Person> personRepository;
		private readonly IPersonService personService;
		private readonly NavigationStore navigationStore;
		public App()
		{
			dbcontext = new ApplicationContext();
			personRepository = new GenericRepository<Person>(dbcontext);
			personService = new PersonService(personRepository);
			navigationStore = new NavigationStore();
		}
		protected override void OnStartup(StartupEventArgs e)
		{
			navigationStore.CurrentViewModel = CreateBookListViewModel();

			MainWindow = new MainWindow()
			{
				DataContext = new MainViewModel(navigationStore)
			};
			MainWindow.Show();
			base.OnStartup(e);
		}
		public AddPersonViewModel CreateAddBookViewModel()
		{
			return new AddPersonViewModel(personService, new NavigationService(navigationStore, CreateBookListViewModel));
		}

		private PersonListViewModel CreateBookListViewModel()
		{
			return new PersonListViewModel(personService, new NavigationService(navigationStore, CreateAddBookViewModel));
		}
	}
}
