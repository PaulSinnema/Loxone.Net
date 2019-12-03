using CommandLine;
using Loxone.Net;
using Loxone.Net.Data;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoxoneTest {
	class Program {

		private static LoxoneClient _client;
		static async Task Main(string[] args) {

			StartupOptions options = null;
			Parser.Default.ParseArguments<StartupOptions>(args)
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


			using (_client = new LoxoneClient(serverIp)) {
				bool open = await _client.Open(options.User, options.Password);

				while (true) {
					Console.Write("Enter command : ");
					string cmd = Console.ReadLine();
					if (string.IsNullOrEmpty(cmd)) continue;
					if (cmd.Equals("exit")) break;

					//string[] cmds = cmd.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
					//if (cmd.Length == 0) continue;

					Parser.Default.ParseArguments<ListCmd>(Tools.Split(cmd)).MapResult(
							(ListCmd c) => RunCmd(c),
							errs => {
								foreach(var err in errs) {
									Console.WriteLine(err);
								}
								return 0;
							}
						);
						

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
		private static int RunCmd(ListCmd cmd) {
			IEnumerable<Control> ctrls = _client.Data.Controls;

			if (!string.IsNullOrEmpty(cmd.Type)) {
				ctrls = ctrls.Where(c => c.GetType().Name.Equals(cmd.Type, StringComparison.OrdinalIgnoreCase));
			}
			if (!string.IsNullOrEmpty(cmd.Name)) {
				ctrls = ctrls.Where(c => c.Name.Equals(cmd.Name, StringComparison.OrdinalIgnoreCase));
			}

			foreach (Control ctrl in ctrls) {
				Console.WriteLine($" - {ctrl.GetType().Name} : {ctrl.Name} ");
			}
			return 1;
		}
	}
}
