using Loxone.Api.Data.LoxApp;
using System.Collections.ObjectModel;

namespace Loxone.Net.Data {
	public class Category : BindableObject
	{

		public Category()	{
			this.Controls = new ObservableCollection<Control>();
		}

		private LoxoneCategory _data;

		internal LoxoneCategory Data {
			get { return _data; }
			set {
				_data = value;
				this.OnPropertyChanged();
			}
		}


		public string Uid => _data?.Uuid;
		public string Name => _data?.Name;

		public ObservableCollection<Control> Controls { get; private set; }

	}
}
