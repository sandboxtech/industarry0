

using System;

namespace Weathering
{

    // 铁锭
    [Depend(typeof(MetalIngot))]
    public class IronIngot { }


    [BindSpriteKey(typeof(WorkshopOfMetalSmelting))]
    [ConstructionCostBase(typeof(ToolPrimitive), 100)]
    public class WorkshopOfIronSmelting : AbstractFactoryStatic
    {
        protected override (Type, long) In_0_Inventory => (typeof(Worker), 1);

        protected override (Type, long) Out0 => (typeof(IronIngot), 1);

        protected override (Type, long) In_0 => (typeof(IronOre), 1);

        protected override (Type, long) In_1 => (typeof(Fuel), 5);
    }
}
