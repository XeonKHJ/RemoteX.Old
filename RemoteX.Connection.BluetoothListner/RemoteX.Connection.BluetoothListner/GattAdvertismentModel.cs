using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;

namespace RemoteX.Connection.BluetoothListner
{
    public class GattAdvertismentModel:BluetoothDeviceModel
    {
        public short SignalStrength;
        public GattAdvertismentModel(BluetoothLEAdvertisementReceivedEventArgs args):base(args)
        {
            SignalStrength = args.RawSignalStrengthInDBm;
        }
        public override string ToString()
        {
            return base.ToString() + "\n信号强度：" + SignalStrength;
        }
    }
}
