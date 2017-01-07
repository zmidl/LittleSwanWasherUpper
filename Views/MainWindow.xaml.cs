using Demo.ViewModels;
using System.Windows;
using Demo.Views;
using Demo.Models;

namespace Demo.Views
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		private MainWindowViewModel ViewModel = new MainWindowViewModel();

		public MainWindow()
		{
			InitializeComponent();
			this.DataContext = this.ViewModel;
			this.ViewModel.NotifyUIRaiseData += ViewModel_NotifyUIRaiseData;
			this.ViewModel.NotifyUIContinueAutoCommand += ViewModel_NotifyUIContinueAutoCommand;
		}

		private void ViewModel_NotifyUIContinueAutoCommand(object sender, System.EventArgs e)
		{
			this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
				new System.Action(() =>
				{
					this.ViewModel.TransmitDataPacket();
				}));
		}

		private void ViewModel_NotifyUIRaiseData(object sender, NotifyUIRaiseDataArgs e)
		{
			this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
				new System.Action(() =>
				{
					this.ViewModel.RaiseNewDataToUI(e.Mark, e.DataPackets, e.Message);
					this.listbox1.ScrollIntoView(this.listbox1.Items[this.listbox1.Items.Count - 1]);
				}));
		}
	}
}
