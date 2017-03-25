using Akka.Actor;
using AkkaTest.Issues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication.ViewModels;

namespace WpfApplication.Bridges
{
	public static class IssuesListBridgeMessages
	{
		public class Create
		{
			public readonly string NewTitle;

			public Create(string newTitle)
			{
				NewTitle = newTitle;
			}
		}
	}

	public class IssuesListBridgeActor : ReceiveActor
	{
		private readonly IIssuesList _issuesList;
		private readonly IActorRef _issuesActor;

		public IssuesListBridgeActor(IIssuesList issuesList, IActorRef issuesActor)
		{
			_issuesList = issuesList;
			_issuesActor = issuesActor;

			Receive<IssuesListBridgeMessages.Create>(OnCreateIssue, null);
			Receive<IssuesMessages.Created>(OnIssueCreated, null);
			Receive<IssuesMessages.CreateFailed>(OnIssueCreateFailed, null);

			_issuesActor.Tell(new IssuesMessages.Subscribe(true), Self);

			var self = Self;
			_issuesList.CreateIssueEvent += newTitle => self.Tell(new IssuesListBridgeMessages.Create(newTitle)); 
		}

		public static Props Create(IIssuesList issuesList, IActorRef issuesActor)
		{
			return Props.Create(() => new IssuesListBridgeActor(issuesList, issuesActor));
		}

		private void OnCreateIssue(IssuesListBridgeMessages.Create msg)
		{
			_issuesActor.Tell(new IssuesMessages.Create(msg.NewTitle), Self);
		}

		private void OnIssueCreated(IssuesMessages.Created msg)
		{
			var issueViewModel = new IssueViewModel();
			var issueBridgeActor = Context.ActorOf(IssueBridgeActor.CreateActor(issueViewModel, msg.IssueActor).WithDispatcher("akka.actor.synchronized-dispatcher"));

			_issuesList.AddIssue(issueViewModel);
			_issuesList.NewIssueTitle = string.Empty;
		}

		private void OnIssueCreateFailed(IssuesMessages.CreateFailed msg)
		{
			_issuesList.CreateIssueError = msg.Reason;
		}
	}
}
