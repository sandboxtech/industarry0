

using System;
using System.Collections.Generic;

namespace Weathering
{
    [ConstructionCostBase(typeof(SchoolEquipment), 100, 10)]
    public class SchoolOfAll : AbstractTechnologyCenter
    {
        public const long BaseCost = 1000;
        protected override long TechnologyPointMaxRevenue => BaseCost;
        protected override Type TechnologyPointType => typeof(SchoolEquipment);

        protected override List<(Type, long)> TechList => new List<(Type, long)> {

            (typeof(LibraryOfAll), 0),
            (typeof(SchoolOfAll), 0),

            (typeof(SchoolOfGeology), 1*BaseCost),
            (typeof(SchoolOfEngineering), 1*BaseCost),
            (typeof(SchoolOfConstruction), 1*BaseCost),
            (typeof(SchoolOfLogistics), 1*BaseCost),
            (typeof(SchoolOfPhysics), 2*BaseCost),
            (typeof(SchoolOfBiology), 2*BaseCost),
            (typeof(SchoolOfChemistry), 2*BaseCost),
            (typeof(SchoolOfElectronics), 2*BaseCost),
            (typeof(SchoolOfSpace), 3*BaseCost),
        };
    }
}
