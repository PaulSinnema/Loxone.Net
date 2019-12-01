using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loxone.Net
{

	internal enum LoxoneMessageType : byte
	{
		Text = 0,
		Binary = 1,
		ValueState = 2,
		TextState = 3,
		DayTimerState = 4,
		OutOfService = 5,
		KeepAlive = 6,
		WeatherState = 7
	}

	internal class LoxoneHeader
	{

		public LoxoneMessageType MessageType { get; set; }

		public int MessageSize { get; set; }
	}
}
