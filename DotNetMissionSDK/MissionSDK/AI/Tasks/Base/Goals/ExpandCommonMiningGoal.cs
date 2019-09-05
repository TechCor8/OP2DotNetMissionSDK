using DotNetMissionSDK.AI.Managers;
using DotNetMissionSDK.AI.Tasks.Base.Mining;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System.Linq;

namespace DotNetMissionSDK.AI.Tasks.Base.Goals
{
	/// <summary>
	/// Controls the importance of expanding common mining operations.
	/// </summary>
	public class ExpandCommonMiningGoal : Goal
	{
		private ExpandRareMiningTask m_ExpandRareMiningTask;
		private SaturateCommonSmelterTask m_TruckSaturationTask;


		public ExpandCommonMiningGoal(int ownerID, MiningBaseState miningBaseState, float weight) : base(ownerID, weight)
		{
			m_Task = new ExpandCommonMiningTask(ownerID, miningBaseState);
			m_Task.GeneratePrerequisites();

			m_ExpandRareMiningTask = new ExpandRareMiningTask(ownerID, miningBaseState);
			m_ExpandRareMiningTask.GeneratePrerequisites();

			m_TruckSaturationTask = new SaturateCommonSmelterTask(ownerID, miningBaseState);
			m_TruckSaturationTask.GeneratePrerequisites();
		}

		/// <summary>
		/// Updates the importance of this goal.
		/// </summary>
		public override void UpdateImportance(StateSnapshot stateSnapshot, PlayerState owner)
		{
			// If we don't have a command center, we are completely dysfunctional. Restoring command access is top priority.
			if (owner.units.commandCenters.Count == 0)
			{
				importance = 1;
				blockLowerPriority = true;
				return;
			}

			blockLowerPriority = false;

			// Importance is based on how much excess metal is in reserve
			int excessOre = owner.ore;

			// If we don't have a smelter in a convec, we need to set aside some metal to build one.
			bool smelterInConvec = owner.units.convecs.FirstOrDefault((convec) => convec.cargoType == map_id.CommonOreSmelter) != null;

			if (!smelterInConvec)
			{
				excessOre -= owner.structureInfo[map_id.CommonOreSmelter].oreCost;

				if (owner.units.convecs.Count == 0)
					excessOre -= owner.vehicleInfo[map_id.ConVec].oreCost;

				if (owner.units.commonOreSmelters.Count == 0)
					blockLowerPriority = true;
			}
			
			bool chargeForVehicleFactory = false;

			// If we don't have a common ore mine, we need to set aside some metal to build one.
			if (owner.units.commonOreMines.Count == 0)
			{
				if (owner.units.roboMiners.Count == 0)
				{
					excessOre -= owner.vehicleInfo[map_id.RoboMiner].oreCost;

					chargeForVehicleFactory = true;

					blockLowerPriority = true;
				}
			}

			// If we don't have a cargo truck, we need to set aside some metal to build one.
			if (owner.units.cargoTrucks.Count == 0)
			{
				excessOre -= owner.vehicleInfo[map_id.CargoTruck].oreCost;

				chargeForVehicleFactory = true;

				blockLowerPriority = true;
			}

			// Include the cost of a vehicle factory and lab if we need those, too.
			if (chargeForVehicleFactory && owner.units.vehicleFactories.Count == 0)
			{
				excessOre -= owner.structureInfo[map_id.VehicleFactory].oreCost;

				if (!owner.HasTechnologyForUnit(stateSnapshot, map_id.VehicleFactory))
					excessOre -= owner.structureInfo[map_id.StandardLab].oreCost;
			}

			// Importance increases as reserves dwindle
			importance = 1 - Clamp(excessOre / 10000.0f);

			importance = Clamp(importance * weight);

			// Don't block if task is complete (full saturation).
			if (m_Task.IsTaskComplete(stateSnapshot))
				blockLowerPriority = false;
		}

		/// <summary>
		/// Performs this goal's task.
		/// </summary>
		public override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			PlayerState owner = stateSnapshot.players[m_OwnerID];

			// If we need to build trucks and the trucks cost rare metal that we don't have, we need to expand rare instead
			if (owner.vehicleInfo[map_id.CargoTruck].rareOreCost > owner.rareOre && !m_TruckSaturationTask.IsTaskComplete(stateSnapshot))
			{
				if (!m_ExpandRareMiningTask.IsTaskComplete(stateSnapshot))
					return m_ExpandRareMiningTask.PerformTaskTree(stateSnapshot, unitActions);
			}

			// Don't expand if smelters are disabled
			foreach (StructureState structure in owner.units.commonOreSmelters)
			{
				if (!structure.isEnabled)
					return true;
			}

			return base.PerformTask(stateSnapshot, unitActions);
		}
	}
}
