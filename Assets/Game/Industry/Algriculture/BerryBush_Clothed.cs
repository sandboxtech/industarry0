

using System;
using System.Collections.Generic;

namespace Weathering
{
    public class BerryBush_Clothed_Clothing { }

    [ConstructionCostBase(typeof(Book), 100, 10)]
    public class BerryBush_Clothed : AbstractTechnologyCenter
    {
        private const long BaseCost = 1000;
        protected override long TechnologyPointMaxRevenue => BaseCost;
        protected override long TechnologyPointIncRequired => 4;
        protected override Type TechnologyPointType => typeof(Berry);

        protected override List<(Type, long)> TechList => new List<(Type, long)> {

            (typeof(BerryBush_Clothed_Clothing), 0),
            (typeof(BerryBush_Clothed_Residence), 1*BaseCost),
            (typeof(BerryBush_Clothed_Tree), 3*BaseCost),
        };
    }
}
