

using System;

namespace Weathering
{
    // 石砖
    [Depend(typeof(DiscardableSolid))]
    public class StoneBrick { }


    [BindSpriteKey(typeof(Workshop))]
    [ConstructionCostBase(typeof(WoodPlank), 100)]
    public class WorkshopOfStonecutting : AbstractFactoryStatic
    {
        protected override (Type, long) In_0_Inventory => (typeof(Worker), 1);

        protected override (Type, long) Out0 => (typeof(StoneBrick), 1);

        protected override (Type, long) In_0 => (typeof(Stone), 3);
    }
}
