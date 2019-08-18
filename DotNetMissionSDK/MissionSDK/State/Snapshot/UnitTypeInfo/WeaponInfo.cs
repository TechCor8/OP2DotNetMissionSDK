using DotNetMissionSDK.HFL;

namespace DotNetMissionSDK.State.Snapshot.UnitTypeInfo
{
	/// <summary>
	/// Contains immutable unit info state at a moment in TethysGame.Time().
	/// Should only be created by StateSnapshot.
	/// </summary>
	public class WeaponInfo : UnitInfoState
	{
		public int weaponRange						{ get; private set; }
		public int turretTurnRate					{ get; private set; }
		public int concussionDamage					{ get; private set; }
		public int penetrationDamage				{ get; private set; }
		public int reloadTime						{ get; private set; }
		public int weaponSightRange					{ get; private set; }


		/// <summary>
		/// Creates an immutable state for a unit type.
		/// </summary>
		/// <param name="unitTypeID">The unit type to pull data from.</param>
		/// <param name="playerID">The player to pull data from.</param>
		public WeaponInfo(map_id unitTypeID, int playerID) : base(unitTypeID, playerID)
		{
			UnitInfo info = new UnitInfo(unitTypeID);

			weaponRange			= info.GetWeaponRange(playerID);
			turretTurnRate		= info.GetTurretTurnRate(playerID);
			concussionDamage	= info.GetConcussionDamage(playerID);
			penetrationDamage	= info.GetPenetrationDamage(playerID);
			reloadTime			= info.GetReloadTime(playerID);
			weaponSightRange	= info.GetWeaponSightRange(playerID);
		}
	}
}
