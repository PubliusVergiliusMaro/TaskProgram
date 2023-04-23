using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TaskProgram.Database.Repository;
using TaskProgram.Database;
using TaskProgram.Dekstop.NavigationServices.Stores;
using TaskProgram.Dekstop.ViewModels;
using TaskProgram.Database.Models;
using TaskProgram.Services.PersonServices;
using TaskProgram.Dekstop.NavigationServices;

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
