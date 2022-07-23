

using System;

namespace Weathering
{
    [ConstructionCostBase(typeof(LightMaterial), 100, 20)]
    public class ResidenceOfSkyscraper : AbstractFactoryStatic
    {
        protected override (Type, long) In_0 => (typeof(Food), 36);
        protected override (Type, long) Out0_Inventory => (typeof(Worker), 12);
    }
}
