using BluetoothLowEnergy.Attribute;

namespace BluetoothLowEnergy
{
	/// <summary>
	/// Represents some of the services available to TI Sensortag
	/// Values represented here: http://processors.wiki.ti.com/images/a/a8/BLE_SensorTag_GATT_Server.pdf
	/// Make sure to manually add the service guids to your Windows 8.1 Package.appxmanifest
	///		This must be added manually since the GUI does not support this
	///		Sample:
	///		<Capabilities>
	///			<Capability Name="internetClient" />
	///			<m2:DeviceCapability Name="bluetooth.genericAttributeProfile">  
	///				<m2:Device Id="any">  
	///					<!-- IR Temperature Service -->
	///					<m2:Function Type="serviceId:f000aa00-0451-4000-b000-000000000000"/>  
	///				</m2:Device>  
	///			</m2:DeviceCapability>  
	///		</Capabilities>
	public enum BluetoothServiceType
	{
		[Guid( "00001800-0000-1000-8000-00805f9b34fb" )]
		GenericAccess,
		[Guid( "00001801-0000-1000-8000-00805f9b34fb" )]
		GenericAttribute,
		[Guid( "0000180A-0000-1000-8000-00805f9b34fb" )]
		DeviceInformation,
		[Guid( "0000ffe0-0000-1000-8000-00805f9b34fb" )]
		SimpleKeys,

		[Guid( "f000AA00-0451-4000-b000-000000000000" )]
		IRTemperature,
		[Guid( "F000AA10-0451-4000-B000-000000000000" )]
		Accelerometer,
		[Guid( "F000AA20-0451-4000-B000-000000000000" )]
		Humidity,
		[Guid( "F000AA30-0451-4000-B000-000000000000" )]
		Magnetometer,
		[Guid( "F000AA40-0451-4000-B000-000000000000" )]
		Barometer,
		[Guid( "F000AA50-0451-4000-B000-000000000000" )]
		Gyroscope
	}
}
