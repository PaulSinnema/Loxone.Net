using Loxone.Api.Data.LoxApp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loxone.Net.Data {
	public class Control : BindableObject {

		protected readonly LoxoneClient _client;
		protected readonly Dictionary<string, object> _stateValues = new Dictionary<string, object>();
		internal protected Control(LoxoneClient client) {
			_client = client;
		}

		private LoxoneControl _data;

		internal LoxoneControl Data {
			get { return _data; }
			set {
				_data = value;
				this.OnPropertyChanged();
			}
		}

		public string Uid => _data?.UuidAction;
		public string Name => _data?.Name;

		private Room _room;

		[JsonIgnore]
		public Room Room {
			get { return _room; }
			set {
				this.SetProperty<Room>(ref _room, value, nameof(Room));
			}
		}

		private Category _category;

		[JsonIgnore]
		public Category Category {
			get { return _category; }
			set {
				this.SetProperty<Category>(ref _category, value, nameof(Category));
			}
		}


		public async Task<bool> SendCmd(string cmd) {
			if (_client == null) return false;
			if (string.IsNullOrEmpty(this.Uid)) return false;

			string msg = await _client.WriteValue(this.Uid, cmd);
			if (string.IsNullOrEmpty(msg)) return false;

			return true;
		}

		public async Task<bool> Query() {
			if (_client == null) return false;
			if (string.IsNullOrEmpty(this.Uid)) return false;

			string msg = await _client.ReadValue(this.Uid);
			if (string.IsNullOrEmpty(msg)) return false;

			return true;
		}




		internal bool HasState(string uid) {
			if (this.Data == null) return false;
			if (this.Data.States == null) return false;

			if (this.Data.States.ContainsValue(uid)) return true;


			//if (uid.Equals(this.Data.States.Active)) return true;
			//if (uid.Equals(this.Data.States.Value)) return true;
			//if (uid.Equals(this.Data.States.Error)) return true;

			return false;
		}

		internal void SetState(string uid, double value) {
			string nm = this.GetStateName(uid);
			if (_stateValues.ContainsKey(nm)) {
				_stateValues[nm] = value;
			} else {
				_stateValues.Add(nm, value);
			}
			this.OnStateChanged(nm, value);
		}
		internal void SetState(string uid, string value) {
			string nm = this.GetStateName(uid);
			if (_stateValues.ContainsKey(nm)) {
				_stateValues[nm] = value;
			} else {
				_stateValues.Add(nm, value);
			}
			this.OnStateChanged(this.GetStateName(uid), value);
		}
		private string GetStateName(string uid) {
			foreach (string nm in this.Data.States.Keys) {
				if (this.Data.States[nm].Equals(uid)) {
					return nm;
				}
			}

			if (uid.Equals(this.Uid)) return "control";
			return string.Empty;
		}
		public object GetState(string name) {
			if (!_stateValues.ContainsKey(name)) return null;
			return _stateValues[name];
		}
		public IEnumerable<(string, object)> GetStates() {
			foreach(string key in _stateValues.Keys) {
				yield return (key, _stateValues[key]);
			}
		}

		protected virtual void OnStateChanged(string name, double value) {
			//Console.WriteLine($"{this.Name}.{name} = {value}");
		}
		protected virtual void OnStateChanged(string name, string value) {
			//Console.WriteLine($"{this.Name}.{name} = {value}");
		}


	}


}
