

using System;

namespace Weathering
{

    [BindSpriteKey(typeof(Workshop))]
    [ConstructionCostBase(typeof(ResetPointTool), 10, 2)]
    public class ResetWorkshopOfToolPrimitive : AbstractFactoryStatic
    {
        protected override (Type, long) In_0_Inventory => (typeof(Worker), 2);

        protected override (Type, long) Out0 => (typeof(ToolPrimitive), 1);
    }
}
