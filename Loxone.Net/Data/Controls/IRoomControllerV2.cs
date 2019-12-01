using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Loxone.Net.Data.Controls {
	public class IRoomControllerV2 : Control {

		internal protected IRoomControllerV2(LoxoneClient client) : base(client) {

		}


		private int _activeMode;

		public int ActiveMode {
			get { return _activeMode; }
			set {
				this.SetProperty<int>(ref _activeMode, value, nameof(ActiveMode));
			}
		}

		private int _operatingMode;
		public int OperatingMode {
			get { return _operatingMode;  }
			set {
				this.SetProperty<int>(ref _operatingMode, value, nameof(OperatingMode));
			}
		}


		private double _actualTemp;
		public double ActualTemp {
			get { return _actualTemp; }
			set {
				this.SetProperty<double>(ref _actualTemp, value, nameof(ActualTemp));
			}
		}

		private double _targetTemp;

		public double TargetTemp {
			get { return _targetTemp; }
			set {
				this.SetProperty<double>(ref _targetTemp, value, nameof(TargetTemp));
			}
		}

		protected override void OnStateChanged(string name, double value) {
			base.OnStateChanged(name, value);


			if (name.Equals("ActiveMode", StringComparison.OrdinalIgnoreCase)) {
				this.ActiveMode = (int)value;
			} else if (name.Equals("OperatingMode", StringComparison.OrdinalIgnoreCase)) {
				this.OperatingMode = (int)value;
			} else if (name.Equals("tempActual", StringComparison.OrdinalIgnoreCase)) {
				this.ActualTemp = value;
			} else if (name.Equals("tempTarget", StringComparison.OrdinalIgnoreCase)) {
				this.TargetTemp = value;
			}


		}
		protected override void OnStateChanged(string name, string value) {
			base.OnStateChanged(name, value);



		}

	}
}
