using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace RemoteX.Connection.BluetoothListner
{
    public class GattServiceModel : BluetoothDeviceModel
    {
        public Guid ServiceUUID;
        public GattDeviceService Service;
        public GattServiceModel(GattDeviceService service):base(service.Device)
        {
            Service = service;
            ServiceUUID = service.Uuid;
        }

        public override string ToString()
        {
            return base.ToString() + ": " + ServiceUUID.ToString();
        }
    }
}
