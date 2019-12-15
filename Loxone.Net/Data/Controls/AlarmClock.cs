using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Loxone.Net.Data.Controls {

	public class AlarmClock : Control {


		public class AlarmClockEntry {
			public int Id { get; set; }

			public string Name { get; set; }

			public bool IsActive { get; set; }

			public bool Daily { get; set; }
			public bool NightLight { get; set; }

			public TimeSpan Time { get; set; }
			public string[] Modes { get; set; }
		}


		internal protected AlarmClock(LoxoneClient client) : base(client) {
		}

		private bool _isEnabled = false;
		public bool IsEnabled {
			get { return _isEnabled; }
			internal set {
				this.SetProperty<bool>(ref _isEnabled, value, nameof(IsEnabled));
			}
		}


		private bool _isAlarmActive = false;
		public bool IsAlarmActive {
			get { return _isAlarmActive; }
			internal set {
				this.SetProperty<bool>(ref _isAlarmActive, value, nameof(IsAlarmActive));
			}
		}


		private bool _confirmationNeeded = false;
		public bool ConfirmationNeeded {
			get { return _confirmationNeeded; }
			internal set {
				this.SetProperty<bool>(ref _confirmationNeeded, value, nameof(ConfirmationNeeded));
			}
		}

		private List<AlarmClockEntry> _entryList = new List<AlarmClockEntry>();

		public IEnumerable<AlarmClockEntry> EntryList {
			get { return _entryList; }
		}

		private int _currentEntry = 0;

		public AlarmClockEntry CurrentEntry {
			get {
				if (_currentEntry <= 0) return null;
				return _entryList.FirstOrDefault(e => e.Id == _currentEntry);
			}
		}

		private int _nextEntry = 0;

		public AlarmClockEntry NextEntry {
			get {
				if (_nextEntry <= 0) return null;
				return _entryList.FirstOrDefault(e => e.Id == _nextEntry);
			}
		}

		/// <summary>
		/// Activates the Alarm clock
		/// </summary>
		/// <returns></returns>
		public Task<bool> TurnOn() {
			return base.SendCmd("setActive/1");
		}
		/// <summary>
		/// Deactivates the Alarm clock
		/// </summary>
		/// <returns></returns>
		public Task<bool> TurnOff() {
			return base.SendCmd("setActive/0");
		}


		/// <summary>
		/// Snooze the active alarm
		/// </summary>
		/// <returns></returns>
		public Task<bool> Snooze() {
			return base.SendCmd("snooze");
		}

		/// <summary>
		/// dismiss the active alarm
		/// </summary>
		/// <returns></returns>
		public Task<bool> Dismiss() {
			return base.SendCmd("dismiss");
		}

		protected override void OnStateChanged(string name, double value) {
			base.OnStateChanged(name, value);

			if (name.Equals(nameof(IsEnabled), StringComparison.OrdinalIgnoreCase)) {
				this.IsEnabled = (value != 0);
			} else if (name.Equals(nameof(IsAlarmActive), StringComparison.OrdinalIgnoreCase)) {
				this.IsAlarmActive = (value != 0);
			} else if (name.Equals(nameof(ConfirmationNeeded), StringComparison.OrdinalIgnoreCase)) {
				this.ConfirmationNeeded = (value != 0);
			} else if (name.Equals(nameof(NextEntry), StringComparison.OrdinalIgnoreCase)) {
				_nextEntry = (int)value;
				this.OnPropertyChanged(nameof(NextEntry));
			} else if (name.Equals(nameof(CurrentEntry), StringComparison.OrdinalIgnoreCase)) {
				_currentEntry = (int)value;
				this.OnPropertyChanged(nameof(CurrentEntry));
			}
		}
		protected override void OnStateChanged(string name, string value) {
			base.OnStateChanged(name, value);

			if (name.Equals(nameof(EntryList), StringComparison.OrdinalIgnoreCase)) {
				Dictionary<int, AlarmClockEntryListItem> items = JsonConvert.DeserializeObject<Dictionary<int, AlarmClockEntryListItem>>(value);

				foreach(int i in items.Keys) { 
					AlarmClockEntryListItem item = items[i];
					AlarmClockEntry entry = this.EntryList.FirstOrDefault(e => e.Id == i);
					if (entry == null) {
						if (item != null) {
							entry = new AlarmClockEntry();
							entry.Id = i;
							_entryList.Add(entry);
						} else {
							_entryList.Remove(entry);
							entry = null;
						}
					}
					if ((entry != null) && (item != null)) {
						entry.Name = item.name;
						entry.IsActive = item.isActive;
						entry.Daily = item.daily;
						entry.NightLight = item.nightLight;
						entry.Time = TimeSpan.FromSeconds(item.alarmTime);
						entry.Modes = new string[item.modes.Length];
						for(int d = 0; d < item.modes.Length; d ++) {
							entry.Modes[d] = _client.Data.OperatingModes[item.modes[d]];
						}
					}
				}
			}

		}
	}

	
	internal class AlarmClockEntryListItem {
		public string name { get; set; }
		public bool isActive { get; set; }
		public int alarmTime { get; set; }
		public bool nightLight { get; set; }
		public bool daily { get; set; }
		public int[] modes { get; set; }
	}

}
