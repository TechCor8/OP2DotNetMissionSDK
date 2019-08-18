using DotNetMissionSDK.Triggers;
using System.Runtime.InteropServices;

namespace DotNetMissionSDK.State.Game
{
	/// <summary>
	/// Contains persistent state that is stored in the SaveData object. Used by PlayerStarship class.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi, Pack=1)]
	public class PlayerStarshipSaveData
	{
		// Starship module counts
		public byte EDWARDSatelliteCount;
		public byte solarSatelliteCount;
		public byte ionDriveModuleCount;
		public byte fusionDriveModuleCount;
		public byte commandModuleCount;
		public byte fuelingSystemsCount;
		public byte habitatRingCount;
		public byte sensorPackageCount;
		public byte skydockCount;
		public byte stasisSystemsCount;
		public byte orbitalPackageCount;
		public byte phoenixModuleCount;

		public byte rareMetalsCargoCount;
		public byte commonMetalsCargoCount;
		public byte foodCargoCount;
		public byte evacuationModuleCount;
		public byte childrenModuleCount;
	}

	/// <summary>
	/// Maintains starship module counts for a player.
	/// </summary>
	public class PlayerStarship
	{
		public const int TriggerID_EDWARDSatellite		= 30000;
		public const int TriggerID_SolarSatellite		= 30001;
		public const int TriggerID_IonDriveModule		= 30002;
		public const int TriggerID_FusionDriveModule	= 30003;
		public const int TriggerID_CommandModule		= 30004;
		public const int TriggerID_FuelingSystems		= 30005;
		public const int TriggerID_HabitatRing			= 30006;
		public const int TriggerID_SensorPackage		= 30007;
		public const int TriggerID_Skydock				= 30008;
		public const int TriggerID_StasisSystems		= 30009;
		public const int TriggerID_OrbitalPackage		= 30010;
		public const int TriggerID_PhoenixModule		= 30011;
		
		public const int TriggerID_RareMetalsCargo		= 30012;
		public const int TriggerID_CommonMetalsCargo	= 30013;
		public const int TriggerID_FoodCargo			= 30014;
		public const int TriggerID_EvacuationModule		= 30015;
		public const int TriggerID_ChildrenModule		= 30016;

		private PlayerStarshipSaveData m_SaveData;
		private TriggerManager m_TriggerManager;
		private int m_PlayerID;

		// Starship module counts
		public byte EDWARDSatelliteCount	{ get { return m_SaveData.EDWARDSatelliteCount;		} }
		public byte solarSatelliteCount		{ get { return m_SaveData.solarSatelliteCount;		} }
		public byte ionDriveModuleCount		{ get { return m_SaveData.ionDriveModuleCount;		} }
		public byte fusionDriveModuleCount	{ get { return m_SaveData.fusionDriveModuleCount;	} }
		public byte commandModuleCount		{ get { return m_SaveData.commandModuleCount;		} }
		public byte fuelingSystemsCount		{ get { return m_SaveData.fuelingSystemsCount;		} }
		public byte habitatRingCount		{ get { return m_SaveData.habitatRingCount;			} }
		public byte sensorPackageCount		{ get { return m_SaveData.sensorPackageCount;		} }
		public byte skydockCount			{ get { return m_SaveData.skydockCount;				} }
		public byte stasisSystemsCount		{ get { return m_SaveData.stasisSystemsCount;		} }
		public byte orbitalPackageCount		{ get { return m_SaveData.orbitalPackageCount;		} }
		public byte phoenixModuleCount		{ get { return m_SaveData.phoenixModuleCount;		} }

		public byte rareMetalsCargoCount	{ get { return m_SaveData.rareMetalsCargoCount;		} }
		public byte commonMetalsCargoCount	{ get { return m_SaveData.commonMetalsCargoCount;	} }
		public byte foodCargoCount			{ get { return m_SaveData.foodCargoCount;			} }
		public byte evacuationModuleCount	{ get { return m_SaveData.evacuationModuleCount;	} }
		public byte childrenModuleCount		{ get { return m_SaveData.childrenModuleCount;		} }


