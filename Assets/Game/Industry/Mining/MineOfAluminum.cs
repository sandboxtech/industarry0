

using System;

namespace Weathering
{
    // 铝土 Bauxite
    [BindMine(typeof(MineOfAluminum))]
    [Depend(typeof(DiscardableSolid))]
    public class AluminumOre : IMineralType { }

    [BindSpriteKey(typeof(MountainMine))]
    [ConstructionCostBase(typeof(BuildingPrefabrication), 100)]
    [CanBeBuildOnNotPassableTerrain]
    [BindTerrainType(typeof(TerrainType_Mountain))]
    [BindMineral(typeof(AluminumOre))]
    [Concept]
    public class MineOfAluminum : AbstractFactoryStatic, IPassable
    {
        protected override bool PreserveLandscape => true;
        protected override (Type, long) In_0_Inventory => (typeof(Worker), 1);
        protected override (Type, long) Out0 => (typeof(AluminumOre), 3);

        public bool Passable => false;
    }
}
