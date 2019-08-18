using DotNetMissionSDK.HFL;

namespace DotNetMissionSDK.State.Snapshot.UnitTypeInfo
{
	/// <summary>
	/// Contains immutable unit info state at a moment in TethysGame.Time().
	/// Should only be created by StateSnapshot.
	/// </summary>
	public class GlobalWeaponInfo : GlobalUnitInfo
	{
		public int damageRadius					{ get; private set; }
		public int pixelsSkippedWhenFiring		{ get; private set; }

		public int weaponStrength
		{
			get
			{
				switch (unitType)
				{
					case map_id.AcidCloud:				return 4;
					case map_id.EMP:					return 3;
					case map_id.Laser:					return 2;
					case map_id.Microwave:				return 2;
					case map_id.RailGun:				return 4;
					case map_id.RPG:					return 4;
					case map_id.Starflare:				return 2;
					case map_id.Supernova:				return 3;
					case map_id.Starflare2:				return 1;
					case map_id.Supernova2:				return 2;
					case map_id.ESG:					return 5;
					case map_id.Stickyfoam:				return 3;
					case map_id.ThorsHammer:			return 6;
					case map_id.EnergyCannon:			return 1;
				}

				return 0;
			}
		}

		/// <summary>
		/// Creates an immutable state for a unit type.
		/// </summary>
		/// <param name="unitTypeID">The unit type to pull data from.</param>
		public GlobalWeaponInfo(map_id unitTypeID) : base(unitTypeID)
		{
			UnitInfo info = new UnitInfo(unitTypeID);

			damageRadius				= info.GetDamageRadius();
			pixelsSkippedWhenFiring		= info.GetPixelsSkippedWhenFiring();
		}
	}
}
