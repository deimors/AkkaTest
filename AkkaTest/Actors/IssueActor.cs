using Akka.Actor;
using Akka.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AkkaTest.Actors
{
	public class SetTitleMessage
	{
		public readonly string Title;

		public SetTitleMessage(string title)
		{
			Title = title;
		}
	}

	public class SubscribeToTitleMessage
	{
		public readonly bool SendCurrent;

		public SubscribeToTitleMessage(bool sendCurrent)
		{
			SendCurrent = sendCurrent;
		}
	}

	public class TitleSetEvent
	{
		public readonly string Title;

		public TitleSetEvent(string title)
		{
			Title = title;
		}
	}

	public class IssueActor : ReceiveActor
	{
		private Guid _issueId;

		private string _title;
		private IList<IActorRef> _titleSubscribers = new List<IActorRef>();

		public IssueActor(Guid issueId)
		{
			_issueId = issueId;

			Receive<SetTitleMessage>(SetTitle, null);
			Receive<SubscribeToTitleMessage>(SubscribeToTitle, null);
		}

		public static Props WithIssueId(Guid issueId)
		{
			return Props.Create(() => new IssueActor(issueId));
		}

		private void SetTitle(SetTitleMessage msg)
		{
			_title = msg.Title;

			foreach (var sub in _titleSubscribers)
				sub.Tell(_title, Self);
		}

		private void SubscribeToTitle(SubscribeToTitleMessage msg)
		{
			if (msg.SendCurrent)
				Sender.Tell(new TitleSetEvent(_title), Self);

			_titleSubscribers.Add(Sender);
		}
	}
}
