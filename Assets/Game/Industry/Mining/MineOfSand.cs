

using System;

namespace Weathering
{
    // 沙子
    [Depend(typeof(DiscardableSolid))]
    [Concept]
    public class Sand { }

    [BindSpriteKey(typeof(MountainMine))]
    [ConstructionCostBase(typeof(WoodPlank), 100)]
    [BindTerrainType(typeof(TerrainType_Mountain))]
    [Concept]
    public class MineOfSand : AbstractFactoryStatic, IPassable
    {
        protected override bool PreserveLandscape => true;
        public override string SpriteKey => typeof(MountainMine).Name;

        protected override (Type, long) In_0_Inventory => (typeof(Worker), 1);
        protected override (Type, long) Out0 => (typeof(Clay), 3);

        public bool Passable => false;
    }
}
