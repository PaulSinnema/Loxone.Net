﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loxone.Net.Data.Controls
{
	public class Switch : Control
	{
		internal protected Switch(LoxoneClient client) : base(client)
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
		public Task<bool> Toggle()
		{
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
		public Task<bool> On()
		{
			return this.SendCmd("on");
		}

		/// <summary>
		/// Deactivates the switch
		/// </summary>
		/// <returns></returns>
		public Task<bool> Off()
		{
			return this.SendCmd("off");
		}


		protected override void OnStateChanged(string name, double value)
		{
			base.OnStateChanged(name, value);

			if (name.Equals("active", StringComparison.OrdinalIgnoreCase)) {
				this.IsActive = (value != 0);
			}

		}

	}
}
