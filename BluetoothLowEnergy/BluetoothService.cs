using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace BluetoothLowEnergy
{
	public class BluetoothService
	{
		private readonly BluetoothServiceType _serviceType;
		private readonly BluetoothServiceDataType _serviceDataType;
		private readonly GattDeviceService _serviceDevice;
		private readonly GattCharacteristic _gattCharacteristic;

		public BluetoothService( BluetoothServiceType serviceType, BluetoothServiceDataType serviceDataType, GattDeviceService serviceDevice, GattCharacteristic gattCharacteristic )
		{
			_serviceType = serviceType;
			_serviceDataType = serviceDataType;
			_serviceDevice = serviceDevice;
			_gattCharacteristic = gattCharacteristic;

			GetValue();
		}

		#region public properties

		public string Value { get; private set; }

		#endregion

		#region public methods

		public void Register()
		{
			_gattCharacteristic.ValueChanged += _gattCharacteristic_ValueChanged;
		}

		public void UnRegister()
		{
			_gattCharacteristic.ValueChanged -= _gattCharacteristic_ValueChanged;
		}

		#endregion

		#region event handlers

		private async void _gattCharacteristic_ValueChanged( GattCharacteristic sender, GattValueChangedEventArgs args )
		{
			if ( _serviceDataType == BluetoothServiceDataType.AccelerometerData )
			{
				var values = ( await sender.ReadValueAsync() ).Value.ToArray();
				var x = values[0];
				var y = values[1];
				var z = values[2];
			}
		}

		#endregion

		#region private methods

		private async void GetValue()
		{
			switch ( _serviceDataType )
			{
				case BluetoothServiceDataType.GenericAccessDeviceName:
				case BluetoothServiceDataType.DeviceInformationFirmware:

					var valueBytes = ( await _gattCharacteristic.ReadValueAsync() ).Value.ToArray();
					Value = Encoding.UTF8.GetString( valueBytes, 0, valueBytes.Length );
					break;
			}
		}

		#endregion
	}
}
