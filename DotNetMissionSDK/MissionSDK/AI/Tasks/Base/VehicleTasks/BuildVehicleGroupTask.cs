using DotNetMissionSDK.AI.Combat.Groups;
using DotNetMissionSDK.Async;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetMissionSDK.AI.Tasks.Base.VehicleTasks
{
	/// <summary>
	/// Task for building a vehicle group.
	/// </summary>
	public class BuildVehicleGroupTask : Task
	{
		private BuildSingleVehicleTask m_BuildSingleVehicleTask;
		private BuildSingleArachnidTask m_BuildSingleArachnidTask;

		private IEnumerable<VehicleGroup.UnitSlot> m_UnitSlotsToBuild;


		public BuildVehicleGroupTask(int ownerID) : base(ownerID) { }

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			// Task is complete when all vehicle slots are filled.
			foreach (VehicleGroup.UnitSlot slot in m_UnitSlotsToBuild)
			{
				if (slot.unitInSlot == -1)
					return false;
			}

			return true;
		}

		public override void GeneratePrerequisites()
		{
			m_BuildSingleVehicleTask = new BuildSingleVehicleTask(ownerID);
			m_BuildSingleArachnidTask = new BuildSingleArachnidTask(ownerID);

			m_BuildSingleVehicleTask.GeneratePrerequisites();
			m_BuildSingleArachnidTask.GeneratePrerequisites();
		}

		protected override bool CanPerformTask(StateSnapshot stateSnapshot)
		{
			// Task is "best effort" and so can always be performed.
			return true;
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Get number of available factories
			int vehicleFactoryCount = owner.units.vehicleFactories.Count((FactoryState unit) => unit.isEnabled && !unit.isBusy);
			int arachnidFactoryCount = owner.units.arachnidFactories.Count((FactoryState unit) => unit.isEnabled && !unit.isBusy);

			// Loop through each slot and try to build a unit for it.
			foreach (VehicleGroup.UnitSlot combatSlot in m_UnitSlotsToBuild)
			{
				// Supported unit types are in a prioritized order
				//foreach (VehicleGroup.UnitWithWeaponType unitWithWeaponType in combatSlot.supportedSlotTypes)

				for (int i=0; i < 4; ++i) // Attempt up to X passes to find slot
				{
					// Randomly select a unit from supported slot types
					int index = AsyncRandom.Range(0, combatSlot.supportedSlotTypes.Count);
					VehicleGroup.UnitWithWeaponType unitWithWeaponType = combatSlot.supportedSlotTypes[index];

					// Use correct factory task to build unit
					BuildSingleVehicleTask vehicleTask;
					bool hasAvailableFactory;
					
					switch (unitWithWeaponType.unit)
					{
						case map_id.Spider:
						case map_id.Scorpion:
							vehicleTask = m_BuildSingleArachnidTask;
							hasAvailableFactory = arachnidFactoryCount > 0;
							break;

						default:
							vehicleTask = m_BuildSingleVehicleTask;
							hasAvailableFactory = vehicleFactoryCount > 0;
							break;
					}

					// Must set this before the "continue" or AddFactory may break when types are set to "none".
					vehicleTask.SetVehicle(unitWithWeaponType.unit, unitWithWeaponType.weapon);

					if (!hasAvailableFactory)
						continue;

					if (vehicleTask.PerformTaskTree(stateSnapshot, unitActions))
					{
						// If we successfully performed the task, remove the factory from the available count
						if (vehicleTask == m_BuildSingleVehicleTask)
							--vehicleFactoryCount;
						else if (vehicleTask == m_BuildSingleArachnidTask)
							--arachnidFactoryCount;

						// We are done with this slot, move to the next one
						break;
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Sets the vehicle slots to build.
		/// </summary>
		public void SetVehicleGroupSlots(IEnumerable<VehicleGroup.UnitSlot> slotsToBuild)
		{
			m_UnitSlotsToBuild = slotsToBuild;
		}
	}
}
