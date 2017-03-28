using Akka.Actor;
using AkkaTest.Issues;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfApplication.Bridges;
using WpfApplication.ViewModels;

namespace WpfApplication
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private ActorSystem _system;
		private async void ApplicationStartup(object sender, StartupEventArgs e)
		{
			_system = ActorSystem.Create("ComicsClient");
			/*
			var issues = system.ActorOf<IssuesActor>("issues");

			issues.Tell(new IssuesMessages.Create("title 1"));
			issues.Tell(new IssuesMessages.Create("title 2"));
			issues.Tell(new IssuesMessages.Create("title 3"));
			*/

			var issuesViewModel = new IssuesListViewModel();
			var issuesBridgeActor = _system.ActorOf(IssuesListBridgeActor.Create(issuesViewModel).WithDispatcher("akka.actor.synchronized-dispatcher"));

			var mainWindow = new MainWindow();
			mainWindow.DataContext = issuesViewModel;

			mainWindow.Show();

			var issuesSelection = _system.ActorSelection("akka.tcp://ComicsServer@localhost:8080/user/issues");

			var issues = await issuesSelection.ResolveOne(TimeSpan.FromSeconds(10));

			issuesBridgeActor.Tell(new IssuesListBridgeMessages.Initialize(issues));
		}

		private void ApplicationExit(object sender, ExitEventArgs e)
		{
			_system.Dispose();
		}
	}
}
