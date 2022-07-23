

using System;

namespace Weathering
{
    public interface IRoadAsCanalRoad_CanLinkWith { }

    [ConstructionCostBase(typeof(ToolPrimitive), 10, 0)]
    [BindTerrainType(typeof(TerrainType_Plain))]
    public class RoadAsCanal : AbstractRoad, ILinkTileTypeRestriction, IRoadAsCanalRoad_CanLinkWith
    {

        private const string road = "RoadAsCanal";
        protected override string SpriteKeyRoadBase => road;
        public override long RoadQuantityRestriction => 60;

        public override Type LinkTypeRestriction => typeof(DiscardableSolid);

        public Type LinkTileTypeRestriction => typeof(IRoadAsCanalRoad_CanLinkWith);
    }
}
