using DotNetMissionSDK.AI.Combat.Groups;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Vehicle
{
	/// <summary>
	/// Task for building a vehicle group.
	/// </summary>
	public class BuildVehicleGroupTask : Task
	{
		private BuildSingleVehicleTask m_BuildSingleVehicleTask;
		private BuildSingleArachnidTask m_BuildSingleArachnidTask;

		private IEnumerable<VehicleGroup.UnitSlot> m_UnitSlotsToBuild;


		public BuildVehicleGroupTask() { }
		public BuildVehicleGroupTask(PlayerInfo owner) : base(owner) { }

		public override bool IsTaskComplete()
		{
			// Task is complete when all vehicle slots are filled.
			foreach (VehicleGroup.UnitSlot slot in m_UnitSlotsToBuild)
			{
				if (slot.unitInSlot == null)
					return false;
			}

			return true;
		}

		public override void GeneratePrerequisites()
		{
			m_BuildSingleVehicleTask = new BuildSingleVehicleTask(owner);
			m_BuildSingleArachnidTask = new BuildSingleArachnidTask(owner);

			m_BuildSingleVehicleTask.GeneratePrerequisites();
			m_BuildSingleArachnidTask.GeneratePrerequisites();
		}

		protected override bool CanPerformTask()
		{
			// Task is "best effort" and so can always be performed.
			return true;
		}

		protected override bool PerformTask()
		{
			// Get number of available factories
			int vehicleFactoryCount = owner.units.vehicleFactories.FindAll((UnitEx unit) => unit.IsEnabled() && !unit.IsBusy()).Count;
			int arachnidFactoryCount = owner.units.arachnidFactories.FindAll((UnitEx unit) => unit.IsEnabled() && !unit.IsBusy()).Count;

			bool shouldBuildVehicleFactory = false;
			bool shouldBuildArachnidFactory = false;

			// Loop through each slot and try to build a unit for it.
			foreach (VehicleGroup.UnitSlot combatSlot in m_UnitSlotsToBuild)
			{
				// Supported unit types are in a prioritized order
				foreach (VehicleGroup.UnitWithWeaponType unitWithWeaponType in combatSlot.supportedSlotTypes)
				{
					BuildSingleVehicleTask vehicleTask;
					bool hasAvailableFactory;
					
					switch (unitWithWeaponType.unit)
					{
						case map_id.Spider:
						case map_id.Scorpion:
							vehicleTask = m_BuildSingleArachnidTask;
							hasAvailableFactory = arachnidFactoryCount > 0;
							shouldBuildArachnidFactory = !hasAvailableFactory;
							break;

						default:
							vehicleTask = m_BuildSingleVehicleTask;
							hasAvailableFactory = vehicleFactoryCount > 0;
							shouldBuildVehicleFactory = !hasAvailableFactory;
							break;
					}

					if (!hasAvailableFactory)
						continue;

					vehicleTask.SetVehicle(unitWithWeaponType.unit, unitWithWeaponType.weapon);

					if (vehicleTask.PerformTaskTree())
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

			// If factories are not available, build more
			if (shouldBuildVehicleFactory)
			{
				m_BuildSingleVehicleTask.AddFactory();
				m_BuildSingleVehicleTask.PerformTaskTree();
			}
			else if (shouldBuildArachnidFactory)
			{
				m_BuildSingleArachnidTask.AddFactory();
				m_BuildSingleArachnidTask.PerformTaskTree();
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
