using DotNetMissionSDK.AI.Tasks.Base.Vehicle;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;
using System.Collections.Generic;

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

		public BuildStructureKitTask() { }
		public BuildStructureKitTask(PlayerInfo owner) : base(owner) { }

		public BuildStructureKitTask Initialize(map_id[] skipConvecsWithKits)
		{
			m_SkipConvecsWithKits = skipConvecsWithKits;

			return this;
		}


		public override bool IsTaskComplete()
		{
			// Task is complete when convec has kit
			return owner.units.convecs.Find((UnitEx unit) => unit.GetCargo() == m_KitToBuild) != null;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildStructureFactoryTask());
			AddPrerequisite(new BuildConvecTask());
		}

		protected override bool PerformTask()
		{
			// Fail Check: Research
			UnitInfo unitInfo = new UnitInfo(m_KitToBuild);
			TechInfo techInfo = Research.GetTechInfo(unitInfo.GetResearchTopic());

			if (!owner.player.HasTechnology(techInfo.GetTechID()))
				return false;

			// Get structure factories with kit
			List<UnitEx> factories = owner.units.structureFactories.FindAll((UnitEx unit) => unit.HasBayWithCargo(m_KitToBuild));
			foreach (UnitEx factoryWithKit in factories)
			{
				if (!factoryWithKit.IsEnabled())
					continue;

				// Get convec on dock
				UnitEx convec = owner.units.convecs.Find((UnitEx unit) => unit.IsOnDock(factoryWithKit) && !UnitContainsKit(unit, m_SkipConvecsWithKits));
				if (convec != null)
				{
					// Wait if docking is in progress
					if (convec.GetCurAction() != ActionType.moDone)
						return true;

					factoryWithKit.DoTransferCargo(factoryWithKit.GetBayWithCargo(m_KitToBuild));
					return true;
				}

				// Get closest convec that isn't doing anything
				List<UnitEx> emptyConvecs = owner.units.convecs.FindAll((UnitEx unit) => unit.GetCargo() == map_id.None && (unit.GetCurAction() == ActionType.moDone || unit.GetCurAction() == ActionType.moMove));
				if (emptyConvecs.Count > 0)
				{
					emptyConvecs.Sort((a,b) => a.GetPosition().GetDiagonalDistance(factoryWithKit.GetPosition()).CompareTo(b.GetPosition().GetDiagonalDistance(factoryWithKit.GetPosition())));
					convec = emptyConvecs[0];
				}
				else
				{
					// As a last resort, pull an idle convec that has cargo
					List<UnitEx> idleConvecs = owner.units.convecs.FindAll((UnitEx unit) => unit.GetCurAction() == ActionType.moDone && !UnitContainsKit(unit, m_SkipConvecsWithKits));
					if (idleConvecs.Count > 0)
					{
						idleConvecs.Sort((a,b) => a.GetPosition().GetDiagonalDistance(factoryWithKit.GetPosition()).CompareTo(b.GetPosition().GetDiagonalDistance(factoryWithKit.GetPosition())));
						convec = idleConvecs[0];
					}
				}

				if (convec == null)
					return true;

				// Send convec to dock
				convec.DoDock(factoryWithKit);

				return true;
			}

			if (factories.Count > 0)
				return true;

			// Get active structure factory
			UnitEx factory = owner.units.structureFactories.Find((UnitEx unit) => unit.IsEnabled());

			// If factory not found, most likely it is not enabled
			if (factory == null)
				return false;

			if (factory.IsBusy())
				return true;

			UnitInfo moduleInfo = new UnitInfo(m_KitToBuild);

			// Fail Check: Kit cost
			if (owner.player.Ore() < moduleInfo.GetOreCost(owner.player.playerID)) return false;
			if (owner.player.RareOre() < moduleInfo.GetRareOreCost(owner.player.playerID)) return false;

			// Build kit
			factory.DoProduce(m_KitToBuild, m_KitToBuildCargo);

			return true;
		}

		private bool UnitContainsKit(UnitEx unit, map_id[] kits)
		{
			foreach (map_id type in kits)
			{
				if (unit.GetCargo() == type)
					return true;
			}

			return false;
		}
	}
}
