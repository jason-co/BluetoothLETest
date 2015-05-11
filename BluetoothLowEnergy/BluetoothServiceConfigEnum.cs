using BluetoothLowEnergy.Attribute;

namespace BluetoothLowEnergy
{
	public enum BluetoothServiceConfig
	{
		None,
		[Guid( "f000AA02-0451-4000-b000-000000000000" )]
		IRTemperatureConfig,
		[Guid( "F000AA12-0451-4000-B000-000000000000" )]
		AccelerometerConfig,
		[Guid( "F000AA22-0451-4000-B000-000000000000" )]
		HumidityConfig,
		[Guid( "F000AA32-0451-4000-B000-000000000000" )]
		MagnetometerConfig,
		[Guid( "F000AA42-0451-4000-B000-000000000000" )]
		BarometerConfig,
		[Guid( "F000AA52-0451-4000-B000-000000000000" )]
		GyroscopeConfig,
	}
}
