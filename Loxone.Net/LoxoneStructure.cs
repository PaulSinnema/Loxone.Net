using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loxone.Net
{
	internal class LoxoneStructure : LoxoneMessage
	{

		public LoxoneStructure()
		{
			this.OperatingModes = new List<OperatingMode>();
			this.Rooms = new List<RoomData>();
			this.Cats = new List<CatData>();
			this.Controls = new List<ControlData>();
		}

		public DateTime? LastModified { get; set; }

		public MsInfo Info { get;  private set; }

		public List<OperatingMode> OperatingModes { get; private set; }

		public List<RoomData> Rooms { get; private set; }

		public List<CatData> Cats { get; private set; }

		public List<ControlData> Controls { get; private set; }

		public void Read(string data)
		{
			JsonSerializer serializer = new JsonSerializer();

			JsonTextReader reader = new JsonTextReader(new StringReader(data));
			reader.SupportMultipleContent = true;
			while (reader.Read()) {
				if (reader.TokenType == JsonToken.PropertyName) {
					string prp = (string)reader.Value;

					switch (prp) {
						case "lastModified":
							this.LastModified = reader.ReadAsDateTime();
							break;
						case "msInfo":
							reader.Read(); //skip startobject
							this.Info = serializer.Deserialize<MsInfo>(reader);
							break;
						case "operatingModes":
							reader.Read(); //skip startobject
							reader.Read(); //Read next 'property'
							while (reader.TokenType == JsonToken.PropertyName) {
								OperatingMode mode = new OperatingMode();
								mode.Id = int.Parse((string)reader.Value);
								mode.Name = reader.ReadAsString();
								this.OperatingModes.Add(mode);
							}
							break;
						case "rooms":
							reader.Read(); //skip startobject
							reader.Read(); //Read next 'property'
							while (reader.TokenType == JsonToken.PropertyName) {
								string uid = (string)reader.Value;
								reader.Read(); //startobject
								RoomData room = serializer.Deserialize<RoomData>(reader);
								room.Uid = uid;
								this.Rooms.Add(room);
								reader.Read(); //endobject
							}
							break;
						case "cats":
							reader.Read(); //skip startobject
							reader.Read(); //Read next 'property'
							while (reader.TokenType == JsonToken.PropertyName) {
								string uid = (string)reader.Value;
								reader.Read(); //startobject
								CatData cat = serializer.Deserialize<CatData>(reader);
								cat.Uid = uid;
								this.Cats.Add(cat);
								reader.Read(); //endobject
							}
							break;
						case "controls":
							reader.Read(); //skip startobject
							reader.Read(); //Read next 'property'
							while (reader.TokenType == JsonToken.PropertyName) {
								string uid = (string)reader.Value;
								reader.Read(); //startobject
								ControlData ctrl = serializer.Deserialize<ControlData>(reader);
								ctrl.Uid = uid;
								this.Controls.Add(ctrl);
								reader.Read(); //endobject
							}
							break;
					}


				}
			}
		}
	}

	public class MsInfo
	{
		public string serialNr { get; set; }
		public string msName { get; set; }
		public string projectName { get; set; }
		public string localUrl { get; set; }
		public string remoteUrl { get; set; }

		public byte tempUnit { get; set; }

		public string currency { get; set; }

		public string squareMeasure { get; set; }

		public string location { get; set; }
		public string languageCode { get; set; }
		public string catTitle { get; set; }
		public string roomTitle { get; set; }
		public byte miniserverType { get; set; }

		public bool sortByRating { get; set; }
	}

	public class OperatingMode
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class RoomData
	{
		public string Uid { get; set; }

		public string Name { get; set; }

		public string Image { get; set; }
	}


	public class CatData
	{
		public string Uid { get; set; }


		public string Name { get; set; }

		public string Image { get; set; }

		public string Color { get; set; }
	}

	public class ControlData
	{
		public string Uid { get; set; }

		public string Type { get; set; }

		public string Name { get; set; }

		public string Image { get; set; }

		public string uuidAction { get; set; }

		public string Room { get; set; }

		public string Cat { get; set; }

		public int DefaultRating { get; set; }

		public bool IsFavorite { get; set; }
		public bool IsSecure { get; set; }


		public ControlState States { get; set; }
	}


	public class ControlState
	{
		public string Active { get; set; }

		public string Value { get; set; }

		public string Error { get; set; }

		public string DeactivationDelay { get; set; }

		public string DeactivationDelayTotal { get; set; }
	}


}
