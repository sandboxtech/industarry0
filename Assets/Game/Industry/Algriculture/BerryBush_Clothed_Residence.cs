

using System;

namespace Weathering
{
    [ConstructionCostBase(typeof(Berry), 100, 5)]
    public class BerryBush_Clothed_Residence : AbstractFactoryStatic
    {
        protected override (Type, long) In_0 => (typeof(Berry), 6);
        protected override (Type, long) Out0_Inventory => (typeof(Worker), 3);
    }
}
