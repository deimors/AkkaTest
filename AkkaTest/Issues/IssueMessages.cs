using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaTest.Issues
{
	public static class IssueMessages
	{
		public class SetTitle
		{
			public readonly string Title;

			public SetTitle(string title)
			{
				Title = title;
			}
		}

		public class SubscribeTitle
		{
			public readonly bool SendCurrent;

			public SubscribeTitle(bool sendCurrent)
			{
				SendCurrent = sendCurrent;
			}
		}

		public class TitleChanged
		{
			public readonly string Title;

			public TitleChanged(string title)
			{
				Title = title;
			}
		}

	}
}
