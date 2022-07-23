

using System;
using System.Collections.Generic;

namespace Weathering
{
    [ConstructionCostBase(typeof(SchoolEquipment), 100, 10)]
    public class SchoolOfConstruction : AbstractTechnologyCenter
    {
        public const long BaseCost = 1000;

        protected override long TechnologyPointMaxRevenue => BaseCost;
        protected override Type TechnologyPointType => typeof(ConcretePowder);

        protected override List<(Type, long)> TechList => new List<(Type, long)> {

            (typeof(WorkshopOfBuildingPrefabrication),  1*BaseCost),
            (typeof(ResidenceOfConcrete),  1*BaseCost),
            (typeof(WareHouseOfConcrete), 1*BaseCost),
            (typeof(ResidenceOfApartment),  2*BaseCost),
            (typeof(WareHouseOfBuildingPrefabrication), 2*BaseCost),
            (typeof(ResidenceOfSkyscraper),  3*BaseCost),
            (typeof(WareHouseOfLightMaterial), 3*BaseCost),
        };
    }
}
