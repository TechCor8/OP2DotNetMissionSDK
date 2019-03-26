﻿using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class DataLocation
	{
		[DataMember(Name = "X")]		public int x			{ get; private set; }
		[DataMember(Name = "Y")]		public int y			{ get; private set; }


		public DataLocation(LOCATION location)
		{
			x = location.x;
			y = location.y;
		}

		public static implicit operator LOCATION(DataLocation data)
		{
			return new LOCATION(data.x, data.y);
		}

		public static implicit operator DataLocation(LOCATION data)
		{
			return new DataLocation(data);
		}
	}
}
