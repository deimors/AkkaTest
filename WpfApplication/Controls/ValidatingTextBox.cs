using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication.Controls
{
	public class ValidatingTextBox : TextBox
	{
		public static DependencyProperty ValidationMessageProperty = DependencyProperty.Register(nameof(ValidationMessage), typeof(string), typeof(ValidatingTextBox));

		public string ValidationMessage
		{
			get { return (string)GetValue(ValidationMessageProperty); }
			set { SetValue(ValidationMessageProperty, value); }
		}
	}
}
