// Research.h
// Classes for modifying Research structures
#ifndef _RESEARCH_H_
#define _RESEARCH_H_

enum LabType
{
	ltBasic			= 1,
	ltStandard		= 2,
	ltAdvanced		= 3
};

// "CATEGORY" values (for both items and upgrades to these items)
enum TechCategory
{
	tcFree			= 0, // 0 = Free technologies (and unavailable technologies)
	tcBasic			= 1, // 1 = Basic labratory sciences
	tcDefense		= 2, // 2 = Defenses (GP upgrade, walls, and efficiency engineering)
	tcPower			= 3, // 3 = Power
	tcVehicles		= 4, // 4 = Vehicles (which ones can be built, speed upgrades, armour upgrades)
	tcFood			= 5, // 5 = Food
	tcMetals		= 6, // 6 = Metals gathering
	tcWeapons		= 7, // 7 = Weapons
	tcSpace			= 8, // 8 = Space (spaceport, observatory, launch vehicle, skydock)
	tcMorale		= 9, // 9 = Population (happiness)
	tcDisaster		= 10,// 10 = Disaster warning (and defense)
	tcPopulation	= 11,// 11 = Population (health, growth)
	tcSpaceship		= 12 // 12 = Spaceship module
};

class TechInfo
{
public:
	int IsValid();
	int GetTechID();
	TechCategory GetCategory();
	int GetTechLevel();
	int GetPlymouthCost();
	int GetEdenCost();
	int GetMaxScientists();
	LabType GetLab();
	int GetPlayerHasTech(); // (bit vector, bit set if player has tech)
	int GetNumUpgrades(); // (number of "UNIT_PROP"s specified in the tech file)
	int GetNumRequiredTechs(); // (number of "REQUIRES"s specified in the tech file)
	char* GetTechName();
	char* GetDescription();
	char* GetTeaser();
	char* GetImproveDesc();
	int GetRequiredTechIndex(int index);
	//int* GetRequiredTechNum(); // (pointer to array of techNums that are required for this tech)
	// void* GetUpgradeInfo();
	//int GetNumDependentTech(); // (number of techs that depend on this one) (below is a pointer to an int? list of this many elements)
	//int* GetDependentTechNum(int index); // (pointer to array of techNums of techs that depend on this one)

	void *internalPtr;
};

class Research
{
public:
	static int GetTechCount();
	static TechInfo GetTechInfo(int index);
	static int GetMaxTechID(); // [maxTechLevel * 1000 + 999]
};

struct OP2Research;
extern OP2Research *researchObj;



#endif // TECHINFO_H_