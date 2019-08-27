using DotNetMissionSDK.AI.Tasks.Base.VehicleTasks;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetMissionSDK.AI.Tasks.Base.Structure
{
	/// <summary>
	/// This abstract class builds a structure kit and loads it into a convec.
	/// </summary>
	public abstract class BuildStructureKitTask : Task
	{
		protected map_id m_KitToBuild = map_id.Agridome;
		protected map_id m_KitToBuildCargo = map_id.None;

		private map_id[] m_SkipConvecsWithKits = new map_id[0];		// Task will not control convecs containing these kits

		public BuildStructureKitTask(int ownerID) : base(ownerID) { }

		public BuildStructureKitTask Initialize(map_id[] skipConvecsWithKits)
		{
			m_SkipConvecsWithKits = skipConvecsWithKits;

			return this;
		}


		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Task is complete when convec has kit
			return owner.units.convecs.FirstOrDefault((ConvecState unit) => unit.cargoType == m_KitToBuild) != null;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new MaintainStructureFactoryTask(ownerID));
			AddPrerequisite(new BuildConvecTask(ownerID));
		}

		protected override bool CanPerformTask(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Find enabled factory
			bool hasFactoryWithCargo = false;

			List<FactoryState> factories = owner.units.structureFactories.Where((FactoryState unit) => unit.isEnabled).ToList();
			foreach (FactoryState factory in factories)
			{
				if (factory.HasBayWithCargo(m_KitToBuild))
				{
					hasFactoryWithCargo = true;
					break;
				}
			}

			// No factories available, can't build a kit
			if (factories.Count == 0)
				return false;

			// Kit in factory, can build structure
			if (hasFactoryWithCargo)
				return true;

			// colony type, tech, metal requirements
			return owner.CanBuildUnit(stateSnapshot, m_KitToBuild, m_KitToBuildCargo);
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Fail Check: Research
			if (!owner.HasTechnologyForUnit(stateSnapshot, m_KitToBuild))
				return true;

			if (!owner.HasTechnologyForUnit(stateSnapshot, m_KitToBuildCargo))
				return true;

			// Get structure factories with kit
			List<FactoryState> factories = owner.units.structureFactories.Where((FactoryState unit) => unit.HasBayWithCargo(m_KitToBuild)).ToList();
			foreach (FactoryState factoryWithKit in factories)
			{
				if (!factoryWithKit.isEnabled)
					continue;

				// Get convec on dock
				ConvecState convec = owner.units.convecs.FirstOrDefault((ConvecState unit) => unit.IsOnDock(factoryWithKit) && !UnitContainsKit(unit, m_SkipConvecsWithKits));
				if (convec != null)
				{
					// Wait if docking is in progress
					if (convec.curAction != ActionType.moDone)
						return true;

					unitActions.AddUnitCommand(convec.unitID, 1, () => GameState.GetUnit(factoryWithKit.unitID)?.DoTransferCargo(factoryWithKit.GetBayWithCargo(m_KitToBuild)));
					return true;
				}

				// Get closest convec that isn't doing anything
				List<ConvecState> emptyConvecs = owner.units.convecs.Where((ConvecState unit) => unit.cargoType == map_id.None && (unit.curAction == ActionType.moDone || unit.curAction == ActionType.moMove)).ToList();
				if (emptyConvecs.Count > 0)
				{
					emptyConvecs.Sort((a,b) => a.position.GetDiagonalDistance(factoryWithKit.position).CompareTo(b.position.GetDiagonalDistance(factoryWithKit.position)));
					convec = emptyConvecs[0];
				}
				else
				{
					// As a last resort, pull an idle convec that has cargo
					List<ConvecState> idleConvecs = owner.units.convecs.Where((ConvecState unit) => unit.curAction == ActionType.moDone && !UnitContainsKit(unit, m_SkipConvecsWithKits)).ToList();
					if (idleConvecs.Count > 0)
					{
						idleConvecs.Sort((a,b) => a.position.GetDiagonalDistance(factoryWithKit.position).CompareTo(b.position.GetDiagonalDistance(factoryWithKit.position)));
						convec = idleConvecs[0];
					}
				}

				if (convec == null)
					return true;

				// Send convec to dock
				unitActions.AddUnitCommand(convec.unitID, 1, () =>
				{
					UnitEx facWithKit = GameState.GetUnit(factoryWithKit.unitID);
					if (facWithKit != null)
						GameState.GetUnit(convec.unitID)?.DoDock(facWithKit);
				});

				return true;
			}

			if (factories.Count > 0)
				return true;

			// Get active structure factory
			FactoryState factory = owner.units.structureFactories.FirstOrDefault((FactoryState unit) => unit.isEnabled);

			// If factory not found, most likely it is not enabled
			if (factory == null)
				return false;

			if (factory.isBusy)
				return true;

			if (!factory.HasEmptyBay())
				return true;

			if (!owner.CanAffordUnit(stateSnapshot, m_KitToBuild, m_KitToBuildCargo))
				return false;

			// Build kit
			ProduceUnit(unitActions, factory.unitID, m_KitToBuild, m_KitToBuildCargo);
			
			return true;
		}

		private static void ProduceUnit(BotCommands unitActions, int factoryID, map_id kitToBuild, map_id kitToBuildCargo)
		{
			unitActions.AddUnitCommand(factoryID, 1, () => GameState.GetUnit(factoryID)?.DoProduce(kitToBuild, kitToBuildCargo));
		}

		private bool UnitContainsKit(ConvecState unit, map_id[] kits)
		{
			foreach (map_id type in kits)
			{
				if (unit.cargoType == type)
					return true;
			}

			return false;
		}
	}
}
