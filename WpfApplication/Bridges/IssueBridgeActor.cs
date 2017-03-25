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
	public class IssueBridgeActor : ReceiveActor
	{
		private readonly IIssue _issue;
		private readonly IActorRef _issueActor;

		public IssueBridgeActor(IIssue issue, IActorRef issueActor)
		{
			_issue = issue;
			_issueActor = issueActor;

			Receive<IssueMessages.TitleChanged>(msg => OnTitleSet(msg));
			_issueActor.Tell(new IssueMessages.SubscribeTitle(true), Self);
		}

		public static Props CreateActor(IIssue issue, IActorRef issueActor)
		{
			return Props.Create(() => new IssueBridgeActor(issue, issueActor));
		}

		private void OnTitleSet(IssueMessages.TitleChanged e)
		{
			_issue.Title = e.Title;
		}
	}
}
