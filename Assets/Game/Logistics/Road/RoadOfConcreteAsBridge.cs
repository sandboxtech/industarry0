

using System;
using System.Collections.Generic;

namespace Weathering
{
    [CanBeBuildOnNotPassableTerrain]
    [ConstructionCostBase(typeof(ConcretePowder), 50, 0)]
    [BindTerrainType(typeof(TerrainType_Sea))]
    public class RoadOfConcreteAsBridge : AbstractRoad
    {

        public override string SpriteKeyRoad {
            get {
                int index = TileUtility.Calculate4x4RuleTileIndex(this, (tile, direction) => Refs.Has(direction)
                || (tile is IPassable passable && passable.Passable)
                || ((RoadRef.Type == null) && (tile is AbstractRoad) && (tile as AbstractRoad).RoadRef.Type == null)
                );
                return $"RoadOfConcreteAsBridge_{index}";
            }
        }

        protected override bool PreserveLandscape => true;

        protected override string SpriteKeyRoadBase => "RoadOfConcreteAsBridge";

        public override long LinkQuantityRestriction => RoadForSolid.CAPACITY * 3;

        public override Type LinkTypeRestriction => typeof(DiscardableSolid);


    }
}
