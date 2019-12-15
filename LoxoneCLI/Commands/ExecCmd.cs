using CommandLine;
using Loxone.Net;
using Loxone.Net.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxoneCLI.Commands {


	[Verb("exec", HelpText = "Executes a command on the selected Loxone Control")]
	public class ExecCmd {

		[Value(0, HelpText = "Command to execute")]
		public string Cmd { get; set; }


		[Option('t', HelpText = "Get control based on the Type")]
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
			if (ctrls.Count() > 1) {
				Console.WriteLine("Multiple controls selected");
				return -1;
			}

			Control ctrl = ctrls.First();

			bool res = await ctrl.SendCmd(this.Cmd);
			if (!res) {
				Console.WriteLine($"Command '{this.Cmd}' failed");
			} else {
				Console.WriteLine($"Command '{this.Cmd}' send");
			}

			return 1;

		}

	}

}
