

using System;
using System.Collections.Generic;

namespace Weathering
{
    // "Weathering.SchoolOfBiology": null,
    [ConstructionCostBase(typeof(SchoolEquipment), 100, 10)]
    public class SchoolOfBiology : AbstractTechnologyCenter
    {
        public const long BaseCost = 1000;
        protected override long TechnologyPointMaxRevenue => BaseCost;
        protected override Type TechnologyPointType => typeof(BookWithNoteAdvanced);
        protected override long TechnologyPointIncRequired => 1;

        protected override List<(Type, long)> TechList => new List<(Type, long)> {

            (typeof(WorkshopOfBookWithNoteAdvanced), 0),
            (typeof(BerryBushHydroponics), 1*BaseCost),
            (typeof(FarmHydroponics), 1*BaseCost),
        };
    }
}
