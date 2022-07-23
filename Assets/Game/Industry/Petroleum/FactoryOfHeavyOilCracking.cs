﻿

using System;

namespace Weathering
{
    [BindSpriteKey(typeof(FactoryOfHeavyOilCracking))]
    [ConstructionCostBase(typeof(BuildingPrefabrication), 100)]
    public class FactoryOfHeavyOilCracking : AbstractFactoryStatic
    {
        protected override (Type, long) In_1_Inventory => (typeof(Electricity), 5);

        protected override (Type, long) Out0 => (typeof(LightOil), 1);
        protected override (Type, long) In_0 => (typeof(HeavyOil), 1);
    }
}
