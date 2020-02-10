
namespace DotNetMissionSDK.Json
{
	public enum TriggerEventType
	{
		CheckVictoryCondition, // Conditions restricted to "OP2 Trigger" type
		CheckDefeatCondition, // Conditions restricted to "OP2 Trigger" type
		OnGameMark,
		OnGameTick,
		OnPlayerDefeated,
		OnVehicleEnteredRegion,
		OnVehicleInRegion,
		OnVehicleExitedRegion,
		OnVehicleCreated,
		OnVehicleDestroyed,
		OnStructureEnteredRegion,
		OnStructureInRegion,
		OnStructureExitedRegion,
		OnStructureCreated,
		OnStructureDestroyed,
		OnStructureOperational,
		OnStructureInoperable,
		OnStructureKitEnteredBay,
		//OnVehicleEnteredBay,
		//OnDisasterEnteredRegion, // Vortex, Lightning, Meteor
		//OnDisasterInRegion,
		//OnDisasterExitedRegion,
		OnResearchStarted = 22,
		OnResearchCompleted,
		OnStarshipModuleDeployed,
		OnWreckageDiscovered,
		OnWreckageLoaded,
		OnConvecLoaded,
		//OnRocketLaunched,
	}
}
