﻿

using System;
using System.Collections.Generic;

namespace Weathering
{
    [ConstructionCostBase(typeof(SchoolEquipment), 100, 10)]
    public class SchoolOfPhysics : AbstractTechnologyCenter
    {
        public const long BaseCost = 1000;
        protected override long TechnologyPointMaxRevenue => BaseCost;
        protected override Type TechnologyPointType => typeof(SchoolEquipment);

        protected override List<(Type, long)> TechList => new List<(Type, long)> {

            (typeof(SeaWaterPump), 0),
            (typeof(PowerGeneratorOfWood), 0),
            (typeof(PowerGeneratorOfCoal), 1*BaseCost),
            (typeof(PowerGeneratorOfLiquefiedPetroleumGas), 2*BaseCost),
            (typeof(PowerGeneratorOfWindTurbineStation), 3*BaseCost),
            (typeof(PowerGeneratorOfSolarPanelStation), 3*BaseCost),
            (typeof(PowerGeneratorOfNulearFissionEnergy), 5*BaseCost),
            (typeof(PowerGeneratorOfNulearFusionEnergy), 5*BaseCost),
        };
    }
}
