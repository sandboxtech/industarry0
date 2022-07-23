

using System;

namespace Weathering
{
    [ConstructionCostBase(typeof(SolarPanelComponent), 100, 50)]
    public class PowerGeneratorOfSolarPanelStation : AbstractFactoryStatic
    {

        protected override (Type, long) Out0_Inventory => (typeof(Electricity), 30);
    }
}
