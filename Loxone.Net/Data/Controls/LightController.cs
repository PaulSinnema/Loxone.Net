using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Loxone.Net.Data.Controls {
	public class LightController : Control {

		internal protected LightController(LoxoneClient client) : base(client) {

		}

		private bool _isActive = false;

		public bool IsActive {
			get { return _isActive; }
			internal set {
				this.SetProperty<bool>(ref _isActive, value, nameof(IsActive));
			}
		}

		private int _scene = 0;
		public int Scene {
			get { return _scene;  }
			set {
				this.SetProperty<int>(ref _scene, value, nameof(Scene));
			}
		}

		/// <summary>
		/// A short push/pulse of the button
		/// </summary>
		/// <returns></returns>
		public Task<bool> Toggle() {
			if (this.IsActive) {
				return this.Off();
			} else {
				return this.On();
			}
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


		/// <summary>
		/// Activates the scene with the given number
		/// </summary>
		/// <param name="sceneNr"></param>
		/// <returns></returns>
		public Task<bool> ActivateScene(int sceneNr) {
			return this.SendCmd(sceneNr.ToString());
		}


		/// <summary>
		/// Changes to the next scene
		/// </summary>
		/// <returns></returns>
		public Task<bool> Plus() {
			return this.SendCmd("plus");
		}


		/// <summary>
		/// Changes to the previous scene
		/// </summary>
		/// <returns></returns>
		public Task<bool> Minus() {
			return this.SendCmd("minus");
		}


		protected override void OnStateChanged(string name, double value) {
			base.OnStateChanged(name, value);

			if (name.Equals("active", StringComparison.OrdinalIgnoreCase)) {
				this.IsActive = (value != 0);
			} else if (name.Equals("activeScene", StringComparison.OrdinalIgnoreCase)) {
				this.Scene = (int)value;
				if (this.Scene == 0) {
					this.IsActive = false;
				} else if (this.Scene == 9) {
					this.IsActive = true;
				}
			}

		}

	}
}
