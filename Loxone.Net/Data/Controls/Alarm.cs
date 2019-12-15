using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loxone.Net.Data.Controls {


	public class Alarm : Control {

		public enum AlarmLevel : int {
			None = 0,
			Silent = 1,
			Acustic = 2,
			Optical = 3,
			Internal = 4,
			External = 5,
			Remote = 6,
		}


		internal protected Alarm(LoxoneClient client) : base(client) {
		}


		#region States
		private bool _isArmed = false;
		/// <summary>
		/// If the alarm is armed
		/// </summary>
		public bool IsArmed {
			get { return _isArmed; }
			internal set {
				this.SetProperty<bool>(ref _isArmed, value, nameof(IsArmed));
			}
		}

		private bool _isMotionDisabled = false;

		/// <summary>
		/// Are the motion sensors disabled
		/// </summary>
		public bool IsMotionDisabled {
			get { return _isMotionDisabled;  }
			internal set {
				this.SetProperty<bool>(ref _isMotionDisabled, value, nameof(IsMotionDisabled));
			}
		}

		private int _armedDelay = 0;
		/// <summary>
		/// Delay in seconds before the alarm is armed (Countdown)
		/// </summary>
		public int ArmedDelay {
			get { return _armedDelay; }
			internal set {
				this.SetProperty<int>(ref _armedDelay, value, nameof(ArmedDelay));
			}
		}

		private int _armedDelayTotal = 0;
		/// <summary>
		/// Total dealy in seconds befor the alarm is armed
		/// </summary>
		public int ArmedDelayTotal {
			get { return _armedDelayTotal;  }
			internal set {
				this.SetProperty<int>(ref _armedDelayTotal, value, nameof(ArmedDelayTotal));
			}
		}


		private AlarmLevel _level = AlarmLevel.None;
		/// <summary>
		/// The current alarm Level
		/// </summary>
		public AlarmLevel Level {
			get { return _level; }
			internal set {
				this.SetProperty<AlarmLevel>(ref _level, value, nameof(Level));
			}
		}

		private AlarmLevel _nextLevel = AlarmLevel.None;

		/// <summary>
		/// The next alarm level
		/// </summary>
		public AlarmLevel NextLevel {
			get { return _nextLevel; }
			internal set {
				this.SetProperty<AlarmLevel>(ref _nextLevel, value, nameof(NextLevel));
			}
		}

		private int _nextLevelDelay = 0;
		/// <summary>
		/// The delay of the next level in seconds, this can be specified with the parameters D1 - D6 in Loxone Config.This increments every second.
		/// </summary>
		public int NextLevelDelay {
			get { return _nextLevelDelay; }
			internal set {
				this.SetProperty<int>(ref _nextLevelDelay, value, nameof(NextLevelDelay));
			}
		}

		private int _nextLevelDelayTotal = 0;
		public int NextLevelDelayTotal {
			get { return _nextLevelDelayTotal; }
			internal set {
				this.SetProperty<int>(ref _nextLevelDelayTotal, value, nameof(this.NextLevelDelayTotal));
			}
		}

		private string[] _sensors = null;
		public string[] Sensors {
			get { return _sensors; }
			internal set {
				this.SetProperty<string[]>(ref _sensors, value, nameof(Sensors));
			}
		}


		private DateTime _startTime;

		/// <summary>
		/// Timestamp when the alarm started
		/// </summary>
		public DateTime StartTime {
			get { return _startTime;  }
			internal set {
				this.SetProperty<DateTime>(ref _startTime, value, nameof(StartTime));
			}
		}

		#endregion

		#region Commands

		/// <summary>
		/// Arms the AlarmControl
		/// </summary>
		/// <param name="withMovement">Enabled motion detection</param>
		/// <param name="delayed">Arms the alarm control with the given delay ('Da' parameter)</param>
		/// <returns></returns>
		public Task<bool> TurnOn(bool withMovement = true, bool delayed = true) {
			string cmd = (delayed ? "delayedon" : "on") + (withMovement ? "/0" : "/1");
			return base.SendCmd(cmd);
		}


		/// <summary>
		/// Disarms the AlarmControl
		/// </summary>
		/// <returns></returns>
		public Task<bool> TurnOff() {
			return this.SendCmd("off");
		}

		/// <summary>
		/// Acknowledge the alarm
		/// </summary>
		/// <returns></returns>
		public Task<bool> Quit() {
			return this.SendCmd("quit");
		}

		#endregion

		protected override void OnStateChanged(string name, double value) {
			base.OnStateChanged(name, value);

			if (name.Equals("level", StringComparison.OrdinalIgnoreCase)) {
				this.Level = (AlarmLevel)value;
			} else if (name.Equals("nextLevel", StringComparison.OrdinalIgnoreCase)) {
				this.NextLevel = (AlarmLevel)value;
			} else if (name.Equals("armed", StringComparison.OrdinalIgnoreCase)) {
				this.IsArmed = (value != 0);
			} else if (name.Equals("armedDelay", StringComparison.OrdinalIgnoreCase)) {
				this.ArmedDelay = (int)value;
			} else if (name.Equals("armedDelayTotal", StringComparison.OrdinalIgnoreCase)) {
				this.ArmedDelayTotal = (int)value;
			} else if (name.Equals("NextLevelDelay", StringComparison.OrdinalIgnoreCase)) {
				this.NextLevelDelay = (int)value;
			} else if (name.Equals("NextLevelDelayTotal", StringComparison.OrdinalIgnoreCase)) {
				this.NextLevelDelayTotal = (int)value;
			} else if (name.Equals("disableMove", StringComparison.OrdinalIgnoreCase)) {
				this.IsMotionDisabled = (value != 0);
			}
		}
		protected override void OnStateChanged(string name, string value) {
			base.OnStateChanged(name, value);

			if (name.Equals("Sensors", StringComparison.OrdinalIgnoreCase)) {
				this.Sensors = value.Split('|', StringSplitOptions.RemoveEmptyEntries);
			} else if (name.Equals("StartTime", StringComparison.OrdinalIgnoreCase)) {
				DateTime dt;
				if (DateTime.TryParse(value, out dt)) {
					this.StartTime = dt;
				}				
			}
		}

	}
}
