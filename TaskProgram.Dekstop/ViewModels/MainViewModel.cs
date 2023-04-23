using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProgram.Dekstop.NavigationServices.Stores;
using TaskProgram.Services.PersonServices;

namespace TaskProgram.Dekstop.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		private readonly IPersonService _bookService;
		private readonly NavigationStore _navigationStore;
		public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

		public MainViewModel(NavigationStore navigationStore)
		{
			_navigationStore = navigationStore;

			_navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
		}

		private void OnCurrentViewModelChanged()
		{
			OnPropertyChanged(nameof(CurrentViewModel));
		}
	}
}
