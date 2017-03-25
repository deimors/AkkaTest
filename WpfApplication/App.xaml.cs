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
		private void ApplicationStartup(object sender, StartupEventArgs e)
		{
			var system = ActorSystem.Create("comics");

			var issues = system.ActorOf<IssuesActor>("issues");

			issues.Tell(new IssuesMessages.Create("title 1"));
			issues.Tell(new IssuesMessages.Create("title 2"));
			issues.Tell(new IssuesMessages.Create("title 3"));

			var issuesViewModel = new IssuesListViewModel();
			var issuesBridgeActor = system.ActorOf(IssuesListBridgeActor.Create(issuesViewModel, issues).WithDispatcher("akka.actor.synchronized-dispatcher"));

			var mainWindow = new MainWindow();
			mainWindow.DataContext = issuesViewModel;

			mainWindow.Show();
		}
	}
}
