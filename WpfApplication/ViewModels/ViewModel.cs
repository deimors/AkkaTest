using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WpfApplication.ViewModels
{
	public abstract class ViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
	{
		private readonly IDictionary<string, IEnumerable<string>> _propertyValidationErrors = new Dictionary<string, IEnumerable<string>>();

		public bool HasErrors => _propertyValidationErrors.Any();

		protected void SetPropertyValidationError(string propertyName, string error)
		{
			if (string.IsNullOrEmpty(error))
			{
				_propertyValidationErrors.Remove(propertyName);
			}
			else
			{
				IEnumerable<string> propertyErrors;

				_propertyValidationErrors[propertyName] = _propertyValidationErrors.TryGetValue(propertyName, out propertyErrors)
					? propertyErrors.Concat(new[] { error })
					: new[] { error };
			}

			ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		protected void NotifyOfPropertyChanged(string propertyName)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		public IEnumerable GetErrors(string propertyName)
		{
			IEnumerable<string> errors;

			return _propertyValidationErrors.TryGetValue(propertyName, out errors)
				? errors
				: Enumerable.Empty<string>();
		}
	}
}
