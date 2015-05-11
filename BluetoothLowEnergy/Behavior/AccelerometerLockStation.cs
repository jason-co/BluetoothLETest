using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace BluetoothLowEnergy.Behavior
{
	public class AccelerometerLockStation : BluetoothService
	{
		private int _xInitialCoord;
		private int _yInitialCoord;
		private int _zInitialCoord;

		[DllImport( "user32.dll", SetLastError = true )]
		static extern bool LockWorkStation();

		public AccelerometerLockStation( BluetoothServiceType serviceType, BluetoothServiceDataType serviceDataType, BluetoothServiceConfig serviceConfig, GattDeviceService serviceDevice, GattCharacteristic dataCharacteristic, GattCharacteristic configCharacteristic )
			: base( serviceType, serviceDataType, serviceConfig, serviceDevice, dataCharacteristic, configCharacteristic )
		{
		}

		#region overrides

		protected override async Task GetValue()
		{
			var values = ( await _dataCharacteristic.ReadValueAsync() ).Value.ToArray();
			_xInitialCoord = values[0];
			_yInitialCoord = values[1];
			_zInitialCoord = values[2];
		}

		protected override async Task OnValueChanged( GattValueChangedEventArgs args )
		{
			var values = ( await _dataCharacteristic.ReadValueAsync() ).Value.ToArray();
			int x = values[0];
			int y = values[1];
			int z = values[2];

			var distance = GetDistance( x, y, z );

			var diffX = Math.Abs(_xInitialCoord - x);
			var diffy = Math.Abs(_yInitialCoord - y);
			var diffz = Math.Abs(_zInitialCoord - z);

			if ( distance > 200 )
			{
				LockWorkStation();
			}
		}

		#endregion

		#region private methods

		private int GetDistance( int xCoord, int yCoord, int zCoord )
		{
			return ( Math.Abs( _xInitialCoord - xCoord ) + Math.Abs( _yInitialCoord - yCoord ) + Math.Abs( _zInitialCoord - zCoord ) ) / 3;
		}

		#endregion
	}
}
