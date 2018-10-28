using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace RemoteX.Connection.BluetoothListner
{
    public class GattCharacteristicModel:GattServiceModel
    {
        public GattCharacteristic Characteristic;
        public Guid CharateristicUUID;
        public GattCharacteristicModel(GattCharacteristic characteristic):base(characteristic.Service)
        {
            this.Characteristic = characteristic;
            CharateristicUUID = characteristic.Uuid;
        }
        public override string ToString()
        {
            return "特征：" + CharateristicUUID.ToString();
        }
    }
}
