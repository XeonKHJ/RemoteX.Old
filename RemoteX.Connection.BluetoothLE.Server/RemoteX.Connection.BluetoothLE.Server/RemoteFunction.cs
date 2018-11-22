using System;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace RemoteX.Connection.BluetoothLE.Server
{
    class RemoteFunction
    {
        enum ControlType {MouseMove, MouseClick, Keyboard};
        static public readonly Guid MouseMoveControl = new Guid("e1a3da24-0c8e-4935-b0a1-0cf579c867a2");
        static public readonly Guid KeyboardControl = new Guid("fbb7dbc9-9e35-465a-a2f8-cad2da11b583");
        static public readonly Guid MouseClickControl = new Guid("c31e083e-a495-4b75-910c-04203e97753b");

        void RemoteFunction(BluetoothLESerivce service)
        {

        }
        GattLocalCharacteristicParameters mouseControlCharacteristicParameters = new GattLocalCharacteristicParameters
        {
                CharacteristicProperties = (GattCharacteristicProperties.Read)|(GattCharacteristicProperties.Notify),
                ReadProtectionLevel = GattProtectionLevel.Plain,
                UserDescription = "鼠标移动特征"
        };

        GattLocalCharacteristicParameters mouseControlCharacteristicParameters = new GattLocalCharacteristicParameters
        {
                CharacteristicProperties = (GattCharacteristicProperties.Read)|(GattCharacteristicProperties.Notify),
                ReadProtectionLevel = GattProtectionLevel.Plain,
                UserDescription = "鼠标点击特征"
        };
        GattLocalCharacteristicParameters keyboardControlCharacteristicParameters = new GattLocalCharacteristicParameters
        {
                CharacteristicProperties = (GattCharacteristicProperties.Read)|(GattCharacteristicProperties.Notify),
                ReadProtectionLevel = GattProtectionLevel.Plain,
                UserDescription = "键盘控制特征"
        };

        private async void createCharacteristics(ControlType controlType)
        {
            GattLocalCharacteristicResult result;
            GattLocalCharacteristic characteristic;
            if(controlType == ControlType.MouseMove)
            {
                result = await serviceProvider.Service.CreateCharacteristicAsync(MouseMoveControl, mouseControlCharacteristicParameters);
                characteristic = result.Characteristic;
            }
            else if(controlType == ControlType.Keyboard)
            {
                result = await serviceProvider.Service.CreateCharacteristicAsync(KeyboardControl, keyboardControlCharacteristicParameters);
                characteristic = result.Characteristic;
            }
        }
    }
}