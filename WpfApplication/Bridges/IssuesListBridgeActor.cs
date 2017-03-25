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
	public class IssuesListBridgeActor : ReceiveActor
	{
		private readonly IIssuesList _issuesList;
		private readonly IActorRef _issuesActor;

		public IssuesListBridgeActor(IIssuesList issuesList, IActorRef issuesActor)
		{
			_issuesList = issuesList;
			_issuesActor = issuesActor;

			Receive<IssuesMessages.Created>(OnIssueCreated, null);
			_issuesActor.Tell(new IssuesMessages.Subscribe(true), Self);

			_issuesList.CreateIssueEvent += newTitle => _issuesActor.Tell(new IssuesMessages.Create(newTitle));
		}

		public static Props Create(IIssuesList issuesList, IActorRef issuesActor)
		{
			return Props.Create(() => new IssuesListBridgeActor(issuesList, issuesActor));
		}

		private void OnIssueCreated(IssuesMessages.Created e)
		{
			var issueViewModel = new IssueViewModel();
			var issueBridgeActor = Context.ActorOf(IssueBridgeActor.CreateActor(issueViewModel, e.IssueActor).WithDispatcher("akka.actor.synchronized-dispatcher"));

			_issuesList.AddIssue(issueViewModel);
		}
	}
}
