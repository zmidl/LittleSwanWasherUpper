   M S B A M L   `   `   `  ���� ?  ;Demo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Demo.ViewModelsDemo.ViewModels  'clr-namespace:Demo.Views
Demo.Views  1clr-namespace:Demo.ViewModelsDemo.ViewModels  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem ? ;ZMCL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=*clr-namespace:ZMCL.MyControl;assembly=ZMCLZMCL.MyControl S5clr-namespace:ZMCL.MyControl.WhiteStyle;assembly=ZMCLZMCL.MyControl.WhiteStyle Q4clr-namespace:ZMCL.MyControl.WhiteGold;assembly=ZMCLZMCL.MyControl.WhiteGold     Demo.Views.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     1convertclr-namespace:Demo.ViewModels     *localclr-namespace:Demo.Views     3	ViewModelclr-namespace:Demo.ViewModels     9System&clr-namespace:System;assembly=mscorlib     @	MyControl*clr-namespace:ZMCL.MyControl;assembly=ZMCL     IMyWhite5clr-namespace:ZMCL.MyControl.WhiteStyle;assembly=ZMCL     J	WhiteGold4clr-namespace:ZMCL.MyControl.WhiteGold;assembly=ZMCL     -     �Name$  MainWindow1�� � WindowStyle$
 None=� � Title$ 
MainWindow��$	��620��$
��1000�� � WindowState �� *MainWindowState+ �� UpdateSourceTrigger$ PropertyChanged=� �� FallbackValue$ 	Maximized�� � 	Resources    Column0Header �� Orientation*  $ZMCL.MyControl.WhiteGold.WG_CheckBox d� 	IsChecked	 �� Mode
 �� Path �� RelativeSource �� AncestorType  Column1Header  Column2Header  Column3Header  Column4Header  Column5Header  EllipseStyle �� 
TargetType �� Property �� Value  EllipseStyle2  TextBlockStyle1 �� EndPoint �� 
StartPoint � Color � Offset �� ScaleX �� ScaleY3  -System.Windows.Media.Effects.DropShadowEffect   Color   	Direction   ShadowDepth   Opacity 	 TextBlockStyle2 �� BasedOn 
 TextBlockStyle3  TextBlockStyle4.   (Demo.ViewModels.HexStringToByteConverter  HexStringToByteConverter��%}	  &	        &	 �     &	 �    &	 �    &	 �    &	 h    &	 B    &	 �    &	     &		 {    7  &	
 �    7  &	 y    7  &	 �    �� .R��� $ 
Horizontal=�.+�  $��15��$��15�� �� $	 TwoWay=�$%
 DataContext.IsNeedToExportTempM� �� $ WindowL��� $��2���� $
��Auto������ $
 DataContext.TempM� �� $ WindowL��� .R��� $ 
Horizontal=�.+�  $��15��$��15�� �� $	 TwoWay=�$'
 !DataContext.IsNeedToExportTempIPM� �� $ WindowL��� $��2���� $
��Auto������ $
 DataContext.TempIPM� �� $ WindowL��� .R��� $ 
Horizontal=�.+�  $��15��$��15�� �� $	 TwoWay=�$)
 #DataContext.IsNeedToExportFaultCode� �� $ WindowL��� $��2���� $
��Auto������ $
 DataContext.FaultCode� �� $ WindowL��� .R��� $ 
Horizontal=�.+�  $��15��$��15�� �� $	 TwoWay=�$)
 #DataContext.IsNeedToExportDrumSpeed� �� $ WindowL��� $��2���� $
��Auto������ $
 DataContext.DrumSpeed� �� $ WindowL��� .R��� $ 
Horizontal=�.+�  $��15��$��15�� �� $	 TwoWay=�$$
 DataContext.IsNeedToExportLoad� �� $ WindowL��� $��2���� $
��Auto������ $
 DataContext.Load� �� $ WindowL��� .R��� $ 
Horizontal=�.+�  $��15��$��15�� �� $	 TwoWay=�$#
 DataContext.IsNeedToExportOOB� �� $ WindowL��� $��2���� $
��Auto������ $
 DataContext.OOB� �� $ WindowL��� " A�.���  � ��$ 10����  � ��$ 10����  � ��
 �������  � ��$ 10,0,0,0q��� " A�.���  � ��$ 10����  � ��$ 10����  � ��
 �������  � ��$ 0,10,0,0q��� " ��.���  �@��TextAlignment$ Center=���  � �� �� $ 0.5,1&�$ 0.5,0&�.��� $ 	#FFD7C9A7��� $ White��$ 1Q���  � ��$ 30(���  � ��$
 Bold"���  � z� V� .~��� $	 0.7Q�$	 0.8Q��� �� T� ��  � ��$ Center=���  �@J�Effect   $ Black��$ 1Q�$ 1Q�$ 10Q��� 8   " ��.���  � �� �� $ 0.5,1&�$ 0.5,0&�.��� $ 	#FFE7FFCE��$ 0.017Q�� $ 	#FF81BF37��$ 1Q��� 8   " ��.���  � �� �� $ 0.5,1&�$ 0.5,0&�.��� $ 	#FFFFF0CC��$ 0Q�� $ 	#FF985C00��$ 1Q��� 8   " ��.���  � �� �� $ 0.5,1&�$ 0.5,0&�.��� $ 	#FFA7A9D7��� $ White��$ 1Q�  .��(� $
��auto��$
��auto�� (� Stretch$
 Fill=�.���� $	��620��$
��1000��.+��� $	��158��$	��313�� �� Left$	 610��
 �� Top$	 263��.+�   ZMCL.MyControl.UcTextBlock  #��[ $��SerialPortSwitch��$ 5��$ 19��  #��[
 $	 195��$ 19������ *
PortStatus+$	 OFF��  #��[ $��AutoCmdSwitch��$ 66��$ 6��  #��[
 $	 195��$ 68������ *AutoCommandStatus+$	 OFF��  #��[
 $	 195��$	 117������ *WorkingModeStatus+$	 OFF��  #��[ $��WorkingMode��$	 112��$ 6��  ZMCL.MyControl.UcCheckBox  -     Name$ MyCheckBox_PortSwitch��$	 249��$ 19��$��30��$��50�� J� RenderTransformOrigin$ 0.5,0.5&� �� *
IsPortOpen+  $��50��$��30��$	 250��$ 68��$ 0.5,0.5&�{��� *
IsPortOpen+ �� *IsAutoCommand+$ PropertyChanged=�  $��50��$��30��$	 250��$	 117��$ 0.5,0.5&� �� *IsMasterModel+$ PropertyChanged=��� $��43��$	��870��$ 14��$	 562��.+�/  )ZMCL.MyControl.WhiteGold.WG_RoundCheckBox  $��30��$��30��$ 6��$ 5�� �� *
IsPingJoin+  #��[ $
��0xB2��$ 5��$ 50��  #��[ $	 120��$ 5������ *ID+  �� 	Converter#  [ $
 0x00��  #��[ $
��0xA1��$ 5��$	 190��  #��[ $
��0x03��$ 5��$	 260��  ZMCL.MyControl.UcTextBox  $��52��$��30��$	 330��$ 5��{��� *
IsPortOpen+���� *PlatformMessage+#  [   $��52��$��30��$	 400��$ 5��{��� *
IsPortOpen+���� *
PingData+#  [   $��52��$��30��$	 470��$ 5��{��� *
IsPortOpen+���� *MotorNumber+#  [   #��[ $	 540��$ 5������ *PingCrcHigh+#  [ $
 0x00��  #��[ $	 610��$ 5������ *
PingCrcLow+#  [ $
 0x00��  ZMCL.MyControl.UcButton  $
��Ping��$��14(�$	��180��$��30��$	 680��$ 5������ *Ping+{��� *
IsPortOpen+�� $��43��$	��590��$ 14��$	 257��.+�  $��30��$��30��$ 6��$ 5�� �� *IsRequestJoin+  #��[ $
��0xB2��$ 5��$ 50��  #��[ $ 5��$	 120������ *ID+#  [ $
 0x00��  #��[ $
��0xA2��$ 5��$	 190��  #��[ $
��0x00��$ 5��$	 260��  #��[ $ 5��$	 330������ *RequestCrcHigh+#  [ $
 0x00��  #��[ $ 5��$	 400������ *RequestCrcLow+#  [ $
 0x00��  $��Request��$��14(�$	��110��$��30��$	 470��$ 5������ *	Request+{��� *
IsPortOpen+�� $��43��$	��590��$ 14��$	 301��.+�  $��30��$��30��$ 6��$ 5�� �� *IsLoadEnableJoin+  #��[ $
��0xB2��$ 5��$ 50��  #��[ $ 5��$	 120������ *ID+#  [ $
 0x00��  #��[ $
��0xA3��$ 5��$	 190��  #��[ $
��0x00��$ 5��$	 260��  #��[ $ 5��$	 330������ *LoadEnableCrcHigh+#  [ $
 0x00��  #��[ $ 5��$	 400������ *LoadEnableCrcLow+#  [ $
 0x00��  $��Load Enable��$��14(�$	��110��$��30��$	 470��$ 5������ *
LoadEnable+{��� *
IsPortOpen+�� $��43��$	��590��$ 14��$	 345��.+�  $��30��$��30��$ 6��$ 5�� �� *IsFctTestJoin+  #��[ $
��0xB2��$ 5��$ 50��  #��[ $ 7��$	 120������ *ID+#  [ $
 0x00��  #��[ $
��0xA4��$ 5��$	 190��  #��[ $
��0x00��$ 5��$	 260��  #��[ $ 5��$	 330������ *FctTestCrcHigh+#  [ $
 0x00��  #��[ $ 5��$	 400������ *FctTestCrcLow+#  [ $
 0x00��  $��FCT Test��$��14(�$	��110��$��30��$	 470��$ 5������ *	FctTest+{��� *
IsPortOpen+�� $��43��$	��870��$ 14��$	 476��.+�  $��30��$��30��$ 6��$ 5�� �� *IsSpeedSetJoin+  #��[ $
��0xB2��$ 5��$ 50��  #��[ $ 5��$	 120������ *ID+#  [ $
 0x00��  #��[ $
��0xA5��$ 5��$	 190��  #��[ $
��0x04��$ 5��$	 260��  $��52��$��30��$	 330��$ 5��{��� *
IsPortOpen+���� *AccelerateTimeHigh+#  [   $��52��$��30��$	 400��$ 5��{��� *
IsPortOpen+���� *AccelerateTimeLow+#  [   $��52��$��30��$	 470��$ 5��{��� *
IsPortOpen+���� *DrumSpeedHigh+#  [   $��52��$��30��$	 540��$ 5��{��� *
IsPortOpen+���� *DrumSpeedLow+#  [   #��[ $ 5��$	 610������ *SpeedSetCrcHigh+#  [ $
 0x00��  #��[ $ 5��$	 680������ *SpeedSetCrcLow+#  [ $
 0x00��  $	��110��$��30��$��SpeedSet��$��14(�$	 750��$ 5������ *
SpeedSet+{��� *
IsPortOpen+�� $��43��$	��590��$ 14��$	 389��.+�  $��30��$��30��$ 6��$ 5�� �� *" IsMotorTemperatureCorrectionJoin+  #��[ $
��0xB2��$ 5��$ 50��  #��[ $ 5��$	 120������ *ID+#  [ $
 0x00��  #��[ $
��0xA6��$ 5��$	 190��  #��[ $
��0x00��$ 5��$	 260��  #��[ $ 5��$	 330������ *#!MotorTemperatureCorrectionCrcHigh+#  [ $
 0x00��  #��[ $ 5��$	 400������ *" MotorTemperatureCorrectionCrcLow+#  [ $
 0x00��  $��
Correction��$��14(�$	��110��$��30��$	 470��$ 5������ *MotorTemperatureCorrection+{��� *
IsPortOpen+�� $��43��$	��590��$ 14��$	 433��.+�  $��30��$��30��$ 6��$ 5�� �� *IsFctPingJoin+  #��[ $
��0xB2��$ 5��$ 50��  #��[ $ 7��$	 120������ *ID+#  [ $
 0x00��  #��[ $
��0xA7��$ 4��$	 190��  #��[ $
��0x00��$ 5��$	 260��  #��[ $ 5��$	 330������ *FctPingCrcHigh+#  [ $
 0x00��  #��[ $ 5��$	 400������ *FctPingCrcLow+#  [ $
 0x00��  $	��110��$��30��$��FCT_Ping��$��14(�$	 470��$ 5������ *	FctPing+{��� *
IsPortOpen+�� $��43��$	��870��$ 14��$	 519��.+�  $��30��$��30��$ 6��$ 5�� �� *IsOobAndDoobLimitSetJoin+  #��[ $
��0xB2��$ 5��$ 50��  #��[ $ 5��$	 120������ *ID+#  [ $
 0x00��  #��[ $
��0xA8��$ 5��$	 190��  #��[ $
��0x04��$ 5��$	 260��  $��52��$��30��$	 330��$ 5��{��� *
IsPortOpen+���� *	OobLimit1+#  [   $��52��$��30��$	 400��$ 5��{��� *
IsPortOpen+���� *	OobLimit2+#  [   $��52��$��30��$	 470��$ 5��{��� *
IsPortOpen+���� *	DoobLimit+#  [   $��52��$��30��$	 540��$ 5��{��� *
IsPortOpen+���� *	OobData+#  [   #��[ $ 5��$	 610������ *OobAndDoobLimitSetCrcHigh+#  [ $
 0x00��  #��[ $ 5��$	 680������ *OobAndDoobLimitSetCrcLow+#  [ $
 0x00��  $��OOB/DOOB��$��14(�$	��110��$��30��$	 750��$ 5������ *OobAndDoobLimitSet+{��� *
IsPortOpen+)	  #ZMCL.MyControl.WhiteGold.WG_ListBox	  -   ! 	 Name$! listbox1��$��Hidden=�$	��540��$	��230��$ 14��$ 16������ *DataPacketLogs+���� .R��� $ 
Horizontal=�$��30��$	��504��$��
0,-2,-5,-2q�.+��� $��30��$��30��" �� Padding$" 	0,-10,0,0q�$��30(����� *Label+���� *Color+�� $��30��$
��auto������ *Data+  #��[ $	��ID:��$	 625��$	 435��  #��[ $��Port:��$	 725��$	 434��  $��50��$��30��$	 658��$	 435��{��� *
IsPortOpen+���� *ID+#  [ .������ $ 0.5,1&�$ 0.5,0&�.��� $ Black��$ 0Q�� $ White��$ 1Q�.��"
  ZMCL.MyControl.UcGlassButton
  # � ToolTip$# export log data as text�����	#FF0093FF$��50��$��50��$	 928��$��0q�$	 340������ *ExportTxtFile+.��F� $��18��$��18��$ �� Stretch$
$ Fill=����	#FF0075FF�����@]� @T� ѠI@T� �@]� �g5 �@]� @T� Ѐ�[ �� р�� �� � -1 z � -1 -1׀���O��� �� р�[ ��OӐ�J ��O 	= ��A 	=  -1� 	=  	= � 	= , ��J �� ��[ �� ���� ������  $� р�[  $� р�[ ������ �������� р� � р�[  � р�[ ��� р���� 	$��0��
  $# minimize�����	#FF0093FF$��50��$��50��$	 928��$��0q�$	 480������ *
Minimize+.��F� $��18��$��18��$
$ Fill=����	#FF0075FF�����@o@��� ���- ��� ���-  	= �@o@ 	= �@o@�� ���- �� ��� �� ,  	= �@B  $� ���- �� �� �� р�� ��� z @o@� z ��O� $� ��O� $� @o@р�� ���@o@�����^ $�  �� �� ���^ 	= ���^,  8Q�� @o@�� 	$��0��
  $# export file as csv�����	#FF0093FF$��50��$��50��$	 928��$��0q�$	 410������ *ExportCsvFile+.��F� $��18��$��18��$
$ Fill=����	#FF0075FF����Ѐ�[ �� р�� �� � -1 z � -1 -1׀���O��� �� р�[ ��O� 	=  -1��� �� � 	=  	= ׀�[ �� ��� �� �@]� �g5 �@]� @T� ѠI@T� �@]� �g5 �@f�ا �@]� �ا �@]� @]� р�� @]� � � ��� р�� @]� ��ا @]� ��ا �ا ���j �ا ���j @]� � z @]� ��ا ��� � z �����j �����j  -1��ا  -1��ا ��р�� ��Q � �G�zT0@р�� ���@]� ���@]�  -1�@f -1�@f��� $� ���@]� ��� � $� @]� �@f@]� �@f�ا 	$��0��
  $# refresh port status�����	#FF0093FF$��50��$��50��$	 928��$��0q�$	 270������ *	Refresh+.��F� $��18��$��18��$
$ Fill=����	#FF0075FF�����PQ��` �@1� ��J ���  	=  �  	= � 	=  �   z  z � �  -1  z  z �P�  -1�yD
P-��� �PQ��� S0� �G�zT0@P�� �� � ��׀�[  � ���[ ��[ � � ��[ ���[ ��[ �`o� ��[ �� �f ���Q80@��v �@]� �ا � -1�ا � -1 	= �PQ��` 	$��0��
  $# to exit app�����	#FF0093FF$��50��$��50��$	 928��$��0q�$	 550������ *	ExitAPP+.��F� $��18��$��18��$
$ Fill=����	#FF0075FF�������!��- �@KL ��- �P[; ��- ��- P[; ��- @KL ���- @T� �@KL @T� �@KL @KL ���!@KL ���!��!�@KL ��!�@KL ��� ���- ��� ���- ��!�@KL @o@ �� �� ���!@o@�@o@��! �� �� �@o@@KL �@o@P[; ��2��- ��!��- � ϙ `�� ��y� @f� ��  � ��y� ��j � ϙ �S� �0T� �ا ���- �ا ���- @]� �0T� @]� � ϙ `�� 	$��0��  ZMCL.MyControl.UcDataGrid  &   System.Windows.Controls.DataGrid%   AutoGenerateColumns% .  $	��414��$	��230��$	 568��$ 16������ *WorkingDatas+&   Columns& 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn'   HeaderTemplate#' [  1  +System.Windows.Controls.DataGridBoundColumn(   Binding( �� *MotorTemperature+  #' [ ( �� *IPMTemperature+  #' [ ( �� *	FaultCode+  #' [ ( �� *	DrumSpeed+  #' [ ( �� *Load+  #' [ ( �� *OOB+) J� Effect)   $ 1Q�$ 	#FFA4A4A4��  ZMCL.MyControl.UcComboBox  -   *  Name$
* cbx1��$��90��$��42��$��14(�$	 783��$	 429������ *SerialPortNames+$ PropertyChanged=�+ �� SelectedItem+ �� *SelectedPortName+$	 OneWayToSource=�$ PropertyChanged=����� $ 0.5,1&�$ 0.5,0&�.��� $ 	#FFC3AA0B��$ 1Q�� $ 	#FF5F4D04��