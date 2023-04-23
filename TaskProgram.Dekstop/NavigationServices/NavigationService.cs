using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProgram.Dekstop.NavigationServices.Stores;
using TaskProgram.Dekstop.ViewModels;

namespace TaskProgram.Dekstop.NavigationServices
{
	public class NavigationService : INavigationService
	{
		private readonly NavigationStore _navigateStore;
		private readonly Func<ViewModelBase> createViewModel;

		public NavigationService(NavigationStore navigateStore, Func<ViewModelBase> createViewModel)
		{
			_navigateStore = navigateStore;
			this.createViewModel = createViewModel;
		}

		public void Navigate()
		{
			_navigateStore.CurrentViewModel = createViewModel();
		}
	}
}
