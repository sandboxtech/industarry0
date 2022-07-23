

using System;
using System.Collections.Generic;

namespace Weathering
{

    public class KnowledgeOfRunning { }

    public class KnowledgeOfRunningFast { }

    [ConstructionCostBase(typeof(Book), 100, 10)]
    public class LibraryOfGeography : AbstractTechnologyCenter
    {
        private const long BaseCost = 3000;
        protected override long TechnologyPointMaxRevenue => BaseCost;
        protected override long TechnologyPointIncRequired => 1;
        protected override Type TechnologyPointType => typeof(Stone);

        protected override List<(Type, long)> TechList => new List<(Type, long)> {

            (typeof(MountainQuarry), 0),
            (typeof(MineOfClay), 1*BaseCost),
            (typeof(MineOfGold), 2*BaseCost),
            (typeof(MineOfCopper), 5*BaseCost),
            (typeof(MineOfIron), 5*BaseCost),
            //(typeof(MineOfSand), 1000),
            //(typeof(MineOfSalt), 1000),
        };

        public const long ToForestCost = 60;
        protected override void DecorateItems(List<IUIItem> items, Action onTap) {
            base.DecorateItems(items, onTap);
            if (Globals.Unlocked<KnowledgeOfClearingForest>()) {
                IValue ival = Globals.Ins.Values.Get(TechnologyPointType);
                const long cost = ToForestCost;
                items.Add(CreateTerraformButton("批量砍树", 3, cost, ival, typeof(TerrainType_Forest), typeof(TerrainType_Plain)));
                items.Add(CreateTerraformButton("批量种树", 3, cost, ival, typeof(TerrainType_Plain), typeof(TerrainType_Forest)));
            }
        }
        private UIItem CreateTerraformButton(string description, int range, long cost, IValue ival, Type source, Type target) {
            return UIItem.CreateStaticButton($"{description}{2 * range + 1}x{2 * range + 1} 每格{Localization.Ins.Val(TechnologyPointType, -cost)} ", () => {
                for (int i = -range; i <= range; i++) {
                    for (int j = -range; j <= range; j++) {
                        ITile tile = Map.Get(Pos + new UnityEngine.Vector2Int(i, j));
                        if (ival.Val >= cost && tile is MapOfPlanetDefaultTile planetTile && planetTile.TerraformedTerrainType == source) {
                            planetTile.TerraformedTerrainType = target;
                            ival.Val -= cost;
                        }
                    }
                }
            }, ival.Val >= cost);
        }
    }
}
