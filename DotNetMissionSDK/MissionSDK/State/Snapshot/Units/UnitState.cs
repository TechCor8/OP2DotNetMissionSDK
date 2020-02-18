using DotNetMissionSDK.HFL;

namespace DotNetMissionSDK.State.Snapshot.Units
{
	/// <summary>
	/// Contains immutable unit state at a moment in TethysGame.Time().
	/// Should only be created by StateSnapshot.
	/// NOTE: UnitStates that have the same unitID are considered equal.
	/// </summary>
	public class UnitState
	{
		public int unitID					{ get; private set; }

		public map_id unitType				{ get; private set; }
		public int ownerID					{ get; private set; }

		public bool isBuilding				{ get; private set; }
		public bool isVehicle				{ get; private set; }
		public bool isBusy					{ get; private set; }
		public bool isLive					{ get; private set; }

		public LOCATION position			{ get; private set; }

		// Combat Units
		public map_id weapon				{ get; private set; }
	
		// UnitEx
		public CommandType lastCommand		{ get; private set; }
		public ActionType curAction			{ get; private set; }

		public int creatorID				{ get; private set; }

		public bool isEMPed					{ get; private set; }
		public int timeEMPed				{ get; private set; }

		public int damage					{ get; private set; }

		public bool areLightsOn				{ get; private set; }
		

		public bool doubleFireRate			{ get; private set; }
		public bool invisible				{ get; private set; }

		//public UnitInfo GetUnitInfo()		{ return new UnitInfo(GetUnitType());		}
		//public UnitInfo GetWeaponInfo()	{ return new UnitInfo(GetWeapon());			}

		public bool hasWeapon				{ get { return weapon != map_id.None;	}	}


		/// <summary>
		/// Creates an immutable state from UnitEx.
		/// </summary>
		/// <param name="unit">The unit to pull data from.</param>
		public UnitState(UnitEx unit)
		{
			unitID			= unit.GetStubIndex();

			unitType		= unit.GetUnitType();
			ownerID			= unit.GetOwnerID();

			isBuilding		= unit.IsBuilding();
			isVehicle		= unit.IsVehicle();
			isBusy			= unit.IsBusy();
			isLive			= unit.IsLive();

			position		= unit.GetPosition();

			// Combat Units
			weapon			= unit.GetWeapon();

			if (weapon < map_id.AcidCloud || weapon > map_id.EnergyCannon)
				weapon = map_id.None;
	
			// UnitEx
			lastCommand		= unit.GetLastCommand();
			curAction		= unit.GetCurAction();

			creatorID		= unit.CreatorID();

			isEMPed			= unit.IsEMPed();
			timeEMPed		= unit.GetTimeEMPed();

			damage			= unit.GetDamage();

			areLightsOn		= unit.GetLights();
		
			doubleFireRate	= unit.GetDoubleFireRate();
			invisible		= unit.GetInvisible();
		}

		public override bool Equals(object obj)
		{
			UnitState t = obj as UnitState;
			if (t == null)
				return false;

			return unitID == t.unitID;
		}

		public override int GetHashCode()
		{
			return unitID;
		}
	}
}
