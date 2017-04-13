using Akka.Actor;
using Akka.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaTest.Issues
{
	public class IssuesActor : ReceiveActor
	{
		private readonly IList<IActorRef> _createSubscribers = new List<IActorRef>();

		public IssuesActor()
		{
			Receive<IssuesMessages.Create>(msg => CreateIssue(msg));
			Receive<IssuesMessages.GetAll>(msg => GetIssues());
			Receive<IssuesMessages.Subscribe>(msg => SubscribeToIssues(msg));
		}

		private void CreateIssue(IssuesMessages.Create msg)
		{
			if (string.IsNullOrEmpty(msg.Title))
			{
				Sender.Tell(new IssuesMessages.CreateFailed($"{nameof(msg.Title)} can't be empty"), Self);
				return;
			}

			var newIssueId = Guid.NewGuid();
			var actorRef = Context.ActorOf(IssueActor.WithIssueId(newIssueId), newIssueId.ToString());

			actorRef.Tell(new IssueMessages.SetTitle(msg.Title), Self);

			var createdEvent = new IssuesMessages.Created(actorRef);

			foreach (var sub in _createSubscribers)
			{
				sub.Tell(createdEvent, Self);
			}
		}

		private void GetIssues()
		{
			foreach (var child in Context.GetChildren())
			{
				Sender.Tell(new IssuesMessages.Created(child), Self);
			}
		}

		private void SubscribeToIssues(IssuesMessages.Subscribe msg)
		{
			_createSubscribers.Add(Sender);

			if (msg.SendCurrentIssues)
			{
				GetIssues();
			}
		}
	}
}
