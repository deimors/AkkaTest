using Akka.Actor;
using AkkaTest.Issues;
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
			var system = ActorSystem.Create("ComicsServer");

			var issues = system.ActorOf<IssuesActor>("issues");

			Console.WriteLine($"Created {issues.Path}");

			issues.Tell(new IssuesMessages.Create("title 1"));
			issues.Tell(new IssuesMessages.Create("title 2"));
			issues.Tell(new IssuesMessages.Create("title 3"));

			Console.ReadLine();
		}
	}
}
