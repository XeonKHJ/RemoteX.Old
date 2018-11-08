using System;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace RemoteX.Connection.BluetoothLE.Server
{
    class BluetoothLESerivce
    {

        private GattServiceProvider serviceProvider;
        private readonly Guid serviceUuid = new Guid("7424694b-356c-4950-8e7c-079e66c86a4a");


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
            get
            {
                return serviceUuid;
            }
        }
        private async void createServiceProvider()
        {
            var result = await GattServiceProvider.CreateAsync(serviceUuid);
            if (result.Error == BluetoothError.Success)
            {
                serviceProvider = result.ServiceProvider;
            }
        }


    }
}
