using DotNetMissionSDK.Async;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Pathfinding;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;

namespace DotNetMissionSDK.Units
{
	/// <summary>
	/// Extension class for vehicles.
	/// </summary>
	public class Vehicle : UnitEx
	{
		private Unit m_DebugMarker;

		// Movement variables
		private LOCATION[] m_MovementPath;
		private int m_CurrentPathIndex;
		private int m_IdleTimer;

		private Unit m_AttackTarget;

		public bool isSearchingForPath		{ get; private set; }
		public bool hasPath					{ get { return m_MovementPath != null;							}	}
		public bool isSearchingOrHasPath	{ get { return isSearchingForPath || m_MovementPath != null;	}	}
		public LOCATION destination			{ get; private set; }

		// TODO: Set destination when parent class DoMove is called.

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
			ThreadAssert.MainThreadRequired();

			// If unit has been destroyed since we started pathfinding, cancel
			if (!IsLive())
				return;

			m_MovementPath = path;
			m_CurrentPathIndex = 0;
			destination = path[path.Length-1];
			m_IdleTimer = 0;

			DoMove(m_MovementPath[m_CurrentPathIndex].x, m_MovementPath[m_CurrentPathIndex].y);

			SetDebugMarker(m_MovementPath[m_MovementPath.Length-1]);
		}

		/// <summary>
		/// Generates a path for the unit to navigate through terrain.
		/// </summary>
		public void DoMoveWithPathfinder(StateSnapshot stateSnapshot, int tileX, int tileY)
		{
			ThreadAssert.MainThreadRequired();

			DoMoveWithPathfinder(stateSnapshot, new LOCATION(tileX, tileY));
		}

		/// <summary>
		/// Generates a path for the unit to navigate through terrain.
		/// </summary>
		public void DoMoveWithPathfinder(StateSnapshot stateSnapshot, LOCATION targetPosition)
		{
			ThreadAssert.MainThreadRequired();

			int ownerID = GetOwnerID();

			stateSnapshot.Retain();

			DoMoveWithPathfinder(targetPosition, (x,y) => GetTileCost(stateSnapshot, ownerID, x,y), (path) => stateSnapshot.Release());
		}

		/// <summary>
		/// Generates a path for the unit to navigate through terrain with custom tile cost.
		/// </summary>
		public void DoMoveWithPathfinder(LOCATION targetPosition, Pathfinder.TileCostCallback tileCostCB, Pathfinder.OnGetPathCompleteCallback onCompleteCB=null)
		{
			ThreadAssert.MainThreadRequired();

			isSearchingForPath = true;

			Pathfinder.GetPathAsync(GetPosition(), targetPosition, true, tileCostCB, (LOCATION[] path) =>
			{
				isSearchingForPath = false;

				if (path != null)
					DoMove(path);

				onCompleteCB?.Invoke(path);
			});
		}

		/// <summary>
		/// Generates a path for the unit to navigate based on custom parameters.
		/// </summary>
		public void DoMoveWithPathfinder(Pathfinder.TileCostCallback tileCostCB, Pathfinder.ValidTileCallback validTileCB, Pathfinder.OnGetPathCompleteCallback onCompleteCB=null)
		{
			ThreadAssert.MainThreadRequired();

			isSearchingForPath = true;

			Pathfinder.GetClosestValidTileAsync(GetPosition(), tileCostCB, validTileCB, (LOCATION[] path) =>
			{
				isSearchingForPath = false;

				if (path != null)
					DoMove(path);

				onCompleteCB?.Invoke(path);
			});
		}

		// Callback for determining tile cost
		private static int GetTileCost(StateSnapshot state, int ownerID, int x, int y)
		{
			if (!state.tileMap.IsTilePassable(x,y))
				return Pathfinder.Impassable;

			// Buildings, and units that aren't our own, are impassable
			UnitState blockingUnit = state.unitMap.GetUnitOnTile(new LOCATION(x,y));
			if (blockingUnit != null && (!blockingUnit.isVehicle || blockingUnit.ownerID != ownerID))
				return Pathfinder.Impassable;

			return 1;
		}

		private void SetDebugMarker(LOCATION position)
		{
			if (GetOwnerID() != TethysGame.LocalPlayer())
				return;

			// Debug marker
			if (m_DebugMarker != null)
				m_DebugMarker.DoDeath();
			m_DebugMarker = TethysGame.PlaceMarker(position.x, position.y, MarkerType.DNA);
		}

		private void ClearDebugMarker()
		{
			if (m_DebugMarker != null)
				m_DebugMarker.DoDeath();
		}

		public override void DoAttack(Unit targetUnit)
		{
			if (targetUnit != m_AttackTarget)
			{
				base.DoAttack(targetUnit);
				m_AttackTarget = targetUnit;
				m_IdleTimer = 0;
			}
		}

		/// <summary>
		/// Updates this vehicle.
		/// Call every frame.
		/// </summary>
		public override void Update()
		{
			if (GetCurAction() == ActionType.moDone)
			{
				if (m_IdleTimer < 10)
					m_IdleTimer += TethysGame.Tick();
				else
				{
					// Unit lost path or target
					m_MovementPath = null;
					m_AttackTarget = null;
					ClearDebugMarker();
					return;
				}
			}
			else
			{
				// Unit not idle
				m_IdleTimer = 0;
			}

			UpdateMovement();
		}

		private void UpdateMovement()
		{
			if (m_MovementPath == null)
				return;

			// If we reached destination, move to the next path node
			if (GetPosition().Equals(m_MovementPath[m_CurrentPathIndex]))
			{
				++m_CurrentPathIndex;
				if (m_CurrentPathIndex >= m_MovementPath.Length)
				{
					// Path following complete
					m_MovementPath = null;
					ClearDebugMarker();
				}
				else
				{
					DoMove(m_MovementPath[m_CurrentPathIndex].x, m_MovementPath[m_CurrentPathIndex].y);
				}
			}
		}

		/// <summary>
		/// Called when unit ceases to exist.
		/// </summary>
		public override void OnDestroy()
		{
			if (m_DebugMarker != null)
			{
				// Remove marker
				m_DebugMarker.DoDeath();
			}
		}
	}
}
