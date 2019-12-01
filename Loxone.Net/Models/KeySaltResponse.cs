using System;
using System.Collections.Generic;
using System.Text;

namespace Loxone.Net.Models {
	class KeySaltResponse {
		public KeySalt LL { get; set; }
	}

	public class KeySalt {
		public string control { get; set; }
		public int code { get; set; }
		public KeySaltValue value { get; set; }
	}

	public class KeySaltValue {
		public string key { get; set; }
		public string salt { get; set; }
	}

}
