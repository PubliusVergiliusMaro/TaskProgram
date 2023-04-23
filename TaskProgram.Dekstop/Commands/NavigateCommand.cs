using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProgram.Dekstop.NavigationServices;

namespace TaskProgram.Dekstop.Commands
{
	public class NavigateCommand : CommandBase
	{
		private readonly INavigationService _booksListMavigationService;
		public NavigateCommand(INavigationService booksListMavigationService)
		{
			this._booksListMavigationService = booksListMavigationService;
		}

		public override void Execute(object? parameter)
		{
			_booksListMavigationService.Navigate();
		}
	}
}
