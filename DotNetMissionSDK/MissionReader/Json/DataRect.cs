using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class DataRect
	{
		[DataMember(Name = "MinX")]		public int minX			{ get; private set; }
		[DataMember(Name = "MinY")]		public int minY			{ get; private set; }
		[DataMember(Name = "MaxX")]		public int maxX			{ get; private set; }
		[DataMember(Name = "MaxY")]		public int maxY			{ get; private set; }

		/// <summary>
		/// Gets random point in rect. Does not clip.
		/// </summary>
		/// <returns>Random point in rect.</returns>
		public LOCATION GetRandomPointInRect()
		{
			return new MAP_RECT(this).GetRandomPointInRect();
		}

		public static implicit operator MAP_RECT(DataRect data)
		{
			return new MAP_RECT(data.minX, data.minY, data.maxX, data.maxY);
		}
	}
}
