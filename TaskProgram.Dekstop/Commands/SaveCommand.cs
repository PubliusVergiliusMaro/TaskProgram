using System;
using System.ComponentModel;
using System.Windows;
using TaskProgram.Database.Models;
using TaskProgram.Dekstop.NavigationServices;
using TaskProgram.Dekstop.ViewModels;
using TaskProgram.Services.PersonServices;

namespace TaskProgram.Dekstop.Commands
{
	public class SaveCommand : CommandBase
	{
		private readonly AddPersonViewModel _addPersonViewModel;
		private readonly IPersonService _personService;
		private readonly INavigationService _peopleListMavigationService;

		public SaveCommand(AddPersonViewModel addPersonViewModel, IPersonService personService,
			INavigationService peopleListMavigationService)
		{
			_addPersonViewModel = addPersonViewModel;
			_personService = personService;
			_peopleListMavigationService = peopleListMavigationService;
			_addPersonViewModel.PropertyChanged += OnViewModelPropertyChanged;
		}
		public override bool CanExecute(object parameter)
		{
			return !string.IsNullOrEmpty(_addPersonViewModel.FirstName)
				&& base.CanExecute(parameter);
			// перевирки
		}
		public override void Execute(object? parameter)
		{
			var book = new Person(_addPersonViewModel.FirstName, _addPersonViewModel.LastName, _addPersonViewModel.Gender, _addPersonViewModel.Age, _addPersonViewModel.PhoneNumber,
				new Address(_addPersonViewModel.StreetAddress, _addPersonViewModel.City, _addPersonViewModel.State, _addPersonViewModel.PostalCode));
			try
			{
				_personService.Create(book);
				MessageBox.Show("Successfully created book.", "Success",
				  MessageBoxButton.OK, MessageBoxImage.Information);

				_peopleListMavigationService.Navigate();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error");
			}
		}

		private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(AddPersonViewModel.FirstName)
				|| e.PropertyName == nameof(AddPersonViewModel.LastName))
			{
				OnCanExecutedChanged();
			}
		}
	}
}
