

using System;
using System.Collections.Generic;

namespace Weathering
{
    [Depend]
    public class FloppyDisk { }

    [ConstructionCostBase(typeof(Book), 100, 10)]
    public class LibraryOfClothing : AbstractTechnologyCenter
    {
        private const long BaseCost = 1000;
        protected override long TechnologyPointMaxRevenue => BaseCost;
        protected override long TechnologyPointIncRequired => 1;
        protected override Type TechnologyPointType => typeof(BookWithNote);

        protected override List<(Type, long)> TechList => new List<(Type, long)> {

            (typeof(WorkshopOfBookWithNote), 0),
            (typeof(PlantationOfCotton), 1*BaseCost),
            (typeof(SpinningWheel), 1*BaseCost),
            (typeof(Wardrobe), 1*BaseCost),
            (typeof(WallOfStoneBrick), 1*BaseCost),
            (typeof(Television), 1*BaseCost),

            (typeof(BerryBush_Clothed), 2*BaseCost),
            (typeof(Pasture_Clothed), 3*BaseCost),
            (typeof(Scarecrow_Clothed), 5*BaseCost),
            //(typeof(HuntingGround), 1*BaseCost),
            //(typeof(SeaFishery), 2*BaseCost),
            //(typeof(Pasture), 5*BaseCost),
            //(typeof(Hennery), 5*BaseCost),
        };

    }
}
