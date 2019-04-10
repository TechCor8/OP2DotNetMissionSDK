using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Starship
{
	public abstract class DeployStarshipModuleTask : Task
	{
		protected map_id m_StarshipModule = map_id.EvacuationModule;

		public DeployStarshipModuleTask() { }
		public DeployStarshipModuleTask(PlayerInfo owner) : base(owner) { }


		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildRocketTask());
		}

		protected override bool PerformTask()
		{
			// Fail Check: Research
			if (!owner.player.HasTechnology(TechInfoOld.GetTechID(m_StarshipModule)))
				return false;
			
			// Get spaceports with rocket
			List<UnitEx> spaceports = owner.units.spaceports.FindAll((UnitEx unit) => (unit.GetObjectOnPad() == map_id.SULV || unit.GetObjectOnPad() == map_id.RLV) && unit.IsEnabled());

			// If no spaceports are found, most likely they are not enabled
			if (spaceports.Count == 0)
				return false;

			// Get spaceport with module in rocket and launch
			UnitEx spaceport = spaceports.Find((UnitEx unit) => unit.GetLaunchpadCargo() == m_StarshipModule);
			if (spaceport != null)
			{
				if (spaceport.IsBusy()) return true;

				spaceport.DoLaunch(0, 0, false);
				return true;
			}
			
			// Get spaceport with module in bay and load rocket
			spaceport = spaceports.Find((UnitEx unit) => unit.GetBayWithCargo(m_StarshipModule) >= 0);
			if (spaceport != null)
			{
				if (spaceport.IsBusy()) return true;

				int bayIndex = spaceport.GetBayWithCargo(m_StarshipModule);
				spaceport.DoTransferLaunchpadCargo(bayIndex);
				spaceport.DoLaunch(0, 0, false);
				return true;
			}

			// Get spaceport with empty bay and build module
			spaceport = spaceports.Find((UnitEx unit) => unit.HasEmptyBay());
			if (spaceport != null)
			{
				if (spaceport.IsBusy()) return true;

				UnitInfo moduleInfo = new UnitInfo(m_StarshipModule);

				// Fail Check: Module cost
				if (owner.player.Ore() < moduleInfo.GetOreCost(owner.player.playerID)) return false;
				if (owner.player.RareOre() < moduleInfo.GetRareOreCost(owner.player.playerID)) return false;

				spaceport.DoDevelop(m_StarshipModule);
				return true;
			}

			return true;
		}
	}
}
