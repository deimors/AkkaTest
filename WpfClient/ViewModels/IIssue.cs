using System;

namespace WpfApplication.ViewModels
{
	public interface IIssue
	{
		string Title { get; set; }

		void Delete();
		event Action Deleted;
		event Action DeleteClicked;
	}
}
