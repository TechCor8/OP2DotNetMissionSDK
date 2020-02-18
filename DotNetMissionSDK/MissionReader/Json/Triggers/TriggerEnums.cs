
namespace DotNetMissionSDK.Json
{
	/// <summary>
	/// Non-negative values are used as the PlayerID.
	/// </summary>
	public enum TriggerPlayerCategory
	{
		Any				= -1,
		All				= -2,
		TriggerOwner	= -3,
		EventPlayer		= -4,
		OwnerAllies		= -5,
		OwnerEnemies	= -6,
		EventAllies		= -7,
		EventEnemies	= -8,
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

	public enum TriggerValueType
	{
		Integer,
		PlayerCategory,
		UnitCategory,
		Difficulty,
		ColonyType,
		MoraleLevel,
		TruckCargo,
		Command,
		Action,
		MapID,
		MarkerType,
		Direction,
		OreType,
		Yield,
		Variant,
		Region,
		Color,
		BotType,
		String
	}

	public class TriggerValueTypeUtility
	{
		/// <summary>
		/// Returns an integer value for a string that represents a TriggerValueType.
		/// </summary>
		/// <param name="value">The string to convert to an integer.</param>
		/// <param name="type">The TriggerValueType of the string.</param>
		public static int GetStringAsInt(string value, TriggerValueType type)
		{
			switch (type)
			{
				case TriggerValueType.PlayerCategory:		return GetEnumOrInt<TriggerPlayerCategory>(value);
				case TriggerValueType.UnitCategory:			return GetEnumOrInt<TriggerUnitCategory>(value);
				case TriggerValueType.Difficulty:			return GetEnumOrInt<PlayerDifficulty>(value);
				case TriggerValueType.ColonyType:			return GetEnumOrInt<TriggerColonyType>(value);
				case TriggerValueType.MoraleLevel:			return GetEnumOrInt<MoraleLevel>(value);
				case TriggerValueType.TruckCargo:			return GetEnumOrInt<TruckCargo>(value);
				case TriggerValueType.Command:				return GetEnumOrInt<HFL.CommandType>(value);
				case TriggerValueType.Action:				return GetEnumOrInt<HFL.ActionType>(value);
				case TriggerValueType.MapID:				return GetEnumOrInt<map_id>(value);
				case TriggerValueType.MarkerType:			return GetEnumOrInt<MarkerType>(value);
				case TriggerValueType.Direction:			return GetEnumOrInt<UnitDirection>(value);
				case TriggerValueType.OreType:				return GetEnumOrInt<BeaconType>(value);
				case TriggerValueType.Yield:				return GetEnumOrInt<Yield>(value);
				case TriggerValueType.Variant:				return GetEnumOrInt<Variant>(value);
				case TriggerValueType.Region:				return GetEnumOrInt<TriggerRegion>(value);
				case TriggerValueType.Color:				return GetEnumOrInt<PlayerColor>(value);
				case TriggerValueType.BotType:				return GetEnumOrInt<AI.BotType>(value);
				//case ValueType.String:
			}
			
			int result;
			int.TryParse(value, out result);
			return result;
		}

		private static int GetEnumOrInt<T>(string val) where T : struct
		{
			int result;
			if (int.TryParse(val, out result))
				return result;

			return System.Convert.ToInt32(GetEnum<T>(val));
		}

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}
