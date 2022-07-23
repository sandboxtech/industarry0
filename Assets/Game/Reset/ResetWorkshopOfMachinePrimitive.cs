

using System;

namespace Weathering
{


    [BindSpriteKey(typeof(Workshop))]
    [ConstructionCostBase(typeof(ResetPointMachine), 10, 2)]
    public class ResetWorkshopOfMachinePrimitive : AbstractFactoryStatic
    {
        protected override (Type, long) In_0_Inventory => (typeof(Worker), 5);

        protected override (Type, long) Out0 => (typeof(MachinePrimitive), 1);

    }
}
