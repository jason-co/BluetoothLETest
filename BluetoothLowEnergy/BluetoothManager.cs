﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using BluetoothLowEnergy.Attribute;
using BluetoothLowEnergy.Behavior;

namespace BluetoothLowEnergy
{
	public class BluetoothManager
	{
		private readonly IDictionary<BluetoothServiceType, BluetoothService> _services;

		public BluetoothManager()
		{
			_services = new Dictionary<BluetoothServiceType, BluetoothService>();
		}

		public async Task Initialize()
		{
			_services.Clear();

			foreach ( var serviceType in Enum.GetValues( typeof( BluetoothServiceType ) ).Cast<BluetoothServiceType>() )
			{
				var devices = await DeviceInformation.FindAllAsync( GattDeviceService.GetDeviceSelectorFromUuid( serviceType.ToGuid() ) );
				if ( devices.Count > 0 )
				{
					var device = devices.First();
					var serviceDevice = await GattDeviceService.FromIdAsync( device.Id );
					if ( serviceDevice != null )
					{
						BluetoothServiceDataType serviceDataType = GetServiceDataType( serviceType );
						if ( serviceDataType != BluetoothServiceDataType.None )
						{
							var characteristics = serviceDevice.GetCharacteristics( serviceDataType.ToGuid() );
							if ( characteristics.Any() )
							{
								var characteristic = characteristics.First();

								BluetoothServiceConfig serviceConfig = GetServiceConfig( serviceType );
								var configCharacteristic = serviceDevice.GetCharacteristics( serviceConfig.ToGuid() ).FirstOrDefault();

								BluetoothService service;
								if ( serviceType == BluetoothServiceType.Accelerometer )
								{
									service = new AccelerometerLockStation( serviceType, serviceDataType, serviceConfig, serviceDevice, characteristic, configCharacteristic );
								}
								else
								{
									service = new BluetoothService( serviceType, serviceDataType, serviceConfig, serviceDevice, characteristic, configCharacteristic );
								}
								_services.Add( serviceType, service );
							}
						}
					}
				}
			}
		}

		public void Register( params BluetoothServiceType[] types )
		{
			foreach ( var service in _services.Where( s => types.Contains( s.Key ) ).Select( s => s.Value ) )
			{
				service.Register();
			}
		}

		public void UnRegister( params BluetoothServiceType[] types )
		{
			foreach ( var service in _services.Where( s => types.Contains( s.Key ) ).Select( s => s.Value ) )
			{
				service.UnRegister();
			}
		}

		private BluetoothServiceDataType GetServiceDataType( BluetoothServiceType serviceType )
		{
			switch ( serviceType )
			{
				case BluetoothServiceType.GenericAccess:
					return BluetoothServiceDataType.GenericAccessDeviceName;
				case BluetoothServiceType.DeviceInformation:
					return BluetoothServiceDataType.DeviceInformationFirmware;
				case BluetoothServiceType.IRTemperature:
					return BluetoothServiceDataType.IRTemperatureData;
				case BluetoothServiceType.Accelerometer:
					return BluetoothServiceDataType.AccelerometerData;
				case BluetoothServiceType.Humidity:
					return BluetoothServiceDataType.HumidityData;
				case BluetoothServiceType.Magnetometer:
					return BluetoothServiceDataType.MagnetometerData;
				case BluetoothServiceType.Barometer:
					return BluetoothServiceDataType.BarometerData;
				case BluetoothServiceType.Gyroscope:
					return BluetoothServiceDataType.GyroscopeData;
			}

			return BluetoothServiceDataType.None;
		}

		private BluetoothServiceConfig GetServiceConfig( BluetoothServiceType serviceType )
		{
			switch ( serviceType )
			{
				case BluetoothServiceType.IRTemperature:
					return BluetoothServiceConfig.IRTemperatureConfig;
				case BluetoothServiceType.Accelerometer:
					return BluetoothServiceConfig.AccelerometerConfig;
				case BluetoothServiceType.Humidity:
					return BluetoothServiceConfig.HumidityConfig;
				case BluetoothServiceType.Magnetometer:
					return BluetoothServiceConfig.MagnetometerConfig;
				case BluetoothServiceType.Barometer:
					return BluetoothServiceConfig.BarometerConfig;
				case BluetoothServiceType.Gyroscope:
					return BluetoothServiceConfig.GyroscopeConfig;
			}

			return BluetoothServiceConfig.None;
		}
	}
}
