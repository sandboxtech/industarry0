

using System;

namespace Weathering
{
    // 铁器
    [Depend(typeof(MetalProduct))]
    public class IronProduct { }


    [BindSpriteKey(typeof(WorkshopOfMetalCasting))]
    [ConstructionCostBase(typeof(ToolPrimitive), 100)]
    public class WorkshopOfIronCasting : AbstractFactoryStatic
    {
        protected override (Type, long) In_0_Inventory => (typeof(Worker), 1);

        protected override (Type, long) Out0 => (typeof(IronProduct), 1);

        protected override (Type, long) In_0 => (typeof(IronIngot), 2);
    }
}
