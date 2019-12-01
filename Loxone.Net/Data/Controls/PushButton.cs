using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loxone.Net.Data.Controls
{
	public class PushButton : Control
	{
		internal protected PushButton(LoxoneClient client) : base(client)
		{

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
		public Task<bool> Pulse()
		{
			return this.SendCmd("pulse");
		}

		/// <summary>
		/// Push and hold the button
		/// </summary>
		/// <returns></returns>
		public Task<bool> On()
		{
			return this.SendCmd("on");
		}

		/// <summary>
		/// Release the button after a hold (on)
		/// </summary>
		/// <returns></returns>
		public Task<bool> Off()
		{
			return this.SendCmd("off");
		}

		protected override void OnStateChanged(string name, double value) {
			base.OnStateChanged(name, value);

			if (name.Equals("active", StringComparison.OrdinalIgnoreCase)) {
				this.IsActive = (value != 0);
			}
		}
	}
}
