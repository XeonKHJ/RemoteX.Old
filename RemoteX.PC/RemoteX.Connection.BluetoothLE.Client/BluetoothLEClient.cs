using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;
using System.Windows;

namespace RemoteX.Connection.BluetoothLE.Client
{
    public class BluetoothLEClient
    {
        BluetoothLEDevice bluetoothLEDevice;
        GattDeviceService deviceService;

        private readonly Guid mouseControl = new Guid("e1a3da24-0c8e-4935-b0a1-0cf579c867a2");
        private readonly Guid keyboardControl = new Guid("fbb7dbc9-9e35-465a-a2f8-cad2da11b583");

        async void connectService(DeviceInformation deviceInfo)
        {
            bluetoothLEDevice = await BluetoothLEDevice.FromIdAsync(deviceInfo.Id);
            if (bluetoothLEDevice.ConnectionStatus == BluetoothConnectionStatus.Connected)
            {
                connectService(bluetoothLEDevice);
            }
        }

        async void connectService(BluetoothLEDevice bluetoothLEDevice)
        {
            var result = await bluetoothLEDevice.GetGattServicesForUuidAsync(new Guid());
            deviceService = result.Services[0];
        }

        async void connectMouseControl()
        {
            var result = await deviceService.GetCharacteristicsForUuidAsync(mouseControl);
            try
            {
                var mouseControlCharacteristic = result.Characteristics[0];
                GattCommunicationStatus status = await mouseControlCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
                if (status == GattCommunicationStatus.Success)
                {
                    // Server has been informed of clients interest.
                    mouseControlCharacteristic.ValueChanged += MouseControlCharacteristic_ValueChanged;
                }
            }
            catch (Exception expt)
            {

            }
        }

        private void MouseControlCharacteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            var reader = DataReader.FromBuffer(args.CharacteristicValue);
            receiveMousePosition(new Point(reader.ReadDouble(), reader.ReadDouble()));
        }

        public void receiveMousePosition(Point point)
        {

        }
    }
}
