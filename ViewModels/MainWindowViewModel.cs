using Demo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Demo.ViewModels
{
	public class MainWindowViewModel : ViewModel
	{
		private int TEST_INDEX = 0;
		public RelayCommand TEST { get; private set; }



		#region 私有字段
		/// <summary>
		/// 串口对象
		/// </summary>
		private SerialPort _MySerialPort;

		/// <summary>
		/// 当前主控模式下请求的命令
		/// </summary>
		private Command _CurrentCommand;

		/// <summary>
		/// 主机尝试重新握手从机时的状态标帜
		/// </summary>
		private RetryStatus _RetryStatus;

		/// <summary>
		/// 
		/// </summary>
		private int _FeedbackIncrement;

		/// <summary>
		/// 
		/// </summary>
		private int _FeedbackTimeOut;

		/// <summary>
		/// 通信超时计时器
		/// </summary>
		private System.Timers.Timer _Timer;

		/// <summary>
		/// 
		/// </summary>
		private Action[] _ManualCommandActions;
		#endregion

		#region 字符串
		/// <summary>
		/// 反映串口开关状态
		/// </summary>
		public string PortStatus
		{
			get { return this.IsPortOpen ? GeneralMethods.StatusOn : GeneralMethods.StatusOff; }
		}

		/// <summary>
		/// 反映是否打开自动模式
		/// </summary>
		public string AutoCommandStatus
		{
			get { return this.IsAutoCommand ? GeneralMethods.StatusOn : GeneralMethods.StatusOff; }
		}

		/// <summary>
		/// 反映是否打开主控模式
		/// </summary>
		public string ModeSwitchStatus
		{
			get { return this.IsMasterModel ? GeneralMethods.Master : GeneralMethods.Watch; }
		}
		public string TempM { get; set; }
		public string TempIPM { get; set; }
		public string FaultCode { get; set; }
		public string DrumSpeed { get; set; }
		public string Load { get; set; }
		public string OOB { get; set; }
		#endregion

		private ObservableCollection<WorkingData> _WorkingDatas = new ObservableCollection<WorkingData>();
		public ObservableCollection<WorkingData> WorkingDatas
		{
			get { return _WorkingDatas; }
			set { _WorkingDatas = value; }
		}
		private ObservableCollection<DataPacketLog> _DataPacketLogs = new ObservableCollection<DataPacketLog>();
		public ObservableCollection<DataPacketLog> DataPacketLogs
		{
			get { return _DataPacketLogs; }
			set { _DataPacketLogs = value; }
		}

		#region 属性--其他
		public System.Windows.WindowState MainWindowState { get; set; }

		private bool _IsMasterModel;
		public bool IsMasterModel
		{
			get { return _IsMasterModel; }
			set
			{
				if (value == false)
				{
					this.IsAutoCommand = false;
					if (this._MySerialPort != null) this._MySerialPort.ReceivedBytesThreshold = GeneralMethods.FixedLength;
				}
				_IsMasterModel = value;
				this.RaisePropertyChanged(nameof(this.IsMasterModel));
				this.RaisePropertyChanged(nameof(this.ModeSwitchStatus));
				this.RaiseAllCanExecuteManualCommandChanged();
			}
		}

		private bool _IsAutoCommand = false;
		public bool IsAutoCommand
		{
			get { return _IsAutoCommand; }
			set
			{
				if (IsAutoCommand == false && value == true)
				{
					if (this.CanAutoCommand() == true && this.IsMasterModel == true)
					{
						this._IsAutoCommand = value;
						this.RaisePropertyChanged(() => this.IsAutoCommand);
						this.RaisePropertyChanged(() => this.AutoCommandStatus);
						this.RaiseAllCanExecuteManualCommandChanged();
						this.TransmitDataPacket();
					}
				}
				else if (this.IsAutoCommand == true && value == false)
				{
					this._IsAutoCommand = value;
					this.RaisePropertyChanged(() => this.IsAutoCommand);
					this.RaisePropertyChanged(() => this.AutoCommandStatus);
					this.RaiseAllCanExecuteManualCommandChanged();
				}
			}
		}

		private int _AutoCommandIndex = 0;
		public int AutoCommandIndex
		{
			get { return this._AutoCommandIndex; }
			set
			{
				if (value > 7) value = 0;
				this._AutoCommandIndex = value;
				this.RaisePropertyChanged(nameof(this.AutoCommandIndex));
			}
		}
		#endregion

		#region 属性--是否加入到自动模式
		private List<AutoCommand> _AutoCommands;
		public bool IsPingJoin
		{
			get { return this._AutoCommands[0].IsJoin; }
			set
			{
				this._AutoCommands[0].IsJoin = value;
				this.RaisePropertyChanged(() => this.IsPingJoin);
				this.RaiseAutoCommand();
			}
		}
		public bool IsRequestJoin
		{
			get { return this._AutoCommands[1].IsJoin; }
			set
			{
				this._AutoCommands[1].IsJoin = value;
				this.RaisePropertyChanged(() => this.IsRequestJoin);
				this.RaiseAutoCommand();
			}
		}
		public bool IsLoadEnableJoin
		{
			get { return this._AutoCommands[2].IsJoin; }
			set
			{
				this._AutoCommands[2].IsJoin = value;
				this.RaisePropertyChanged(nameof(this.IsLoadEnableJoin));
				this.RaiseAutoCommand();
			}
		}
		public bool IsFctTestJoin
		{
			get { return this._AutoCommands[3].IsJoin; }
			set
			{
				this._AutoCommands[3].IsJoin = value;
				this.RaisePropertyChanged(nameof(this.IsFctTestJoin));
				this.RaiseAutoCommand();
			}
		}
		public bool IsSpeedSetJoin
		{
			get { return this._AutoCommands[4].IsJoin; }
			set
			{
				this._AutoCommands[4].IsJoin = value;
				this.RaisePropertyChanged(nameof(this.IsSpeedSetJoin));
				this.RaiseAutoCommand();
			}
		}
		public bool IsMotorTemperatureCorrectionJoin
		{
			get { return this._AutoCommands[5].IsJoin; }
			set
			{
				this._AutoCommands[5].IsJoin = value;
				this.RaisePropertyChanged(nameof(this.IsMotorTemperatureCorrectionJoin));
				this.RaiseAutoCommand();
			}
		}
		public bool IsFctPingJoin
		{
			get { return this._AutoCommands[6].IsJoin; }
			set
			{
				this._AutoCommands[6].IsJoin = value;
				this.RaisePropertyChanged(nameof(this.IsFctPingJoin));
				this.RaiseAutoCommand();
			}
		}
		public bool IsOobAndDoobLimitSetJoin
		{
			get { return this._AutoCommands[7].IsJoin; }
			set
			{
				this._AutoCommands[7].IsJoin = value;
				this.RaisePropertyChanged(nameof(this.IsOobAndDoobLimitSetJoin));
				this.RaiseAutoCommand();
			}
		}
		#endregion

		#region 属性--是否需要导出
		private bool[] _IsNeedToExportColumns;
		public bool IsNeedToExportTempM
		{
			get { return this._IsNeedToExportColumns[0]; }
			set { this._IsNeedToExportColumns[0] = value; this.RaisePropertyChanged(nameof(IsNeedToExportTempM)); }
		}
		public bool IsNeedToExportTempIPM
		{
			get { return this._IsNeedToExportColumns[1]; }
			set { this._IsNeedToExportColumns[1] = value; this.RaisePropertyChanged(nameof(IsNeedToExportTempIPM)); }
		}
		public bool IsNeedToExportFaultCode
		{
			get { return this._IsNeedToExportColumns[2]; }
			set { this._IsNeedToExportColumns[2] = value; this.RaisePropertyChanged(nameof(IsNeedToExportFaultCode)); }
		}
		public bool IsNeedToExportDrumSpeed
		{
			get { return this._IsNeedToExportColumns[3]; }
			set { this._IsNeedToExportColumns[3] = value; this.RaisePropertyChanged(nameof(IsNeedToExportDrumSpeed)); }
		}
		public bool IsNeedToExportLoad
		{
			get { return this._IsNeedToExportColumns[4]; }
			set { this._IsNeedToExportColumns[4] = value; this.RaisePropertyChanged(nameof(IsNeedToExportLoad)); }
		}
		public bool IsNeedToExportOOB
		{
			get { return this._IsNeedToExportColumns[5]; }
			set { this._IsNeedToExportColumns[5] = value; this.RaisePropertyChanged(nameof(IsNeedToExportOOB)); }
		}
		#endregion

		#region 属性--串口字段与属性
		private bool _IsPortListening { get; set; }
		private bool _IsPortClosing;
		public bool IsPortOpen
		{
			get { return this._MySerialPort == null ? false : this._MySerialPort.IsOpen; }
			set
			{
				if (this.IsPortOpen == false && value)
				{
					if (this.ExecuteOpenPort())
					{
						this.IsPortOpen = value;
						this.RaisePropertyChanged(nameof(IsPortOpen));
						this.RaisePropertyChanged(nameof(this.PortStatus));
						this.RaiseAllCanExecuteManualCommandChanged();
					}
				}
				else if (this.IsPortOpen && value == false)
				{
					if (this.ExecuteClosePort())
					{
						this.IsPortOpen = value;
						this.RaisePropertyChanged(nameof(IsPortOpen));
						this.RaisePropertyChanged(nameof(this.PortStatus));
						this.RaiseAllCanExecuteManualCommandChanged();
					}
				}
				else if (this.IsPortOpen == false && value == false)
				{
					this.ExecuteClosePort();
				}
			}
		}
		public string[] SerialPortNames
		{
			get { return SerialPort.GetPortNames(); }
		}
		private string _SelectedPortName;
		public string SelectedPortName
		{
			get { return _SelectedPortName; }
			set
			{
				_SelectedPortName = value;
				this.RaisePropertyChanged(nameof(this.SelectedPortName));
			}
		}
		#endregion

		#region 属性--用户输入的参数
		private byte _ID;
		public byte ID
		{
			get { return _ID; }
			set
			{
				_ID = value;
				this.RaisePropertyChanged(nameof(this.ID));
				this.RaiseAllCrc();
			}
		}
		private byte _PlatformMessage;
		public byte PlatformMessage
		{
			get { return _PlatformMessage; }
			set
			{
				_PlatformMessage = value;
				this.RaisePropertyChanged(nameof(this.PlatformMessage));
				this.UpdateCrc(Command.Ping);
			}
		}
		private byte _PingData;
		public byte PingData
		{
			get { return _PingData; }
			set
			{
				_PingData = value; this.RaisePropertyChanged(nameof(this.PingData));
			}
		}
		private byte _MotorNumber;
		public byte MotorNumber
		{
			get { return _MotorNumber; }
			set
			{
				_MotorNumber = value;
				this.RaisePropertyChanged(nameof(this.MotorNumber));
				this.UpdateCrc(Command.Ping);
			}
		}
		private byte _AccelerateTimeHigh;
		public byte AccelerateTimeHigh
		{
			get { return _AccelerateTimeHigh; }
			set
			{
				_AccelerateTimeHigh = value;
				this.RaisePropertyChanged(nameof(this.AccelerateTimeHigh));
				this.UpdateCrc(Command.SpeedSet);
			}
		}
		private byte _AccelerateTimeLow;
		public byte AccelerateTimeLow
		{
			get { return _AccelerateTimeLow; }
			set
			{
				_AccelerateTimeLow = value;
				this.RaisePropertyChanged(nameof(this.AccelerateTimeLow));
				this.UpdateCrc(Command.SpeedSet);
			}
		}
		private byte _DrumSpeedHigh;
		public byte DrumSpeedHigh
		{
			get { return _DrumSpeedHigh; }
			set
			{
				_DrumSpeedHigh = value;
				this.RaisePropertyChanged(nameof(this.DrumSpeedHigh));
				this.UpdateCrc(Command.SpeedSet);
			}
		}
		private byte _DrumSpeedLow;
		public byte DrumSpeedLow
		{
			get { return _DrumSpeedLow; }
			set
			{
				_DrumSpeedLow = value;
				this.RaisePropertyChanged(nameof(this.DrumSpeedLow));
				this.UpdateCrc(Command.SpeedSet);
			}
		}
		private byte _OobLimit1;
		public byte OobLimit1
		{
			get { return _OobLimit1; }
			set
			{
				_OobLimit1 = value;
				this.RaisePropertyChanged(nameof(this.OobLimit1));
				this.UpdateCrc(Command.OobAndDoobLimitSet);
			}
		}
		private byte _OobLimit2;
		public byte OobLimit2
		{
			get { return _OobLimit2; }
			set
			{
				_OobLimit2 = value;
				this.RaisePropertyChanged(nameof(this.OobLimit2));
				this.UpdateCrc(Command.OobAndDoobLimitSet);
			}
		}
		private byte _DoobLimit;
		public byte DoobLimit
		{
			get { return _DoobLimit; }
			set
			{
				_DoobLimit = value;
				this.RaisePropertyChanged(nameof(this.DoobLimit));
				this.UpdateCrc(Command.OobAndDoobLimitSet);
			}
		}
		private byte _OobData;
		public byte OobData
		{
			get { return _OobData; }
			set
			{
				_OobData = value;
				this.RaisePropertyChanged(nameof(this.OobData));
				this.UpdateCrc(Command.OobAndDoobLimitSet);
			}
		}
		#endregion

		#region 属性--数据包校验位字节
		public byte PingCrcHigh { get; set; }
		public byte PingCrcLow { get; set; }
		public byte RequestCrcHigh { get; set; }
		public byte RequestCrcLow { get; set; }
		public byte LoadEnableCrcHigh { get; set; }
		public byte LoadEnableCrcLow { get; set; }
		public byte FctTestCrcHigh { get; set; }
		public byte FctTestCrcLow { get; set; }
		public byte SpeedSetCrcHigh { get; set; }
		public byte SpeedSetCrcLow { get; set; }
		public byte MotorTemperatureCorrectionCrcHigh { get; set; }
		public byte MotorTemperatureCorrectionCrcLow { get; set; }
		public byte FctPingCrcHigh { get; set; }
		public byte FctPingCrcLow { get; set; }
		public byte OobAndDoobLimitSetCrcHigh { get; set; }
		public byte OobAndDoobLimitSetCrcLow { get; set; }
		#endregion

		#region 命令--业务方法

		public RelayCommand Minimize { get; private set; }
		public RelayCommand Refresh { get; private set; }
		public RelayCommand ExitAPP { get; private set; }
		public RelayCommand ExportCsvFile { get; private set; }
		public RelayCommand ExportTxtFile { get; private set; }
		public RelayCommand ManualCommandPing { get; private set; }
		public RelayCommand ManualCommandRequest { get; private set; }
		public RelayCommand ManualCommandLoadEnable { get; private set; }
		public RelayCommand ManualCommandFctTest { get; private set; }
		public RelayCommand ManualCommandSpeedSet { get; private set; }
		public RelayCommand ManualCommandMotorTemperatureCorrection { get; private set; }
		public RelayCommand ManualCommandFctPing { get; private set; }
		public RelayCommand ManualCommandOobAndDoobLimitSet { get; private set; }
		#endregion

		public event EventHandler<NotifyUIRaiseDataArgs> NotifyUIRaiseData;

		public event EventHandler NotifyUIContinueAutoCommand;

		/// <summary>
		/// 业务类构造函数
		/// </summary>
		public MainWindowViewModel()
		{
			this._ManualCommandActions = new Action[]
			{
				()=> { this.TransmitDataPacket(this._CurrentCommand = Command.Ping); },
				() => { this.TransmitDataPacket(this._CurrentCommand = Command.Request); },
				() => { this.TransmitDataPacket(this._CurrentCommand = Command.LoadEnable); },
				() => { this.TransmitDataPacket(this._CurrentCommand = Command.FctTest); },
				() => { this.TransmitDataPacket(this._CurrentCommand = Command.SpeedSet); },
				() => { this.TransmitDataPacket(this._CurrentCommand = Command.MotorTemperatureCorrection); },
				() => { this.TransmitDataPacket(this._CurrentCommand = Command.FctPing); },
				() => { this.TransmitDataPacket(this._CurrentCommand = Command.OobAndDoobLimitSet); }
			};
			this.TEST = new RelayCommand(this.TEST_ACTION);
			this.Minimize = new RelayCommand(ExecuteMinimize);
			this.ExportCsvFile = new RelayCommand(this.ExecuteExportCsvFile);
			this.ExportTxtFile = new RelayCommand(this.ExecuteExportTxtFile);
			this.Refresh = new RelayCommand(() => { this.RaisePropertyChanged(nameof(this.SerialPortNames)); });
			this.ExitAPP = new RelayCommand(() => { System.Environment.Exit(0); });
			this.ManualCommandPing = new RelayCommand(this._ManualCommandActions[0], this.CanExecuteManualCommand);
			this.ManualCommandRequest = new RelayCommand(this._ManualCommandActions[1], this.CanExecuteManualCommand);
			this.ManualCommandLoadEnable = new RelayCommand(this._ManualCommandActions[2], this.CanExecuteManualCommand);
			this.ManualCommandFctTest = new RelayCommand(this._ManualCommandActions[3], this.CanExecuteManualCommand);
			this.ManualCommandSpeedSet = new RelayCommand(this._ManualCommandActions[4], this.CanExecuteManualCommand);
			this.ManualCommandMotorTemperatureCorrection = new RelayCommand(this._ManualCommandActions[5], this.CanExecuteManualCommand);
			this.ManualCommandFctPing = new RelayCommand(this._ManualCommandActions[6], this.CanExecuteManualCommand);
			this.ManualCommandOobAndDoobLimitSet = new RelayCommand(this._ManualCommandActions[7], this.CanExecuteManualCommand);
			this.InitializeAutoCommand();
			this.InitializeApplication();
		}

		/// <summary>
		/// 初始化业务
		/// </summary>
		private void InitializeApplication()
		{
			this.TempM = GeneralMethods.ColumnNames[0];
			this.TempIPM = GeneralMethods.ColumnNames[1];
			this.FaultCode = GeneralMethods.ColumnNames[2];
			this.DrumSpeed = GeneralMethods.ColumnNames[3];
			this.Load = GeneralMethods.ColumnNames[4];
			this.OOB = GeneralMethods.ColumnNames[5];
			this._IsNeedToExportColumns = new bool[] { true, true, true, true, true, true };
			this._Timer = new System.Timers.Timer(20);
			this._Timer.Elapsed += TimerElapsed;
			this._Timer.AutoReset = true;
			this._Timer.Enabled = true;
			this._FeedbackIncrement = 0;
			this._FeedbackTimeOut = 0;
			this._RetryStatus = RetryStatus.Recover;
			this.IsMasterModel = false;
			this.ID = 0x01;
			this.PlatformMessage = 0x15;
			this.MotorNumber = 0x01;
		}

		private void TEST_ACTION()
		{
			
			if (++this.TEST_INDEX > 6) this.TEST_INDEX = 0;

			
			this._ManualCommandActions[this.TEST_INDEX]();
			//for (int i = 0; i < 10000; i++)
			//{
			//	this.WorkingDatas.Add(new WorkingData(11, 22, 333, 4444, 55555, (ushort)i));
			//}

		}

		/// <summary>
		/// 计时器中断事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			this._FeedbackTimeOut += this._FeedbackIncrement;
			if (this._FeedbackTimeOut == GeneralMethods.FeedbackTimeOut)
			{
				if (this._MySerialPort.IsOpen == true)
				{
					this._MySerialPort.DiscardInBuffer();
					this._MySerialPort.DiscardOutBuffer();
				}
				this.NotifyUIRaiseData(this, new NotifyUIRaiseDataArgs("error", CommunicationMark.NoFeedback));
				//this._IsNoFeedback = true;
				//this._IsRetransfered = true;
				this._RetryStatus = RetryStatus.RePing;
				this.NotifyUIContinueAutoCommand(this, null);
			}
		}

		/// <summary>
		/// 初始化自动发送控制的命令集合
		/// </summary>
		private void InitializeAutoCommand()
		{
			var commandArray = Enum.GetValues(typeof(Command));
			this._AutoCommands = new List<AutoCommand>(commandArray.Length);
			foreach (var current in commandArray)
			{
				this._AutoCommands.Add(new AutoCommand(false, (Command)current));
			}
		}

		/// <summary>
		/// 更新数据到界面
		/// </summary>
		/// <param name="mark"></param>
		/// <param name="bytes"></param>
		/// <param name="message"></param>
		public void RaiseNewDataToUI(CommunicationMark mark, byte[] bytes = null, string message = null)
		{
			if (mark == CommunicationMark.Slave)
			{
				if (bytes[2] == 0xA2 && bytes[3] == 0x0A)
				{
					WorkingData data = new WorkingData
						(bytes[4],
						 bytes[5],
						 (ushort)((bytes[6] << 8) + bytes[7]),
						 (ushort)((bytes[8] << 8) + bytes[9]),
						 (ushort)((bytes[10] << 8) + bytes[11]),
						 (ushort)((bytes[12] << 8) + bytes[13]));
					this.WorkingDatas.Add(data);
				}
			}

			if (bytes != null) this.LogCommunicationData(bytes, mark);
			if (message != null) this.LogCommunicationData(message, mark);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private bool CanAutoCommand()
		{
			bool result = false;
			foreach (var autoCommand in this._AutoCommands)
			{
				if (autoCommand.IsJoin == true) { result = true; break; }
			}
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		private void RaiseAutoCommand()
		{
			if (this.CanAutoCommand() == false) this.IsAutoCommand = false;
		}

		/// <summary>
		/// 
		/// </summary>
		private void SetFeedbackSuccess()
		{
			this._FeedbackIncrement = 0;
			this._FeedbackTimeOut = 0;
		}

		/// <summary>
		/// 
		/// </summary>
		private void SetFeedbackFail()
		{
			this._FeedbackIncrement = 1;
			this._FeedbackTimeOut = 0;
		}

		/// <summary>
		/// 收集下位机工作参数
		/// </summary>
		/// <param name="buffer"></param>
		private void CollectWorkingData(byte[] buffer)
		{
			WorkingData data = new WorkingData
				(buffer[4],
				buffer[5],
				 (ushort)((buffer[6] << 8) + buffer[7]),
				 (ushort)((buffer[8] << 8) + buffer[9]),
				 (ushort)((buffer[10] << 8) + buffer[11]),
				 (ushort)((buffer[12] << 8) + buffer[13]));
			this.WorkingDatas.Add(data);
		}

		/// <summary>
		/// 串口注册的“接收”事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _MySerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			this.ReceiveDataPacket();
		}

		/// <summary>
		/// 更具指定命令发送数据包
		/// </summary>
		/// <param name="command"></param>
		private void TransmitDataPacket(Command command)
		{
			if (this._MySerialPort.IsOpen == true)
			{
				byte[] data = this.EncodeDataPacket(command);
				this._MySerialPort.Write(data, 0, data.Length);
				this.LogCommunicationData(data, CommunicationMark.Master);
				this._MySerialPort.DiscardOutBuffer();
				this.SetFeedbackFail();
			}
			else
			{
				this.RaisePropertyChanged(nameof(this.IsPortOpen));
			}
		}

		/// <summary>
		///发送各类数据包
		/// </summary>
		public void TransmitDataPacket()
		{
			switch (this._RetryStatus)
			{
				case RetryStatus.RePing:
				{
					Thread.Sleep(GeneralMethods.AutoCommandInterval);
					this.TransmitDataPacket(Command.Ping);
					break;
				}
				case RetryStatus.Retransmission:
				{
					Thread.Sleep(GeneralMethods.AutoCommandInterval);
					this.TransmitDataPacket(this._CurrentCommand);
					break;
				}
				case RetryStatus.Recover:
				{
					if (this._IsAutoCommand == true)
					{
						Thread.Sleep(GeneralMethods.AutoCommandInterval);
						this.TransmitDataPacket(this._CurrentCommand = this.GetPollingCommand());
					}
					break;
				}
				default: break;
			}
		}

		/// <summary>
		/// 处理接收到的数据包
		/// </summary>
		private void ReceiveDataPacket()
		{
			if (this._IsPortClosing == false)
			{
				this._IsPortListening = true;

				if (this.IsMasterModel == false) Thread.Sleep(70);

				byte[] buffer = new byte[this._MySerialPort.BytesToRead];

				this._MySerialPort.Read(buffer, 0, buffer.Length);

				this._MySerialPort.DiscardInBuffer();

				if (this.IsMasterModel == true)
				{
					if (this.CheckReceivedDataPacket(buffer) == true)
					{
						if (buffer[0].Equals(0xB2) && buffer[2].Equals(0xA8) && buffer[3].Equals(0x01)) buffer = buffer.Take(7).ToArray();
						this.SetFeedbackSuccess();
						this.NotifyUIRaiseData(this, new NotifyUIRaiseDataArgs(buffer, CommunicationMark.Slave));
						if (this._RetryStatus == RetryStatus.RePing) this._RetryStatus = RetryStatus.Retransmission;
						else if (this._RetryStatus == RetryStatus.Retransmission) this._RetryStatus = RetryStatus.Recover;
						this.NotifyUIContinueAutoCommand(this, null);
					}
				}
				else
				{
					this.SetFeedbackSuccess();

					var mark = CommunicationMark.Unknow;
					if (buffer.Length < GeneralMethods.FixedLength) mark = CommunicationMark.FeedbackError;
					else if (this.CheckReceivedDataPacket(buffer) == false) mark = CommunicationMark.FeedbackError;
					else mark = GetMark(buffer);
					this.NotifyUIRaiseData(this, new NotifyUIRaiseDataArgs(buffer, mark));
				}
				this._IsPortListening = false;
				this.RaisePropertyChanged(nameof(this._IsPortListening));
			}
		}

		/// <summary>
		/// 通过数据包确认通讯属性标帜
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		private CommunicationMark GetMark(byte[] bytes)
		{
			CommunicationMark result = CommunicationMark.Unknow;
			switch (bytes[2])
			{
				case 0xA1:
				{
					if (bytes[3] == 0x03) result = CommunicationMark.Master;
					else if (bytes[3] == 0x04) result = CommunicationMark.Slave;
					else result = CommunicationMark.Unknow;
					break;
				}
				case 0xA2:
				{
					if (bytes[3] == 0x00) result = CommunicationMark.Master;
					else if (bytes[3] == 0x0A) result = CommunicationMark.Slave;
					else result = CommunicationMark.Unknow;
					break;
				}
				case 0xA3:
				{
					result = CommunicationMark.Unknow;
					break;
				}
				case 0xA4:
				{
					if (bytes[3] == 0x00) result = CommunicationMark.Master;
					else if (bytes[3] == 0x0D) result = CommunicationMark.Slave;
					else result = CommunicationMark.Unknow;
					break;
				}
				case 0xA5:
				{
					if (bytes[3] == 0x04) result = CommunicationMark.Master;
					else if (bytes[3] == 0x00) result = CommunicationMark.Slave;
					else result = CommunicationMark.Unknow;
					break;
				}
				case 0xA6:
				{
					result = CommunicationMark.Unknow;
					break;
				}
				case 0xA7:
				{
					if (bytes[3] == 0x00) result = CommunicationMark.Master;
					else if (bytes[3] == 0x02) result = CommunicationMark.Slave;
					else result = CommunicationMark.Unknow;
					break;
				}
				case 0xA8:
				{
					if (bytes[3] == 0x04) result = CommunicationMark.Master;
					else if (bytes[3] == 0x01) result = CommunicationMark.Slave;
					else result = CommunicationMark.Unknow;
					break;
				}
			}
			return result;
		}

		/// <summary>
		/// 序列化数据包
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		private byte[] EncodeDataPacket(Command command)
		{
			List<byte> result = default(List<byte>);
			byte model = GeneralMethods.CommandToByte(0, command);
			byte length = GeneralMethods.CommandToByte(1, command);
			if (this.IsMasterModel == true) this._MySerialPort.ReceivedBytesThreshold = GeneralMethods.CommandToByte(2, command);
			List<byte> tempList = new List<byte>(length);
			switch (command)
			{
				case Command.Ping:
				{
					tempList.Add(this.PlatformMessage);
					tempList.Add(this.PingData);
					tempList.Add(this.MotorNumber);
					break;
				}
				case Command.Request:
				case Command.LoadEnable:
				case Command.FctTest:
				case Command.MotorTemperatureCorrection:
				case Command.FctPing:
				{
					break;
				}
				case Command.SpeedSet:
				{
					tempList.Add(this.AccelerateTimeHigh);
					tempList.Add(this.AccelerateTimeLow);
					tempList.Add(this.DrumSpeedHigh);
					tempList.Add(this.DrumSpeedLow);
					break;
				}
				case Command.OobAndDoobLimitSet:
				{
					tempList.Add(this.OobLimit1);
					tempList.Add(this.OobLimit2);
					tempList.Add(this.DoobLimit);
					tempList.Add(this._OobData);
					break;
				}
				default:
				{
					break;
				}
			}
			result = new List<byte>(GeneralMethods.FixedLength + length);
			result.Add(GeneralMethods.HeaderByte);
			result.Add(this.ID);
			result.Add(model);
			result.Add(length);
			result.AddRange(tempList.ToArray());
			result.AddRange(GeneralMethods.GetCrcAsByteArray(result.Skip(1).Take(result.Count - 1).ToArray()));
			return result.ToArray();
		}

		/// <summary>
		/// 刷新指定CRC校验值
		/// </summary>
		/// <param name="command"></param>
		private void UpdateCrc(Command command)
		{
			var byteArray = this.EncodeDataPacket(command);
			var crcHigh = byteArray[byteArray.Length - 2];
			var crcLow = byteArray[byteArray.Length - 1];
			switch (command)
			{
				case Command.Ping:
				{
					this.PingCrcHigh = crcHigh;
					this.PingCrcLow = crcLow;
					this.RaisePropertyChanged(nameof(this.PingCrcHigh));
					this.RaisePropertyChanged(nameof(this.PingCrcLow));
					break;
				}
				case Command.Request:
				{
					this.RequestCrcHigh = crcHigh;
					this.RequestCrcLow = crcLow;
					this.RaisePropertyChanged(nameof(this.RequestCrcHigh));
					this.RaisePropertyChanged(nameof(this.RequestCrcLow));
					break;
				}
				case Command.LoadEnable:
				{
					this.LoadEnableCrcHigh = crcHigh;
					this.LoadEnableCrcLow = crcLow;
					this.RaisePropertyChanged(nameof(this.LoadEnableCrcHigh));
					this.RaisePropertyChanged(nameof(this.LoadEnableCrcLow));
					break;
				}
				case Command.FctTest:
				{
					this.FctTestCrcHigh = crcHigh;
					this.FctTestCrcLow = crcLow;
					this.RaisePropertyChanged(nameof(this.FctTestCrcHigh));
					this.RaisePropertyChanged(nameof(this.FctTestCrcLow));
					break;
				}
				case Command.SpeedSet:
				{
					this.SpeedSetCrcHigh = crcHigh;
					this.SpeedSetCrcLow = crcLow;
					this.RaisePropertyChanged(nameof(this.SpeedSetCrcHigh));
					this.RaisePropertyChanged(nameof(this.SpeedSetCrcLow));
					break;
				}
				case Command.MotorTemperatureCorrection:
				{
					this.MotorTemperatureCorrectionCrcHigh = crcHigh;
					this.MotorTemperatureCorrectionCrcLow = crcLow;
					this.RaisePropertyChanged(nameof(this.MotorTemperatureCorrectionCrcHigh));
					this.RaisePropertyChanged(nameof(this.MotorTemperatureCorrectionCrcLow));
					break;
				}
				case Command.FctPing:
				{
					this.FctPingCrcHigh = crcHigh;
					this.FctPingCrcLow = crcLow;
					this.RaisePropertyChanged(nameof(this.FctPingCrcHigh));
					this.RaisePropertyChanged(nameof(this.FctPingCrcLow));
					break;
				}
				case Command.OobAndDoobLimitSet:
				{
					this.OobAndDoobLimitSetCrcHigh = crcHigh;
					this.OobAndDoobLimitSetCrcLow = crcLow;
					this.RaisePropertyChanged(nameof(this.OobAndDoobLimitSetCrcHigh));
					this.RaisePropertyChanged(nameof(this.OobAndDoobLimitSetCrcLow));
					break;
				}
				default:
				{
					break;
				}
			}
		}

		/// <summary>
		/// 刷新所有CRC校验值
		/// </summary>
		private void RaiseAllCrc()
		{
			foreach (var current in Enum.GetValues(typeof(Command)))
			{
				this.UpdateCrc((Command)current);
			}
		}

		/// <summary>
		/// 把数据包解析成字符串
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		private string ShowData(byte[] bytes)
		{
			StringBuilder sb = new StringBuilder();
			foreach (var c in bytes)
			{
				var cc = Convert.ToString(c, 16).ToUpper();
				if (cc.Length == 1) cc = "0" + cc;
				sb.Append(cc + " ");
			}
			return sb.ToString();
		}

		/// <summary>
		/// 添加一个数据包信息到日志
		/// </summary>
		/// <param name="bytes"></param>
		/// <param name="mark"></param>
		public void LogCommunicationData(byte[] bytes, CommunicationMark mark)
		{
			this.DataPacketLogs.Add(new DataPacketLog(this.ShowData(bytes), mark));
		}

		/// <summary>
		/// 添加一条字符串信息到日志
		/// </summary>
		/// <param name="message"></param>
		/// <param name="mark"></param>
		public void LogCommunicationData(string message, CommunicationMark mark)
		{
			this.DataPacketLogs.Add(new DataPacketLog(message, mark));
		}

		/// <summary>
		/// 打开串口
		/// </summary>
		/// <returns></returns>
		private bool ExecuteOpenPort()
		{
			bool result = false;

			if (/*this._MySerialPort == null && */this.SelectedPortName != null)
			{
				this._MySerialPort = new SerialPort();
				this._MySerialPort.PortName = this.SelectedPortName;
				this._MySerialPort.BaudRate = GeneralMethods.BuadRate;
				this._MySerialPort.DataBits = GeneralMethods.DataBits;
				this._MySerialPort.StopBits = GeneralMethods.StopBits;
				this._MySerialPort.Parity = GeneralMethods.Parity;
				this._MySerialPort.DataReceived += _MySerialPort_DataReceived;

				try
				{
					if (this._MySerialPort.IsOpen == false)
					{
						this._MySerialPort.Open();
						this.RaiseAllCrc();
						result = true;
					}
				}
				catch { result = false; }
			}
			return result;
		}

		/// <summary>
		/// 关闭串口
		/// </summary>
		/// <returns></returns>
		private bool ExecuteClosePort()
		{
			bool result = true;
			try
			{
				if (this._MySerialPort != null)
				{
					this._IsPortClosing = true;
					//while (this._IsPortClosing == true && this._IsPortListening == false)
					Thread.Sleep(200);
					if (this._IsPortListening == false)
					{
						this.IsAutoCommand = false;
						this._MySerialPort.DataReceived -= _MySerialPort_DataReceived;
						this._MySerialPort.Close();
						this._MySerialPort.Dispose();
						this._IsPortClosing = false;
					}
				}
			}
			catch { result = false; }
			return result;
		}

		/// <summary>
		/// 导出文件
		/// </summary>
		/// <param name="expandedName"></param>
		private void ExportFile(string expandedName)
		{
			SaveFileDialog saveFile = new SaveFileDialog();
			saveFile.DefaultExt = string.Format("*.{0}", expandedName);
			saveFile.AddExtension = true;
			saveFile.Filter = string.Format("{0} files|*.{0}", expandedName);
			saveFile.OverwritePrompt = true;
			saveFile.CheckPathExists = true;
			saveFile.FileName = DateTime.Now.ToString("yyyyMMddhhmmss");

			if (saveFile.ShowDialog() == DialogResult.OK)
			{
				try
				{
					if (expandedName == "Csv") { this.WriteTextAsCsv(saveFile.FileName, this.WorkingDatas); MessageBox.Show("export data as csv successfully"); }
					else { this.WriteTextAsTxt(saveFile.FileName, this.DataPacketLogs); MessageBox.Show("export data as txt successfully"); }
				}
				catch
				{

				}
			}
		}

		/// <summary>
		/// 导出CSV文件到本地磁盘
		/// </summary>
		private void ExecuteExportCsvFile()
		{
			this.ExportFile("Csv");
		}

		/// <summary>
		/// 导出TXT文件到本地磁盘
		/// </summary>
		private void ExecuteExportTxtFile()
		{
			this.ExportFile("Txt");
		}

		/// <summary>
		/// 执行最小化窗口
		/// </summary>
		private void ExecuteMinimize()
		{
			this.MainWindowState = System.Windows.WindowState.Minimized;
			this.RaisePropertyChanged(nameof(this.MainWindowState));
		}

		/// <summary>
		/// 将数据写成CSV文件
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="datas"></param>
		public void WriteTextAsCsv(string fileName, ObservableCollection<WorkingData> datas)
		{
			StringBuilder stringBuilder = new StringBuilder();
			var headers = new string[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };

			StringBuilder header = new StringBuilder();
			for (int i = 0; i < this._IsNeedToExportColumns.Length; i++)
			{
				if (this._IsNeedToExportColumns[i] == true) header.AppendFormat("{0},", GeneralMethods.ColumnNames[i]);
			}
			stringBuilder.AppendLine(header.ToString());

			StringBuilder body = new StringBuilder();
			foreach (var data in datas)
			{
				if (this._IsNeedToExportColumns[0]) body.AppendFormat("{0},", data.MotorTemperature);
				if (this._IsNeedToExportColumns[1]) body.AppendFormat("{0},", data.IPMTemperature);
				if (this._IsNeedToExportColumns[2]) body.AppendFormat("{0},", data.FaultCode);
				if (this._IsNeedToExportColumns[3]) body.AppendFormat("{0},", data.DrumSpeed);
				if (this._IsNeedToExportColumns[4]) body.AppendFormat("{0},", data.Load);
				if (this._IsNeedToExportColumns[5]) body.AppendFormat("{0},", data.OOB);
				stringBuilder.AppendLine(body.ToString());
				body.Clear();
			}

			File.WriteAllText(fileName, stringBuilder.ToString());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="logs"></param>
		public void WriteTextAsTxt(string fileName, ObservableCollection<DataPacketLog> logs)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (var data in logs)
			{
				stringBuilder.AppendLine(string.Format("{0}:{1}", data.Label, data.Data));
			}
			File.WriteAllText(fileName, stringBuilder.ToString());
		}

		/// <summary>
		/// 返回下一个自动轮询发送的命令
		/// </summary>
		/// <returns></returns>
		private Command GetPollingCommand()
		{
			Command result = default(Command);
			while (this._AutoCommands[this.AutoCommandIndex].IsJoin == false)
			{
				this.AutoCommandIndex += 1;
			}
			result = this._AutoCommands[this.AutoCommandIndex].Command;
			this.AutoCommandIndex += 1;
			return result;
		}

		/// <summary>
		/// 核对CRC
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		private bool ValidateCrc(byte[] bytes)
		{
			bool result = false;
			var newBytes = bytes.Skip(1).Take(bytes.Length - 3).ToArray();
			var crcBytes = GeneralMethods.GetCrcAsByteArray(newBytes);
			if (bytes[bytes.Length - 2].Equals(crcBytes[0]) && bytes[bytes.Length - 1].Equals(crcBytes[1])) result = true;
			return result;
		}

		/// <summary>
		/// 验证接收到的数据包
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		private bool CheckReceivedDataPacket(byte[] bytes)
		{
			bool result = false;
			byte headerByte = GeneralMethods.HeaderByte;

			//byte lengthByte = GeneralMethods.CommandToByte(2, this._CurrentCommand);
			//if (bytes[2] == 0xA8) return true;
			if (headerByte == bytes[0] && /*lengthByte == bytes.Length &&*/ this.ValidateCrc(bytes) == true) result = true;
			return result;
		}

		private bool CanExecuteManualCommand()
		{
			bool result = false;
			if (this.IsPortOpen == true && this.IsAutoCommand == false && this.IsMasterModel == true) result = true;
			return result;
		}

		private void RaiseAllCanExecuteManualCommandChanged()
		{
			this.ManualCommandPing.RaiseCanExecuteChanged();
			this.ManualCommandRequest.RaiseCanExecuteChanged();
			this.ManualCommandLoadEnable.RaiseCanExecuteChanged();
			this.ManualCommandFctTest.RaiseCanExecuteChanged();
			this.ManualCommandSpeedSet.RaiseCanExecuteChanged();
			this.ManualCommandMotorTemperatureCorrection.RaiseCanExecuteChanged();
			this.ManualCommandFctPing.RaiseCanExecuteChanged();
			this.ManualCommandOobAndDoobLimitSet.RaiseCanExecuteChanged();
		}
	}
}
