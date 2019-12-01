using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Loxone.Net.Data.Controls {
	public class LightControllerV2 : Control {

		internal protected LightControllerV2(LoxoneClient client) : base(client) {

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
		/// Changes to the next mood
		/// </summary>
		/// <returns></returns>
		public Task<bool> Plus() {
			return this.SendCmd("plus");
		}


		/// <summary>
		/// Changes to the previous mood
		/// </summary>
		/// <returns></returns>
		public Task<bool> Minus() {
			return this.SendCmd("minus");
		}


		protected override void OnStateChanged(string name, double value) {
			base.OnStateChanged(name, value);


			

		}
		protected override void OnStateChanged(string name, string value) {
			base.OnStateChanged(name, value);

			if (name.Equals("activeMoods", StringComparison.OrdinalIgnoreCase)) {

			} else if (name.Equals("moodList", StringComparison.OrdinalIgnoreCase))  {
			}


		}

	}
}
