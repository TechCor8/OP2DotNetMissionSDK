using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Pathfinding;

namespace DotNetMissionSDK.Units
{
	/// <summary>
	/// Extension class for vehicles.
	/// </summary>
	public class Vehicle : UnitEx
	{
		// Movement variables
		private LOCATION[] m_MovementPath;
		private int m_CurrentPathIndex;


		/// <summary>
		/// Creates an empty Vehicle stub. Call SetStubIndex with a valid vehicle before using.
		/// </summary>
		public Vehicle() { }
		public Vehicle(int stubIndex) : base(stubIndex) { }
		public Vehicle(Unit unit) : base(unit.GetStubIndex()) { }

		/// <summary>
		/// Moves unit along path.
		/// </summary>
		/// <param name="path">The path to move the unit on.</param>
		public void DoMove(LOCATION[] path)
		{
			m_MovementPath = path;
			m_CurrentPathIndex = 0;

			DoMove(m_MovementPath[m_CurrentPathIndex].x, m_MovementPath[m_CurrentPathIndex].y);
		}

		/// <summary>
		/// Generates a path for the unit to navigate through terrain.
		/// </summary>
		public void DoMoveWithPathfinder(int tileX, int tileY)
		{
			LOCATION[] path = Pathfinder.GetPath(GetPosition(), new LOCATION(tileX, tileY), true, GetTileCost);
			if (path == null)
				return;

			DoMove(path);
		}

		// Callback for determining tile cost
		private int GetTileCost(int x, int y)
		{
			if (!GameMap.IsTilePassable(x,y))
				return Pathfinder.Impassable;

			return 1;
		}

		/// <summary>
		/// Updates this vehicle.
		/// Call every frame.
		/// </summary>
		public void Update()
		{
			UpdateMovement();
		}

		private void UpdateMovement()
		{
			if (m_MovementPath == null)
				return;

			//if (GetCurAction() != ActionType.moDone)
			//	return;

			// If we reached destination, move to the next path node
			if (GetPosition().Equals(m_MovementPath[m_CurrentPathIndex]))
			{
				++m_CurrentPathIndex;
				DoMove(m_MovementPath[m_CurrentPathIndex].x, m_MovementPath[m_CurrentPathIndex].y);
			}
		}
	}
}
