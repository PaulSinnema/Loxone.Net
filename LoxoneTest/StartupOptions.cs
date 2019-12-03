using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoxoneTest {
	public class StartupOptions {

		[Option('s', HelpText ="Set MiniServer IP address or SerialNr", Required = false)]
		public string Server { get; set; }

		[Option('u', HelpText = "UserName to login on MiniServer", Required = false)]
		public string User { get; set; }

		[Option('p', HelpText = "Password to login on MiniServer", Required = false)]
		public string Password { get; set; }
	}
}
