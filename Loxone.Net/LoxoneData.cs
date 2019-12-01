using Loxone.Api.Data.LoxApp;
using Loxone.Net.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loxone.Net
{
	public class LoxoneData
	{
		private readonly LoxoneClient _client;

		internal LoxoneData(LoxoneClient client)
		{
			_client = client;

			this.LastModified = default(DateTime);
			this.Rooms = new List<Room>();
			this.Categories = new List<Category>();
			this.Controls = new List<Control>();
		}

		public DateTime LastModified { get; set; }

		public List<Room> Rooms { get; private set; }
		public List<Category> Categories { get; private set; }
		public List<Control> Controls { get; private set; }


		internal void Update(LoxApp3Data file) {
			this.LastModified = file.LastModified;


			this.Rooms = file.Rooms.Values.Select(r => new Room { Data = r }).ToList();
			this.Categories = file.Categories.Values.Select(c => new Category { Data = c }).ToList();

			this.Controls = new List<Control>();
			foreach(LoxoneControl data in file.Controls.Values) {


				Control ctrl = this.Controls.FirstOrDefault(c => c.Uid.Equals(data.UuidAction));
				if (ctrl == null) {
					switch (data.Type) {
						case "Pushbutton":
							ctrl = new Data.Controls.PushButton(_client);
							break;
						case "Switch":
							ctrl = new Data.Controls.Switch(_client);
							break;
						case "TimedSwitch":
							ctrl = new Data.Controls.TimedSwitch(_client);
							break;
						case "LightController":
							ctrl = new Data.Controls.LightController(_client);
							break;
						case "LightControllerV2":
							ctrl = new Data.Controls.LightControllerV2(_client);
							break;
						case "IRoomControllerV2":
							ctrl = new Data.Controls.IRoomControllerV2(_client);
							break;
						default:
							Console.WriteLine($"Unknown control : {data.Type} ");
							ctrl = new Control(_client);
							break;
					}
					this.Controls.Add(ctrl);
				}
				ctrl.Data = data;
				ctrl.Room = this.Rooms.FirstOrDefault(r => r.Uid.Equals(data.RoomUuid));
				if ((ctrl.Room != null) && (!ctrl.Room.Controls.Contains(ctrl))) {
					ctrl.Room.Controls.Add(ctrl);
				}
				ctrl.Category = this.Categories.FirstOrDefault(r => r.Equals(data.CategoryUuid));
				if ((ctrl.Category != null) && (!ctrl.Category.Controls.Contains(ctrl))) {
					ctrl.Category.Controls.Add(ctrl);
				}
			}
		}

	
	}
}
