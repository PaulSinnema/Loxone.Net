using Loxone.Net;
using Loxone.Net.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxoneTest
{
	public static class LoxoneExtensions
	{

		public static IEnumerable<Control> GetAllControlsOfType(this LoxoneClient client, string controlType)
		{
			return client.Data.Controls.Where(c => c.GetType().Name.Equals(controlType, StringComparison.OrdinalIgnoreCase));
		}

		public static Control GetControlByName(this LoxoneClient client, string name, string controlType = "")
		{
			foreach(Control ctrl in client.Data.Controls.Where(c => c.Name.Replace(" ", "").Equals(name, StringComparison.OrdinalIgnoreCase))) {
				if ((string.IsNullOrEmpty(controlType)) || (ctrl.GetType().Name.Equals(controlType, StringComparison.OrdinalIgnoreCase))) {
					return ctrl;
				}
			}
			return null;
		}

	}
}
