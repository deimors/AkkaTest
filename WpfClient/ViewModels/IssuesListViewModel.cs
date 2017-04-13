using System;
using System.Collections.ObjectModel;

namespace WpfApplication.ViewModels
{
	public class IssuesListViewModel : ViewModel, IIssuesList
	{
		public ObservableCollection<IIssue> Issues { get; } = new ObservableCollection<IIssue>();

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
		
		public string CreateIssueError
		{
			set
			{
				SetPropertyValidationError(nameof(NewIssueTitle), value);
			}
		}

		public event Action<string> CreateIssueEvent;

		public void AddIssue(IIssue issue)
		{
			Issues.Add(issue);
			issue.Deleted += () => OnIssueDeleted(issue);
		}

		private void OnIssueDeleted(IIssue issue)
		{
			Issues.Remove(issue);
			issue.Deleted -= () => OnIssueDeleted(issue);
		}

		public void CreateIssue()
			=> CreateIssueEvent?.Invoke(NewIssueTitle);
	}
}
