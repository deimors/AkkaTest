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

		public class Initialize
		{
			public readonly IActorRef IssuesActor;

			public Initialize(IActorRef issuesActor)
			{
				IssuesActor = issuesActor;
			}
		}
	}

	public class IssuesListBridgeActor : ReceiveActor
	{
		private readonly IIssuesList _issuesList;
		private IActorRef _issuesActor;

		public IssuesListBridgeActor(IIssuesList issuesList)
		{
			_issuesList = issuesList;

			Receive<IssuesListBridgeMessages.Initialize>(OnInitialize, null);

			Become(Uninitialized);
		}

		public static Props Create(IIssuesList issuesList)
		{
			return Props.Create(() => new IssuesListBridgeActor(issuesList));
		}

		private void Uninitialized()
		{
			Receive<IssuesListBridgeMessages.Initialize>(OnInitialize, null);
		}

		private void Initialized()
		{
			Receive<IssuesListBridgeMessages.Create>(OnCreateIssue, null);
			Receive<IssuesMessages.Created>(OnIssueCreated, null);
			Receive<IssuesMessages.CreateFailed>(OnIssueCreateFailed, null);
		}

		private void OnInitialize(IssuesListBridgeMessages.Initialize msg)
		{
			_issuesActor = msg.IssuesActor;
				
			_issuesActor.Tell(new IssuesMessages.Subscribe(true), Self);

			var self = Self;
			_issuesList.CreateIssueEvent += newTitle => self.Tell(new IssuesListBridgeMessages.Create(newTitle));

			Become(Initialized);
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
