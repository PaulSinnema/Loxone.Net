using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loxone.Net.Data.Controls {
	public class TimedSwitch : Control {
		internal protected TimedSwitch(LoxoneClient client) : base(client) {

		}


		private int _totalDelay = 0;
		public int TotalDelay {
			get { return _totalDelay; }
			set {
				this.SetProperty<int>(ref _totalDelay, value, nameof(TotalDelay));
			}
		}

		private int _delay = 0;

		public int Delay {
			get { return _delay; }
			set {
				this.SetProperty<int>(ref _delay, value, nameof(Delay));
				if (_delay == 0) {
					if (_isActive) this.IsActive = false;
				} else {
					if (!_isActive) this.IsActive = true;
				}
			}
		}



		private bool _isActive = false;

		public bool IsActive {
			get { return _isActive; }
			internal set {
				this.SetProperty<bool>(ref _isActive, value, nameof(IsActive));
			}
		}

		/// <summary>
		/// A short push/pulse of the button
		/// </summary>
		/// <returns></returns>
		public Task<bool> Pulse() {
			return this.SendCmd("pulse");
		}

		/// <summary>
		/// Activates the switch
		/// </summary>
		/// <returns></returns>
		public Task<bool> On() {
			return this.SendCmd("on");
		}

		/// <summary>
		/// Deactivates the switch
		/// </summary>
		/// <returns></returns>
		public Task<bool> Off() {
			return this.SendCmd("off");
		}


		protected override void OnStateChanged(string name, double value) {
			base.OnStateChanged(name, value);

			if (name.Equals("deactivationDelay", StringComparison.OrdinalIgnoreCase)) {
				this.Delay = (int)value;
			} else if (name.Equals("deactivationDelayTotal", StringComparison.OrdinalIgnoreCase)) {
				this.TotalDelay = (int)value;
			}

		}

	}
}
