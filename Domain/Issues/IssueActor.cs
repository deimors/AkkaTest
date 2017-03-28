using Akka.Actor;
using Akka.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AkkaTest.Issues
{
	public class IssueActor : ReceiveActor
	{
		private Guid _issueId;

		private string _title;
		private IList<IActorRef> _titleSubscribers = new List<IActorRef>();

		public IssueActor(Guid issueId)
		{
			_issueId = issueId;

			Receive<IssueMessages.SetTitle>(SetTitle, null);
			Receive<IssueMessages.SubscribeTitle>(SubscribeToTitle, null);
		}

		public static Props WithIssueId(Guid issueId)
		{
			return Props.Create(() => new IssueActor(issueId));
		}

		private void SetTitle(IssueMessages.SetTitle msg)
		{
			_title = msg.Title;

			foreach (var sub in _titleSubscribers)
				sub.Tell(_title, Self);
		}

		private void SubscribeToTitle(IssueMessages.SubscribeTitle msg)
		{
			if (msg.SendCurrent)
				Sender.Tell(new IssueMessages.TitleChanged(_title), Self);

			_titleSubscribers.Add(Sender);
		}
	}
}
