using System;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace RemoteX.Connection.BluetoothLE.Server
{
    class BluetoothServer
    {
        private GattServiceProvider serviceProvider;
        private Guid serviceUuid;
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

        private async void createCharacteristics()
        {

        }
    }
}
