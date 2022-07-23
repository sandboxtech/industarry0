﻿

using System;

namespace Weathering
{
    [BindSpriteKey(typeof(FactoryOfLightOilCracking))]
    [ConstructionCostBase(typeof(BuildingPrefabrication), 100)]
    public class FactoryOfLightOilCracking : AbstractFactoryStatic
    {
        protected override (Type, long) In_1_Inventory => (typeof(Electricity), 5);

        protected override (Type, long) Out0 => (typeof(LiquefiedPetroleumGas), 1);
        protected override (Type, long) In_0 => (typeof(LightOil), 1);
    }
}
