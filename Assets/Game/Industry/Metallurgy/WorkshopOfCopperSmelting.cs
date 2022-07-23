

using System;

namespace Weathering
{

    // 铜锭
    [Depend(typeof(MetalIngot))]
    public class CopperIngot { }


    [BindSpriteKey(typeof(WorkshopOfMetalSmelting))]
    [ConstructionCostBase(typeof(ToolPrimitive), 100)]
    public class WorkshopOfCopperSmelting : AbstractFactoryStatic
    {

        protected override (Type, long) In_0_Inventory => (typeof(Worker), 1);

        protected override (Type, long) Out0 => (typeof(CopperIngot), 1);

        protected override (Type, long) In_0 => (typeof(CopperOre), 1);

        protected override (Type, long) In_1 => (typeof(Fuel), 5);
    }
}
