

using System;
using System.Collections.Generic;

namespace Weathering
{
    [ConstructionCostBase(typeof(SchoolEquipment), 100, 10)]
    public class SchoolOfChemistry : AbstractTechnologyCenter
    {
        public const long BaseCost = 3000;
        protected override long TechnologyPointMaxRevenue => BaseCost;
        protected override Type TechnologyPointType => typeof(LiquefiedPetroleumGas);

        protected override List<(Type, long)> TechList => new List<(Type, long)> {

            (typeof(FactoryOfDesalination), 0*BaseCost),
            (typeof(FactoryOfPetroleumRefining), 0),
            (typeof(FactoryOfLightOilCracking), BaseCost),
            (typeof(FactoryOfHeavyOilCracking), BaseCost),
            (typeof(FactoryOfPlastic), 3*BaseCost),

            (typeof(FactoryOfJetFuel), 6*BaseCost),
            (typeof(FactoryOfFuelPack_Oxygen_Hydrogen), 6*BaseCost),
            (typeof(FactoryOfFuelPack_Oxygen_JetFuel), 6*BaseCost),

            (typeof(FactoryAsAirSeparator), 6*BaseCost),
            (typeof(FactoryOfElectrolysisOfSaltedWater), 6*BaseCost),
            (typeof(FactoryOfElectrolysisOfWater), 6*BaseCost),
        };
    }
}
