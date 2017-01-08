using System;
using System.IO.Ports;

namespace Demo.ViewModels
{
	public class GeneralMethods
	{
		public static readonly string[] ColumnNames = new string[] { "Motor(T)", "IPM(T)", "Fault", "Speed", "Load", "OOB" };
		
		public const string StatusOn = "ON";

		public const string StatusOff = "OFF";

		public const string Master = "MASTER";

		public const string Watch = "WATCH";

		public const int AutoCommandDefaultInterval = 20;

		public const int FeedbackTimeOut = 10;

		public const int FixedLength = 6;

		private const int _Polynome = 0x1021;

		private const int _InitialByte = 0xFFFF;

		public const byte HeaderByte = 0xB2;

		public const int StartBits = 1;

		public const int DataBits = 8;

		public const int BuadRate = 2400;

		public static readonly StopBits StopBits = StopBits.One;

		public static readonly Parity Parity = Parity.None;

		public const byte DefaultByte = 0x00;

		private static int CalculateCrc(byte[] byte_array)
		{
			int crc_code = GeneralMethods._InitialByte;

			for (int a = 0; a < byte_array.Length; a++)
			{
				crc_code ^= (byte_array[a] << 8);

				for (byte bit = 8; bit > 0; bit--)
				{
					if ((crc_code & 0x8000) >= 0x8000)
					{
						crc_code = (crc_code << 1) ^ GeneralMethods._Polynome;
					}
					else
					{
						crc_code = crc_code << 1;
					}
				}
			}
			return crc_code;
		}

		public static byte[] GetCrcAsByteArray(byte[] byte_array)
		{
			var crc = CalculateCrc(byte_array);
			return new byte[] { (byte)((crc >> 8) & 0xFF), (byte)(crc & 0xFF) };
		}

		public static string ByteToHexString(byte bytes)
		{
			string result = Convert.ToString(bytes, 16).ToUpper();
			if (result.Length.Equals(1)) result = $"0x0{result}";
			else result = $"0x{result}";
			return result;
		}

		public static byte HexStringToByte(string hexString)
		{
			hexString = hexString.Replace("0x", string.Empty);
			switch (hexString.Length)
			{
				case 0: { hexString = "00"; break; }
				case 1: { hexString = "0" + hexString; break; }
				default: { break; }
			}
			byte result = default(byte);
			try
			{
				result = byte.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
			}
			catch
			{
				
			}
			return result;
		}

		public static byte CommandToByte(int index, Models.Command command)
		{
			byte result = 0;
			switch(index)
			{
				case 0: { result = (byte)(((int)command >> 16) & 0xFF); break; }
				case 1: {result = (byte)(((ushort)command >> 8) & 0xFF);break; }
				case 2: { result = (byte)((ushort)command & 0xFF); break; }
				default: { break; }
			}
			return result;
		}
	}
}
