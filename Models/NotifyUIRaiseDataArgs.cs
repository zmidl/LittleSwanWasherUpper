using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Models
{
	public class NotifyUIRaiseDataArgs: EventArgs
	{
		public byte[] DataPackets { get; set; }
		public string Message { get; set; }
		public CommunicationMark Mark { get; set; }
		public NotifyUIRaiseDataArgs(byte[] bytes , CommunicationMark mark)
		{
			this.DataPackets = bytes;
			this.Mark = mark;
		}
		public NotifyUIRaiseDataArgs(string message, CommunicationMark mark)
		{
			this.Message = message;
			this.Mark = mark;
		}
	}
}
