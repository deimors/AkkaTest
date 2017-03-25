using System;

namespace WpfApplication.ViewModels
{
	public interface IIssuesList
	{
		void AddIssue(IssueViewModel issue);
		string NewIssueTitle { get; set; }
		string CreateIssueError { get; set; }
		event Action<string> CreateIssueEvent;
	}
}