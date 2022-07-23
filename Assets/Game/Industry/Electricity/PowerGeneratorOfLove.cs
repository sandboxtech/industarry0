

using System;

namespace Weathering
{
    [ConstructionCostBase(typeof(ViewAdPoint), 1, 10)]
    public class PowerGeneratorOfLove : AbstractFactoryStatic
    {
        protected override (Type, long) Out0_Inventory => (typeof(Electricity), 10);
    }
}
