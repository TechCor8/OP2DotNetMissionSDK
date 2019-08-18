using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetMissionSDK.AI.Tasks.Base.Starship
{
	public abstract class DeployStarshipModuleTask : Task
	{
		protected map_id m_StarshipModule = map_id.EvacuationModule;

		public DeployStarshipModuleTask(int ownerID) : base(ownerID) { }


		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildRocketTask(ownerID));
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, List<Action> unitActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Fail Check: Research
			if (!owner.HasTechnologyForUnit(stateSnapshot, m_StarshipModule))
				return false;
			
			// Get spaceports with rocket
			List<SpaceportState> spaceports = owner.units.spaceports.Where((SpaceportState unit) => (unit.objectOnPad == map_id.SULV || unit.objectOnPad == map_id.RLV) && unit.isEnabled).ToList();

			// If no spaceports are found, most likely they are not enabled
			if (spaceports.Count == 0)
				return false;

			// Get spaceport with module in rocket and launch
			SpaceportState spaceport = spaceports.Find((SpaceportState unit) => unit.launchpadCargo == m_StarshipModule);
			if (spaceport != null)
			{
				if (spaceport.isBusy) return true;

				unitActions.Add(() => GameState.GetUnit(spaceport.unitID)?.DoLaunch(0, 0, false));
				return true;
			}
			
			// Get spaceport with module in bay and load rocket
			spaceport = spaceports.Find((SpaceportState unit) => unit.GetBayWithCargo(m_StarshipModule) >= 0);
			if (spaceport != null)
			{
				if (spaceport.isBusy) return true;

				int bayIndex = spaceport.GetBayWithCargo(m_StarshipModule);
				unitActions.Add(() =>
				{
					UnitEx liveSpaceport = GameState.GetUnit(spaceport.unitID);
					if (liveSpaceport == null)
						return;

					liveSpaceport.DoTransferLaunchpadCargo(bayIndex);
					liveSpaceport.DoLaunch(0, 0, false);
				});
				return true;
			}

			// Get spaceport with empty bay and build module
			spaceport = spaceports.Find((SpaceportState unit) => unit.HasEmptyBay());
			if (spaceport != null)
			{
				if (spaceport.isBusy) return true;

				// Fail Check: Module cost
				if (!owner.CanAffordUnit(stateSnapshot, m_StarshipModule))
					return false;

				unitActions.Add(() => GameState.GetUnit(spaceport.unitID)?.DoDevelop(m_StarshipModule));
				return true;
			}

			return true;
		}
	}
}
