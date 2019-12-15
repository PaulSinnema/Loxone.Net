using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Loxone.Net;
using Loxone.Net.Data;

using CommandLine;
using CommandLine.Text;

using LoxoneCLI.Commands;


namespace LoxoneCLI {
	class Program {

		private static LoxoneClient _client;
		static async Task Main(string[] args) {

			StartupOptions options = null;

			Parser mainParser = new Parser((s) => {
				s.AutoHelp = true;
				s.HelpWriter = Console.Out;
			});
			mainParser.ParseArguments<StartupOptions>(args)
					.WithParsed<StartupOptions>(o => options = o)
					.WithNotParsed<StartupOptions>(e => e.Output());


			if (options == null) options = new StartupOptions();
			if (string.IsNullOrEmpty(options.Server)) {
				Console.Write("Loxone Miniserver IpAddress or serialnr : ");
				options.Server = Console.ReadLine();
			}
			if (string.IsNullOrEmpty(options.User)) {
				Console.Write("Username : ");
				options.User = Console.ReadLine();
			}
			if (string.IsNullOrEmpty(options.Password)) {
				Console.Write("Password : ");
				options.Password = Console.ReadLine();
			}


			string serverIp = null;
			if (options.Server.Contains(".")) { //quick and dirty hack to check if it is an ip-address
				serverIp = options.Server;
			} else {
				serverIp = LoxoneClient.GetIP(options.Server);
			}


			Parser cmdParser = new Parser((s) => {
				s.AutoHelp = false;
				s.HelpWriter = null;
			});

			using (_client = new LoxoneClient(serverIp)) {
				bool open = await _client.Open(options.User, options.Password);

				while (true) {
					Console.Write("Enter command : ");
					string cmd = Console.ReadLine();
					if (string.IsNullOrEmpty(cmd)) continue;
					if (cmd.Equals("exit")) break;

					//string[] cmds = cmd.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
					//if (cmd.Length == 0) continue;

					IEnumerable<string> cmds = Tools.Split(cmd);
					var result = cmdParser.ParseArguments<HelpCmd, ListCmd, GetCmd>(cmds);

					//var helpText = HelpText.AutoBuild(result, h =>
					//{
					//	//configure HelpText
					//	h.AdditionalNewLineAfterOption = false; //remove newline between options
					//	h.Heading = "Loxone CLI v0.0.1 beta"; //change header
					//	h.Copyright = "Copyright (c) 2019 FVDL Consulting"; //change copyright text
					//																								 // more options ...
					//	return h;
					//}, e => e);

					//result.WithNotParsed((errs) => {
					//	Console.WriteLine(helpText);
					//});

					result.WithNotParsed(errs => PrintErros(errs))
							.WithParsed<HelpCmd>(c => c.Run())
							.WithParsed<ListCmd>(c => c.Run(_client))
							.WithParsed<GetCmd>(c => c.Run(_client).Wait());

					//result.MapResult(
					//		(ListCmd c) => c.Run(_client),
					//		(GetCmd c) => c.Run(_client),
					//		errs => {
					//			foreach (var err in errs) {
					//				Console.WriteLine(err);
					//			}
					//			return 0;
					//		}
					//	);


					//if (cmds.Length == 1) {
					//	if (cmds[0].Equals("list")) {
					//		foreach (Control ctrl in client.Data.Controls) {
					//			Console.WriteLine($" - {ctrl.GetType().Name} : {ctrl.Name} ");
					//		}
					//	} else if (cmds[0].Equals("getstates")) {
					//		client.RefreshStates();
					//	} else {
					//		foreach (Control ctrl in client.GetAllControlsOfType(cmds[0])) {
					//			Console.WriteLine($" - {ctrl.Name} ");
					//		}
					//	}
					//} else if (cmds.Length == 2) {
					//	if (cmds[0].Equals("cmd")) {
					//		//client.Send(cmds[1]);
					//	} else {
					//		Control ctrl = client.GetControlByName(cmds[1], cmds[0]);
					//		if (ctrl == null) {
					//			Console.WriteLine(" - Not found");
					//		} else {
					//			await ctrl.Query();

					//			Console.WriteLine($"  Name : {ctrl.Name}");
					//			Console.WriteLine($"  Room : {ctrl.Room}");
					//			Console.WriteLine($"  Uid  : {ctrl.Uid}");
					//			Console.WriteLine($"  States");
					//			foreach ((string key, object val) state in ctrl.GetStates()) {
					//				Console.WriteLine($"    {state.key} = {state.val}");
					//			}
					//		}
					//	}
					//} else if (cmds.Length == 3) {
					//	Control ctrl = client.GetControlByName(cmds[1], cmds[0]);
					//	if (ctrl == null) {
					//		Console.WriteLine(" - Not found");
					//	} else {
					//		await ctrl.SendCmd(cmds[2]);
					//	}
					//}




				}

				_client.Close();
			}
		}
		private static void PrintErros(IEnumerable<Error> errs) {
			foreach (var err in errs) {
				Console.WriteLine(err);
			}
		}
	}
}
