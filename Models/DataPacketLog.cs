using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Demo.Models
{
	public class DataPacketLog
	{
		public string Label { get; set; }
		public SolidColorBrush Color { get; set; }
		public string Data { get; set; }

		public DataPacketLog(string data,CommunicationMark mark)
		{
			switch(mark)
			{
				case CommunicationMark.Master:
				{
					this.Label = "⇒"; this.Color = new SolidColorBrush(Colors.Green);
					break;
				}
				case CommunicationMark.Slave:
				{
					this.Label = "⇐"; this.Color = new SolidColorBrush(Colors.Green);
					break;
				}
				case CommunicationMark.Unknow:
				{
					this.Label = "⇔"; this.Color = new SolidColorBrush(Colors.Brown);
					break;
				}
				case CommunicationMark.FeedbackError:
				{
					this.Label = "⇍"; this.Color = new SolidColorBrush(Colors.Red);
					break;
				}
				case CommunicationMark.NoFeedback:
				{
					this.Label = "⇍"; this.Color = new SolidColorBrush(Colors.Red);
					break;
				}
			}
		
			this.Data = data;
		}
	}
}
