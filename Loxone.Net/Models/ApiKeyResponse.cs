using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loxone.Net.Models {
	class ApiKeyResponse {
		public ApiKey LL { get; set; }

		public ApiKeyValue Value {
			get {
				if (string.IsNullOrEmpty(this.LL?.value)) return new ApiKeyValue();
				return JsonConvert.DeserializeObject<ApiKeyValue>(this.LL.value.Replace('\'', '\"'));
			}
		}

	}

	public class ApiKey {
		public string control { get; set; }
		public string value { get; set; }
		public string Code { get; set; }
	}

	public class ApiKeyValue {
		public string snr { get; set; }
		public string version { get; set; }

		public string key { get; set; }

	}

}
