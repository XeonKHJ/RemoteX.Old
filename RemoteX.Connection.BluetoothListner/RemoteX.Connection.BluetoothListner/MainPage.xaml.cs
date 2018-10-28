using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace RemoteX.Connection.BluetoothListner
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<GattAdvertismentModel> _ListDevices;
        private ObservableCollection<GattServiceModel> _ListServers;
        private ObservableCollection<GattCharacteristicModel> _ListCharacteristics;
        List<GattAdvertismentModel> readyToRemove;
        List<GattAdvertismentModel> readyToAdd;
        Object readyToLock;
        public MainPage()
        {
            //fuck git
            this.InitializeComponent();
            readyToLock = new object();
            _ListDevices = new ObservableCollection<GattAdvertismentModel>();
            lvBTDevice.ItemsSource = _ListDevices;
            StartUnpairedBluetoothLEDeviceWatcher();
        }

        public void StartUnpairedBluetoothLEDeviceWatcher()
        {
            var bluetoothLEWatcher = new BluetoothLEAdvertisementWatcher();
            bluetoothLEWatcher.Received += BluetoothLEWatcher_Received;
            bluetoothLEWatcher.Stopped += BluetoothLEWatcher_Stoped;
            bluetoothLEWatcher.ScanningMode = BluetoothLEScanningMode.Active; 
            bluetoothLEWatcher.Start();
        }

        private void BluetoothLEWatcher_Stoped(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementWatcherStoppedEventArgs args)
        {
            TextBlock_Debug.Text = "停止监视";
        }

        private void BluetoothLEWatcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            var bluetoothLEAdvertisement = args.Advertisement;
            var uuids = args.Advertisement.ServiceUuids;
            if (args.Advertisement.LocalName != "")
            {
                _UpdateItemList(args);
            }
        }

        private void _UpdateItemList(BluetoothLEAdvertisementReceivedEventArgs args)
        {
            readyToRemove = new List<GattAdvertismentModel>();
            readyToAdd = new List<GattAdvertismentModel>();
            lock (readyToLock)
            {
                foreach (var btModel in _ListDevices)
                {
                    if (btModel.Address == args.BluetoothAddress)
                    {
                        readyToRemove.Add(btModel);
                    }
                }
                readyToAdd.Add(new GattAdvertismentModel(args));
            }
            Invoke(_ChangeItemList);
        }

        public async void Invoke(Action action, Windows.UI.Core.CoreDispatcherPriority Priority = Windows.UI.Core.CoreDispatcherPriority.Normal)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Priority, () => { action(); });
        }

        private void _ChangeItemList()
        {
            lock (readyToLock)
            {
                foreach (var readyToRemoveModel in readyToRemove)
                {
                    _ListDevices.Remove(readyToRemoveModel);
                }
                foreach (var readyToAddModel in readyToAdd)
                {
                        _ListDevices.Add(readyToAddModel);
                }
            }
        }

        BluetoothLEDevice LEDevice;
        private async void ConnecttoService(GattAdvertismentModel bluetoothDeviceModel)
        {
            ulong address = bluetoothDeviceModel.Address;
            //LEDevice = await BluetoothLEDevice.FromBluetoothAddressAsync(100033822969601);
            //LEDevice = await BluetoothLEDevice.FromIdAsync("HONGJIA-SURFACEGO");
            LEDevice = await BluetoothLEDevice.FromBluetoothAddressAsync(address);
            LEDevice.ConnectionStatusChanged += LEDevice_ConnectionStatusChanged;
            try
            {
                if (LEDevice == null)
                {
                    throw new Exception("配对设备" + bluetoothDeviceModel.Name + "失败。");
                }
                Debug.WriteLine(LEDevice.DeviceId);
                Debug.WriteLine("连接设备" + LEDevice.Name + "成功。");
                Invoke(() => { TextBlock_Debug.Text = "连接设备" + LEDevice.Name + "成功。"; });
                AchieveService(LEDevice);
            }
            catch(Exception exp)
            {
                Debug.WriteLine(exp.Message);
                Invoke(() => { TextBlock_Debug.Text = exp.Message; });
            }
        }

        private void LEDevice_ConnectionStatusChanged(BluetoothLEDevice sender, object args)
        {
            Debug.WriteLine("连接状态改变");
        }

        private async void AchieveService(BluetoothLEDevice LEDevice)
        {
            var result = await LEDevice.GetGattServicesAsync();
            var services = result.Services;
            _ListServers = new ObservableCollection<GattServiceModel>();
            foreach (var service in services)
            {
                Debug.WriteLine("服务：" + service.Uuid);
                _ListServers.Add(new GattServiceModel(service));
            }
            //listServices = _ListServers;
            lvBTDevice.ItemsSource = _ListServers;
        }

        private async void AchieveCharateristic(GattDeviceService service)
        {
                var result = await service.GetCharacteristicsAsync();
                var characteristics = result.Characteristics;
                _ListCharacteristics = new ObservableCollection<GattCharacteristicModel>();
                foreach (var characteristic in characteristics)
                {
                    Debug.WriteLine(characteristic.Service.Uuid + ": " + characteristic.Uuid);
                    _ListCharacteristics.Add(new GattCharacteristicModel(characteristic));
                }
                lvBTDevice.ItemsSource = _ListCharacteristics;
        }
        
        private async void lvBTDevice_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is GattAdvertismentModel bluetoothDeviceModel)
            {
                ConnecttoService(bluetoothDeviceModel);
            }
            else if (e.ClickedItem is GattCharacteristicModel characteristicModel)
            {
                var characteristic = characteristicModel.Characteristic;
                if (characteristic.CharacteristicProperties == GattCharacteristicProperties.Read)
                {
                    var charateristicResult = await characteristic.GetDescriptorsAsync();
                    var descriptors = charateristicResult.Descriptors;
                    foreach (var descriptor in descriptors)
                    {
                        Debug.WriteLine(descriptor.Uuid);
                    }
                    var valueResult = await characteristic.ReadValueAsync();
                    var buffer = valueResult.Value;
                    byte[] abc = buffer.ToArray();
                    Invoke(() => { TextBlock_Debug.Text = Encoding.UTF32.GetString(abc); });
                }
                else if(characteristic.CharacteristicProperties == GattCharacteristicProperties.Write)
                {
                    var charateristicResult = await characteristic.GetDescriptorsAsync();
                    var descriptors = charateristicResult.Descriptors;
                    foreach (var descriptor in descriptors)
                    {
                        Debug.WriteLine(descriptor.Uuid);
                    }

                    var sendBuffer = System.Text.Encoding.UTF32.GetBytes(TextBox_Send.Text).AsBuffer();
                    var writer = new DataWriter();
                    //Invoke(() =>
                    //{
                    //    writer.WriteString(TextBox_Send.Text);
                    //});
                    writer.WriteString(TextBox_Send.Text);
                    var valueResult = await characteristic.WriteValueAsync(writer.DetachBuffer());
                    Invoke(() => { TextBlock_Debug.Text = "fuck off"; });
                }
                else if(characteristic.CharacteristicProperties == GattCharacteristicProperties.WriteWithoutResponse)
                {
                    var charateristicResult = await characteristic.GetDescriptorsAsync();
                    var descriptors = charateristicResult.Descriptors;
                    foreach (var descriptor in descriptors)
                    {
                        Debug.WriteLine(descriptor.Uuid);
                    }

                    var sendBuffer = System.Text.Encoding.UTF32.GetBytes(TextBox_Send.Text).AsBuffer();
                    var writer = new DataWriter();
                    byte value = 2;
                    writer.WriteByte(value);

                    var valueResult = await characteristic.WriteValueAsync(writer.DetachBuffer());
                    Invoke(() => { TextBlock_Debug.Text = "fuck off response"; });
                }
            }
            else if (e.ClickedItem is GattServiceModel serviceModel)
            {
                AchieveCharateristic(serviceModel.Service);
            }
        }

        private void Button_Restart_Click(object sender, RoutedEventArgs e)
        {
            _ListDevices.Clear();
            lvBTDevice.ItemsSource = _ListDevices;
            StartUnpairedBluetoothLEDeviceWatcher();
        }
    }
}
