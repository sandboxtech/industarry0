
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Weathering
{
    [Depend]
    class ViewAdPoint : IResetPoint { }
    [Depend]
    class TryViewAdPoint : IResetPoint { }

    public class CharacterLanded { }

    public interface ILandable
    {
        bool Landed { get; }
        void Land(Vector2Int pos);
        void Leave(Vector2Int pos);
    }


    public class ResetCooldown { }

    public interface IResetPoint { }

    [Depend]
    public class ResetPointPop : IResetPoint { }
    [Depend]
    public class ResetPointPop100 : IResetPoint { }
    [Depend]
    public class ResetPointTool : IResetPoint { }
    [Depend]
    public class ResetPointMachine : IResetPoint { }
    [Depend]
    public class ResetPointLightMaterial : IResetPoint { }
    [Depend]
    public class ResetPointCircuit : IResetPoint { }


    public class PlanetLanderPos { }

    public class PlanetLanderRes { }

    [BindTerrainType(typeof(TerrainType_Plain))]
    public class PlanetLander : StandardTile, IStepOn, IIgnoreTool, IPassable
    {
        public bool Passable => true;

        public override string SpriteKey => typeof(PlanetLander).Name;
        public override string SpriteKeyHighLight => GlobalLight.Decorated(SpriteKey);

        //public override bool HasDynamicSpriteAnimation => true;
        //public override string SpriteLeft => Refs.Has<IRight>() && Refs.Get<IRight>().Value > 0 ? ConceptResource.Get(TypeOfResource.Type).Name : null;
        //public override string SpriteRight => Refs.Has<ILeft>() && Refs.Get<ILeft>().Value > 0 ? ConceptResource.Get(TypeOfResource.Type).Name : null;
        //public override string SpriteUp => Refs.Has<IDown>() && Refs.Get<IDown>().Value > 0 ? ConceptResource.Get(TypeOfResource.Type).Name : null;
        //public override string SpriteDown => Refs.Has<IUp>() && Refs.Get<IUp>().Value > 0 ? ConceptResource.Get(TypeOfResource.Type).Name : null;

        public bool IgnoreTool => true;

        public void OnStepOn() {
            // Res是以前火箭接入物流时用的
            if (Globals.Unlocked<KnowledgeOfPlanetLander>()) {
                LeavePlanet();
            }
        }

        public override void OnDestructWithMap() {
            LeavePlanet();
        }

        public void LeavePlanet() {
            ILandable landable = Map as ILandable;
            if (landable == null) throw new Exception();

            UI.Ins.Active = false;
            landable.Leave(Pos);

            IRef r = Globals.Ins.Refs.GetOrCreate<PlanetLanderPos>();
            r.X = -1;
            r.Y = -1;
        }

        public static PlanetLander Ins { get; private set; }
        public override void OnConstruct(ITile tile) {
            IRef r = Globals.Ins.Refs.GetOrCreate<PlanetLanderPos>();
            r.X = Pos.x;
            r.Y = Pos.y;
            base.OnConstruct(tile);
        }

        public override void OnDestruct(ITile newTile) {
            base.OnDestruct(newTile);
            Ins = null;
        }

        public override void OnEnable() {
            base.OnEnable();

            if (Ins != null) throw new Exception();
            Ins = this;

        }

        public override void OnTap() {
            var items = UI.Ins.GetItems();

            ILandable landable = Map as ILandable;
            if (landable == null) throw new Exception();

            if (Globals.Unlocked<KnowledgeOfPlanetLander>()) {
                items.Add(UIItem.CreateButton("开启飞船", () => {
                    LeavePlanet();
                }));
                items.Add(UIItem.CreateButton("暂不开启", () => {
                    UI.Ins.Active = false;
                }));
            } else {
                items.Add(UIItem.CreateMultilineText($"{Localization.Ins.Get<PlanetLander>()}已经坏了, 需要研究{Localization.Ins.Get<KnowledgeOfPlanetLander>()}"));
            }

            items.Add(UIItem.CreateSeparator());
            items.Add(UIItem.CreateButton("进行<color=#ff6666ff>时间回溯</color>", ResetPlanetPage));

            items.Add(UIItem.CreateSeparator());

            items.Add(UIItem.CreateText("飞船仪表盘还有读数："));

            IWeatherDefinition weather = Map as IWeatherDefinition;

            int hour = (int)(((weather.ProgressOfDay + 0.25f) % 1) * 24) + 1;
            string monthDescription = GeographyUtility.MonthTimeDescription(weather.MonthInYear + 1);
            string dateDescription = GeographyUtility.DateDescription(weather.DayInMonth + 1);
            string hourDescription = GeographyUtility.HourDescription(hour);
            items.Add(UIItem.CreateDynamicText(() => $"{weather.YearCount + 1}年 {monthDescription} {dateDescription} {hourDescription}"));

            items.Add(UIItem.CreateText($"星球污染程度 {Map.Refs.GetOrCreate<Polution>().Value}"));



            items.Add(UIItem.CreateButton("星球参数", PlanetInfoPage));

            items.Add(UIItem.CreateSeparator());

            foreach (var revenue in RevenuesOfReset) {
                Type type = revenue.Item1;
                IValue value = Globals.Ins.Values.GetOrCreate(type);
                if (value.Max > 0) {
                    items.Add(UIItem.CreateValueProgress(type, value));
                }
            }

            items.Add(UIItem.CreateTileImage(typeof(Television)));
            items.Add(UIItem.CreateButton("查看飞船上的旧世界电视广告", () => {
                Television.Ad = AdSelection.Television;
                PlanetLander.TryPlayAd();
            }));

            UI.Ins.ShowItems(Localization.Ins.Get<PlanetLander>(), items);
        }

        public static bool CanPlayAd() {
            return Globals.Ins.Values.Get<AdCoolDown>().Val > 0;
        }
        public static void TryPlayAd() {
            //if (CanPlayAd()) {
            //    PangleAd.Instance.PlayerAdvertisement();
            //}
        }

        private void PlanetInfoPage() {
            var items = UI.Ins.GetItems();

            items.Add(UIItem.CreateReturnButton(OnTap));

            IWeatherDefinition weather = Map as IWeatherDefinition;

            items.Add(UIItem.CreateDynamicText(() => $"风场强度 {Mathf.Round(weather.WindStrength * 100)}"));
            items.Add(UIItem.CreateDynamicText(() => $"相对湿度 {(int)(weather.Humidity * 100)}"));
            items.Add(UIItem.CreateDynamicText(() => $"迷雾浓度 {(int)(weather.FogDensity * 100)}"));
            items.Add(UIItem.CreateDynamicText(() => $"雨雪密度 {(int)(Mathf.Max(weather.RainDensity, weather.SnowDensity) * 100)}"));

            items.Add(UIItem.CreateSeparator());

            int hour = (int)(((weather.ProgressOfDay + 0.25f) % 1) * 24) + 1;
            string dayTimeDescription = GeographyUtility.DayTimeDescription(weather.ProgressOfDay);
            items.Add(UIItem.CreateDynamicText(() => $"{weather.YearCount + 1}年 {weather.MonthInYear + 1}月 {weather.DayInMonth + 1}日 {dayTimeDescription} {hour} 点"));

            items.Add(UIItem.CreateText($"昼夜周期 {weather.SecondsForADay}秒"));
            items.Add(UIItem.CreateText($"四季周期 {weather.DaysForAYear}天"));
            items.Add(UIItem.CreateText($"月相周期 {weather.DaysForAMonth}天"));
            items.Add(UIItem.CreateText($"四季月相 {MapOfPlanet.MonthForAYear}月"));


            UI.Ins.ShowItems("星球参数", items);
        }

        private static List<(Type, Func<PlanetLander, long>)> RevenuesOfReset = new List<(Type, Func<PlanetLander, long>)> {
            (typeof(ResetPointPop), (PlanetLander pl) => pl.Map.Refs.GetOrCreate<Worker>().Value),
            (typeof(ResetPointPop100), (PlanetLander pl) => pl.Map.Refs.GetOrCreate<Worker>().Value/100),
            (typeof(ResetPointTool), (PlanetLander pl) => pl.Map.Refs.GetOrCreate<ToolPrimitive>().Value),
            (typeof(ResetPointMachine), (PlanetLander pl) => pl.Map.Refs.GetOrCreate<MachinePrimitive>().Value),
            (typeof(ResetPointLightMaterial), (PlanetLander pl) => pl.Map.Refs.GetOrCreate<LightMaterial>().Value),
            (typeof(ResetPointCircuit), (PlanetLander pl) => pl.Map.Refs.GetOrCreate<CircuitBoardAdvanced>().Value),
            (typeof(ViewAdPoint), (PlanetLander pl) => 0),
            (typeof(TryViewAdPoint), (PlanetLander pl) => 0),
        };

        private void ResetPlanetPage() {
            var items = UI.Ins.GetItems();

            items.Add(UIItem.CreateReturnButton(PlanetInfoPage));
            items.Add(UIItem.CreateMultilineText($"时间回溯介绍：(还没写)"));

            items.Add(UIItem.CreateSeparator());

            IValue resetCooldown = Globals.Ins.Values.GetOrCreate<ResetCooldown>();
            if (!resetCooldown.Maxed) {
                items.Add(UIItem.CreateValueProgress<ResetCooldown>(Globals.Ins.Values));
                items.Add(UIItem.CreateSeparator());
            }

            IMapDefinition mapDefinition = Map as IMapDefinition;
            if (mapDefinition == null) throw new Exception();

            bool canDelete = mapDefinition.CanReset;
            items.Add(UIItem.CreateStaticButton("按下飞船上的<color=#ff6666ff>星球时间回溯</color>按钮,  (注意, 星球将回到最初状态, 清空所有修改)", () => {

                if (resetCooldown.Max == 0) { resetCooldown.Max = 60; resetCooldown.Del = Value.Second; resetCooldown.Inc = 1; }
                resetCooldown.Val = 0;

                mapDefinition.Reset();

                foreach (var revenue in RevenuesOfReset) {
                    Type type = revenue.Item1;
                    long quantity = revenue.Item2(this);

                    if (quantity > 0) {
                        IValue value = Globals.Ins.Values.GetOrCreate(type);
                        value.Max += quantity;
                        value.Val += quantity;
                    }
                }

            }, canDelete && resetCooldown.Maxed));
            if (!canDelete) {
                items.Add(UIItem.CreateText($"无法重置星球时间, 必须先关闭所有运行中的{Localization.Ins.Get<SpaceElevator>()}"));
            }
            items.Add(UIItem.CreateSeparator());



            long popCount = Map.Refs.GetOrCreate<Worker>().Value;
            int added = 0;
            foreach (var revenue in RevenuesOfReset) {
                Type type = revenue.Item1;
                long quantity = revenue.Item2(this);

                if (quantity > 0) {
                    IValue value = Globals.Ins.Values.GetOrCreate(type);
                    items.Add(UIItem.CreateText(Localization.Ins.Val(type, quantity)));
                    added++;
                }
            }
            if (added > 0) {
                items.Add(UIItem.CreateMultilineText($"本次回溯可获得上述天赋："));
            }




            UI.Ins.ShowItems("星球时间重置", items);
        }
    }
}

