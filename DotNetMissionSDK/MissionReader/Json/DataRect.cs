using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class DataRect
	{
		[DataMember(Name = "MinX")]		public int xMin			{ get; set; }
		[DataMember(Name = "MinY")]		public int yMin			{ get; set; }
		[DataMember(Name = "MaxX")]		public int xMax			{ get; set; }
		[DataMember(Name = "MaxY")]		public int yMax			{ get; set; }

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
			return MAP_RECT.FromMinMax(data.xMin, data.yMin, data.xMax, data.yMax);
		}
	}
}
