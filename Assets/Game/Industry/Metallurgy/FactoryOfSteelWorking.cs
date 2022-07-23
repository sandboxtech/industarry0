

using System;

namespace Weathering
{
    public class Factory { }


    // 钢厂 混合
    [BindSpriteKey(typeof(Factory))]
    [ConstructionCostBase(typeof(BuildingPrefabrication), 100)]
    public class FactoryOfSteelWorking : AbstractFactoryStatic
    {
        protected override (Type, long) In_0_Inventory => (typeof(Worker), 1);
        protected override (Type, long) In_1_Inventory => (typeof(Electricity), 20);
        protected override (Type, long) Out0 => (typeof(SteelIngot), 2);
        protected override (Type, long) In_0 => (typeof(IronIngot), 3);
    }
}
