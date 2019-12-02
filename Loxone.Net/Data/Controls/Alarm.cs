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

		private bool _isArmed = false;
		public bool IsArmed {
			get { return _isArmed; }
			internal set {
				this.SetProperty<bool>(ref _isArmed, value, nameof(IsArmed));
			}
		}

		private AlarmLevel _level = AlarmLevel.None;
		public AlarmLevel Level {
			get { return _level; }
			internal set {
				this.SetProperty<AlarmLevel>(ref _level, value, nameof(Level));
			}
		}



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
		public Task<bool> Confirm() {
			return this.SendCmd("quit");
		}


	}
}
