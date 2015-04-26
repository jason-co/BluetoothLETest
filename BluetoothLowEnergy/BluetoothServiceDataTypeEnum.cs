using BluetoothLowEnergy.Attribute;

namespace BluetoothLowEnergy
{
	public enum BluetoothServiceDataType
	{
		None,
		[Guid( "00002A00-0000-1000-8000-00805f9b34fb" )]
		GenericAccessDeviceName,
		[Guid( "00002A26-0000-1000-8000-00805f9b34fb" )]
		DeviceInformationFirmware,
		[Guid( "f000AA01-0451-4000-b000-000000000000" )]
		IRTemperatureData,
		[Guid( "F000AA11-0451-4000-B000-000000000000" )]
		AccelerometerData,
		[Guid( "F000AA21-0451-4000-B000-000000000000" )]
		HumidityData,
		[Guid( "F000AA31-0451-4000-B000-000000000000" )]
		MagnetometerData,
		[Guid( "F000AA41-0451-4000-B000-000000000000" )]
		BarometerData,
		[Guid( "F000AA51-0451-4000-B000-000000000000" )]
		GyroscopeData,
	}
}
