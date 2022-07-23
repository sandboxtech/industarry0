

using System;

namespace Weathering
{
    public class DefaultBuilding { }

    [BindSpriteKey(typeof(DefaultBuilding))]
    [ConstructionCostBase(typeof(BuildingPrefabrication), 10, 0)]
    [BindTerrainType(typeof(TerrainType_Plain))]
    public class RoadLoaderOfRoadAsCanal : AbstractRoad, IRoadAsCanalRoad_CanLinkWith
    {
        public override string SpriteKeyRoad => UIItem.TryGetIconName(typeof(RoadLoaderOfRoadAsCanal));

        public override long RoadQuantityRestriction => 20;

        public override Type LinkTypeRestriction => typeof(DiscardableSolid);

    }
}
