

using System;

namespace Weathering
{
    [ConstructionCostBase(typeof(BuildingPrefabrication), 100, 20)]
    public class ResidenceOfApartment : AbstractFactoryStatic
    {
        protected override (Type, long) In_0 => (typeof(Food), 30);
        protected override (Type, long) Out0_Inventory => (typeof(Worker), 10);
    }
}
