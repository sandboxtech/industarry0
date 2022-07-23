

using System;

namespace Weathering
{
    /// <summary>
    /// 永磁铁
    /// </summary>
    [Depend(typeof(DiscardableSolid))]
    public class PermanentMagnet { }


    [BindSpriteKey(typeof(Factory))]
    [ConstructionCostBase(typeof(BuildingPrefabrication), 100)]
    public class FactoryOfPermanentMagnet : AbstractFactoryStatic
    {
        protected override (Type, long) In_0_Inventory => (typeof(Worker), 1);
        protected override (Type, long) In_1_Inventory => (typeof(Electricity), 20);

        protected override (Type, long) Out0 => (typeof(PermanentMagnet), 1);
        protected override (Type, long) In_0 => (typeof(SteelIngot), 1);
    }
}
