
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State.Snapshot.UnitTypeInfo;

namespace DotNetMissionSDK.State.Snapshot.Units
{
	/// <summary>
	/// Contains immutable unit state at a moment in TethysGame.Time().
	/// Should only be created by StateSnapshot.
	/// </summary>
	public class StructureState : UnitState
	{
		private MAP_RECT m_Rect;

		public StructureInfo structureInfo	{ get; private set; }
		public WeaponInfo weaponInfo		{ get; private set; }

		public bool hasPower				{ get; private set; }
		public bool hasWorkers				{ get; private set; }
		public bool hasScientists			{ get; private set; }
		public bool isInfected				{ get; private set; }
		public bool isCritical				{ get; private set; }

		/// <summary>
		/// True if structure is enabled.
		/// </summary>
		public bool isEnabled				{ get; private set; }

		/// <summary>
		/// True if structure is disabled. A structure is not disabled if it is idle.
		/// </summary>
		public bool isDisabled				{ get; private set; }

		public LOCATION dockLocation		{ get; private set; }


		public MAP_RECT GetRect(bool includeBulldozedArea=false)
		{
			if (includeBulldozedArea)
			{
				MAP_RECT bulldozedRect = m_Rect;
				bulldozedRect.Inflate(1);
				return bulldozedRect;
			}

			return m_Rect;
		}


		/// <summary>
		/// Creates an immutable state from UnitEx.
		/// </summary>
		/// <param name="unit">The unit to pull data from.</param>
		/// <param name="structureInfo">The info that applies to this unit.</param>
		/// <param name="weaponInfo">The info that applies to this unit's weapon.</param>
		public StructureState(UnitEx unit, StructureInfo structureInfo, WeaponInfo weaponInfo) : base(unit)
		{
			this.structureInfo	= structureInfo;
			this.weaponInfo		= weaponInfo;

			hasPower			= unit.HasPower();
			hasWorkers			= unit.HasWorkers();
			hasScientists		= unit.HasScientists();
			isInfected			= unit.IsInfected();
			isCritical			= damage / (float)structureInfo.hitPoints >= 0.75f;

			isEnabled			= unit.IsEnabled();
			isDisabled			= unit.IsDisabled();

			dockLocation		= unit.GetDockLocation();

			m_Rect				= unit.GetRect(false);
		}
	}
}
