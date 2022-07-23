

using System;

namespace Weathering
{
    [ConstructionCostBase(typeof(WoodPlank), 100, 0)]
    public class Wardrobe : AbstractDecoration
    {
        public override void OnTap() {

            var items = UI.Ins.GetItems();

            items.Add(UIItem.CreateMultilineText("衣柜!!! 衣柜里面有下面的套装, 点击换装"));
            items.Add(UIItem.CreateSeparator());

            foreach (var item in CharacterView.Ins.Dict) {
                long id = item.Key;
                string name = item.Value.Item1;
                Type tech = item.Value.Item3;
                if (Globals.Unlocked(tech)) {
                    items.Add(UIItem.CreateButton(name, () => {
                        CharacterView.Ins.SetClothingID(id);
                        // OnTap();
                        UI.Ins.Active = false;
                    }));
                    if (item.Value.Item4 != null) {
                        items.Add(UIItem.CreateMultilineText(item.Value.Item4));
                        items.Add(UIItem.CreateSeparator());
                    }
                }

            }


            items.Add(UIItem.CreateStaticDestructButton(this));

            UI.Ins.ShowItems(Localization.Ins.Get<Wardrobe>(), items);
        }

        public override string SpriteKey => typeof(Wardrobe).Name;

    }
}
