using CommandLine;
using Loxone.Net;
using Loxone.Net.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxoneCLI.Commands {


	[Verb("get", HelpText="Get the status of the selected LoxoneControl")]
	public class GetCmd  {


		[Option('t',  HelpText = "Get control based on the Type")]
		public string Type { get; set; }

		[Option('n', HelpText = "Get control based on the Name")]
		public string Name { get; set; }


		public async Task<int> Run(LoxoneClient client) {
			IEnumerable<Control> ctrls = client.Data.Controls;

			if (!string.IsNullOrEmpty(this.Type)) {
				ctrls = ctrls.Where(c => c.GetType().Name.Equals(this.Type, StringComparison.OrdinalIgnoreCase));
			}
			if (!string.IsNullOrEmpty(this.Name)) {
				ctrls = ctrls.Where(c => c.Name.Equals(this.Name, StringComparison.OrdinalIgnoreCase));
			}
			if (!ctrls.Any()) {
				Console.WriteLine("Not Found");
				return -1;
			}


			foreach (Control ctrl in ctrls) {
				await ctrl.Query();

				Console.WriteLine($"**************************************************");
				Console.WriteLine($"  Name : {ctrl.Name}");
				Console.WriteLine($"  Room : {ctrl.Room}");
				Console.WriteLine($"  Uid  : {ctrl.Uid}");
				Console.WriteLine($"  States");
				foreach ((string key, object val) state in ctrl.GetStates()) {
					Console.WriteLine($"    {state.key} = {state.val}");
				}
				Console.WriteLine($"**************************************************");
			}
			return ctrls.Count(); ;

		}

	}

}
