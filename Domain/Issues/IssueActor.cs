using Akka.Actor;
using Akka.Persistence;
using System;
using System.Collections.Generic;

namespace AkkaTest.Issues
{
	public class IssueActor : ReceivePersistentActor
	{
		private Guid _issueId;

		private string _title;

		private IList<IActorRef> _titleSubscribers = new List<IActorRef>();
		private IList<IActorRef> _deletedSubscribers = new List<IActorRef>();

		public override string PersistenceId => $"issue-{_issueId.ToString()}";

		public IssueActor(Guid issueId)
		{
			_issueId = issueId;

			Recover<IssueMessages.SetTitle>(msg => SetTitle(msg));
			Command<IssueMessages.SetTitle>(msg => Persist(msg, SetTitle));

			Command<IssueMessages.SubscribeTitle>(msg => SubscribeToTitle(msg));

			Recover<IssueMessages.Delete>(msg => Delete(msg));
			Command<IssueMessages.Delete>(msg => Persist(msg, Delete));

			Command<IssueMessages.SubscribeDeleted>(msg => SubscribeToDelete(msg));
		}
		
		public static Props WithIssueId(Guid issueId)
		{
			return Props.Create(() => new IssueActor(issueId));
		}

		private void SetTitle(IssueMessages.SetTitle msg)
		{
			_title = msg.Title;

			foreach (var sub in _titleSubscribers)
				sub.Tell(new IssueMessages.TitleChanged(_title), Self);
		}

		private void SubscribeToTitle(IssueMessages.SubscribeTitle msg)
		{
			if (msg.SendCurrent)
				Sender.Tell(new IssueMessages.TitleChanged(_title), Self);

			_titleSubscribers.Add(Sender);
		}

		private void Delete(IssueMessages.Delete msg)
		{
			Context.Stop(Self);
		}

		private void SubscribeToDelete(IssueMessages.SubscribeDeleted msg)
		{
			_deletedSubscribers.Add(Sender);
		}

		protected override void PostStop()
		{
			foreach (var sub in _deletedSubscribers)
				sub.Tell(new IssueMessages.Deleted(), Self);
		}
	}
}
