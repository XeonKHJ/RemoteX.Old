using System;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace RemoteX.Connection.BluetoothLE.Server
{
    class BluetoothServer
    {
        enum ControlType {Mouse, Keyboard};
        private GattServiceProvider serviceProvider;
        private Guid serviceUuid;

        private Guid mouseControl = new Guid("e1a3da24-0c8e-4935-b0a1-0cf579c867a2");
        private Guid keyboardControl = new Guid("fbb7dbc9-9e35-465a-a2f8-cad2da11b583");
        GattLocalCharacteristicParameters mouseControlCharacteristicParameters = new GattLocalCharacteristicParameters
        {
                CharacteristicProperties = (GattCharacteristicProperties.Read)|(GattCharacteristicProperties.Notify),
                ReadProtectionLevel = GattProtectionLevel.Plain,
                UserDescription = "鼠标控制特征"
        };
        GattLocalCharacteristicParameters keyboardControlCharacteristicParameters = new GattLocalCharacteristicParameters
        {
                CharacteristicProperties = (GattCharacteristicProperties.Read)|(GattCharacteristicProperties.Notify),
                ReadProtectionLevel = GattProtectionLevel.Plain,
                UserDescription = "键盘控制特征"
        };


        public GattServiceProvider ServiceProvider
        {
            private set
            {
                serviceProvider = value;
            }
            get
            {
                return serviceProvider;
            }
        }
        public Guid SerivceUuid
        {
            private set
            {
                serviceUuid = value;
            }
            get
            {
                return serviceUuid;
            }
        }
        private async void createServiceProvider()
        {
            serviceUuid = new Guid("7424694b-356c-4950-8e7c-079e66c86a4a");
            var result = await GattServiceProvider.CreateAsync(serviceUuid);
            if (result.Error == BluetoothError.Success)
            {
                serviceProvider = result.ServiceProvider;
            }
        }

        private async void createCharacteristics(ControlType controlType)
        {
            GattLocalCharacteristicResult result;
            GattLocalCharacteristic characteristic;
            if(controlType == ControlType.Mouse)
            {
                result = await serviceProvider.Service.CreateCharacteristicAsync(mouseControl, mouseControlCharacteristicParameters);
                characteristic = result.Characteristic;
            }
            else if(controlType == ControlType.Keyboard)
            {
                result = await serviceProvider.Service.CreateCharacteristicAsync(keyboardControl, keyboardControlCharacteristicParameters);
                characteristic = result.Characteristic;
            }
        }
    }
}
