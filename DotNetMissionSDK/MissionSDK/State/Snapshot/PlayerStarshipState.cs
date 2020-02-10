using DotNetMissionSDK.State.Game;
using System;

namespace DotNetMissionSDK.State.Snapshot
{
	public class PlayerStarshipState
	{
		// Starship module counts
		public byte EDWARDSatelliteCount	{ get; private set; }
		public byte solarSatelliteCount		{ get; private set; }
		public byte ionDriveModuleCount		{ get; private set; }
		public byte fusionDriveModuleCount	{ get; private set; }
		public byte commandModuleCount		{ get; private set; }
		public byte fuelingSystemsCount		{ get; private set; }
		public byte habitatRingCount		{ get; private set; }
		public byte sensorPackageCount		{ get; private set; }
		public byte skydockCount			{ get; private set; }
		public byte stasisSystemsCount		{ get; private set; }
		public byte orbitalPackageCount		{ get; private set; }
		public byte phoenixModuleCount		{ get; private set; }

		public byte rareMetalsCargoCount	{ get; private set; }
		public byte commonMetalsCargoCount	{ get; private set; }
		public byte foodCargoCount			{ get; private set; }
		public byte evacuationModuleCount	{ get; private set; }
		public byte childrenModuleCount		{ get; private set; }

		/// <summary>
		/// Does not include satellites.
		/// </summary>
		public ushort moduleCount			{ get; private set; }

		/// <summary>
		/// Module and satellite count.
		/// </summary>
		public ushort count					{ get; private set; }

		/// <summary>
		/// A 0-1 value of how close the starship is to completion. Does not include children module.
		/// </summary>
		public float progress				{ get; private set; }

		/// <summary>
		/// Creates an immutable starship state from the game state starship.
		/// </summary>
		public PlayerStarshipState(PlayerStarship starship)
		{
			EDWARDSatelliteCount			= starship.EDWARDSatelliteCount;
			solarSatelliteCount				= starship.solarSatelliteCount;
			ionDriveModuleCount				= starship.ionDriveModuleCount;
			fusionDriveModuleCount			= starship.fusionDriveModuleCount;
			commandModuleCount				= starship.commandModuleCount;
			fuelingSystemsCount				= starship.fuelingSystemsCount;
			habitatRingCount				= starship.habitatRingCount;
			sensorPackageCount				= starship.sensorPackageCount;
			skydockCount					= starship.skydockCount;
			stasisSystemsCount				= starship.stasisSystemsCount;
			orbitalPackageCount				= starship.orbitalPackageCount;
			phoenixModuleCount				= starship.phoenixModuleCount;

			rareMetalsCargoCount			= starship.rareMetalsCargoCount;
			commonMetalsCargoCount			= starship.commonMetalsCargoCount;
			foodCargoCount					= starship.foodCargoCount;
			evacuationModuleCount			= starship.evacuationModuleCount;
			childrenModuleCount				= starship.childrenModuleCount;

			moduleCount = ionDriveModuleCount;
			moduleCount += fusionDriveModuleCount;
			moduleCount += commandModuleCount;
			moduleCount += fuelingSystemsCount;
			moduleCount += habitatRingCount;
			moduleCount += sensorPackageCount;
			moduleCount += skydockCount;
			moduleCount += stasisSystemsCount;
			moduleCount += orbitalPackageCount;
			moduleCount += phoenixModuleCount;

			moduleCount += rareMetalsCargoCount;
			moduleCount += commonMetalsCargoCount;
			moduleCount += foodCargoCount;
			moduleCount += evacuationModuleCount;
			moduleCount += childrenModuleCount;

			count = moduleCount;
			count += EDWARDSatelliteCount;
			count += solarSatelliteCount;

			int partsCount = Math.Min(ionDriveModuleCount, (byte)1);
			partsCount += Math.Min(fusionDriveModuleCount, (byte)1);
			partsCount += Math.Min(commandModuleCount, (byte)1);
			partsCount += Math.Min(fuelingSystemsCount, (byte)1);
			partsCount += Math.Min(habitatRingCount, (byte)1);
			partsCount += Math.Min(sensorPackageCount, (byte)1);
			partsCount += Math.Min(skydockCount, (byte)1);
			partsCount += Math.Min(stasisSystemsCount, (byte)1);
			partsCount += Math.Min(orbitalPackageCount, (byte)1);
			partsCount += Math.Min(phoenixModuleCount, (byte)1);

			partsCount += Math.Min(rareMetalsCargoCount, (byte)1);
			partsCount += Math.Min(commonMetalsCargoCount, (byte)1);
			partsCount += Math.Min(foodCargoCount, (byte)1);
			partsCount += Math.Min(evacuationModuleCount, (byte)1);

			progress = partsCount / 14.0f;

		}
	}
}
