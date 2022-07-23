

using System;
using System.Collections.Generic;

namespace Weathering
{
    [ConstructionCostBase(typeof(Book), 100, 10)]
    public class LibraryOfConstruction : AbstractTechnologyCenter
    {
        public const long BaseCost = 100;
        protected override long TechnologyPointMaxRevenue => BaseCost;
        protected override Type TechnologyPointType => typeof(ToolPrimitive);
        protected override long TechnologyPointIncRequired => 1;

        protected override List<(Type, long)> TechList => new List<(Type, long)> {

            (typeof(ResidenceOfGrass), 0),
            (typeof(WareHouseOfGrass), 0),

            (typeof(ResidenceOfWood), 1*BaseCost),
            (typeof(WareHouseOfWood), 1*BaseCost),

            (typeof(ResidenceCoastal), 1*BaseCost),
            (typeof(ResidenceOverTree), 1*BaseCost),

            (typeof(ResidenceOfStone), 2*BaseCost),
            (typeof(WareHouseOfStone), 2*BaseCost),

            (typeof(ResidenceOfBrick), 3*BaseCost),
            (typeof(WareHouseOfBrick), 3*BaseCost),

            // (typeof(ResidenceOfConcrete), 5*BaseCost),
        };

        public const long ToForestCost = 100;
        protected override void DecorateItems(List<IUIItem> items, Action onTap) {
            base.DecorateItems(items, onTap);
            if (Globals.Unlocked<KnowledgeOfClearingForest>()) {
                IValue ival = Globals.Ins.Values.Get(TechnologyPointType);
                const long cost = ToForestCost;
                items.Add(CreateTerraformButton("批量砍树", 3, cost, ival, typeof(TerrainType_Forest), typeof(TerrainType_Plain)));
                items.Add(CreateTerraformButton("批量种树", 3, cost, ival, typeof(TerrainType_Plain), typeof(TerrainType_Forest)));
            }
        }
        //private UIItem CreateTerraformButton(string description, int range, long cost, IValue ival, Type source, Type target) {
        //    return UIItem.CreateStaticButton($"{description}{2 * range + 1}x{2 * range + 1} 每格{Localization.Ins.Val(TechnologyPointType, -cost)} ", () => {
        //        for (int i = -range; i <= range; i++) {
        //            for (int j = -range; j <= range; j++) {
        //                ITile tile = Map.Get(Pos + new UnityEngine.Vector2Int(i, j));
        //                if (ival.Val >= cost && tile is MapOfPlanetDefaultTile planetTile && planetTile.TerraformedTerrainType == source) {
        //                    planetTile.TerraformedTerrainType = target;
        //                    ival.Val -= cost;
        //                }
        //            }
        //        }
        //    }, ival.Val >= cost);
        //}
        private UIItem CreateTerraformButton(string description, int range, long cost, IValue ival, Type source, Type target) {
            return UIItem.CreateStaticButton($"{description}{2 * range + 1}x{2 * range + 1} 每格{Localization.Ins.Val(TechnologyPointType, -cost)} ", () => {
                for (int i = 0; i <= range; i++) {
                    for (int j = 0; j <= range; j++) {
                        ITile tile = Map.Get(Pos + new UnityEngine.Vector2Int(i, j));
                        if (ival.Val >= cost && tile is MapOfPlanetDefaultTile planetTile && planetTile.TerraformedTerrainType == source) {
                            planetTile.TerraformedTerrainType = target;
                            ival.Val -= cost;
                            if (ival.Val < cost) return;
                        }
                    }
                }
                for (int i = -range; i < 0; i++) {
                    for (int j = -range; j < 0; j++) {
                        ITile tile = Map.Get(Pos + new UnityEngine.Vector2Int(i, j));
                        if (ival.Val >= cost && tile is MapOfPlanetDefaultTile planetTile && planetTile.TerraformedTerrainType == source) {
                            planetTile.TerraformedTerrainType = target;
                            ival.Val -= cost;
                            if (ival.Val < cost) return;
                        }
                    }
                }
                for (int i = 0; i <= range; i++) {
                    for (int j = -range; j < 0; j++) {
                        ITile tile = Map.Get(Pos + new UnityEngine.Vector2Int(i, j));
                        if (ival.Val >= cost && tile is MapOfPlanetDefaultTile planetTile && planetTile.TerraformedTerrainType == source) {
                            planetTile.TerraformedTerrainType = target;
                            ival.Val -= cost;
                            if (ival.Val < cost) return;
                        }
                    }
                }
                for (int i = -range; i < 0; i++) {
                    for (int j = 0; j <= range; j++) {
                        ITile tile = Map.Get(Pos + new UnityEngine.Vector2Int(i, j));
                        if (ival.Val >= cost && tile is MapOfPlanetDefaultTile planetTile && planetTile.TerraformedTerrainType == source) {
                            planetTile.TerraformedTerrainType = target;
                            ival.Val -= cost;
                            if (ival.Val < cost) return;
                        }
                    }
                }

            }, ival.Val >= cost);
        }
    }
}
