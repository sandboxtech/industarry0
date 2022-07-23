﻿

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Weathering
{
    [BindTerrainType(typeof(TerrainType_Sea))]
    /// <summary>
    /// TransportStation的特征：物流输入任意, 背包输出对应的东西
    /// TransportStationDest的特征：输入背包里的东西, 物流输出对应的东西
    /// </summary>
    [ConstructionCostBase(typeof(MachinePrimitive), 100)]
    public class TransportStationPort : AbstractTransportStation
    {
        public const long PortCapacity = 30;
        protected override bool PreserveLandscape => true;
        protected override long Capacity => PortCapacity;
        protected override long CostQuantity => 2;
        protected override Type CostType => typeof(Worker);
    }
}
