using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoxoneCLI.Commands {

	[Verb("help", Hidden = true)]
	public class HelpCmd {

		[Value(0, Required=false, HelpText="Command to show help on", MetaName = "Name" )]
		public string Name { get; set; }


		public void Run() {
			HelpText txt = new HelpText();
			txt.AddDashesToOption = true;
			txt.AutoHelp = false;
			txt.AutoVersion = false;

			if (string.IsNullOrEmpty(this.Name)) {
				txt.Heading = "Loxone CLI - List of possible Verbs";
				txt.AddDashesToOption = false;				
				txt = txt.AddVerbs(typeof(ListCmd), typeof(GetCmd));
			} else if (this.Name.Equals("list", StringComparison.OrdinalIgnoreCase)) {
				txt.Heading = "Loxone CLI - Options for 'list'";
				txt = this.GetHelp<ListCmd>(txt);
			} else if (this.Name.Equals("get", StringComparison.OrdinalIgnoreCase)) {
				txt.Heading = "Loxone CLI - Options for 'get'";
				txt = this.GetHelp<GetCmd>(txt);
			} else if (this.Name.Equals("exec", StringComparison.OrdinalIgnoreCase)) {
				txt.Heading = "Loxone CLI - Options for 'exec'";
				txt = this.GetHelp<ExecCmd>(txt);
			}
			Console.WriteLine(txt);
		}

		private HelpText GetHelp<T>(HelpText txt) where T : class {
			var result = Parser.Default.ParseArguments<T>(new string[] { });
			return txt.AddOptions<T>(result);
		}		

	}
}
