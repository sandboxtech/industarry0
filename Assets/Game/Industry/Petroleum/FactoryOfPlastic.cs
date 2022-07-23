﻿

using System;

namespace Weathering
{
    // 塑料
    [Depend(typeof(DiscardableSolid))]
    [Concept]
    public class Plastic { }

    [BindSpriteKey(typeof(Factory))]
    [ConstructionCostBase(typeof(BuildingPrefabrication), 100)]
    public class FactoryOfPlastic : AbstractFactoryStatic
    {
        protected override (Type, long) In_0_Inventory => (typeof(Worker), 1);
        protected override (Type, long) In_1_Inventory => (typeof(Electricity), 10);
        protected override (Type, long) Out0 => (typeof(Plastic), 1);
        protected override (Type, long) In_0 => (typeof(LiquefiedPetroleumGas), 1);
    }
}
