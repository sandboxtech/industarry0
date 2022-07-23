

using System;

namespace Weathering
{
    [Depend(typeof(DiscardableSolid))]
    public class SteelPipe { }

    [BindSpriteKey(typeof(Factory))]
    [ConstructionCostBase(typeof(BuildingPrefabrication), 100)]
    public class FactoryOfSteelPipe : AbstractFactoryStatic
    {
        protected override (Type, long) In_0_Inventory => (typeof(Worker), 1);
        protected override (Type, long) In_1_Inventory => (typeof(Electricity), 2);
        protected override (Type, long) Out0 => (typeof(SteelPipe), 2);
        protected override (Type, long) In_0 => (typeof(SteelIngot), 1);
    }
}
