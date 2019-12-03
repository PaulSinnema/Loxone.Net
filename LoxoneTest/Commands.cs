using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoxoneTest {


	[Verb("List")]
	public class ListCmd {


		[Option('t', HelpText="List all control based on the Type")]
		public string Type { get; set; }

		[Option('n', HelpText = "List all control based on the Name")]
		public string Name { get; set; }


	}


}
