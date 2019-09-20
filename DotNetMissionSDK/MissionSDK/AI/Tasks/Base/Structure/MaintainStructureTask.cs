using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using DotNetMissionSDK.State.Snapshot.UnitTypeInfo;
using System.Collections.Generic;
using System.Linq;

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
					if (building.damage / (float)info.hitPoints >= RepairStructureTask.CriticalDamagePercentage)
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

			UnitInfoState info = owner.GetUnitInfo(m_StructureToMaintain);

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
				if (building.damage / (float)info.hitPoints >= RepairStructureTask.CriticalDamagePercentage)
				{
					needRepairUnit = true;
					break;
				}
			}

			if (needEarthworker)
				return owner.units.earthWorkers.Count > 0;

			if (needRepairUnit)
				return (owner.units.convecs.Count > 0 || owner.units.repairVehicles.Count > 0 || owner.units.spiders.Count > 0);

			// Get convec with kit
			if (owner.units.convecs.FirstOrDefault((unit) => unit.cargoType == m_StructureToMaintain) != null)
				return true;

			if (owner.CanBuildUnit(m_StructureToMaintain))
				return true;

			return false;
		}

		protected override TaskResult PerformTask(StateSnapshot stateSnapshot, TaskRequirements restrictedRequirements, BotCommands unitActions)
		{
			return new TaskResult(TaskRequirements.None);
		}

		/// <summary>
		/// Gets the list of structures to activate.
		/// </summary>
		/// <param name="stateSnapshot">The state snapshot to use for performing task calculations.</param>
		/// <param name="structureIDs">The list to add structures to.</param>
		public override void GetStructuresToActivate(StateSnapshot stateSnapshot, List<int> structureIDs)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			int numToActivate = targetCountToMaintain;

			// Add structures that we are maintaining
			foreach (UnitState unit in owner.units.GetListForType(m_StructureToMaintain))
			{
				// The assumption here is that maintain structure tasks share activated structures.
				// e.g. Task A activates 2 smelters and Task B activates 1 smelter = 2 smelters are activated (not 3).
				if (!structureIDs.Contains(unit.unitID))
					structureIDs.Add(unit.unitID);

				--numToActivate;
				if (numToActivate == 0)
					break;
			}

			// If there are not enough structures of this type, parse priorities in children
			IReadOnlyCollection<UnitState> units = owner.units.GetListForType(m_StructureToMaintain);
			if (units.Count < targetCountToMaintain)
				base.GetStructuresToActivate(stateSnapshot, structureIDs);
		}
	}
}
