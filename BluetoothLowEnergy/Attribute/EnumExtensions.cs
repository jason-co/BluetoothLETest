using System;
using System.Reflection;

namespace BluetoothLowEnergy.Attribute
{
	public static class EnumExtensions
	{
		public static Guid ToGuid( this Enum en )
		{
			var attribute = en.GetType().GetTypeInfo().GetDeclaredField(en.ToString()).GetCustomAttribute<GuidAttribute>();
			if (attribute != null)
			{
				return attribute.Guid;
			}
			return default( Guid );
		}
	}
}
