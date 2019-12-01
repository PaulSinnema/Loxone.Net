using Loxone.Api.Data.LoxApp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loxone.Net.Data
{
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
	}
}
