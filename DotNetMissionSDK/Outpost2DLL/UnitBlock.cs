/*
// Note: The UnitBlock class can be used for creating blocks of units
//		 of certain predefined types.

class OP2 UnitBlock
{
public:
	UnitBlock(UnitRecord* unitRecordTable);
	UnitBlock& operator = (const UnitBlock& unitBlock);
	int CreateUnits(int playerNum, int bLightsOn) const;	// Returns numUnitsCreated
private:
	void SortAndInit(UnitRecord* unitRecordTable);	// Sort unitRecordTable and initialize classRange table

public:
	struct Range
	{
		int startIndex;
		int untilIndex;
	};

	int numUnits;					// 0x0
	Range classRange[16];			// 0x4  Range of unit indexes in the unitRecordTable for each class
	UnitRecord* unitRecordTable;	// 0x84
};
*/