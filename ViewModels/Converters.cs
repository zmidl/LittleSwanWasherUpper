using System;
using System.Globalization;
using System.Windows.Data;

namespace Demo.ViewModels
{
	public class HexStringToByteConverter : IValueConverter
	{
		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return GeneralMethods.ByteToHexString((byte)value);
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) value = "00";
			byte result = default(byte);
			try
			{
				result = GeneralMethods.HexStringToByte(value.ToString());
			}
			catch { }
			finally { }
			return result;
		}
	}

	public class StringToByteConverter : IValueConverter
	{
		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int result = 0;
			try
			{
				result = byte.Parse(value.ToString());
			}
			catch { }
			return result;
			//throw new NotImplementedException();
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.ToString();
			//throw new NotImplementedException();
		}
	}

	public class StringToInt32Converter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int result = 0;
			try
			{
				result = int.Parse(value.ToString());
			}
			catch { }
			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.ToString();
		}
	}

	public class ByteStringToByteConverter : IValueConverter
	{
		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			byte result = 0x00;
			try
			{
				result = byte.Parse(value.ToString());
			}
			catch { }
			return result;
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			byte result = 0x00;
			try
			{
				result = byte.Parse(value.ToString());
			}
			catch { }
			return result;
		}
	}
}