		public PlayerStarship(TriggerManager triggerManager, int playerID, PlayerStarshipSaveData starshipSaveData)
		{
			m_TriggerManager = triggerManager;
			m_PlayerID = playerID;
			m_SaveData = starshipSaveData;

			triggerManager.onTriggerFired += OnTriggerFired;
		}

		/// <summary>
		/// Creates triggers for new mission. Only call when a new mission is started.
		/// </summary>
		public void InitializeNewMission(PlayerStarshipSaveData starshipSaveData)
		{
			m_SaveData = starshipSaveData;

			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_EDWARDSatellite,	true, false, m_PlayerID, map_id.EDWARDSatellite,	map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_SolarSatellite,	true, false, m_PlayerID, map_id.SolarSatellite,		map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_IonDriveModule,	true, false, m_PlayerID, map_id.IonDriveModule,		map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_FusionDriveModule, true, false, m_PlayerID, map_id.FusionDriveModule,	map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_CommandModule,		true, false, m_PlayerID, map_id.CommandModule,		map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_FuelingSystems,	true, false, m_PlayerID, map_id.FuelingSystems,		map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_HabitatRing,		true, false, m_PlayerID, map_id.HabitatRing,		map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_SensorPackage,		true, false, m_PlayerID, map_id.SensorPackage,		map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_Skydock,			true, false, m_PlayerID, map_id.Skydock,			map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_StasisSystems,		true, false, m_PlayerID, map_id.StasisSystems,		map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_OrbitalPackage,	true, false, m_PlayerID, map_id.OrbitalPackage,		map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_PhoenixModule,		true, false, m_PlayerID, map_id.PhoenixModule,		map_id.None, 1, CompareMode.GreaterEqual));

			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_RareMetalsCargo,	true, false, m_PlayerID, map_id.RareMetalsCargo,	map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_CommonMetalsCargo, true, false, m_PlayerID, map_id.CommonMetalsCargo,	map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_FoodCargo,			true, false, m_PlayerID, map_id.FoodCargo,			map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_EvacuationModule,	true, false, m_PlayerID, map_id.EvacuationModule,	map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_ChildrenModule,	true, false, m_PlayerID, map_id.ChildrenModule,		map_id.None, 1, CompareMode.GreaterEqual));
		}

		private void OnTriggerFired(TriggerStub trigger)
		{
			// Only handle triggers for this player
			if (trigger.stubData.playerID != m_PlayerID)
				return;

			// Increment starship counters
			switch (trigger.id)
			{
				case TriggerID_EDWARDSatellite:		++m_SaveData.EDWARDSatelliteCount; 		break;
				case TriggerID_SolarSatellite:		++m_SaveData.solarSatelliteCount;		break;
				case TriggerID_IonDriveModule:		++m_SaveData.ionDriveModuleCount;		break;
				case TriggerID_FusionDriveModule:	++m_SaveData.fusionDriveModuleCount;	break;
				case TriggerID_CommandModule:		++m_SaveData.commandModuleCount;		break;
				case TriggerID_FuelingSystems:		++m_SaveData.fuelingSystemsCount;		break;
				case TriggerID_HabitatRing:			++m_SaveData.habitatRingCount;			break;
				case TriggerID_SensorPackage:		++m_SaveData.sensorPackageCount;		break;
				case TriggerID_Skydock:				++m_SaveData.skydockCount;				break;
				case TriggerID_StasisSystems:		++m_SaveData.stasisSystemsCount;		break;
				case TriggerID_OrbitalPackage:		++m_SaveData.orbitalPackageCount;		break;
				case TriggerID_PhoenixModule:		++m_SaveData.phoenixModuleCount;		break;

				case TriggerID_RareMetalsCargo:		++m_SaveData.rareMetalsCargoCount;		break;
				case TriggerID_CommonMetalsCargo:	++m_SaveData.commonMetalsCargoCount;	break;
				case TriggerID_FoodCargo:			++m_SaveData.foodCargoCount;			break;
				case TriggerID_EvacuationModule:	++m_SaveData.evacuationModuleCount;		break;
				case TriggerID_ChildrenModule:		++m_SaveData.childrenModuleCount;		break;
			}
		}

		/// <summary>
		/// Disposes events.
		/// </summary>
		public void Dispose()
		{
			m_TriggerManager.onTriggerFired -= OnTriggerFired;
		}
	}
}
