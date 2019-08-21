using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetMissionSDK.AI.Tasks.Base.Starship
{
	public class BuildRocketTask : Task
	{
		public BuildRocketTask(int ownerID) : base(ownerID) { }

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Find spaceport with rocket
			return owner.units.spaceports.Count((SpaceportState unit) => unit.objectOnPad == map_id.SULV || unit.objectOnPad == map_id.RLV) > 0;
		}
		
		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildSpaceportTask(ownerID));
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Get active spaceport
			SpaceportState spaceport = owner.units.spaceports.FirstOrDefault((SpaceportState unit) => unit.objectOnPad == map_id.None && unit.isEnabled);

			// If spaceport not found, most likely it is not enabled, but may have EMP missile instead
			if (spaceport == null)
				return false;

			// Do nothing if we are waiting for RLV to return
			if (owner.rlvCount > 0)
				return true;

			if (spaceport.isBusy)
				return true;

			map_id rocketToBuild = map_id.SULV;

			// Build an RLV if we have the technology
			if (owner.HasTechnologyForUnit(stateSnapshot, map_id.RLV))
				rocketToBuild = map_id.RLV;

			// Fail Check: Rocket cost
			if (owner.CanBuildUnit(stateSnapshot, rocketToBuild))
				unitActions.AddUnitCommand(spaceport.unitID, 1, () => GameState.GetUnit(spaceport.unitID)?.DoDevelop(rocketToBuild));

			return true;
		}
	}
}
