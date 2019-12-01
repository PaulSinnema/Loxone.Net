using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loxone.Net.Data {
	public class LightMood {

		public string Name { get; set;  }

		public int Id { get; set; }

		[JsonProperty("t5")]
		public bool T5 { get; set; }

		public bool Static { get; set; }


		public int Used { get; set; }
	}
}
