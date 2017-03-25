using System;
using System.Collections.ObjectModel;

namespace WpfApplication.ViewModels
{
	public class IssuesListViewModel : ViewModel, IIssuesList
	{
		public ObservableCollection<IssueViewModel> Issues { get; } = new ObservableCollection<IssueViewModel>();

		private string _newIssueTitle;
		public string NewIssueTitle
		{
			get
			{
				return _newIssueTitle;
			}

			set
			{
				if (value == _newIssueTitle) return;

				CreateIssueError = null;
				_newIssueTitle = value;
				NotifyOfPropertyChanged(nameof(NewIssueTitle));
			}
		}

		private string _createIssueError;
		public string CreateIssueError
		{
			get
			{
				return _createIssueError;
			}

			set
			{
				if (value == _createIssueError) return;

				_createIssueError = value;
				NotifyOfPropertyChanged(nameof(CreateIssueError));
			}
		}

		public event Action<string> CreateIssueEvent;

		public void AddIssue(IssueViewModel issue)
			=> Issues.Add(issue);

		public void CreateIssue()
			=> CreateIssueEvent?.Invoke(NewIssueTitle);
	}
}
