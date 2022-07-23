

using System;
using System.Collections.Generic;

namespace Weathering
{
    [ConstructionCostBase(typeof(SchoolEquipment), 100, 10)]
    public class SchoolOfGeology : AbstractTechnologyCenter
    {
        public const long BaseCost = 6000;
        protected override long TechnologyPointMaxRevenue => BaseCost;
        protected override Type TechnologyPointType => typeof(Coal);
        protected override long TechnologyPointIncRequired => 5;

        protected override List<(Type, long)> TechList => new List<(Type, long)> {

            (typeof(MineOfCoal), 0),
            (typeof(KnowledgeOfTerrainform), 1*BaseCost),
            (typeof(MineOfAluminum), 1*BaseCost),
            (typeof(SeaWaterPump), 1*BaseCost),
            (typeof(OilDriller), 3*BaseCost),
        };

        public const long ToSeaCost = 300;
        protected override void DecorateItems(List<IUIItem> items, Action onTap) {
            base.DecorateItems(items, onTap);
            if (Globals.Unlocked<KnowledgeOfTerrainform>()) {
                IValue ival = Globals.Ins.Values.Get(TechnologyPointType);
                const long cost = ToSeaCost;
                items.Add(CreateTerraformButton("批量填海", 3, cost, ival, typeof(TerrainType_Sea), typeof(TerrainType_Plain)));
                items.Add(CreateTerraformButton("批量填海", 7, cost, ival, typeof(TerrainType_Sea), typeof(TerrainType_Plain)));
                items.Add(CreateTerraformButton("批量造海", 3, cost, ival, typeof(TerrainType_Plain), typeof(TerrainType_Sea)));
                items.Add(CreateTerraformButton("批量造海", 7, cost, ival, typeof(TerrainType_Plain), typeof(TerrainType_Sea)));
            }
        }
        private UIItem CreateTerraformButton(string description, int range, long cost, IValue ival, Type source, Type target) {
            return UIItem.CreateStaticButton($"{description}{2 * range + 1}x{2*range+1} 每格{Localization.Ins.Val(TechnologyPointType, -cost)} ", () => {
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
