

using System;
using System.Collections.Generic;

namespace Weathering
{
    public class Pasture_Clothed_Clothing { }

    [ConstructionCostBase(typeof(Book), 100, 10)]
    public class Pasture_Clothed : AbstractTechnologyCenter
    {
        private const long BaseCost = 0;
        protected override long TechnologyPointMaxRevenue => BaseCost;
        protected override long TechnologyPointIncRequired => 1;
        protected override Type TechnologyPointType => typeof(FloppyDisk);

        public override string SpriteKeyHighLight => null;

        protected override List<(Type, long)> TechList => new List<(Type, long)> {

            (typeof(Pasture_Clothed_Clothing), 0),

            (typeof(PastureAdvancedOfCow), 0),
            (typeof(PastureAdvancedOfHen), 0),
            (typeof(PastureAdvancedOfPig), 0),

            //(typeof(Farm), 0),
            //(typeof(HuntingGround), 1*BaseCost),
            //(typeof(SeaFishery), 2*BaseCost),
            //(typeof(Pasture), 5*BaseCost),
            //(typeof(Hennery), 5*BaseCost),
        };

    }
}
