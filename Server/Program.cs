using Akka.Actor;
using Akka.Persistence.Sqlite;
using AkkaTest.Issues;
using System;

namespace ConsoleApplication1
{
	class Program
	{
		static void Main(string[] args)
		{
			var system = ActorSystem.Create("ComicsServer");

			SqlitePersistence.Get(system);

			var issues = system.ActorOf<IssuesActor>("issues");

			Console.WriteLine($"Created {issues.Path}");
			
			Console.ReadLine();
		}
	}
}
