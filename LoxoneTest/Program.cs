using Loxone.Net;
using Loxone.Net.Data;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace LoxoneTest {
	class Program {
		static async Task Main(string[] args) {



			Console.Write("Loxone Miniserver IpAddress or serialnr : ");
			string serialnr = Console.ReadLine();
			Console.Write("Username : ");
			string userName = Console.ReadLine();
			Console.Write("Password : ");
			string passWord = Console.ReadLine();


			string serverIp = null;
			if (serialnr.Contains(".")) { //quick and dirty hack to check if it is an ip-address
				serverIp = serialnr;
			} else {
				serverIp = LoxoneClient.GetIP(serialnr);
			}


			using (LoxoneClient client = new LoxoneClient(serverIp)) {
				bool open = await client.Open(userName, passWord);

				while (true) {
					Console.Write("Enter command : ");
					string cmd = Console.ReadLine();
					if (string.IsNullOrEmpty(cmd)) continue;
					if (cmd.Equals("exit")) break;

					string[] cmds = cmd.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
					if (cmd.Length == 0) continue;
					if (cmds.Length == 1) {
						if (cmds[0].Equals("list")) {
							foreach (Control ctrl in client.Data.Controls) {
								Console.WriteLine($" - {ctrl.GetType().Name} : {ctrl.Name} ");
							}
						} else if (cmds[0].Equals("getstates")) {
							client.RefreshStates();
						} else {
							foreach (Control ctrl in client.GetAllControlsOfType(cmds[0])) {
								Console.WriteLine($" - {ctrl.Name} ");
							}
						}
					} else if (cmds.Length == 2) {
						if (cmds[0].Equals("cmd")) {
							//client.Send(cmds[1]);
						} else {
							Control ctrl = client.GetControlByName(cmds[1], cmds[0]);
							if (ctrl == null) {
								Console.WriteLine(" - Not found");
							} else {
								await ctrl.Query();

								Console.WriteLine($"  Name : {ctrl.Name}");
								Console.WriteLine($"  Room : {ctrl.Room}");
								Console.WriteLine($"  Uid  : {ctrl.Uid}");
								Console.WriteLine($"  States");
								foreach ((string key, object val) state in ctrl.GetStates()) {
									Console.WriteLine($"    {state.key} = {state.val}");
								}
							}
						}
					} else if (cmds.Length == 3) {
						Control ctrl = client.GetControlByName(cmds[1], cmds[0]);
						if (ctrl == null) {
							Console.WriteLine(" - Not found");
						} else {
							await ctrl.SendCmd(cmds[2]);
						}
					}




				}

				client.Close();
			}



		}
	}
}
