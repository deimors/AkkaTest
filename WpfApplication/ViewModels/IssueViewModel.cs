using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication.ViewModels
{
	public class IssueViewModel : ViewModel, IIssue
	{
		private string _title;

		public string Title
		{
			get
			{
				return _title;
			}

			set
			{
				if (_title == value) return;

				_title = value;
				NotifyOfPropertyChanged(nameof(Title));
			}
		}
	}
}
