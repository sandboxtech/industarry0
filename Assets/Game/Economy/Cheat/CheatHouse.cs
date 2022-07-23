﻿

using System.Collections.Generic;

namespace Weathering
{
    public class CheatHouse : StandardTile
    {
        public override string SpriteKey => "Pitaya";
        public override void OnTap() {
            var items = UI.Ins.GetItems();
            items_ = items;

            AddButtonOfSupply<Electricity>(100);
            AddButtonOfSupply<Worker>(10);

            AddButton<GoldCoin>(10);
            AddButton<WoodPlank>(100);
            AddButton<StoneBrick>(100);
            AddButton<Brick>(100);

            items_ = null;

            if (CanDestruct()) items.Add(UIItem.CreateDynamicDestructButton<MapOfPlanetDefaultTile>(this));

            UI.Ins.ShowItems("作弊点", items);
        }
        private List<IUIItem> items_;

        private void AddButton<T>(long quantity) {
            items_.Add(UIItem.CreateButton($"增加{Localization.Ins.Val<T>(quantity)}", () => Map.Inventory.Add<T>(quantity)));
        }
        private void AddButtonOfSupply<T>(long quantity) {
            items_.Add(UIItem.CreateButton($"增加{Localization.Ins.Val<T>(quantity)}", () => Map.InventoryOfSupply.Add<T>(quantity)));
        }
        public override bool CanDestruct() => true;
    }
}
