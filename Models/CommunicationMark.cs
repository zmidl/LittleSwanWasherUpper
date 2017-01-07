using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Models
{
	public enum CommunicationMark
	{
		Master = 0,
		Slave = 1,
		Unknow = 2,
		FeedbackError = 3,
		NoFeedback = 4
	}
}
