using DotNetMissionSDK.AI.Managers;
using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.AI.Tasks.Base.Vehicle;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;

namespace DotNetMissionSDK.AI.Tasks.Base.Mining
{
	/// <summary>
	/// Finds beacons inside of a command center's control area and builds a new mine.
	/// All mines must be saturated before a new mine is created.
	/// </summary>
	public class CreateRareMineTask : Task
	{
		private MiningBaseState m_MiningBaseState;

		private SaturateRareMineTask m_SaturateMineTask;


		public CreateRareMineTask(MiningBaseState miningBaseState)									{ m_MiningBaseState = miningBaseState; }
		public CreateRareMineTask(PlayerInfo owner, MiningBaseState miningBaseState) : base(owner)	{ m_MiningBaseState = miningBaseState; }


		public override bool IsTaskComplete()
		{
			// Task is not complete until every CC beacon has been occupied and saturated
			if (!m_SaturateMineTask.IsTaskComplete())
				return false;
			
			// Task is complete if all controlled beacons have mines
			foreach (MiningBase miningBase in m_MiningBaseState.miningBases)
			{
				foreach (MiningSite site in miningBase.miningSites)
				{
					if (site.beacon.GetOreType() != BeaconType.Rare)
						continue;
					
					if (site.mine == null)
						return false;
				}
			}

			return true;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(m_SaturateMineTask = new SaturateRareMineTask(m_MiningBaseState));
			AddPrerequisite(new BuildSurveyorTask());
			AddPrerequisite(new BuildMinerTask());
		}

		protected override bool PerformTask()
		{
			// Get miner
			if (owner.units.roboMiners.Count == 0) return false;
			UnitEx miner = owner.units.roboMiners[0];
			
			// Check if miner is being deployed (Both exist at the same time for a moment and triggers a bug)
			if (owner.units.rareOreMines.Find((UnitEx mine) => mine.GetPosition().Equals(miner.GetPosition())) != null)
				return false;
			
			// Find closest unoccupied rare beacon
			UnitEx beacon = GetClosestUnusedBeacon(miner);
			LOCATION beaconPosition = beacon.GetPosition();
			
			// Check if beacon is surveyed
			if (beacon.GetSurveyedBy(owner.player.playerID))
			{
				// Deploy miner
				BuildStructureTask.ClearDeployArea(miner, map_id.RareOreMine, beacon.GetPosition());

				if (!miner.GetPosition().Equals(beaconPosition) && miner.GetCurAction() == ActionType.moDone) // WARNING: If unit is EMP'd, it will get stuck
					miner.DoDeployMiner(beaconPosition.x, beaconPosition.y);

				return true;
			}
			
			// Move miner involved in expansion close to beacon for efficiency
			if (miner.GetCurAction() == ActionType.moDone)
				miner.DoMove(beaconPosition.x-1, beaconPosition.y+1);
			
			// Survey beacon
			if (owner.units.roboSurveyors.Count == 0)
				return false;
			
			UnitEx surveyor = owner.units.roboSurveyors[0];

			if (!surveyor.GetPosition().Equals(beaconPosition) && surveyor.GetCurAction() == ActionType.moDone) // WARNING: If unit is EMP'd, it will get stuck
				surveyor.DoMove(beaconPosition.x, beaconPosition.y);
			
			return true;
		}

		private UnitEx GetClosestUnusedBeacon(UnitEx miner)
		{
			// Get closest beacon
			UnitEx closestBeacon = null;
			int closestDistance = 900000;

			foreach (MiningBase miningBase in m_MiningBaseState.miningBases)
			{
				foreach (MiningSite site in miningBase.miningSites)
				{
					if (site.beacon.GetOreType() != BeaconType.Rare) continue;
					if (site.mine != null) continue;

					int distance = miner.GetPosition().GetDiagonalDistance(site.beacon.GetPosition());
					if (distance < closestDistance)
					{
						closestBeacon = site.beacon;
						closestDistance = distance;
					}
				}
			}

			return closestBeacon;
		}
	}
}
