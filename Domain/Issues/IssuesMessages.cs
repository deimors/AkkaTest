﻿using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaTest.Issues
{
	public static class IssuesMessages
	{
		public class Create
		{
			public readonly Guid IssueId;
			public readonly string Title;

			public Create(Guid issueId, string title)
			{
				IssueId = issueId;
				Title = title;
			}
		}

		public class GetAll
		{

		}

		public class Subscribe
		{
			public readonly bool SendCurrentIssues;

			public Subscribe(bool sendCurrentIssues)
			{
				SendCurrentIssues = sendCurrentIssues;
			}
		}

		public class Unsubscribe
		{

		}

		public class Created
		{
			public readonly IActorRef IssueActor;

			public Created(IActorRef issue)
			{
				IssueActor = issue;
			}
		}

		public class CreateFailed
		{
			public readonly string Reason;

			public CreateFailed(string reason)
			{
				Reason = reason;
			}
		}
	}
}
