using Akka.Actor;
using Akka.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaTest.Actors
{
	public class CreateIssueMessage
	{
		public readonly string Title;

		public CreateIssueMessage(string title)
		{
			Title = title;
		}
	}

	public class GetIssuesMessage
	{

	}

	public class SubscribeToIssuesMessage
	{
		public readonly bool SendCurrentIssues;

		public SubscribeToIssuesMessage(bool sendCurrentIssues)
		{
			SendCurrentIssues = sendCurrentIssues;
		}
	}

	public class IssueCreatedEvent
	{
		public readonly IActorRef IssueActor;

		public IssueCreatedEvent(IActorRef issue)
		{
			IssueActor = issue;
		}
	}

	public class IssuesActor : ReceiveActor
	{
		private readonly IList<IActorRef> _createSubscribers = new List<IActorRef>();

		public IssuesActor()
		{
			Receive<CreateIssueMessage>(msg => CreateIssue(msg));
			Receive<GetIssuesMessage>(msg => GetIssues());
			Receive<SubscribeToIssuesMessage>(msg => SubscribeToIssues(msg));
		}

		private void CreateIssue(CreateIssueMessage msg)
		{
			var newIssueId = Guid.NewGuid();
			var actorRef = Context.ActorOf(IssueActor.WithIssueId(newIssueId), newIssueId.ToString());

			actorRef.Tell(new SetTitleMessage(msg.Title), Self);

			var createdEvent = new IssueCreatedEvent(actorRef);

			foreach (var sub in _createSubscribers)
			{
				sub.Tell(createdEvent, Self);
			}
		}

		private void GetIssues()
		{
			foreach (var child in Context.GetChildren())
			{
				Sender.Tell(new IssueCreatedEvent(child), Self);
			}
		}

		private void SubscribeToIssues(SubscribeToIssuesMessage msg)
		{
			_createSubscribers.Add(Sender);

			if (msg.SendCurrentIssues)
			{
				GetIssues();
			}
		}
	}
}
