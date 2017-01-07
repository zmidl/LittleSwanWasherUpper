namespace Demo.Models
{
	public enum Command
	{
		Ping = 0xA1030A,
		Request = 0xA20010,
		LoadEnable = 0xA30006,
		FctTest = 0xA40013,
		SpeedSet = 0xA50406,
		MotorTemperatureCorrection = 0xA60006,
		FctPing = 0xA70008,
		OobAndDoobLimitSet = 0xA80408
	}
}
