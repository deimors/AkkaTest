using Akka.Actor;
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
			public readonly string Title;

			public Create(string title)
			{
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

		public class Created
		{
			public readonly IActorRef IssueActor;

			public Created(IActorRef issue)
			{
				IssueActor = issue;
			}
		}
	}
}
