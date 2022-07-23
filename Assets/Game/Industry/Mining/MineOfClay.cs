﻿

using System;

namespace Weathering
{
    // 粘土
    [Depend(typeof(DiscardableSolid))]
    public class Clay { }

    [BindSpriteKey(typeof(MountainQuarry))]
    [ConstructionCostBase(typeof(WoodPlank), 100)]
    [BindTerrainType(typeof(TerrainType_Mountain))]
    [Concept]
    public class MineOfClay : AbstractFactoryStatic, IPassable
    {
        protected override bool PreserveLandscape => true;
        public override string SpriteKey => typeof(MountainMine).Name;

        protected override (Type, long) In_0_Inventory => (typeof(Worker), 1);
        protected override (Type, long) Out0 => (typeof(Clay), 1);

        public bool Passable => false;
    }
}
