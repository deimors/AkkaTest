using Akka.Actor;
using AkkaTest.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
	class Program
	{
		static void Main(string[] args)
		{
			var system = ActorSystem.Create("comics");

			var issues = system.ActorOf<IssuesActor>("issues");

			issues.Tell(new CreateIssueMessage("title"));

			Console.ReadLine();
		}
	}
}
