

using System;

namespace Weathering
{
    [Depend(typeof(DiscardableSolid))]
    public class CottonThread { }

    [BindSpriteKey(typeof(Workshop))]
    [ConstructionCostBase(typeof(Wood), 100)]
    public class SpinningWheel : AbstractFactoryStatic
    {
        protected override (Type, long) In_0_Inventory => (typeof(Worker), 1);

        protected override (Type, long) Out0 => (typeof(CottonThread), 1);

        protected override (Type, long) In_0 => (typeof(Cotton), 1);
    }
}
