

namespace DotNetMissionSDK.AI.Tasks.Base.Unloading
{
	public class UnloadCommonMetalTask : UnloadSuppliesTask
	{
		public UnloadCommonMetalTask(int ownerID) : base(ownerID)
		{
			m_CargoToUnload		= TruckCargo.CommonMetal;
			m_StructureToDock	= map_id.CommonOreSmelter;
		}
	}

	public class UnloadRareMetalTask : UnloadSuppliesTask
	{
		public UnloadRareMetalTask(int ownerID) : base(ownerID)
		{
			m_CargoToUnload		= TruckCargo.RareMetal;
			m_StructureToDock	= map_id.RareOreSmelter;
		}
	}

	public class UnloadFoodTask : UnloadSuppliesTask
	{
		public UnloadFoodTask(int ownerID) : base(ownerID)
		{
			m_CargoToUnload		= TruckCargo.Food;
			m_StructureToDock	= map_id.Agridome;
		}
	}
}
