using Akka.Actor;
using AkkaTest.Actors;
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

			Receive<IssueCreatedEvent>(OnIssueCreated, null);
			_issuesActor.Tell(new SubscribeToIssuesMessage(true), Self);

			_issuesList.CreateIssueEvent += newTitle => _issuesActor.Tell(new CreateIssueMessage(newTitle));
		}

		public static Props Create(IIssuesList issuesList, IActorRef issuesActor)
		{
			return Props.Create(() => new IssuesListBridgeActor(issuesList, issuesActor));
		}

		private void OnIssueCreated(IssueCreatedEvent e)
		{
			var issueViewModel = new IssueViewModel();
			var issueBridgeActor = Context.ActorOf(IssueBridgeActor.CreateActor(issueViewModel, e.IssueActor).WithDispatcher("akka.actor.synchronized-dispatcher"));

			_issuesList.AddIssue(issueViewModel);
		}
	}
}
