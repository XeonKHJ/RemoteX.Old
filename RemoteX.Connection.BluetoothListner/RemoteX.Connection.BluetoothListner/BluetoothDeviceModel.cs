using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth;

namespace RemoteX.Connection.BluetoothListner
{
    public class BluetoothDeviceModel
    {
        public BluetoothLEAdvertisementReceivedEventArgs BluetoothLEAdvertisementReceivedEventArgs { get; private set; }
        protected string name;
        protected ulong address;
        //protected string id;
        //public string Id
        //{
        //    private set
        //    {
        //        id = value;
        //    }
        //    get
        //    {
        //        return id;
        //    }
        //}
        public string Name
        {
            set
            {
                name = value;
            }
            get
            {
                return name;
            }
        }
        public ulong Address
        {
            set
            {
                address = value;
            }
            get
            {
                return address;
            }
        }

        public BluetoothDeviceModel(BluetoothLEAdvertisementReceivedEventArgs bluetoothLEAdvertisementReceivedEventArgs)
        {
            this.BluetoothLEAdvertisementReceivedEventArgs = bluetoothLEAdvertisementReceivedEventArgs;
            Name = bluetoothLEAdvertisementReceivedEventArgs.Advertisement.LocalName;
            Address = bluetoothLEAdvertisementReceivedEventArgs.BluetoothAddress;
        }

        public BluetoothDeviceModel(BluetoothLEDevice bluetoothLEDevice)
        {
            this.Name = bluetoothLEDevice.Name;
            this.Address = bluetoothLEDevice.BluetoothAddress;
        }

        public override string ToString()
        {
            string showName = Name;
            if (showName == "")
            {
                showName = "NULL";
            }

            return showName + "\n" + DeviceAddressString.ToString();
        }

        public string DeviceAddressString
        {
            get
            {
                byte[] decbyte = BitConverter.GetBytes(Address);
                string ans = "";
                for (int i = 0; i < decbyte.Length - 2; i++)
                {
                    ans += Convert.ToString(decbyte[decbyte.Length - 3 - i], 16);
                }
                for (int j = 2; j <= 14; j += 2)
                {
                    ans = ans.Insert(j, ":");
                    j++;
                }
                ans = ans.ToUpper();
                return ans;
            }
        }
    }
}
