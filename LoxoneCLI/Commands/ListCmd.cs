using CommandLine;
using Loxone.Net;
using Loxone.Net.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoxoneCLI.Commands {


	[Verb("list", HelpText = "List (and filter) all visibile LoxoneControls")]
	public class ListCmd  {


		[Option('t', HelpText = "List all control based on the Type")]
		public string Type { get; set; }

		[Option('n', HelpText = "List all control based on the Name")]
		public string Name { get; set; }


		public int Run(LoxoneClient client) {
			IEnumerable<Control> ctrls = client.Data.Controls;

			if (!string.IsNullOrEmpty(this.Type)) {
				ctrls = ctrls.Where(c => c.GetType().Name.Equals(this.Type, StringComparison.OrdinalIgnoreCase));
			}
			if (!string.IsNullOrEmpty(this.Name)) {
				ctrls = ctrls.Where(c => c.Name.Equals(this.Name, StringComparison.OrdinalIgnoreCase));
			}

			foreach (Control ctrl in ctrls) {
				Console.WriteLine($" - {ctrl.GetType().Name} : {ctrl.Name} ");
			}
			return 1;

		}

	}

}
