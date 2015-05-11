using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace BluetoothLowEnergy
{
	public class BluetoothService
	{
		protected readonly BluetoothServiceType _serviceType;
		protected readonly BluetoothServiceDataType _serviceDataType;
		protected readonly BluetoothServiceConfig _serviceConfig;
		protected readonly GattDeviceService _serviceDevice;
		protected readonly GattCharacteristic _dataCharacteristic;
		protected readonly GattCharacteristic _configCharacteristic;

		public BluetoothService(
			BluetoothServiceType serviceType,
			BluetoothServiceDataType serviceDataType,
			BluetoothServiceConfig serviceConfig,
			GattDeviceService serviceDevice,
			GattCharacteristic dataCharacteristic,
			GattCharacteristic configCharacteristic )
		{
			_serviceType = serviceType;
			_serviceDataType = serviceDataType;
			_serviceConfig = serviceConfig;
			_serviceDevice = serviceDevice;
			_dataCharacteristic = dataCharacteristic;
			_configCharacteristic = configCharacteristic;

			GetValue();
		}

		#region public properties

		public string Value { get; private set; }

		#endregion

		#region public methods

		public void Register()
		{
			Task.Run( () => SetSensor( true ) );
			_dataCharacteristic.ValueChanged += DataCharacteristicValueChanged;
		}

		public void UnRegister()
		{
			Task.Run( () => SetSensor( false ) );
			_dataCharacteristic.ValueChanged -= DataCharacteristicValueChanged;
		}

		#endregion

		#region event handlers

		private void DataCharacteristicValueChanged( GattCharacteristic sender, GattValueChangedEventArgs args )
		{
			if ( _serviceDataType == BluetoothServiceDataType.AccelerometerData )
			{
				Task.Run( () => OnValueChanged( args ) );
			}
		}

		#endregion

		#region protected methods

		protected virtual async Task OnValueChanged( GattValueChangedEventArgs args ) { }

		#endregion

		#region private methods

		protected virtual async Task GetValue()
		{
			switch ( _serviceDataType )
			{
				case BluetoothServiceDataType.GenericAccessDeviceName:
				case BluetoothServiceDataType.DeviceInformationFirmware:

					var valueBytes = ( await _dataCharacteristic.ReadValueAsync() ).Value.ToArray();
					Value = Encoding.UTF8.GetString( valueBytes, 0, valueBytes.Length );
					break;
			}
		}

		private async Task SetSensor( bool isTurningOn )
		{

			if ( _configCharacteristic != null )
			{
				byte[] value = isTurningOn ? new byte[] { 1 } : new byte[] { 0 };
				_configCharacteristic.WriteValueAsync( value.AsBuffer() );

				if ( _serviceDataType == BluetoothServiceDataType.MagnetometerData )
				{
					var values = ( await _dataCharacteristic.ReadValueAsync() ).Value.ToArray();

					var charact = _serviceDevice.GetCharacteristics( new Guid( "F000AA33-0451-4000-B000-000000000000" ) ).FirstOrDefault();
					charact.WriteValueAsync( ( new Byte[] { 50 } ).AsBuffer() );
					_dataCharacteristic.WriteValueAsync( value.AsBuffer() );

					var notif = _serviceDevice.GetCharacteristics( new Guid( "F0002902-0451-4000-B000-000000000000" ) ).FirstOrDefault();
					if ( notif != null )
					{
						notif.WriteValueAsync( value.AsBuffer() );
					}
				}
			}
		}

		#endregion
	}
}
