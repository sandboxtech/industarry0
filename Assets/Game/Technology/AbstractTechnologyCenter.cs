

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Weathering
{

    [Depend]
    public class Technology { }


    public abstract class AbstractTechnologyCenter : StandardTile, ILinkEvent, ILinkConsumer
    {

        protected bool Running => techRef.Value == TechnologyPointIncRequired;
        public override string SpriteKey => GetType().Name;
        public override string SpriteKeyHighLight { get => Running ? $"{SpriteKey}_Working" : null; }


        public override string SpriteLeft => GetSprite(Vector2Int.left, typeof(ILeft));
        public override string SpriteRight => GetSprite(Vector2Int.right, typeof(IRight));
        public override string SpriteUp => GetSprite(Vector2Int.up, typeof(IUp));
        public override string SpriteDown => GetSprite(Vector2Int.down, typeof(IDown));
        private string GetSprite(Vector2Int dir, Type direction) {
            IRefs refs = Map.Get(Pos - dir).Refs;
            if (refs == null) return null;
            if (refs.TryGet(direction, out IRef result)) return result.Value < 0 ? result.Type.Name : null;
            return null;
        }


        private IValue techValue;
        private IRef techRef;
        public void Consume(List<IRef> refs) {
            refs.Add(techRef);
        }

        public override void OnConstruct(ITile oldTile) {
            techValue = Globals.Ins.Values.GetOrCreate(TechnologyPointType);

            Refs = Weathering.Refs.GetOne();

            techRef = Refs.Create(TechnologyPointType);
            techRef.BaseValue = TechnologyPointIncRequired;
        }

        public override void OnDestruct(ITile newTile) {

        }

        private static AudioClip soundEffectOnUnlockTech;
        public override void OnEnable() {
            base.OnEnable();

            // for some player
            //Globals.Ins.Values.GetOrCreate<BookWithNote>();
            //Globals.Ins.Values.GetOrCreate<BookWithNoteAdvanced>();
            //Globals.Ins.Values.GetOrCreate<FloppyDisk>();
            //Refs.GetOrCreate<BookWithNote>();
            //Refs.GetOrCreate<BookWithNoteAdvanced>();
            //Refs.GetOrCreate<FloppyDisk>();

            techValue = Globals.Ins.Values.GetOrCreate(TechnologyPointType); // Get

            techRef = Refs.GetOrCreate(TechnologyPointType); // Get
            techRef.Type = TechnologyPointType;

            if (soundEffectOnUnlockTech == null) {
                soundEffectOnUnlockTech = Sound.Ins.Get("mixkit-magic-potion-music-and-fx-2831");
            }
        }

        private float slider = 0;
        private long sliderRounded = 0;
        protected virtual void DecorateItems(List<IUIItem> items, Action onTap) {
            Type OfferingType = TechnologyPointType;
            IValue techValue = Globals.Ins.Values.Get(TechnologyPointType);
            long quantity = Math.Min(techValue.Max - techValue.Val, Map.Inventory.CanRemove(OfferingType));

            string offeringName = Localization.Ins.ValUnit(OfferingType);

            if (quantity == 0) {
                //if (!techValue.Maxed) {
                //    items.Add(UIItem.CreateMultilineText($"{Localization.Ins.Get(GetType())}发出了一个声音：“给点{offeringName}吧”"));
                //}
            } else {
                items.Add(new UIItem {
                    Type = IUIItemType.Slider,
                    InitialSliderValue = 1,
                    DynamicSliderContent = (float x) => {
                        slider = x;
                        sliderRounded = (long)Math.Round(slider * quantity);
                        return $"选择贡献数量 {sliderRounded}";
                    }
                });
                items.Add(UIItem.CreateDynamicContentButton(() => quantity == 0 ? $"提交{offeringName}" :
                    $"提交{offeringName} {Localization.Ins.ValPlus(OfferingType, sliderRounded)}", () => {

                        Map.Inventory.Remove(OfferingType, sliderRounded);
                        Globals.Ins.Values.Get(TechnologyPointType).Val += sliderRounded;
                        onTap();

                    }, () => sliderRounded > 0));
            }
            items.Add(UIItem.CreateSeparator());
        }

        protected virtual void DecorateIfCompleted(List<IUIItem> items) { }


        public void OnLink(Type direction, long quantity) {
            long incRequired = TechnologyPointIncRequired;
            long maxRevenue = TechnologyPointMaxRevenue;

            if (techRef.Value == incRequired) {
                techValue.Inc += incRequired;
                if (maxRevenue > 0) {
                    techValue.Max += maxRevenue;
                }
            } 
            else if (techRef.Value == incRequired + quantity) {
                techValue.Inc -= incRequired;
                if (maxRevenue > 0) {
                    techValue.Max -= maxRevenue;
                }
            }
            else if (direction == null && Running) {
                techValue.Inc -= incRequired;
                if (maxRevenue > 0) {
                    techValue.Max -= maxRevenue;
                }
            }
            //else {
            //    techValue.Inc -= incRequired;
            //    if (maxRevenue > 0) {
            //        techValue.Max -= maxRevenue;
            //    }
            //}
        }


        private static List<IUIItem> itemsUnlockedBuffer = new List<IUIItem>();
        private List<IUIItem> items;
        public override void OnTap() {
            items = UI.Ins.GetItems();

            techValue.Del = Value.Second;

            DecorateItems(items, OnTap);

            Type techPointType = TechnologyPointType;
            items.Add(UIItem.CreateTileImage(techPointType, 2));

            string name = Localization.Ins.Get(GetType());
            string techPointName = Localization.Ins.ValUnit(techPointType);

            if (TechnologyPointIncRequired > 0) {
                string extraContent = TechnologyPointMaxRevenue > 0 ? $", 以提高上限 {TechnologyPointMaxRevenue}" : null;
                items.Add(UIItem.CreateMultilineText($"每个{name}需要{techPointName}输入 {TechnologyPointIncRequired} {extraContent}"));
            }

            items.Add(UIItem.CreateValueProgress(techPointType, techValue));
            if (techValue.Inc > 0) {
                items.Add(UIItem.CreateTimeProgress(techPointType, techValue));
            }

            items.Add(UIItem.CreateSeparator());

            if (itemsUnlockedBuffer.Count != 0) throw new Exception();
            int techShowed = 0;
            foreach (var item in TechList) {
                Type techType = item.Item1;
                long techPointCount = item.Item2;


                string techName = Localization.Ins.Get(techType);

                bool hasTech = Globals.Ins.Bool(techType);
                if (!hasTech) {
                    items.Add(UIItem.CreateDynamicIconButton(techPointCount == 0 ? $"研究 {techName}" :
                        $"研究 {techName} {(DontConsumeTechnologyPoint ? $"需要{Localization.Ins.Val(techPointType, techPointCount)}" : Localization.Ins.Val(techPointType, -techPointCount))}",
                            UIItem.TryGetIconName(techType), () => {

                                if (!DontConsumeTechnologyPoint && !GameConfig.CheatMode) {
                                    techValue.Val -= techPointCount;
                                }

                                // Globals.Ins.Bool(techType, true);
                                Globals.Unlock(techType);
                                Sound.Ins.Play(soundEffectOnUnlockTech);

                                // OnTap();
                                if (TechnologyResearched_Event.Event.TryGetValue(techType, out var action)) {
                                    var items_ = UI.Ins.GetItems();
                                    items_.Add(UIItem.CreateReturnButton(OnTap));
                                    action(items_);
                                    UI.Ins.ShowItems($"成功研究 {Localization.Ins.Get(techType)}", items_);
                                } else {
                                    OnTap();
                                }

                            }, () => Globals.Ins.Values.Get(techPointType).Val >= techPointCount || GameConfig.CheatMode // techType.Name // icon
                        ));
                    techShowed++;
                } else {
                    if (TechnologyResearched_Event.Event.TryGetValue(techType, out var action)) {
                        itemsUnlockedBuffer.Add(UIItem.CreateIconButton($"已研究 {techName}", UIItem.TryGetIconName(techType), () => {
                            var items_ = UI.Ins.GetItems();
                            items_.Add(UIItem.CreateReturnButton(OnTap));
                            action(items_);
                            UI.Ins.ShowItems($"成功研究 {Localization.Ins.Get(techType)}", items_);
                        }));
                    } else {
                        itemsUnlockedBuffer.Add(UIItem.CreateIconButton($"已研究 {techName}", UIItem.TryGetIconName(techType), () => {
                            var items_ = UI.Ins.GetItems();
                            items_.Add(UIItem.CreateReturnButton(OnTap));
                            items_.Add(UIItem.CreateMultilineText($"游戏开发者太懒了, 还没有写{techName}的介绍"));
                            UI.Ins.ShowItems($"成功研究 {Localization.Ins.Get(techType)}", items_);
                        }));
                    }
                }
                if (techShowed >= ShowedTechToBeResearched) {
                    break;
                }
            }

            if (techShowed == 0) {
                items.Add(UIItem.CreateText($"{Localization.Ins.Get(GetType())}的所有技术, 已经全部被成功研究! {Localization.Ins.Get(GetType())}都可以拆掉了"));
                DecorateIfCompleted(items);
            }

            items.Add(UIItem.CreateSeparator());

            foreach (var item in itemsUnlockedBuffer) {
                items.Add(item);
            }
            itemsUnlockedBuffer.Clear();

            items.Add(UIItem.CreateSeparator());
            items.Add(UIItem.CreateStaticDestructButton(this));
            items.Add(UIItem.CreateSeparator());

            UI.Ins.ShowItems(Localization.Ins.Get(GetType()), items);
            items = null;
        }

        public override bool CanDestruct() => techRef.Value == 0;



        protected virtual long TechnologyPointIncRequired { get => 1; }

        protected virtual long TechnologyPointMaxRevenue { get; } = 0;
        /// <summary>
        /// 同时展示的可研究科技数量
        /// </summary>
        private const int ShowedTechToBeResearched = 3;
        /// <summary>
        /// 科技所消耗资源
        /// </summary>
        protected abstract Type TechnologyPointType { get; }
        /// <summary>
        /// 消耗科技点
        /// </summary>
        protected virtual bool DontConsumeTechnologyPoint { get => false; }

        /// <summary>
        /// 可解锁的科技列表
        /// </summary>
        protected abstract List<ValueTuple<Type, long>> TechList { get; }


        public override void OnDestructWithMap() {
            base.OnDestructWithMap();

            techRef.Value = 0;
            OnLink(null, 0);
        }
    }
}

