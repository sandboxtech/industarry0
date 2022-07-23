

namespace Weathering
{

    public enum AdSelection
    {
        Berry1,

        Television, PlanetLander,
        Population3, Electricity10,

        SanityMax,

        Berry100,
        Grain100,
        WoodPlank100,
        BuildingPrefabrication100,

        Berry1000,
        Grain1000,
        WoodPlant1000,
        BuildingPrefabrication1000,
    }

    [ConstructionCostBase(typeof(MachinePrimitive), 100, 0)]
    public class Television : AbstractDecoration
    {
        public static AdSelection Ad;
        public override void OnTap() {

            var items = UI.Ins.GetItems();

            items.Add(UIItem.CreateTileImage(typeof(Television)));
            items.Add(UIItem.CreateMultilineText("一台旧世界的显示器, 只能循环播放无意义的内容"));


            items.Add(UIItem.CreateButton("查看旧世界电视广告", () => {
                Television.Ad = AdSelection.Television;
                PlanetLander.TryPlayAd();
            }));

            UI.Ins.ShowItems("电视", items);
        }
    }
}
