using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using DotNetMissionSDK.State.Snapshot.UnitTypeInfo;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Structure
{
	/// <summary>
	/// This abstract class maintains a certain number of connected/repaired structures of a type.
	/// </summary>
	public abstract class MaintainStructureTask : Task
	{
		protected map_id m_StructureToMaintain = map_id.Agridome;
		
		public BuildStructureTask buildTask				{ get; protected set;	}
		protected ConnectStructureTask connectTask		{ get; set;				}

		public int targetCountToMaintain = 1;


		public MaintainStructureTask(int ownerID) : base(ownerID) { }

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			int targetCountMet = 0;

			IReadOnlyCollection<UnitState> units = owner.units.GetListForType(m_StructureToMaintain);
			if (units.Count >= targetCountToMaintain)
			{
				UnitInfoState info = owner.GetUnitInfo(m_StructureToMaintain);

				foreach (UnitState unit in owner.units.GetListForType(m_StructureToMaintain))
				{
					StructureState building = (StructureState)unit;

					// If any structures that need tubes aren't connected, task is not complete
					if (BuildStructureTask.NeedsTube(m_StructureToMaintain) && !stateSnapshot.commandMap.ConnectsTo(ownerID, building.GetRect()))
						continue;

					// If any structures crippled, task is not complete
					if (building.damage / (float)info.hitPoints >= 0.75f)
						continue;

					++targetCountMet;
				}

				return targetCountMet >= targetCountToMaintain;
			}
			
			return false;
		}

		public override void GeneratePrerequisites()
		{
		}

		protected override bool CanPerformTask(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			bool needEarthworker = false;
			bool needRepairUnit = false;

			foreach (UnitState unit in owner.units.GetListForType(m_StructureToMaintain))
			{
				StructureState building = (StructureState)unit;

				// If any structures disconnected, earthworker required
				if (!stateSnapshot.commandMap.ConnectsTo(ownerID, building.GetRect()))
				{
					needEarthworker = true;
					break;
				}
			}

			foreach (UnitState unit in owner.units.GetListForType(m_StructureToMaintain))
			{
				StructureState building = (StructureState)unit;

				// If any structures crippled, repair unit required
				UnitInfoState info = owner.GetUnitInfo(m_StructureToMaintain);

				if (building.damage / (float)info.hitPoints >= 0.75f)
				{
					needRepairUnit = true;
					break;
				}
			}

			if (needEarthworker && owner.units.earthWorkers.Count > 0)
				return true;

			if (needRepairUnit && (owner.units.convecs.Count > 0 || owner.units.repairVehicles.Count > 0 || owner.units.spiders.Count > 0))
				return true;

			return false;
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			return true;
		}
	}
}
