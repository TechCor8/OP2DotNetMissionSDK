﻿using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Pathfinding;
using DotNetMissionSDK.Utility.Maps;

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

		public bool hasPath				{ get { return m_MovementPath != null;		}	}
		public LOCATION destination		{ get; private set; }

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
		public void DoMoveWithPathfinder(int tileX, int tileY)
		{
			DoMoveWithPathfinder(new LOCATION(tileX, tileY));
		}

		/// <summary>
		/// Generates a path for the unit to navigate through terrain.
		/// </summary>
		public void DoMoveWithPathfinder(LOCATION targetPosition)
		{
			LOCATION[] path = Pathfinder.GetPath(GetPosition(), targetPosition, true, GetTileCost);
			if (path == null)
				return;

			DoMove(path);
		}

		// Callback for determining tile cost
		private int GetTileCost(int x, int y)
		{
			if (!GameMap.IsTilePassable(x,y))
				return Pathfinder.Impassable;

			// Buildings and units are impassable
			if (PlayerUnitMap.GetUnitOnTile(new LOCATION(x,y)) != null)
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

		/// <summary>
		/// Updates this vehicle.
		/// Call every frame.
		/// </summary>
		public override void Update()
		{
			UpdateMovement();
		}

		private void UpdateMovement()
		{
			if (m_MovementPath == null)
				return;

			if (GetCurAction() == ActionType.moDone)
			{
				// Unit lost path
				if (m_IdleTimer < 10)
					m_IdleTimer += TethysGame.Tick();
				else
				{
					m_MovementPath = null;
					return;
				}
			}
			else
			{
				// Unit not idle
				m_IdleTimer = 0;
			}

			// If we reached destination, move to the next path node
			if (GetPosition().Equals(m_MovementPath[m_CurrentPathIndex]))
			{
				++m_CurrentPathIndex;
				if (m_CurrentPathIndex >= m_MovementPath.Length)
					m_MovementPath = null; // Path following complete
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
