using DotNetMissionSDK.Async;
using DotNetMissionSDK.State.Snapshot.Maps;
using DotNetMissionSDK.State.Snapshot.Units;
using DotNetMissionSDK.State.Snapshot.UnitTypeInfo;
using DotNetMissionSDK.Utility;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace DotNetMissionSDK.State.Snapshot
{
	/// <summary>
	/// Represents the game state at a moment in TethysGame.Time().
	/// Snapshots are immutable and can safely be accessed from multiple threads.
	/// NOTE:	Storing references to StateSnapshot is not recommended, as it becomes
	///			easier to accidentally update the reference that an async operation is using.
	/// </summary>
	public partial class StateSnapshot
	{
		private static ObjectPool<StateSnapshot> m_SnapshotPool = new ObjectPool<StateSnapshot>();

		private int m_References;

		private static ReadOnlyDictionary<map_id, GlobalVehicleInfo> m_VehicleInfo;
		private static ReadOnlyDictionary<map_id, GlobalStructureInfo> m_StructureInfo;
		private static ReadOnlyDictionary<map_id, GlobalWeaponInfo> m_WeaponInfo;
		private static ReadOnlyDictionary<map_id, GlobalUnitInfo> m_StarshipInfo;

		private Dictionary<int, UnitState> m_Units = new Dictionary<int, UnitState>();

		private PlayerState[] m_Players;

		private static StateSnapshot m_LastSnapshot;

		// Snapshot time (From TethysGame.Time() when snapshot was created)
		public int time															{ get; private set; }

		public bool usesMorale													{ get; private set; }

		// Global Unit Info
		public ReadOnlyDictionary<map_id, GlobalVehicleInfo> vehicleInfo		{ get; private set; }
		public ReadOnlyDictionary<map_id, GlobalStructureInfo> structureInfo	{ get; private set; }
		public ReadOnlyDictionary<map_id, GlobalWeaponInfo> weaponInfo			{ get; private set; }
		public ReadOnlyDictionary<map_id, GlobalUnitInfo> starshipInfo			{ get; private set; }

		// Player / Unit State
		public GaiaState gaia													{ get; private set; }
		public ReadOnlyCollection<PlayerState> players							{ get; private set; }
		
		// Maps
		public GaiaMap gaiaMap													{ get; private set; }
		public GameTileMap tileMap												{ get; private set; }
		public PlayerUnitMap unitMap											{ get; private set; }
		public PlayerCommandMap commandMap										{ get; private set; }
		public PlayerStrengthMap strengthMap									{ get; private set; }

		/// <summary>
		/// Returns the unit that has the given ID or null, if unit is not found.
		/// </summary>
		public UnitState GetUnit(int unitID)
		{
			UnitState unit;
			m_Units.TryGetValue(unitID, out unit);
			return unit;
		}

		public GlobalUnitInfo GetGlobalUnitInfo(map_id unitTypeID)
		{
			if ((int)unitTypeID >= 1 && (int)unitTypeID <= 15)		return vehicleInfo[unitTypeID];
			if ((int)unitTypeID >= 21 && (int)unitTypeID <= 58)		return structureInfo[unitTypeID];
			if ((int)unitTypeID >= 59 && (int)unitTypeID <= 73)		return weaponInfo[unitTypeID];
			if ((int)unitTypeID >= 88 && (int)unitTypeID <= 107)		return starshipInfo[unitTypeID];

			throw new System.ArgumentOutOfRangeException("unitTypeID", "unitTypeID is invalid!");
		}

		/// <summary>
		/// Creates a blank snapshot object for use with the SnapshotPool.
		/// Applications should call Create().
		/// </summary>
		public StateSnapshot() { }

		/// <summary>
		/// Creates a new snapshot from the current game state and TethysGame.Time().
		/// NOTE: Creating a snapshot is an intensive operation. Only call once per frame.
		/// </summary>
		public static StateSnapshot Create()
		{
			ThreadAssert.MainThreadRequired();

			StateSnapshot stateSnapshot = m_SnapshotPool.Create();
			stateSnapshot.Initialize();
			stateSnapshot.Retain();
			return stateSnapshot;
		}

		private void Initialize()
		{
			time = TethysGame.Time();

			usesMorale = TethysGame.UsesMorale();

			// Global Unit Info
			if (m_VehicleInfo == null)
				UpdateGlobalInfo();

			vehicleInfo = m_VehicleInfo;
			structureInfo = m_StructureInfo;
			weaponInfo = m_WeaponInfo;
			starshipInfo = m_StarshipInfo;

			// Gaia state
			if (gaia != null) gaia.Initialize();	else gaia = new GaiaState();

			// Player state
			if (m_Players == null || m_Players.Length != GameState.players.Count)
				m_Players = new PlayerState[GameState.players.Count];

			for (int i=0; i < m_Players.Length; ++i)
			{
				if (m_Players[i] != null)
					m_Players[i].Initialize(this, GameState.players[i], m_LastSnapshot?.players[i]);
				else
					m_Players[i] = new PlayerState(this, GameState.players[i], m_LastSnapshot?.players[i]);
			}

			this.players = new ReadOnlyCollection<PlayerState>(m_Players);

			// Unit state
			for (int i=0; i < m_Players.Length; ++i)
			{
				foreach (UnitState unit in m_Players[i].units.GetUnits())
					m_Units.Add(unit.unitID, unit);
			}

			// Map state
			if (gaiaMap != null) gaiaMap.Initialize(gaia);						else gaiaMap = new GaiaMap(gaia);
			if (tileMap != null) tileMap.Initialize();							else tileMap = new GameTileMap();
			if (unitMap != null) unitMap.Initialize(m_Players);					else unitMap = new PlayerUnitMap(m_Players);
			if (commandMap != null) commandMap.Initialize(this);				else commandMap = new PlayerCommandMap(this);
			if (strengthMap != null) strengthMap.Initialize(m_Players);			else strengthMap = new PlayerStrengthMap(m_Players);

			m_LastSnapshot = this;
		}

		/// <summary>
		/// Retains a reference to this snapshot.
		/// You must call this to prevent the snapshot from being released if you are going to hold onto it for more than one frame.
		/// </summary>
		public void Retain()
		{
			Interlocked.Increment(ref m_References);
		}

		/// <summary>
		/// Releases the snapshot by sending it back to the snapshot pool.
		/// </summary>
		public void Release()
		{
			Interlocked.Decrement(ref m_References);

			if (m_References == 0)
			{
				gaia.Release();

				for (int i=0; i < m_Players.Length; ++i)
					m_Players[i].Release();

				vehicleInfo = null;
				structureInfo = null;
				weaponInfo = null;
				starshipInfo = null;

				m_Units.Clear();

				gaiaMap.Clear();
				//tileMap.Clear();
				unitMap.Clear();
				commandMap.Clear();
				strengthMap.Clear();

				m_SnapshotPool.Release(this);
			}
			else if (m_References < 0)
				throw new System.Exception("Object has been released more than it has been retained. Check code for errors.");
		}

		/// <summary>
		/// Global info is usually set at mission start and does not update frequently.
		/// Call this whenever the global info changes.
		/// </summary>
		public static void UpdateGlobalInfo()
		{
			ThreadAssert.MainThreadRequired();

			Dictionary<map_id, GlobalVehicleInfo> vehicleInfo = new Dictionary<map_id, GlobalVehicleInfo>();
			Dictionary<map_id, GlobalStructureInfo> structureInfo = new Dictionary<map_id, GlobalStructureInfo>();
			Dictionary<map_id, GlobalWeaponInfo> weaponInfo = new Dictionary<map_id, GlobalWeaponInfo>();
			Dictionary<map_id, GlobalUnitInfo> starshipInfo = new Dictionary<map_id, GlobalUnitInfo>();

			for (int i=1; i <= 15; ++i)
				vehicleInfo.Add((map_id)i, new GlobalVehicleInfo((map_id)i));

			for (int i=21; i <= 58; ++i)
				structureInfo.Add((map_id)i, new GlobalStructureInfo((map_id)i));

			for (int i=59; i <= 73; ++i)
				weaponInfo.Add((map_id)i, new GlobalWeaponInfo((map_id)i));

			for (int i=88; i <= 107; ++i)
				starshipInfo.Add((map_id)i, new GlobalUnitInfo((map_id)i));

			m_VehicleInfo = new ReadOnlyDictionary<map_id, GlobalVehicleInfo>(vehicleInfo);
			m_StructureInfo = new ReadOnlyDictionary<map_id, GlobalStructureInfo>(structureInfo);
			m_WeaponInfo = new ReadOnlyDictionary<map_id, GlobalWeaponInfo>(weaponInfo);
			m_StarshipInfo = new ReadOnlyDictionary<map_id, GlobalUnitInfo>(starshipInfo);
		}
	}
}
