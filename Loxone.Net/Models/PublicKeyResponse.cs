using System;
using System.Collections.Generic;
using System.Text;

namespace Loxone.Net.Models {

	public class PublicKeyResponse {
		public PublicKey LL { get; set; }
	}

	public class PublicKey {
		public string control { get; set; }
		public string value { get; set; }
		public string Code { get; set; }
	}

}
