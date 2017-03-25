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
	public class IssueBridgeActor : ReceiveActor
	{
		private readonly IIssue _issue;
		private readonly IActorRef _issueActor;

		public IssueBridgeActor(IIssue issue, IActorRef issueActor)
		{
			_issue = issue;
			_issueActor = issueActor;

			Receive<TitleSetEvent>(msg => OnTitleSet(msg));
			_issueActor.Tell(new SubscribeToTitleMessage(true), Self);
		}

		public static Props CreateActor(IIssue issue, IActorRef issueActor)
		{
			return Props.Create(() => new IssueBridgeActor(issue, issueActor));
		}

		private void OnTitleSet(TitleSetEvent e)
		{
			_issue.Title = e.Title;
		}
	}
}
