

using System;

namespace Weathering
{
    [BindSpriteKey(typeof(DefaultBuilding))]
    [ConstructionCostBase(typeof(BuildingPrefabrication), 10, 0)]
    [BindTerrainType(typeof(TerrainType_Plain))]
    public class RoadLoaderOfRoadAsRailRoad : AbstractRoad, IRoadAsRailRoad_CanLinkWith
    {
        public override string SpriteKeyRoad => UIItem.TryGetIconName(typeof(RoadLoaderOfRoadAsRailRoad));

        public override long RoadQuantityRestriction => 60;

        public override Type LinkTypeRestriction => typeof(DiscardableSolid);
    }
}
