﻿

using System;

namespace Weathering
{
    [ConstructionCostBase(typeof(BuildingPrefabrication), 100)]
    public class PowerGeneratorOfCoal : AbstractFactoryStatic
    {
        public override string SpriteKey => typeof(PowerPlant).Name;

        protected override (Type, long) In_0 => (typeof(Coal), 20);
        protected override (Type, long) In_1 => (typeof(Water), 3);
        protected override (Type, long) In_0_Inventory => (typeof(Worker), 1);
        protected override (Type, long) Out0_Inventory => (typeof(Electricity), 100);
    }
}
