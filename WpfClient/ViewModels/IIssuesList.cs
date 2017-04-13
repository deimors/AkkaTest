using System;

namespace WpfApplication.ViewModels
{
	public interface IIssuesList
	{
		void AddIssue(IIssue issue);
		string NewIssueTitle { get; set; }
		string CreateIssueError { set; }
		event Action<string> CreateIssueEvent;
	}
}