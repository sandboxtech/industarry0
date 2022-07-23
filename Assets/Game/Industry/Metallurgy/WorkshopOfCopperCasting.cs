

using System;

namespace Weathering
{
    // 铜器
    [Depend(typeof(MetalProduct))]
    public class CopperProduct { }


    [BindSpriteKey(typeof(WorkshopOfMetalCasting))]
    [ConstructionCostBase(typeof(ToolPrimitive), 100)]
    public class WorkshopOfCopperCasting : AbstractFactoryStatic
    {
        protected override (Type, long) In_0_Inventory => (typeof(Worker), 1);

        protected override (Type, long) Out0 => (typeof(CopperProduct), 1);

        protected override (Type, long) In_0 => (typeof(CopperIngot), 3);
    }
}
