using System;

namespace BluetoothLowEnergy.Attribute
{
	[AttributeUsage( AttributeTargets.All )]
	public class GuidAttribute : System.Attribute
	{
		public Guid Guid { get; set; }

		public GuidAttribute( string guidAsString )
		{
			Guid guid;
			if ( !Guid.TryParse( guidAsString, out guid ) )
			{
				throw new InvalidOperationException( "Value entered cannot be converted to a Guid: " + guidAsString );
			}
			Guid = guid;
		}
		public GuidAttribute( Guid guid )
		{
			Guid = guid;
		}
		public GuidAttribute()
			: this( Guid.Empty )
		{
		}
	}
}
