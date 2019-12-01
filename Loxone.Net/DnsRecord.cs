using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loxone.Net
{
	class DnsRecord
	{

		public string cmd { get; set; }

		public string IP { get; set; }

		public int Code { get; set; }
		public string LastUpdated { get; set; }
		public bool PortOpen { get; set; }

	}

}
