

using System;

namespace Weathering
{
    // 金属锭
    [Depend(typeof(DiscardableSolid))]
    public class MetalIngot { }


    [BindSpriteKey(typeof(WorkshopOfMetalSmelting))]
    [ConstructionCostBase(typeof(StoneBrick), 100)]
    public class WorkshopOfMetalSmelting : AbstractFactoryStatic
    {
        protected override (Type, long) In_0_Inventory => (typeof(Worker), 1);

        protected override (Type, long) Out0 => (typeof(MetalIngot), 1);

        protected override (Type, long) In_0 => (typeof(MetalOre), 2);

        protected override (Type, long) In_1 => (typeof(Fuel), 1);
    }
}
