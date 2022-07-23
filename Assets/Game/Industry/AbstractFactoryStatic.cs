﻿

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Weathering
{
    public class FactoryIn_0 { }
    public class FactoryIn_1 { }
    public class FactoryIn_2 { }
    public class FactoryIn_3 { }

    public class FactoryOut0 { }
    public class FactoryOut1 { }
    public class FactoryOut2 { }
    public class FactoryOut3 { }

    public class FactoryRunning { }

    [Concept]
    public class FactoryInputOnInventory { }
    [Concept]
    public class FactoryInputOnRoad { }
    [Concept]
    public class FactoryOutputOnInventory { }
    [Concept]
    public class FactoryOutputOnRoad { }


    /// <summary>
    /// 目前主要建筑类型：AbstractFactoryStatic, AbstractRoad, TransportStation, TransportStationDest, WareHouse
    /// AbstractFactoryStatic特征：输入指定(或子类), 各种输出指定(改不了)
    /// </summary>
    public abstract class AbstractFactoryStatic : StandardTile, ILinkProvider, ILinkConsumer, IRunnable, ILinkEvent, IStepOn, ILinkEventManual
    {

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

        public override string SpriteKey { 
            get {
                string result = UIItem.TryGetIconName(GetType());
                if (result == null) result = typeof(Pasture).Name;
                return result;
            } 
        }
        public override string SpriteKeyHighLight { get => Running ? GlobalLight.Decorated(SpriteKey) : null; }


        private IRef in_0Ref; // 输入
        private IRef in_1Ref; // 输入
        private IRef in_2Ref; // 输入
        private IRef in_3Ref; // 输入

        private IRef out0Ref; // 输出
        private IRef out1Ref; // 输出
        private IRef out2Ref; // 输出
        private IRef out3Ref; // 输出


        protected virtual (Type, long) In_0_Inventory { get; } = (null, 0);
        protected virtual (Type, long) In_1_Inventory { get; } = (null, 0);

        private bool HasIn_0_Inventory => In_0_Inventory.Item1 != null;
        private bool HasIn_1_Inventory => In_1_Inventory.Item1 != null;

        protected virtual (Type, long) Out0_Inventory { get; } = (null, 0);
        protected virtual (Type, long) Out1_Inventory { get; } = (null, 0);

        private bool HasOut0_Inventory => Out0_Inventory.Item1 != null;
        private bool HasOut1_Inventory => Out1_Inventory.Item1 != null;


        protected virtual (Type, long) In_0 { get; } = (null, 0);
        protected virtual (Type, long) In_1 { get; } = (null, 0);
        protected virtual (Type, long) In_2 { get; } = (null, 0);
        protected virtual (Type, long) In_3 { get; } = (null, 0);
        private bool HasIn_0 => In_0.Item1 != null;
        private bool HasIn_1 => In_1.Item1 != null;
        private bool HasIn_2 => In_2.Item1 != null;
        private bool HasIn_3 => In_3.Item1 != null;

        protected virtual (Type, long) Out0 { get; } = (null, 0);
        protected virtual (Type, long) Out1 { get; } = (null, 0);
        protected virtual (Type, long) Out2 { get; } = (null, 0);
        protected virtual (Type, long) Out3 { get; } = (null, 0);
        private bool HasOut0 => Out0.Item1 != null;
        private bool HasOut1 => Out1.Item1 != null;
        private bool HasOut2 => Out2.Item1 != null;
        private bool HasOut3 => Out3.Item1 != null;

        public bool Running { get => running.Value == 1; set => running.Value = value ? 1 : 0; }

        private IRef running;


        public void Consume(List<IRef> refs) {
            if (HasIn_0) {
                refs.Add(in_0Ref);
            }
            if (HasIn_1) {
                refs.Add(in_1Ref);
            }
            if (HasIn_2) {
                refs.Add(in_2Ref);
            }
            if (HasIn_3) {
                refs.Add(in_3Ref);
            }
        }
        public void Provide(List<IRef> refs) {
            if (HasOut0) {
                refs.Add(out0Ref);
            }
            if (HasOut1) {
                refs.Add(out1Ref);
            }
            if (HasOut2) {
                refs.Add(out2Ref);
            }
            if (HasOut3) {
                refs.Add(out3Ref);
            }
        }

        public void OnLink(Type direction, long quantity) {
            OnOutRefChanged();
        }


        protected virtual bool CanStoreSomething => false;
        protected virtual bool CanStoreOut0 => false;

        private IValue out0Value;

        private void OnOutRefChanged() {
            if (CanStoreSomething) {
                if (CanStoreOut0) {
                    OnOutRef0Changed();
                }
            }
        }
        private void OnOutRef0Changed() {
            out0Value.Inc = out0Ref.Value;
        }

        public override void OnConstruct(ITile tile) {
            base.OnConstruct(tile);

            if (Refs == null) {
                Refs = Weathering.Refs.GetOne();
            }

            if (HasIn_0) {
                in_0Ref = Refs.Create<FactoryIn_0>(); // In_0 记录第一种输入
                in_0Ref.Type = In_0.Item1; // In_0.Item1 是输入的类型
                in_0Ref.Value = 0;
                in_0Ref.BaseValue = In_0.Item2; // In_0.Item2 是输入的数量
            }
            if (HasIn_1) {
                in_1Ref = Refs.Create<FactoryIn_1>();
                in_1Ref.Type = In_1.Item1;
                in_1Ref.BaseValue = In_1.Item2;
            }
            if (HasIn_2) {
                in_2Ref = Refs.Create<FactoryIn_2>();
                in_2Ref.Type = In_2.Item1;
                in_2Ref.BaseValue = In_2.Item2;
            }
            if (HasIn_3) {
                in_3Ref = Refs.Create<FactoryIn_3>();
                in_3Ref.Type = In_3.Item1;
                in_3Ref.BaseValue = In_3.Item2;
            }


            if (HasOut0) {
                out0Ref = Refs.Create<FactoryOut0>(); // Out0 记录第一种输出
                out0Ref.Type = Out0.Item1; // Out0 记录第一种输出的类型
                out0Ref.BaseValue = Out0.Item2; // Out0 记录第一种输出的数量
            }
            if (HasOut1) {
                out1Ref = Refs.Create<FactoryOut1>(); // Out0 记录第一种输出
                out1Ref.Type = Out1.Item1; // Out0 记录第一种输出的类型
                out1Ref.BaseValue = Out1.Item2; // Out0 记录第一种输出的数量
            }
            if (HasOut2) {
                out2Ref = Refs.Create<FactoryOut2>(); // Out0 记录第一种输出
                out2Ref.Type = Out2.Item1; // Out0 记录第一种输出的类型
                out2Ref.BaseValue = Out2.Item2; // Out0 记录第一种输出的数量
            }
            if (HasOut3) {
                out3Ref = Refs.Create<FactoryOut3>(); // Out0 记录第一种输出
                out3Ref.Type = Out3.Item1; // Out0 记录第一种输出的类型
                out3Ref.BaseValue = Out3.Item2; // Out0 记录第一种输出的数量
            }

            if (CanStoreSomething) {
                Values = Weathering.Values.GetOne();
                if (CanStoreOut0) {
                    if (out0Ref == null) throw new Exception();
                    out0Value = Values.Create<FactoryOut0>();
                    out0Value.Del = Value.Second;
                    out0Value.Max = 100;
                }
            }

            running = Refs.Create<FactoryRunning>();

            if (CanRun()) Run(); // 自动运行。不可能?, OnLink里判断吧
        }

        private long QuantityCapacityRequired = 0;
        private long TypeCapacityRequired = 0;

        public override void OnEnable() {
            base.OnEnable();

            if (CanStoreSomething) {
                if (CanStoreOut0) {
                    out0Value = Values.Get<FactoryOut0>();
                }
            }

            if (HasIn_0) {
                in_0Ref = Refs.Get<FactoryIn_0>();
            }
            if (HasIn_1) {
                in_1Ref = Refs.Get<FactoryIn_1>();
            }
            if (HasIn_2) {
                in_2Ref = Refs.Get<FactoryIn_2>();
            }
            if (HasIn_3) {
                in_3Ref = Refs.Get<FactoryIn_3>();
            }
            if (HasOut0) {
                out0Ref = Refs.Get<FactoryOut0>();
            }
            if (HasOut1) {
                out1Ref = Refs.Get<FactoryOut1>();
            }
            if (HasOut2) {
                out2Ref = Refs.Get<FactoryOut2>();
            }
            if (HasOut3) {
                out3Ref = Refs.Get<FactoryOut3>();
            }


            if (HasIn_0_Inventory) { TypeCapacityRequired++; QuantityCapacityRequired += In_0_Inventory.Item2; }
            if (HasIn_1_Inventory) { TypeCapacityRequired++; QuantityCapacityRequired += In_1_Inventory.Item2; }
            if (HasOut0_Inventory) { TypeCapacityRequired++; QuantityCapacityRequired += Out0_Inventory.Item2; }
            if (HasOut1_Inventory) { TypeCapacityRequired++; QuantityCapacityRequired += Out1_Inventory.Item2; }

            running = Refs.Get<FactoryRunning>();
        }


        public bool CanRun() {
            if (Running) return false;

            // 如果有工人和所有原材料, 那么制造输出。
            if (HasIn_0 && in_0Ref.Value != In_0.Item2) return false; // 输入不足, 不能运转
            if (HasIn_1 && in_1Ref.Value != In_1.Item2) return false; // 输入不足, 不能运转
            if (HasIn_2 && in_2Ref.Value != In_2.Item2) return false; // 输入不足, 不能运转
            if (HasIn_3 && in_3Ref.Value != In_3.Item2) return false; // 输入不足, 不能运转

            if (Map.InventoryOfSupply.TypeCapacity - Map.InventoryOfSupply.TypeCount <= TypeCapacityRequired
                || Map.InventoryOfSupply.QuantityCapacity - Map.InventoryOfSupply.Quantity <= QuantityCapacityRequired) {
                UIPreset.InventoryFull(null, Map.InventoryOfSupply);
                return false;
            }

            if (HasIn_0_Inventory && !Map.InventoryOfSupply.CanRemove(In_0_Inventory)) return false; // 背包物品不足, 不能运转
            if (HasIn_1_Inventory && !Map.InventoryOfSupply.CanRemove(In_1_Inventory)) return false; // 背包物品不足, 不能运转
            return true;
        }
        public void Run() {
            if (Running) throw new Exception();
            if (!CanRun()) throw new Exception(); // defensive
            Running = true;  // 派遣工人之后

            LinkUtility.NeedUpdateNeighbors(this);

            if (HasIn_0) {
                in_0Ref.Value = 0; // 消耗输入
                in_0Ref.BaseValue = 0; // 不再需求输入
            }
            if (HasIn_1) {
                in_1Ref.Value = 0; // 消耗输入
                in_1Ref.BaseValue = 0; // 不再需求输入
            }
            if (HasIn_2) {
                in_2Ref.Value = 0; // 消耗输入
                in_2Ref.BaseValue = 0; // 不再需求输入
            }
            if (HasIn_3) {
                in_3Ref.Value = 0; // 消耗输入
                in_3Ref.BaseValue = 0; // 不再需求输入
            }

            if (HasOut0) {
                // out0Ref.Type = Out0.Item1;
                out0Ref.Value = Out0.Item2; // 生产输出
                out0Ref.BaseValue = Out0.Item2;
                Map.Refs.GetOrCreate(Out0.Item1).Value += Out0.Item2; // 记录产量
            }
            if (HasOut1) {
                // out1Ref.Type = Out1.Item1;
                out1Ref.Value = Out1.Item2; // 生产输出
                out1Ref.BaseValue = Out1.Item2;
                Map.Refs.GetOrCreate(Out1.Item1).Value += Out1.Item2; // 记录产量
            }
            if (HasOut2) {
                // out2Ref.Type = Out2.Item1;
                out2Ref.Value = Out2.Item2; // 生产输出
                out2Ref.BaseValue = Out2.Item2;
                Map.Refs.GetOrCreate(Out2.Item1).Value += Out2.Item2; // 记录产量
            }
            if (HasOut3) {
                // out3Ref.Type = Out3.Item1;
                out3Ref.Value = Out3.Item2; // 生产输出
                out3Ref.BaseValue = Out3.Item2;
                Map.Refs.GetOrCreate(Out3.Item1).Value += Out3.Item2; // 记录产量
            }


            if (HasIn_0_Inventory) Map.InventoryOfSupply.Remove(In_0_Inventory);
            if (HasIn_1_Inventory) Map.InventoryOfSupply.Remove(In_1_Inventory);

            if (HasOut0_Inventory) {
                Map.InventoryOfSupply.Add(Out0_Inventory);
                Map.Refs.GetOrCreate(Out0_Inventory.Item1).Value += Out0_Inventory.Item2; // 记录产量
            }
            if (HasOut1_Inventory) {
                Map.InventoryOfSupply.Add(Out1_Inventory);
                Map.Refs.GetOrCreate(Out1_Inventory.Item1).Value += Out1_Inventory.Item2; // 记录产量
            }

            OnOutRefChanged();
        }

        public bool CanStop() {
            if (!Running) return false;

            if (HasOut0 && out0Ref.Value != Out0.Item2) return false; // 产品使用中
            if (HasOut1 && out1Ref.Value != Out1.Item2) return false; // 产品使用中
            if (HasOut2 && out2Ref.Value != Out2.Item2) return false; // 产品使用中
            if (HasOut3 && out3Ref.Value != Out3.Item2) return false; // 产品使用中

            // 有bug !!! 如果每一项都可以加入背包, 但加起来不能加入背包呢
            if (Map.InventoryOfSupply.TypeCapacity - Map.InventoryOfSupply.TypeCount <= TypeCapacityRequired
                || Map.InventoryOfSupply.QuantityCapacity - Map.InventoryOfSupply.Quantity <= QuantityCapacityRequired) {
                UIPreset.InventoryFull(null, Map.InventoryOfSupply);
                return false;
            }
            if (HasOut0_Inventory && !Map.InventoryOfSupply.CanRemove(Out0_Inventory)) return false; // 背包物品不足, 不能回收
            if (HasOut1_Inventory && !Map.InventoryOfSupply.CanRemove(Out1_Inventory)) return false; // 背包物品不足, 不能回收

            return true;
        }

        public void Stop() {
            if (!Running) throw new Exception();
            if (!CanStop()) throw new Exception(); // defensive

            Running = false; // 收回工人之前

            LinkUtility.NeedUpdateNeighbors(this);

            // 收回工人
            if (HasIn_0) {
                in_0Ref.BaseValue = In_0.Item2;
                in_0Ref.Value = In_0.Item2;
            }
            if (HasIn_1) {
                in_1Ref.BaseValue = In_1.Item2;
                in_1Ref.Value = In_1.Item2;
            }
            if (HasIn_2) {
                in_2Ref.BaseValue = In_2.Item2;
                in_2Ref.Value = In_2.Item2;
            }
            if (HasIn_3) {
                in_3Ref.BaseValue = In_3.Item2;
                in_3Ref.Value = In_3.Item2;
            }

            if (HasOut0) {
                // out0Ref.Type = null;
                out0Ref.BaseValue = 0;
                out0Ref.Value = 0;
                Map.Refs.GetOrCreate(Out0.Item1).Value -= Out0.Item2; // 记录产量
            }
            if (HasOut1) {
                // out1Ref.Type = null;
                out1Ref.BaseValue = 0;
                out1Ref.Value = 0;
                Map.Refs.GetOrCreate(Out1.Item1).Value -= Out1.Item2; // 记录产量
            }
            if (HasOut2) {
                // out2Ref.Type = null;
                out2Ref.BaseValue = 0;
                out2Ref.Value = 0;
                Map.Refs.GetOrCreate(Out2.Item1).Value -= Out2.Item2; // 记录产量
            }
            if (HasOut3) {
                // out3Ref.Type = null;
                out3Ref.BaseValue = 0;
                out3Ref.Value = 0;
                Map.Refs.GetOrCreate(Out3.Item1).Value -= Out3.Item2; // 记录产量
            }

            if (HasOut0_Inventory) {
                Map.InventoryOfSupply.Remove(Out0_Inventory);
                Map.Refs.GetOrCreate(Out0_Inventory.Item1).Value -= Out0_Inventory.Item2; // 记录产量
            }
            if (HasOut1_Inventory) {
                Map.InventoryOfSupply.Remove(Out1_Inventory);
                Map.Refs.GetOrCreate(Out1_Inventory.Item1).Value -= Out1_Inventory.Item2; // 记录产量
            }

            if (HasIn_0_Inventory) Map.InventoryOfSupply.Add(In_0_Inventory); // 背包空间不足
            if (HasIn_1_Inventory) Map.InventoryOfSupply.Add(In_1_Inventory);

            TryCollectAnything();
            OnOutRefChanged();
        }

        protected virtual void AddBuildingDescriptionPage(List<IUIItem> items) {

        }


        private static AudioClip clip;
        public void OnStepOn() {
            bool collected = TryCollectAnything();
            if (collected) {
                if (clip == null) {
                    clip = Sound.Ins.Get("mixkit-hard-pop-click-2364");
                }
                Sound.Ins.Play(clip);
            }
        }

        private bool TryCollectAnything() {
            bool result = false;
            if (CanStoreSomething) {
                result |= TryCollectOut0();
            }
            return result;
        }

        private bool TryCollectOut0() {
            Type type = out0Ref.Type;
            long quantity = Math.Min(Map.Inventory.CanAdd(type), out0Value.Val);
            out0Value.Val -= quantity;
            Map.Inventory.Add(type, quantity);
            if (quantity > 0) {
                GameMenu.Ins.PushNotification($"从{Localization.Ins.Get(GetType())}获得{Localization.Ins.Val(type, quantity)}");
            }

            return quantity > 0;
        }


        public override void OnTap() {
            var items = new List<IUIItem>() { };

            items.Add(UIItem.CreateTileImage(SpriteKey));

            if (CanStoreSomething) {
                if (CanStoreOut0) {
                    if (out0Value.Inc > 0) {
                        items.Add(UIItem.CreateValueProgress(out0Ref.Type, out0Value));
                    }
                    if (out0Value.Inc > 0 || out0Value.Val > 0) {
                        items.Add(UIItem.CreateButton($"收集 {Localization.Ins.ValUnit(out0Ref.Type)}", () => { TryCollectOut0(); OnTap(); }));
                    }
                }
                items.Add(UIItem.CreateSeparator());
            }

            AddBuildingDescriptionPage(items);
            items.Add(UIItem.CreateButton("建筑功能", BuildingRecipePage));
            items.Add(UIItem.CreateButton("建筑费用", () => ConstructionCostUtility.ShowBuildingCostPage(OnTap, Map, GetType())));
            items.Add(UIItem.CreateButton("建筑控制", BuildingControlPage));
            items.Add(UIItem.CreateButton("综合产量统计", BuildingProductionStatisticsPage));
            items.Add(UIItem.CreateButton("复制建筑", () => { 
                UIItem.ShortcutType = GetType(); UI.Ins.Active = false; GameMenu.Ins.PushNotification($"复制建筑{Localization.Ins.Get(UIItem.ShortcutType)}"); 
            }));

            //items.Add(UIItem.CreateStaticButton($"开始运转", () => { Run(); OnTap(); }, CanRun()));
            //items.Add(UIItem.CreateStaticButton($"停止运转", () => { Stop(); OnTap(); }, CanStop()));

            //items.Add(UIItem.CreateSeparator());
            //LinkUtility.AddButtons(items, this);

            items.Add(UIItem.CreateStaticDestructButton(this));

            items.Add(UIItem.CreateSeparator());

            UI.Ins.ShowItems(Localization.Ins.Get(GetType()), items);
        }

        public override bool CanDestruct() => Running == false && !LinkUtility.HasAnyLink(this);


        public void OnLinkManually() {
            BuildingControlPage();
        }
        private void BuildingControlPage() {
            var items = UI.Ins.GetItems();

            items.Add(UIItem.CreateReturnButton(OnTap));

            items.Add(UIItem.CreateStaticButton($"开始运转", () => { Run(); BuildingControlPage(); }, CanRun()));
            items.Add(UIItem.CreateStaticButton($"停止运转", () => { Stop(); BuildingControlPage(); }, CanStop()));

            items.Add(UIItem.CreateSeparator());
            LinkUtility.AddButtons(items, this);

            UI.Ins.ShowItems($"{Localization.Ins.Get(GetType())}建筑详情", items);
        }

        private void BuildingRecipePage() {
            var items = UI.Ins.GetItems();

            items.Add(UIItem.CreateReturnButton(OnTap));

            if (HasIn_0_Inventory) AddDescriptionItem(items, In_0_Inventory, "自动输入", true);
            if (HasIn_1_Inventory) AddDescriptionItem(items, In_1_Inventory, "自动输入", true);

            if (HasOut0_Inventory) AddDescriptionItem(items, Out0_Inventory, "自动输出", true);
            if (HasOut1_Inventory) AddDescriptionItem(items, Out1_Inventory, "自动输出", true);

            if (HasIn_0) AddDescriptionItem(items, In_0, "物流输入");
            if (HasIn_1) AddDescriptionItem(items, In_1, "物流输入");
            if (HasIn_2) AddDescriptionItem(items, In_2, "物流输入");
            if (HasIn_3) AddDescriptionItem(items, In_3, "物流输入");

            if (HasOut0) AddDescriptionItem(items, Out0, "物流输出");
            if (HasOut1) AddDescriptionItem(items, Out1, "物流输出");
            if (HasOut2) AddDescriptionItem(items, Out2, "物流输出");
            if (HasOut3) AddDescriptionItem(items, Out3, "物流输出");

            UI.Ins.ShowItems($"{Localization.Ins.Get(GetType())}建筑控制", items);
        }
        private void BuildingProductionStatisticsPage() {
            var items = UI.Ins.GetItems();

            items.Add(UIItem.CreateReturnButton(OnTap));

            Type res;
            if (HasOut0_Inventory) {
                res = Out0_Inventory.Item1;
                items.Add(UIItem.CreateButton($"自动输出产量 {Localization.Ins.Val(res, Map.Refs.GetOrCreate(Out0_Inventory.Item1).Value)}", () => UIPreset.OnTapItem(BuildingProductionStatisticsPage, res)));
            }
            if (HasOut1_Inventory) {
                res = Out0_Inventory.Item1;
                items.Add(UIItem.CreateButton($"自动输出产量 {Localization.Ins.Val(res, Map.Refs.GetOrCreate(Out1_Inventory.Item1).Value)}", () => UIPreset.OnTapItem(BuildingProductionStatisticsPage, res)));
            }

            if (HasOut0) {
                res = Out0.Item1;
                items.Add(UIItem.CreateButton($"物流输出产量 {Localization.Ins.Val(res, Map.Refs.GetOrCreate(Out0.Item1).Value)}", () => UIPreset.OnTapItem(BuildingProductionStatisticsPage, res)));
            }
            if (HasOut1) {
                res = Out1.Item1;
                items.Add(UIItem.CreateButton($"物流输出产量 {Localization.Ins.Val(res, Map.Refs.GetOrCreate(Out1.Item1).Value)}", () => UIPreset.OnTapItem(BuildingProductionStatisticsPage, res)));
            }
            if (HasOut2) {
                res = Out2.Item1;
                items.Add(UIItem.CreateButton($"物流输出产量 {Localization.Ins.Val(res, Map.Refs.GetOrCreate(Out2.Item1).Value)}", () => UIPreset.OnTapItem(BuildingProductionStatisticsPage, res)));
            }
            if (HasOut3) {
                res = Out3.Item1;
                items.Add(UIItem.CreateButton($"物流输出产量 {Localization.Ins.Val(res, Map.Refs.GetOrCreate(Out3.Item1).Value)}", () => UIPreset.OnTapItem(BuildingProductionStatisticsPage, res)));
            }

            UI.Ins.ShowItems($"{Localization.Ins.Get(GetType())}产量", items);
        }

        private void AddDescriptionItem(List<IUIItem> items, (Type, long) pair, string text, bool dontCreateImage = false) {
            Type res = pair.Item1;
            items.Add(UIItem.CreateButton($"{text} {Localization.Ins.Val(res, pair.Item2)}", () => UIPreset.OnTapItem(BuildingRecipePage, res)));
            if (!dontCreateImage) items.Add(UIItem.CreateTileImage(res));
        }

    }
}
