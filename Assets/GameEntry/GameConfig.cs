

using System.Collections.Generic;
using UnityEngine;

namespace Weathering
{
	/// <summary>
	/// 发布时和测试时, 需要改哪几个地方? 
	/// GlobalGameEvents的设置
	/// </summary>
	public static class GameConfig
	{
		public static bool CheatMode = false;
		public static long DefaultInventoryOfResourceQuantityCapacity { get; } = 1000000000000000;
		public static int DefaultInventoryOfResourceTypeCapacity { get; } = 50;
		public static long DefaultInventoryOfSupplyQuantityCapacity { get; } = 10000000000;
		public static int DefaultInventoryOfSupplyTypeCapacity { get; } = 30;

		public const string InitialMapKey = "Weathering.MapOfPlanet#=1,4=14,93=24,31";

		public const int VersionCode = 20210501;

		private static void TryCreateValue<T>(int chance = 1) {
			// 全局理智
			if (_globals.Values.Has<T>()) {
				return;
            }

			IValue v = _globals.Values.Create<T>();
			v.Max = chance;
			v.Val = chance;
			v.Inc = 0;
			v.Del = Value.Second;
		}
		private static IGlobals _globals;

		public static void OnConstruct(IGlobals globals) {

			// 广告冷却
			EnsureADUpdate(globals);

			// 全局理智
			IValue sanity = globals.Values.Create<Sanity>();
			sanity.Max = 100;
			sanity.Val = sanity.Max / 10;
			sanity.Inc = 1;
			sanity.Del = 10 * Value.Second;


			// 饱腹度
			IValue satiety = globals.Values.Create<Satiety>();
			satiety.Max = 100;
			satiety.Inc = 1;
			satiety.Val = 0;
			satiety.Del = Value.Second;

			// 行动冷却
			IValue cooldown = globals.Values.Create<CoolDown>();
			cooldown.Inc = 1;
			cooldown.Max = 1;
			cooldown.Del = Value.Second;

			IInventory inventory = globals.Inventory;
			inventory.QuantityCapacity = DefaultInventoryOfResourceQuantityCapacity;
			inventory.TypeCapacity = 50;

			// inventory.Add<TutorialMapTheBook>(1);
			inventory.Add<TutorialMapTheDiary>(1);
			inventory.Add<TutorialMapTheCurse>(1);


			Globals.Ins.Values.GetOrCreate<QuestResource>().Del = Value.Second;

			Globals.Unlock<TotemOfNature>();

			Globals.Ins.Values.GetOrCreate<KnowledgeOfNature>().Max = KnowledgeOfNature.Max;
			Globals.Ins.Values.GetOrCreate<KnowledgeOfAncestors>().Max = KnowledgeOfAncestors.Max;

			if (!CheatMode) {
				SpecialPages.OpenStartingPage();
			}

		}

		private static void TryCreateAdCooldown() {

			// 广告冷却
			if (_globals.Values.Has<AdCoolDown>()) {
				return;
            }

			IValue adColdDown = _globals.Values.GetOrCreate<AdCoolDown>();
			adColdDown.Max = 3;
			adColdDown.Val = 1;
			adColdDown.Inc = 1;
			adColdDown.Del = 180 * Value.Second;
		}

		public static void OnGameConstruct() {

		}

		public static void OnGameEnable(IGlobals globals) {
			EnsureADUpdate(globals);
		}

		private static void EnsureADUpdate(IGlobals globals) {
			_globals = globals;
			TryCreateAdCooldown();
			TryCreateValue<AdChance_Berry1000>();
			TryCreateValue<AdChance_Grain1000>();
			TryCreateValue<AdChance_WoodPlank1000>();
			TryCreateValue<AdChance_BuildingPrefabrication1000>();
		}

		public static void OnSave() {

		}

	}


	[Depend(typeof(Book))]
	public class AdSuccessReward { }
	[Depend(typeof(Book))]
	public class AdSkipReward { }

	[Depend(typeof(Book))]
	public class AdTelevision { }


	[Depend(typeof(Book))]
	public class TutorialMapTheBook { }

	[Depend(typeof(Book))]
	public class TutorialMapTheDiary { }

	[Depend(typeof(Book))]
	public class TutorialMapTheCurse { }
}

