﻿

using System;
using System.Collections.Generic;

namespace Weathering
{
    [CanBeBuildOnNotPassableTerrain]
    [ConstructionCostBase(typeof(ConcretePowder), 30, 0)]
    [BindTerrainType(typeof(TerrainType_Mountain))]
    public class RoadAsTunnelAdvanced : AbstractRoad
    {
        protected override bool PreserveLandscape => true;

        public override string SpriteKeyRoad {
            get {
                int index = TileUtility.Calculate4x4RuleTileIndex(this, (tile, direction) => Refs.Has(direction) 
                || (tile is IPassable passable && passable.Passable) || ((RoadRef.Type == null) && (tile is AbstractRoad) && (tile as AbstractRoad).RoadRef.Type == null)
                );
                return $"RoadAsTunnelAdvanced_{index}";
            }
        }
        public override long LinkQuantityRestriction => RoadForSolid.CAPACITY * 5;

        public override Type LinkTypeRestriction => typeof(DiscardableSolid);

        protected override void AddBuildingDescriptionPage(List<IUIItem> items) {
            items.Add(UIItem.CreateText($"使用锤子工具, 可以在山里建造{Localization.Ins.Get<RoadAsTunnelAdvanced>()}"));
        }
    }
}
