using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loxone.Net
{
	internal class LoxoneMessage
	{

		public string RawData { get; set; }
	}

	internal class LoxoneTextMessage : LoxoneMessage 
	{
		public LoxoneTextBody LL { get; set; }

	}

	internal class LoxoneTextBody
	{

		public string Control { get; set; }
		public string value { get; set; }
		public int Code { get; set; }

	}
}
