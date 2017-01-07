namespace Demo.Models
{
	public class WorkingData
	{
		public byte MotorTemperature { get; set; }

		public byte IPMTemperature { get; set; }

		public ushort FaultCode { get; set; }

		public ushort DrumSpeed { get; set; }

		public ushort Load { get; set; }

		public ushort OOB { get; set; }

		public WorkingData()
		{

		}

		public WorkingData(byte motorTemperature,byte ipmTemperature,ushort faultCode,ushort drumSpeed,ushort load,ushort oob)
		{
			this.MotorTemperature = motorTemperature;
			this.IPMTemperature = ipmTemperature;
			this.FaultCode = faultCode;
			this.DrumSpeed = drumSpeed;
			this.Load = load;
			this.OOB = oob;
		}
	}
}
