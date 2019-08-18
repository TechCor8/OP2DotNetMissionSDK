using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State.Game;
using DotNetMissionSDK.State.Snapshot.Units;
using DotNetMissionSDK.State.Snapshot.UnitTypeInfo;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DotNetMissionSDK.State.Snapshot
{
	/// <summary>
	/// Contains immutable player unit state at a moment in TethysGame.Time().
	/// Should only be created by StateSnapshot.
	/// </summary>
	public class PlayerUnitState
	{
		// Units
		public ReadOnlyCollection<CargoTruckState> cargoTrucks			{ get; private set; }
		public ReadOnlyCollection<ConvecState> convecs					{ get; private set; }
		public ReadOnlyCollection<VehicleState> spiders					{ get; private set; }
		public ReadOnlyCollection<VehicleState> scorpions				{ get; private set; }
		public ReadOnlyCollection<VehicleState> lynx					{ get; private set; }
		public ReadOnlyCollection<VehicleState> panthers				{ get; private set; }
		public ReadOnlyCollection<VehicleState> tigers					{ get; private set; }
		public ReadOnlyCollection<VehicleState> roboSurveyors			{ get; private set; }
		public ReadOnlyCollection<VehicleState> roboMiners				{ get; private set; }
		public ReadOnlyCollection<VehicleState> geoCons					{ get; private set; }
		public ReadOnlyCollection<VehicleState> scouts					{ get; private set; }
		public ReadOnlyCollection<VehicleState> roboDozers				{ get; private set; }
		public ReadOnlyCollection<VehicleState> evacTransports			{ get; private set; }
		public ReadOnlyCollection<VehicleState> repairVehicles			{ get; private set; }
		public ReadOnlyCollection<VehicleState> earthWorkers			{ get; private set; }

		// Structures
		public ReadOnlyCollection<StructureState> commonOreMines		{ get; private set; }
		public ReadOnlyCollection<StructureState> rareOreMines			{ get; private set; }
		public ReadOnlyCollection<StructureState> guardPosts			{ get; private set; }
		public ReadOnlyCollection<StructureState> lightTowers			{ get; private set; }
		public ReadOnlyCollection<StructureState> commonStorages		{ get; private set; }
		public ReadOnlyCollection<StructureState> rareStorages			{ get; private set; }
		public ReadOnlyCollection<StructureState> forums				{ get; private set; }
		public ReadOnlyCollection<StructureState> commandCenters		{ get; private set; }
		public ReadOnlyCollection<StructureState> mhdGenerators			{ get; private set; }
		public ReadOnlyCollection<StructureState> residences			{ get; private set; }
		public ReadOnlyCollection<StructureState> robotCommands			{ get; private set; }
		public ReadOnlyCollection<StructureState> tradeCenters			{ get; private set; }
		public ReadOnlyCollection<LabState> basicLabs					{ get; private set; }
		public ReadOnlyCollection<StructureState> medicalCenters		{ get; private set; }
		public ReadOnlyCollection<StructureState> nurseries				{ get; private set; }
		public ReadOnlyCollection<StructureState> solarPowerArrays		{ get; private set; }
		public ReadOnlyCollection<StructureState> recreationFacilities	{ get; private set; }
		public ReadOnlyCollection<UniversityState> universities			{ get; private set; }
		public ReadOnlyCollection<StructureState> agridomes				{ get; private set; }
		public ReadOnlyCollection<StructureState> dirts					{ get; private set; }
		public ReadOnlyCollection<GarageState> garages					{ get; private set; }
		public ReadOnlyCollection<StructureState> magmaWells			{ get; private set; }
		public ReadOnlyCollection<StructureState> meteorDefenses		{ get; private set; }
		public ReadOnlyCollection<StructureState> geothermalPlants		{ get; private set; }
		public ReadOnlyCollection<FactoryState> arachnidFactories		{ get; private set; }
		public ReadOnlyCollection<StructureState> consumerFactories		{ get; private set; }
		public ReadOnlyCollection<FactoryState> structureFactories		{ get; private set; }
		public ReadOnlyCollection<FactoryState> vehicleFactories		{ get; private set; }
		public ReadOnlyCollection<LabState> standardLabs				{ get; private set; }
		public ReadOnlyCollection<LabState> advancedLabs				{ get; private set; }
		public ReadOnlyCollection<StructureState> observatories			{ get; private set; }
		public ReadOnlyCollection<StructureState> reinforcedResidences	{ get; private set; }
		public ReadOnlyCollection<StructureState> advancedResidences	{ get; private set; }
		public ReadOnlyCollection<StructureState> commonOreSmelters		{ get; private set; }
		public ReadOnlyCollection<SpaceportState> spaceports			{ get; private set; }
		public ReadOnlyCollection<StructureState> rareOreSmelters		{ get; private set; }
		public ReadOnlyCollection<StructureState> gorfs					{ get; private set; }
		public ReadOnlyCollection<StructureState> tokamaks				{ get; private set; }


		// Starship module counts
		public byte EDWARDSatelliteCount								{ get; private set; }
		public byte solarSatelliteCount									{ get; private set; }
		public byte ionDriveModuleCount									{ get; private set; }
		public byte fusionDriveModuleCount								{ get; private set; }
		public byte commandModuleCount									{ get; private set; }
		public byte fuelingSystemsCount									{ get; private set; }
		public byte habitatRingCount									{ get; private set; }
		public byte sensorPackageCount									{ get; private set; }
		public byte skydockCount										{ get; private set; }
		public byte stasisSystemsCount									{ get; private set; }
		public byte orbitalPackageCount									{ get; private set; }
		public byte phoenixModuleCount									{ get; private set; }

		public byte rareMetalsCargoCount								{ get; private set; }
		public byte commonMetalsCargoCount								{ get; private set; }
		public byte foodCargoCount										{ get; private set; }
		public byte evacuationModuleCount								{ get; private set; }
		public byte childrenModuleCount									{ get; private set; }


		/// <summary>
		/// Creates an immutable player unit state from PlayerInfo.
		/// </summary>
		public PlayerUnitState(int playerID,
								ReadOnlyDictionary<map_id, VehicleInfo> vehicleInfo,
								ReadOnlyDictionary<map_id, StructureInfo> structureInfo,
								ReadOnlyDictionary<map_id, WeaponInfo> weaponInfo,
								PlayerUnitState prevState)
		{
			// Units
			// Prepare lists
			List<CargoTruckState> cargoTrucks			= new List<CargoTruckState>(prevState?.cargoTrucks.Count ?? 0);
			List<ConvecState> convecs					= new List<ConvecState>(	prevState?.convecs.Count ?? 0);
			List<VehicleState> spiders					= new List<VehicleState>(	prevState?.spiders.Count ?? 0);
			List<VehicleState> scorpions				= new List<VehicleState>(	prevState?.scorpions.Count ?? 0);
			List<VehicleState> lynx						= new List<VehicleState>(	prevState?.lynx.Count ?? 0);
			List<VehicleState> panthers					= new List<VehicleState>(	prevState?.panthers.Count ?? 0);
			List<VehicleState> tigers					= new List<VehicleState>(	prevState?.tigers.Count ?? 0);
			List<VehicleState> roboSurveyors			= new List<VehicleState>(	prevState?.roboSurveyors.Count ?? 0);
			List<VehicleState> roboMiners				= new List<VehicleState>(	prevState?.roboMiners.Count ?? 0);
			List<VehicleState> geoCons					= new List<VehicleState>(	prevState?.geoCons.Count ?? 0);
			List<VehicleState> scouts					= new List<VehicleState>(	prevState?.scouts.Count ?? 0);
			List<VehicleState> roboDozers				= new List<VehicleState>(	prevState?.roboDozers.Count ?? 0);
			List<VehicleState> evacTransports			= new List<VehicleState>(	prevState?.evacTransports.Count ?? 0);
			List<VehicleState> repairVehicles			= new List<VehicleState>(	prevState?.repairVehicles.Count ?? 0);
			List<VehicleState> earthWorkers				= new List<VehicleState>(	prevState?.earthWorkers.Count ?? 0);

			// Structures
			List<StructureState> commonOreMines			= new List<StructureState>(	prevState?.commonOreMines.Count ?? 0);
			List<StructureState> rareOreMines			= new List<StructureState>(	prevState?.rareOreMines.Count ?? 0);
			List<StructureState> guardPosts				= new List<StructureState>(	prevState?.guardPosts.Count ?? 0);
			List<StructureState> lightTowers			= new List<StructureState>(	prevState?.lightTowers.Count ?? 0);
			List<StructureState> commonStorages			= new List<StructureState>(	prevState?.commonStorages.Count ?? 0);
			List<StructureState> rareStorages			= new List<StructureState>(	prevState?.rareStorages.Count ?? 0);
			List<StructureState> forums					= new List<StructureState>(	prevState?.forums.Count ?? 0);
			List<StructureState> commandCenters			= new List<StructureState>(	prevState?.commandCenters.Count ?? 0);
			List<StructureState> mhdGenerators			= new List<StructureState>(	prevState?.mhdGenerators.Count ?? 0);
			List<StructureState> residences				= new List<StructureState>(	prevState?.residences.Count ?? 0);
			List<StructureState> robotCommands			= new List<StructureState>(	prevState?.robotCommands.Count ?? 0);
			List<StructureState> tradeCenters			= new List<StructureState>(	prevState?.tradeCenters.Count ?? 0);
			List<LabState> basicLabs					= new List<LabState>(		prevState?.basicLabs.Count ?? 0);
			List<StructureState> medicalCenters			= new List<StructureState>(	prevState?.medicalCenters.Count ?? 0);
			List<StructureState> nurseries				= new List<StructureState>(	prevState?.nurseries.Count ?? 0);
			List<StructureState> solarPowerArrays		= new List<StructureState>(	prevState?.solarPowerArrays.Count ?? 0);
			List<StructureState> recreationFacilities	= new List<StructureState>(	prevState?.recreationFacilities.Count ?? 0);
			List<UniversityState> universities			= new List<UniversityState>(prevState?.universities.Count ?? 0);
			List<StructureState> agridomes				= new List<StructureState>(	prevState?.agridomes.Count ?? 0);
			List<StructureState> dirts					= new List<StructureState>(	prevState?.dirts.Count ?? 0);
			List<GarageState> garages					= new List<GarageState>(	prevState?.garages.Count ?? 0);
			List<StructureState> magmaWells				= new List<StructureState>(	prevState?.magmaWells.Count ?? 0);
			List<StructureState> meteorDefenses			= new List<StructureState>(	prevState?.meteorDefenses.Count ?? 0);
			List<StructureState> geothermalPlants		= new List<StructureState>(	prevState?.geothermalPlants.Count ?? 0);
			List<FactoryState> arachnidFactories		= new List<FactoryState>(	prevState?.arachnidFactories.Count ?? 0);
			List<StructureState> consumerFactories		= new List<StructureState>(	prevState?.consumerFactories.Count ?? 0);
			List<FactoryState> structureFactories		= new List<FactoryState>(	prevState?.structureFactories.Count ?? 0);
			List<FactoryState> vehicleFactories			= new List<FactoryState>(	prevState?.vehicleFactories.Count ?? 0);
			List<LabState> standardLabs					= new List<LabState>(		prevState?.standardLabs.Count ?? 0);
			List<LabState> advancedLabs					= new List<LabState>(		prevState?.advancedLabs.Count ?? 0);
			List<StructureState> observatories			= new List<StructureState>(	prevState?.observatories.Count ?? 0);
			List<StructureState> reinforcedResidences	= new List<StructureState>(	prevState?.reinforcedResidences.Count ?? 0);
			List<StructureState> advancedResidences		= new List<StructureState>(	prevState?.advancedResidences.Count ?? 0);
			List<StructureState> commonOreSmelters		= new List<StructureState>(	prevState?.commonOreSmelters.Count ?? 0);
			List<SpaceportState> spaceports				= new List<SpaceportState>(	prevState?.spaceports.Count ?? 0);
			List<StructureState> rareOreSmelters		= new List<StructureState>(	prevState?.rareOreSmelters.Count ?? 0);
			List<StructureState> gorfs					= new List<StructureState>(	prevState?.gorfs.Count ?? 0);
			List<StructureState> tokamaks				= new List<StructureState>(	prevState?.tokamaks.Count ?? 0);
			
			foreach (UnitEx unit in GameState.units.Values)
			{
				if (unit.GetOwnerID() != playerID)
					continue;

				map_id unitType = unit.GetUnitType();

				WeaponInfo unitWeaponInfo = unit.HasWeapon() ? weaponInfo[unit.GetWeapon()] : null;
				VehicleInfo unitVehicleInfo = unit.IsVehicle() ? vehicleInfo[unitType] : null;
				StructureInfo unitStructureInfo = unit.IsBuilding() ? structureInfo[unitType] : null;

				switch (unitType)
				{
					case map_id.CargoTruck:				cargoTrucks.Add(			new CargoTruckState(unit,	unitVehicleInfo, unitWeaponInfo));			break;
					case map_id.ConVec:					convecs.Add(				new ConvecState(unit,		unitVehicleInfo, unitWeaponInfo));			break;
					case map_id.Spider:					spiders.Add(				new VehicleState(unit,		unitVehicleInfo, unitWeaponInfo));			break;
					case map_id.Scorpion:				scorpions.Add(				new VehicleState(unit,		unitVehicleInfo, unitWeaponInfo));			break;
					case map_id.Lynx:					lynx.Add(					new VehicleState(unit,		unitVehicleInfo, unitWeaponInfo));			break;
					case map_id.Panther:				panthers.Add(				new VehicleState(unit,		unitVehicleInfo, unitWeaponInfo));			break;
					case map_id.Tiger:					tigers.Add(					new VehicleState(unit,		unitVehicleInfo, unitWeaponInfo));			break;
					case map_id.RoboSurveyor:			roboSurveyors.Add(			new VehicleState(unit,		unitVehicleInfo, unitWeaponInfo));			break;
					case map_id.RoboMiner:				roboMiners.Add(				new VehicleState(unit,		unitVehicleInfo, unitWeaponInfo));			break;
					case map_id.GeoCon:					geoCons.Add(				new VehicleState(unit,		unitVehicleInfo, unitWeaponInfo));			break;
					case map_id.Scout:					scouts.Add(					new VehicleState(unit,		unitVehicleInfo, unitWeaponInfo));			break;
					case map_id.RoboDozer:				roboDozers.Add(				new VehicleState(unit,		unitVehicleInfo, unitWeaponInfo));			break;
					case map_id.EvacuationTransport:	evacTransports.Add(			new VehicleState(unit,		unitVehicleInfo, unitWeaponInfo));			break;
					case map_id.RepairVehicle:			repairVehicles.Add(			new VehicleState(unit,		unitVehicleInfo, unitWeaponInfo));			break;
					case map_id.Earthworker:			earthWorkers.Add(			new VehicleState(unit,		unitVehicleInfo, unitWeaponInfo));			break;

					case map_id.CommonOreMine:			commonOreMines.Add(			new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.RareOreMine:			rareOreMines.Add(			new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.GuardPost:				guardPosts.Add(				new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.LightTower:				lightTowers.Add(			new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.CommonStorage:			commonStorages.Add(			new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.RareStorage:			rareStorages.Add(			new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.Forum:					forums.Add(					new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.CommandCenter:			commandCenters.Add(			new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.MHDGenerator:			mhdGenerators.Add(			new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.Residence:				residences.Add(				new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.RobotCommand:			robotCommands.Add(			new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.TradeCenter:			tradeCenters.Add(			new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.BasicLab:				basicLabs.Add(				new LabState(unit,			unitStructureInfo, unitWeaponInfo));		break;
					case map_id.MedicalCenter:			medicalCenters.Add(			new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.Nursery:				nurseries.Add(				new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.SolarPowerArray:		solarPowerArrays.Add(		new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.RecreationFacility:		recreationFacilities.Add(	new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.University:				universities.Add(			new UniversityState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.Agridome:				agridomes.Add(				new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.DIRT:					dirts.Add(					new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.Garage:					garages.Add(				new GarageState(unit,		unitStructureInfo, unitWeaponInfo));		break;
					case map_id.MagmaWell:				magmaWells.Add(				new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.MeteorDefense:			meteorDefenses.Add(			new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.GeothermalPlant:		geothermalPlants.Add(		new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.ArachnidFactory:		arachnidFactories.Add(		new FactoryState(unit,		unitStructureInfo, unitWeaponInfo));		break;
					case map_id.ConsumerFactory:		consumerFactories.Add(		new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.StructureFactory:		structureFactories.Add(		new FactoryState(unit,		unitStructureInfo, unitWeaponInfo));		break;
					case map_id.VehicleFactory:			vehicleFactories.Add(		new FactoryState(unit,		unitStructureInfo, unitWeaponInfo));		break;
					case map_id.StandardLab:			standardLabs.Add(			new LabState(unit,			unitStructureInfo, unitWeaponInfo));		break;
					case map_id.AdvancedLab:			advancedLabs.Add(			new LabState(unit,			unitStructureInfo, unitWeaponInfo));		break;
					case map_id.Observatory:			observatories.Add(			new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.ReinforcedResidence:	reinforcedResidences.Add(	new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.AdvancedResidence:		advancedResidences.Add(		new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.CommonOreSmelter:		commonOreSmelters.Add(		new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.Spaceport:				spaceports.Add(				new SpaceportState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.RareOreSmelter:			rareOreSmelters.Add(		new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.GORF:					gorfs.Add(					new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
					case map_id.Tokamak:				tokamaks.Add(				new StructureState(unit,	unitStructureInfo, unitWeaponInfo));		break;
				}
			}

			this.cargoTrucks			= cargoTrucks.AsReadOnly();
			this.convecs				= convecs.AsReadOnly();
			this.spiders				= spiders.AsReadOnly();
			this.scorpions				= scorpions.AsReadOnly();
			this.lynx					= lynx.AsReadOnly();
			this.panthers				= panthers.AsReadOnly();
			this.tigers					= tigers.AsReadOnly();
			this.roboSurveyors			= roboSurveyors.AsReadOnly();
			this.roboMiners				= roboMiners.AsReadOnly();
			this.geoCons				= geoCons.AsReadOnly();
			this.scouts					= scouts.AsReadOnly();
			this.roboDozers				= roboDozers.AsReadOnly();
			this.evacTransports			= evacTransports.AsReadOnly();
			this.repairVehicles			= repairVehicles.AsReadOnly();
			this.earthWorkers			= earthWorkers.AsReadOnly();

			// Structures
			this.commonOreMines			= commonOreMines.AsReadOnly();
			this.rareOreMines			= rareOreMines.AsReadOnly();
			this.guardPosts				= guardPosts.AsReadOnly();
			this.lightTowers			= lightTowers.AsReadOnly();
			this.commonStorages			= commonStorages.AsReadOnly();
			this.rareStorages			= rareStorages.AsReadOnly();
			this.forums					= forums.AsReadOnly();
			this.commandCenters			= commandCenters.AsReadOnly();
			this.mhdGenerators			= mhdGenerators.AsReadOnly();
			this.residences				= residences.AsReadOnly();
			this.robotCommands			= robotCommands.AsReadOnly();
			this.tradeCenters			= tradeCenters.AsReadOnly();
			this.basicLabs				= basicLabs.AsReadOnly();
			this.medicalCenters			= medicalCenters.AsReadOnly();
			this.nurseries				= nurseries.AsReadOnly();
			this.solarPowerArrays		= solarPowerArrays.AsReadOnly();
			this.recreationFacilities	= recreationFacilities.AsReadOnly();
			this.universities			= universities.AsReadOnly();
			this.agridomes				= agridomes.AsReadOnly();
			this.dirts					= dirts.AsReadOnly();
			this.garages				= garages.AsReadOnly();
			this.magmaWells				= magmaWells.AsReadOnly();
			this.meteorDefenses			= meteorDefenses.AsReadOnly();
			this.geothermalPlants		= geothermalPlants.AsReadOnly();
			this.arachnidFactories		= arachnidFactories.AsReadOnly();
			this.consumerFactories		= consumerFactories.AsReadOnly();
			this.structureFactories		= structureFactories.AsReadOnly();
			this.vehicleFactories		= vehicleFactories.AsReadOnly();
			this.standardLabs			= standardLabs.AsReadOnly();
			this.advancedLabs			= advancedLabs.AsReadOnly();
			this.observatories			= observatories.AsReadOnly();
			this.reinforcedResidences	= reinforcedResidences.AsReadOnly();
			this.advancedResidences		= advancedResidences.AsReadOnly();
			this.commonOreSmelters		= commonOreSmelters.AsReadOnly();
			this.spaceports				= spaceports.AsReadOnly();
			this.rareOreSmelters		= rareOreSmelters.AsReadOnly();
			this.gorfs					= gorfs.AsReadOnly();
			this.tokamaks				= tokamaks.AsReadOnly();

			// Starship module counts
			PlayerStarship starship = GameState.playerStarships[playerID];

			EDWARDSatelliteCount	= starship.EDWARDSatelliteCount;
			solarSatelliteCount		= starship.solarSatelliteCount;
			ionDriveModuleCount		= starship.ionDriveModuleCount;
			fusionDriveModuleCount	= starship.fusionDriveModuleCount;
			commandModuleCount		= starship.commandModuleCount;
			fuelingSystemsCount		= starship.fuelingSystemsCount;
			habitatRingCount		= starship.habitatRingCount;
			sensorPackageCount		= starship.sensorPackageCount;
			skydockCount			= starship.skydockCount;
			stasisSystemsCount		= starship.stasisSystemsCount;
			orbitalPackageCount		= starship.orbitalPackageCount;
			phoenixModuleCount		= starship.phoenixModuleCount;

			rareMetalsCargoCount	= starship.rareMetalsCargoCount;
			commonMetalsCargoCount	= starship.commonMetalsCargoCount;
			foodCargoCount			= starship.foodCargoCount;
			evacuationModuleCount	= starship.evacuationModuleCount;
			childrenModuleCount		= starship.childrenModuleCount;
		}

		public IReadOnlyCollection<UnitState> GetListForType(map_id type)
		{
			switch (type)
			{
				case map_id.CommonOreMine:			return commonOreMines;
				case map_id.RareOreMine:			return rareOreMines;
				case map_id.GuardPost:				return guardPosts;
				case map_id.LightTower:				return lightTowers;
				case map_id.CommonStorage:			return commonStorages;
				case map_id.RareStorage:			return rareStorages;
				case map_id.Forum:					return forums;
				case map_id.CommandCenter:			return commandCenters;
				case map_id.MHDGenerator:			return mhdGenerators;
				case map_id.Residence:				return residences;
				case map_id.RobotCommand:			return robotCommands;
				case map_id.TradeCenter:			return tradeCenters;
				case map_id.BasicLab:				return basicLabs;
				case map_id.MedicalCenter:			return medicalCenters;
				case map_id.Nursery:				return nurseries;
				case map_id.SolarPowerArray:		return solarPowerArrays;
				case map_id.RecreationFacility:		return recreationFacilities;
				case map_id.University:				return universities;
				case map_id.Agridome:				return agridomes;
				case map_id.DIRT:					return dirts;
				case map_id.Garage:					return garages;
				case map_id.MagmaWell:				return magmaWells;
				case map_id.MeteorDefense:			return meteorDefenses;
				case map_id.GeothermalPlant:		return geothermalPlants;
				case map_id.ArachnidFactory:		return arachnidFactories;
				case map_id.ConsumerFactory:		return consumerFactories;
				case map_id.StructureFactory:		return structureFactories;
				case map_id.VehicleFactory:			return vehicleFactories;
				case map_id.StandardLab:			return standardLabs;
				case map_id.AdvancedLab:			return advancedLabs;
				case map_id.Observatory:			return observatories;
				case map_id.ReinforcedResidence:	return reinforcedResidences;
				case map_id.AdvancedResidence:		return advancedResidences;
				case map_id.CommonOreSmelter:		return commonOreSmelters;
				case map_id.Spaceport:				return spaceports;
				case map_id.RareOreSmelter:			return rareOreSmelters;
				case map_id.GORF:					return gorfs;
				case map_id.Tokamak:				return tokamaks;

				case map_id.CargoTruck:				return cargoTrucks;
				case map_id.ConVec:					return convecs;
				case map_id.Spider:					return spiders;
				case map_id.Scorpion:				return scorpions;
				case map_id.Lynx:					return lynx;
				case map_id.Panther:				return panthers;
				case map_id.Tiger:					return tigers;
				case map_id.RoboSurveyor:			return roboSurveyors;
				case map_id.RoboMiner:				return roboMiners;
				case map_id.GeoCon:					return geoCons;
				case map_id.Scout:					return scouts;
				case map_id.RoboDozer:				return roboDozers;
				case map_id.EvacuationTransport:	return evacTransports;
				case map_id.RepairVehicle:			return repairVehicles;
				case map_id.Earthworker:			return earthWorkers;
			}

			return null;
		}

		public UnitState GetClosestUnitOfType(map_id unitType, LOCATION position)
		{
			UnitState closestUnit = null;
			int closestDistance = 999999;

			foreach (UnitState unit in GetListForType(unitType))
			{
				// Closest distance
				int distance = position.GetDiagonalDistance(unit.position);
				if (distance < closestDistance)
				{
					closestUnit = unit;
					closestDistance = distance;
				}
			}

			return closestUnit;
		}

		public IEnumerable<UnitState> GetUnits()		{ return new UnitEnumerable(this, UnitEnumerable.EnumType.Units);			}
		public IEnumerable<UnitState> GetStructures()	{ return new UnitEnumerable(this, UnitEnumerable.EnumType.Structures);		}
		public IEnumerable<UnitState> GetVehicles()		{ return new UnitEnumerable(this, UnitEnumerable.EnumType.Vehicles);		}

		private class UnitEnumerable : IEnumerable<UnitState>
		{
			public enum EnumType { Units, Vehicles, Structures }

			private PlayerUnitState m_List;
			private EnumType m_EnumType;

			public UnitEnumerable(PlayerUnitState list, EnumType enumType)
			{
				m_List = list;
				m_EnumType = enumType;
			}

			public IEnumerator<UnitState> GetEnumerator()
			{
				if (m_EnumType == EnumType.Units || m_EnumType == EnumType.Structures)
				{
					// Enumerate all structures
					for (int i=21; i < 59; ++i)
					{
						IEnumerable<UnitState> structures = m_List.GetListForType((map_id)i);
						foreach (UnitState unit in structures)
							yield return unit;
					}
				}
				if (m_EnumType == EnumType.Units || m_EnumType == EnumType.Vehicles)
				{
					// Enumerate all vehicles
					for (int i=1; i < 16; ++i)
					{
						IEnumerable<UnitState> vehicles = m_List.GetListForType((map_id)i);
						foreach (UnitState unit in vehicles)
							yield return unit;
					}
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}
	}
}
