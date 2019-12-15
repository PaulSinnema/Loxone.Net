using Loxone.Api.Data.LoxApp;
using System.Collections.Generic;

namespace Loxone.Net.Data {
	public class Room : BindableObject
	{

		public Room() {
			this.Controls = new List<Control>();
		}

		private LoxoneRoom _data;
		internal LoxoneRoom Data {
			get { return _data; }
			set {
				_data = value;
				base.OnPropertyChanged();
			}
		}

		public string Uid => _data?.Uuid;
		public string Name => _data?.Name;

		public List<Control> Controls { get; private set; }

		public override string ToString() {
			return this.Name ?? base.ToString();
		}
	}
}
