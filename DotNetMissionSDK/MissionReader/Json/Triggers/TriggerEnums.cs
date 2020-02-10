
namespace DotNetMissionSDK.Json
{
	/// <summary>
	/// Non-negative values are used as the PlayerID.
	/// </summary>
	public enum TriggerPlayerCategory
	{
		TriggerOwner	= -1,
		EventPlayer		= -2,
		Any				= -3,
		All				= -4,
		Allies			= -5,
		Enemies			= -6,
	}

	/// <summary>
	/// Non-negative values are used as the UnitID.
	/// </summary>
	public enum TriggerUnitCategory
	{
		EventUnit		= -1,
	}

	/// <summary>
	/// Non-negative values are used as the RegionID.
	/// </summary>
	public enum TriggerRegion
	{
		Anywhere		= -1,
	}

	public enum TriggerColonyType
	{
		Eden,
		Plymouth
	}

	public enum TriggerCompare
	{
		Equals,
		NotEquals,
		GreaterThan,
		GreaterThanOrEqual,
		LessThan,
		LessThanOrEqual
	}

	public enum TriggerModifier
	{
		Set,
		Add,
		Subtract
	}
}
