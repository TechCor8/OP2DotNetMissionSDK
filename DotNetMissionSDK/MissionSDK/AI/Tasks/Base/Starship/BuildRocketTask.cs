using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Starship
{
	public class BuildRocketTask : Task
	{
		public BuildRocketTask() { }
		public BuildRocketTask(PlayerInfo owner) : base(owner) { }


		public override bool IsTaskComplete()
		{
			// Find spaceport with rocket
			List<UnitEx> spaceports = owner.units.spaceports.FindAll((UnitEx unit) => unit.GetObjectOnPad() == map_id.SULV || unit.GetObjectOnPad() == map_id.RLV);
			return spaceports.Count > 0;
		}
		
		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildSpaceportTask());
		}

		protected override bool PerformTask()
		{
			// Get active spaceport
			UnitEx spaceport = owner.units.spaceports.Find((UnitEx unit) => unit.GetObjectOnPad() == map_id.None && unit.IsEnabled());

			// If spaceport not found, most likely it is not enabled, but may have EMP missile instead
			if (spaceport == null)
				return false;

			// Do nothing if we are waiting for RLV to return
			if (owner.player.GetRLVCount() > 0)
				return true;

			if (spaceport.IsBusy())
				return true;

			map_id rocketToBuild = map_id.SULV;

			// Build an RLV if we have the technology
			UnitInfo unitInfo = new UnitInfo(map_id.RLV);
			TechInfo techInfo = Research.GetTechInfo(unitInfo.GetResearchTopic());

			if (owner.player.HasTechnology(techInfo.GetTechID()))
				rocketToBuild = map_id.RLV;

			UnitInfo moduleInfo = new UnitInfo(rocketToBuild);

			// Fail Check: Rocket cost
			if (owner.player.Ore() < moduleInfo.GetOreCost(owner.player.playerID)) return false;
			if (owner.player.RareOre() < moduleInfo.GetRareOreCost(owner.player.playerID)) return false;

			spaceport.DoDevelop(rocketToBuild);

			return true;
		}
	}
}
