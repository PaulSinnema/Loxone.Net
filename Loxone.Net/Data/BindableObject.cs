using System.Collections.Generic;
using System.ComponentModel;

namespace Loxone.Net.Data {
	public abstract class BindableObject : INotifyPropertyChanged
	{



		protected bool SetProperty<T>(ref T field, T value, string propertyName)
		{
			if (EqualityComparer<T>.Default.Equals(field, value)) return false;
			field = value;
			OnPropertyChanged(propertyName);
			return true;
		}


		protected void OnPropertyChanged(string propertyName = "")
		{
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;
	}
}
