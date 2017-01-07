namespace Demo.Models
{
	public class AutoCommand
	{
		public bool IsJoin { get; set; }
		public Command Command { get; set; }
		public AutoCommand(bool isJoin, Command command)
		{
			this.IsJoin = isJoin;
			this.Command = command;
		}
	}
}
