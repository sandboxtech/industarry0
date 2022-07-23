
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Weathering
{

    [Concept]
    public class GameMenuExitGame { }
    [Concept]
    public class GameMenuSaveGame { }
    [Concept]
    public class GameMenuResetGame { }
    [Concept]


    public class GameSettings { }
    [Concept]
    public class GameLanguage { }
    [Concept]
    public class GameMenuLanguageLabel { }


    [Concept]
    public class GameMenuInspectInventory { }

    [Concept]
    public class GameMenuGotoMainMap { }

    [Concept]
    public class GameMenuResetGameConfirmation { }

    [Concept]
    public class GameMenuLabel { }


    [Concept]
    public class UserInterfaceBackgroundTransparency { }


    [Concept]
    public class UtilityButtonsOnTheLeft { }

    [Concept]
    public class LogisticsAnimationIsLinear { }

    [Concept]
    public class InversedMovement { }

    [Concept]
    public class EnableLight { }
    [Concept]
    public class EnableWeather { }

    [Concept]
    public class OneTapHammer { }


    [Concept]
    public class ToneMapping { }



    public interface ITileDescription
    {
        string TileDescription { get; }
    }

    /// <summary>
    /// 重构方法：配置项：类型, getter, setter
    /// </summary>
    public class GameMenu : MonoBehaviour
    {
        public static GameEntry Entry { get; set; }

        public static GameMenu Ins { get; private set; }

        public static bool IsInEditor { get; private set; }
        public static bool IsInStandalone { get; private set; }
        public static bool IsInMobile { get; private set; }


        private void Awake() {

            if (Ins != null) throw new Exception();
            Ins = this;

#if UNITY_EDITOR
            IsInEditor = true;
#else
            IsInEditor = false;
#endif

#if UNITY_STANDALONE || UNITY_EDITOR
            IsInStandalone = true;
            IsInMobile = false;
#else
            IsInStandalone = false;
            IsInMobile = true;
#endif
            offset = IsInStandalone ? 36 : 0;
            InitializeNotification();

            //fullScreenWidth = Screen.width;
            //fullScreenHeight = Screen.height;
            //if (IsInMobile) {
            //    TryIncreaseGamePerformance();
            //}

            // rawImage.color = Color.white;
        }


        //[Space] // 特殊位置
        //[SerializeField]
        //private UnityEngine.UI.RawImage rawImage;
        //public RenderTexture Raw {
        //    set {
        //        rawImage.texture = value;
        //    }
        //}



        [Header("notification")]
        [SerializeField]
        private GameObject Container;
        public void SetVisible(bool visible) {
            Container.SetActive(visible);
        }

        [SerializeField]
        private UnityEngine.UI.Text TileDescriptionForStandalone;
        [SerializeField]
        private UnityEngine.UI.Text Notification1Text;
        [SerializeField]
        private UnityEngine.UI.Text Notification2Text;
        [SerializeField]
        private RectTransform Notification1Transform;
        [SerializeField]
        private RectTransform Notification2Transform;

        public void SetTileDescriptionForStandalong(string text) {
            TileDescriptionForStandalone.text = text;
        }
        private float pushedTime = 0;
        const float left = 0;
        const float top = 36;
        public void PushNotification(string notice) {
            if (Notification1Text.text.Length == 0) {
                Notification1Text.text = notice;
            } else if (Notification2Text.text.Length == 0) {
                Notification2Text.text = notice;
            } else {
                Notification1Text.text = Notification2Text.text;
                Notification2Text.text = notice;
            }
            Notification1Transform.anchoredPosition = new Vector2(left, -top - offset);
            Notification2Transform.anchoredPosition = new Vector2(left, -top * 2 - offset);
            pushedTime = Time.time;
        }
        float offset = 0;

        private void InitializeNotification() {
            Notification1Text.text = null;
            Notification2Text.text = null;
            Notification1Transform.anchoredPosition = new Vector2(left, -top - offset);
            Notification2Transform.anchoredPosition = new Vector2(left, -top * 2 - offset);
            Notification1Text.material.color = SetA(Notification1Text.color, 0);
            Notification2Text.material.color = SetA(Notification1Text.color, 0);
            pushedTime = 0;
        }

        private float alpha1 = 0;
        private float alpha2 = 0;
        private void Update() {
            float time = Time.time;
            const float animatedTime = 1f;
            float deltaTime = pushedTime == 0 ? animatedTime : time - pushedTime;
            if (deltaTime < animatedTime) {
                float normalTime = deltaTime / animatedTime;
                float sinTime = Mathf.Sin(Mathf.PI * normalTime);
                if (Notification2Text.text.Length == 0) {
                    Notification1Transform.anchoredPosition = new Vector2(left, -top - offset);

                    alpha1 = sinTime > 0.5f ? 1 : Mathf.Lerp(0, 1, sinTime / 0.5f);
                    SyncText1();
                    SyncText2();
                    // Notification1Text.material.color = SetA(Notification1Text.color, sinTime > 0.5f ? 1 : Mathf.Lerp(0, 1, sinTime / 0.5f));
                } else {
                    Notification1Transform.anchoredPosition = new Vector2(left, Mathf.Lerp(-top - offset, 0 - offset, normalTime < 0.5f ? sinTime : 1));
                    Notification2Transform.anchoredPosition = new Vector2(left, Mathf.Lerp(-top * 2 - offset, -top - offset, normalTime < 0.5f ? sinTime : 1));

                    alpha1 = normalTime < 0.5f ? 1 - sinTime : 0;
                    alpha2 = sinTime;
                    SyncText1();
                    SyncText2();
                }
            } else {
                Notification1Text.text = null;
                Notification2Text.text = null;
                if (alpha1 != 0) {
                    alpha1 = 0;
                    SyncText1();
                }
                if (alpha2 != 0) {
                    alpha2 = 0;
                    SyncText2();
                }
            }
        }
        private void SyncText1() {
            if (alpha1 == 0) {
                Notification1Text.enabled = false;
            } else {
                Notification1Text.enabled = true;
                Notification1Text.material.color = SetA(Notification1Text.color, alpha1);
            }
        }
        private void SyncText2() {
            if (alpha2 == 0) {
                Notification2Text.enabled = false;
            } else {
                Notification2Text.enabled = true;
                Notification2Text.material.color = SetA(Notification2Text.color, alpha2);
            }
        }
        private Color SetA(Color c, float a) {
            c.a = a;
            return c;
        }


        private void Start() {
            SynchronizeSettings();
            SyncButtonsOutlines();
            TileDescriptionForStandalone.gameObject.SetActive(IsInStandalone);

            SyncHammer();
            SyncMagnet();

            // Debug.LogWarning(TimeUtility.GetSecondsInDouble().ToString());
            if (TimeUtility.GetSecondsInDouble() < 63756330123 + 60 * 60 * 24) {
                IGlobals globals = Globals.Ins;
                globals.Bool<EnableWeather>(true);
                globals.Bool<EnableLight>(true);
                SyncSound();
                SyncEnableWeather();
                SyncEnableLight();
            }

        }
        public static void OnConstruct() {
            RestoreDefaultSettings();
        }

        public static void RestoreDefaultSettings() {
            // 现在习惯把和游戏设置有关, 游戏逻辑无关的初始化过程, 放到GameMenu。和游戏逻辑有关的放到GameConfig

            IGlobals globals = Globals.Ins;
            // 初始音效音量
            IValue soundVolume = globals.Values.GetOrCreate<SoundVolume>();
            soundVolume.Max = 600;
            // 初始音乐音量
            IValue musicVolume = globals.Values.GetOrCreate<MusicVolume>();
            musicVolume.Max = 600;
            // 初始天气音量
            IValue weatherVolume = globals.Values.GetOrCreate<WeatherVolume>();
            weatherVolume.Max = 600;

            //// 提示设置
            //Globals.Ins.Bool<InventoryQueryInformationOfCostDisabled>(true);
            //Globals.Ins.Bool<InventoryQueryInformationOfRevenueDisabled>(true);

            globals.Bool<SoundEnabled>(true);
            globals.Bool<MusicEnabled>(true);
            globals.Bool<WeatherEnabled>(false);
            globals.Bool<EnableLight>(true);

            globals.Values.GetOrCreate<ToneMapping>().Max = 1; // neutural

            globals.Values.GetOrCreate<MapView.TappingSensitivity>().Max = 100;

            globals.Bool<UsePixelFont>(false);
            Globals.Ins.Bool<ScreenAdaptation.DoubleSizeOption>(false);

            globals.Values.GetOrCreate<UserInterfaceBackgroundTransparency>().Max = (long)(0.75f * userInterfaceBackgroundTransparencyFactor);

            globals.Bool<UtilityButtonsOnTheLeft>(false);
            globals.Bool<LogisticsAnimationIsLinear>(false);
            globals.Bool<InversedMovement>(false);

            globals.Bool<StaticButton>(false);
            globals.Bool<StaticAxis>(true);

            globals.Bool<OneTapHammer>(true);
        }

        public void SynchronizeSettings() {
            SyncEnableLight();
            SyncEnableWeather();
            SyncToneMapping();
            SyncStaticButton();
            SyncStaticAxis();

            SynchronizeFont();
            SyncSound();
            SyncCameraSensitivity();
            SyncDoubleSize();
            SyncUserInterfaceBackgroundTransparency();
            SyncLogisticsAnimation();
            SyncUtilityButtonPosition();
            SyncOneTapHammer();
        }

        public const float VolumeFactor = 1000f;

        private void SyncSound() {
            Sound.Ins.MusicEnabled = Globals.Ins.Bool<MusicEnabled>();
            Sound.Ins.MusicVolume = Globals.Ins.Values.GetOrCreate<MusicVolume>().Max / VolumeFactor;
            Sound.Ins.SoundVolume = Globals.Ins.Values.GetOrCreate<SoundVolume>().Max / VolumeFactor;

            Sound.Ins.WeatherEnabled = Globals.Ins.Bool<WeatherEnabled>();
            Sound.Ins.WeatherVolume = Sound.Ins.WeatherEnabled ? Globals.Ins.Values.GetOrCreate<WeatherVolume>().Max / VolumeFactor : 0;
        }


        public static bool LightEnabled { get; private set; }
        public void SyncEnableLight() {
            LightEnabled = Globals.Ins.Bool<EnableLight>();
            (MapView.Ins as MapView).EnableLight = LightEnabled;
        }
        public static bool WeatherEnabled { get; private set; }
        public void SyncEnableWeather() {
            WeatherEnabled = Globals.Ins.Bool<EnableWeather>();
            (MapView.Ins as MapView).EnableWeather = WeatherEnabled;
        }
        public void SyncToneMapping() {
            long val = Globals.Ins.Values.GetOrCreate<ToneMapping>().Max;
            switch (val) {
                case 1:
                    GlobalVolume.Ins.Tonemapping.mode.value = UnityEngine.Rendering.Universal.TonemappingMode.Neutral;
                    // = new UnityEngine.Rendering.Universal.TonemappingModeParameter(UnityEngine.Rendering.Universal.TonemappingMode.Neutral, true);
                    break;
                case 2:
                    GlobalVolume.Ins.Tonemapping.mode.value = UnityEngine.Rendering.Universal.TonemappingMode.ACES;
                    // = new UnityEngine.Rendering.Universal.TonemappingModeParameter(UnityEngine.Rendering.Universal.TonemappingMode.ACES, true);
                    break;
                default:
                    GlobalVolume.Ins.Tonemapping.mode.value = UnityEngine.Rendering.Universal.TonemappingMode.None;
                    // = new UnityEngine.Rendering.Universal.TonemappingModeParameter(UnityEngine.Rendering.Universal.TonemappingMode.None, true);
                    break;
            }
        }


        private const float camerSensitivityFactor = 100f;
        private void SyncCameraSensitivity() {
            MapView.Ins.TappingSensitivityFactor = MapView.DefaultTappingSensitivity * (Globals.Ins.Values.GetOrCreate<MapView.TappingSensitivity>().Max / camerSensitivityFactor);
        }
        private void SyncDoubleSize() {
            ScreenAdaptation.Ins.DoubleSize = Globals.Ins.Bool<ScreenAdaptation.DoubleSizeOption>();
            ScreenAdaptation.Ins.RecalcSize();
        }

        private const float userInterfaceBackgroundTransparencyFactor = 100f;
        private void SyncUserInterfaceBackgroundTransparency() {
            UI.Ins.SetBackgroundTransparency(Globals.Ins.Values.GetOrCreate<UserInterfaceBackgroundTransparency>().Max / userInterfaceBackgroundTransparencyFactor);
        }

        public static bool UseInversedMovement { get; private set; }
        private void SyncInversedMovement() {
            UseInversedMovement = Globals.Ins.Bool<InversedMovement>();
        }
        private void SyncUtilityButtonPosition() {
            if (LinkUnlinkButtonImage.transform is RectTransform rect) {
                rect.anchoredPosition = new Vector2(Globals.Ins.Bool<UtilityButtonsOnTheLeft>() ? (72 - 640) : 0, rect.anchoredPosition.y);
            }
            if (ConstructDestructButtonImage.transform is RectTransform rect2) {
                rect2.anchoredPosition = new Vector2(Globals.Ins.Bool<UtilityButtonsOnTheLeft>() ? (72 - 640) : 0, rect2.anchoredPosition.y);
            }
        }

        public static bool TapHammer { get; private set; } = false;
        private void SyncOneTapHammer() {
            TapHammer = Globals.Ins.Bool<OneTapHammer>();
        }

        public static bool IsLinear { get; private set; } = false;
        private void SyncLogisticsAnimation() {
            IsLinear = Globals.Ins.Bool<LogisticsAnimationIsLinear>();
        }

        public enum ShortcutMode
        {
            None,
            // Construct, Destruct, Run, Stop, Consume, Provide, Consume_Undo, Provide_Undo, Consume_Provide, Provide_Consume_Undo, 
            ConstructDestruct, LinkUnlink, RunStop
        }
        public ShortcutMode CurrentShortcutMode { get; set; }



        [Header("Construct Destruct")]
        [SerializeField]
        private Sprite ConstructDestructButtonSprite_Activating;
        [SerializeField]
        private Sprite ConstructDestructButtonSprite;
        [SerializeField]
        private UnityEngine.UI.Image ConstructDestructButtonImage;
        public void SyncHammer() {
            ConstructDestructButtonImage.gameObject.SetActive(Globals.Unlocked<KnowledgeOfHammer>());
        }
        public void OnTapConstructDestruct() {
            if (CurrentShortcutMode == ShortcutMode.ConstructDestruct) {
                CurrentShortcutMode = ShortcutMode.None;
            } else {
                CurrentShortcutMode = ShortcutMode.ConstructDestruct;
            }
            SyncButtonsOutlines();
            MapView.InterceptInteractionOnce = true;
        }

        [Header("Link Unlink")]
        [SerializeField]
        private Sprite LinkUnlinkButtonSprite_Activating;
        [SerializeField]
        private Sprite LinkUnlinkButtonSprite;
        [SerializeField]
        private UnityEngine.UI.Image LinkUnlinkButtonImage;
        public void SyncMagnet() {
            LinkUnlinkButtonImage.gameObject.SetActive(Globals.Unlocked<KnowledgeOfMagnet>());
        }
        public void OnTapLinkUnlink() {
            if (CurrentShortcutMode == ShortcutMode.LinkUnlink) {
                CurrentShortcutMode = ShortcutMode.None;
            } else {
                CurrentShortcutMode = ShortcutMode.LinkUnlink;
            }
            SyncButtonsOutlines();
            MapView.InterceptInteractionOnce = true;
        }

        [Header("Run Stop")]
        [SerializeField]
        private Sprite RunStopButtonSprite_Activating;
        [SerializeField]
        private Sprite RunStopButtonSprite;
        [SerializeField]
        private UnityEngine.UI.Image RunStopButtonImage;
        public void OnTapRunStop() {
            if (CurrentShortcutMode == ShortcutMode.RunStop) {
                CurrentShortcutMode = ShortcutMode.None;
            } else {
                CurrentShortcutMode = ShortcutMode.RunStop;
            }
            SyncButtonsOutlines();
            MapView.InterceptInteractionOnce = true;
        }

        [Header("Shortcut")]
        [SerializeField]
        private Sprite ShortcutButtonSprite_Activating;
        [SerializeField]
        private Sprite ShortcutButtonSprite;
        [SerializeField]
        private UnityEngine.UI.Image ShortcutButtonImage;
        private void SyncButtonsOutlines() {
            bool noneMode = CurrentShortcutMode == ShortcutMode.None;
            bool constructDestruct = CurrentShortcutMode == ShortcutMode.ConstructDestruct;
            bool linkUnlink = CurrentShortcutMode == ShortcutMode.LinkUnlink;
            bool runStop = CurrentShortcutMode == ShortcutMode.RunStop;

            ShortcutButtonImage.sprite = (!noneMode && !constructDestruct && !linkUnlink && !runStop) ? ShortcutButtonSprite_Activating : ShortcutButtonSprite;

            ConstructDestructButtonImage.sprite = constructDestruct ? ConstructDestructButtonSprite_Activating : ConstructDestructButtonSprite;
            LinkUnlinkButtonImage.sprite = linkUnlink ? LinkUnlinkButtonSprite_Activating : LinkUnlinkButtonSprite;
            RunStopButtonImage.sprite = runStop ? RunStopButtonSprite_Activating : RunStopButtonSprite;
        }

        ////帮助我们 播放广告
        //public void OnTapSupport() {
        //    MapView.InterceptInteractionOnce = true;
        //    PlanetLander.TryPlayAd();
        //}

        // 问号按钮
        public void OnTapQuest() {
            MapView.InterceptInteractionOnce = true;
            //MainQuest.Ins.OnTap();
        }

        // 点玩家自己
        public void OnTapPlayerInventory() {
            Vector2Int position = MapView.Ins.CharacterPosition;
            IMap map = MapView.Ins.TheOnlyActiveMap;
            if (map.Get(position) is PlanetLander planetLander) {
                // 防止玩家卡在一格位置
                planetLander.OnTap();
            } else {
                List<IUIItem> items = new List<IUIItem>();
                UIItem.AddEntireInventory(Globals.Ins.Inventory, items, OnTapPlayerInventory, true);
                items.Add(UIItem.CreateSeparator());
                items.Add(UIItem.CreateValueProgress<Sanity>(Globals.Ins.Values));
                items.Add(UIItem.CreateTimeProgress<Sanity>(Globals.Ins.Values));
                items.Add(UIItem.CreateValueProgress<Satiety>(Globals.Ins.Values));
                IRef planetLanderPos = Globals.Ins.Refs.Get<PlanetLanderPos>();
                if (planetLanderPos.X != -1)
                    items.Add(UIItem.CreateButton("回到火箭", () => {
                        MapView.Ins.CharacterPosition = new Vector2Int((int)planetLanderPos.X, (int)planetLanderPos.Y);
                        MapView.Ins.SyncCharacterPosition();
                        Globals.Ins.Values.GetOrCreate<Sanity>().Val = 0;
                    }));
                UI.Ins.ShowItems("【随身物品】", items);
            }
        }

        // 地图资源按钮
        public void OnTapInventoryOfResource() {
            MapView.InterceptInteractionOnce = true;
            List<IUIItem> items = new List<IUIItem>();
            UIItem.AddEntireInventory(MapView.Ins.TheOnlyActiveMap.Inventory, items, OnTapInventoryOfResource, true);
            items.Add(UIItem.CreateSeparator());
            UI.Ins.ShowItems("【星球资源仓库】", items);
        }

        // 地图盈余按钮
        public void OnTapInventoryOfSupply() {
            MapView.InterceptInteractionOnce = true;
            List<IUIItem> items = new List<IUIItem>();
            UIItem.AddEntireInventory(MapView.Ins.TheOnlyActiveMap.InventoryOfSupply, items, OnTapInventoryOfSupply, false);
            items.Add(UIItem.CreateSeparator());
            UI.Ins.ShowItems("【星球盈余产出】", items);
        }


        // 齿轮按钮
        public void OnTapSettings() {
            MapView.InterceptInteractionOnce = true;

            IMap map = MapView.Ins.TheOnlyActiveMap;


            UI.Ins.ShowItems(Localization.Ins.Get<GameMenuLabel>(), new List<IUIItem>() {

#if UNITY_EDITOR
                
            UIItem.CreateButton($"改变作弊模式。当前{(GameConfig.CheatMode ? "开启" : "关闭")}", () => {
                GameConfig.CheatMode = !GameConfig.CheatMode;
                SyncHammer();
                SyncMagnet();
                OnTapSettings();
            }),
#endif


            Sound.Ins.IsPlaying ? UIItem.CreateDynamicText(() => $"背景音乐《{Sound.Ins.PlayingMusicName}》播放中") : null,


                UIItem.CreateSeparator(),

                // UIItem.CreateButton("游戏怎么玩? 点这里看攻略", IntroPage),

                UIItem.CreateButton("游戏遇到各种问题，怎么办? 常见问题解决方法", OpenFAQPage),

                UIItem.CreateButton("作者又挖了什么新坑? 《挂机工厂2》", IndustarryPage),

                UIItem.CreateButton("点此访问玩家QQ群", PlayerQQPage),

                new UIItem {
                    Type = IUIItemType.Button,
                    Content = Globals.Ins.Bool<ScreenAdaptation.DoubleSizeOption>() ? $"双倍视野：启用" : $"双倍视野：禁用",
                    OnTap = () => {
                        Globals.Ins.Bool<ScreenAdaptation.DoubleSizeOption>(!Globals.Ins.Bool<ScreenAdaptation.DoubleSizeOption>());
                        SyncDoubleSize();
                        // OpenGameSettingMenu();
                        UI.Ins.Active = false;
                    }
                },

                UIItem.CreateText("操作不习惯，也许可以查看下方更多游戏设置"),


                UIItem.CreateSeparator(),

                UIItem.CreateButton(Localization.Ins.Get<GameMenuSaveGame>(), OnTapSaveGameButton),
                UIItem.CreateButton(Localization.Ins.Get<GameSettings>(), OpenGameSettingMenu),



                UIItem.CreateSeparator(),

                UIItem.CreateButton(Localization.Ins.Get<GameMenuExitGame>(), UIDecorator.ConfirmBefore(() => Entry.ExitGame(), OnTapSettings)),



                UIItem.CreateButton(string.Format(Localization.Ins.Get<GameMenuLanguageLabel>(), Localization.Ins.Get<GameLanguage>()), () => {
                    Localization.Ins.SwitchNextLanguage();
                    OnTapSettings();
                }),

                new UIItem {
                    Type = IUIItemType.Image,
                    // Content = "global",
                    DynamicContent = () => $"first_{MapView.Ins.AnimationIndex % 16}",
                    LeftPadding = 32,
                    Scale = 6,
                    OnTap = () => {
                        Localization.Ins.SwitchNextLanguage();
                        OnTapSettings();
                    }
                },

                // UIItem.CreateButton("游戏作者还在搬砖打工! 施舍点广告费吧! 求求了", AdPage),

                UIItem.CreateSeparator(),
            });
        }


        public void AdPage() {
            var items = UI.Ins.GetItems();

            items.Add(UIItem.CreateReturnButton(OnTapSettings));

            // items.Add(UIItem.CreateButton("游戏怎么玩? 点这里看攻略", IntroPage));
            items.Add(UIItem.CreateButton("点此访问玩家QQ群", PlayerQQPage));



            items.Add(UIItem.CreateSeparator());

            items.Add(UIItem.CreateText("点击下面的按钮看广告拿奖励。如果没反应就是广告商不给看"));
            items.Add(UIItem.CreateText("可选的奖励也会随着游戏进程解锁"));

            if (Globals.Ins.Values.Get<Sanity>().Max < 1000) {
                items.Add(UIItem.CreateDynamicButton($"观看广告，体力上限变为1000并且充满1次。永久只能领取一次", () => {
                    Television.Ad = AdSelection.SanityMax;
                    PlanetLander.TryPlayAd();
                }, PlanetLander.CanPlayAd));
            }


            if (Globals.Ins.Values.Get<AdChance_Berry1000>().Max > 0) {
                items.Add(UIItem.CreateDynamicButton($"观看广告，拿走{Localization.Ins.Val<Berry>(1000)}。永久只能领取一次", () => {
                    Television.Ad = AdSelection.Berry1000;
                    PlanetLander.TryPlayAd();
                }, PlanetLander.CanPlayAd));
            } else {
                items.Add(UIItem.CreateDynamicButton($"观看广告，拿走{Localization.Ins.Val<Berry>(100)}", () => {
                    Television.Ad = AdSelection.Berry100;
                    PlanetLander.TryPlayAd();
                }, PlanetLander.CanPlayAd));
            }

            //else {
            //    items.Add(UIItem.CreateDynamicButton($"观看广告，拿走{Localization.Ins.Val<Berry>(1)}", () => {
            //        Television.Ad = AdSelection.Berry1;
            //        PlanetLander.TryPlayAd();
            //    }, PlanetLander.CanPlayAd));
            //}

            if (Globals.Unlocked<KnowledgeOfMagnet>()) {
                if (Globals.Ins.Values.Get<AdChance_Grain1000>().Max > 0) {
                    items.Add(UIItem.CreateDynamicButton($"观看广告，拿走{Localization.Ins.Val<Grain>(1000)}。永久只能领取一次", () => {
                        Television.Ad = AdSelection.Grain1000;
                        PlanetLander.TryPlayAd();
                    }, PlanetLander.CanPlayAd));
                } else {
                    items.Add(UIItem.CreateDynamicButton($"观看广告，拿走{Localization.Ins.Val<Grain>(100)}", () => {
                        Television.Ad = AdSelection.Grain100;
                        PlanetLander.TryPlayAd();
                    }, PlanetLander.CanPlayAd));
                }
            }

            if (Globals.Unlocked<WorkshopOfWoodcutting>()) {

                if (Globals.Ins.Values.Get<AdChance_WoodPlank1000>().Max > 0) {
                    items.Add(UIItem.CreateDynamicButton($"观看广告，拿走{Localization.Ins.Val<WoodPlank>(1000)}。永久只能领取一次", () => {
                        Television.Ad = AdSelection.WoodPlant1000;
                        PlanetLander.TryPlayAd();
                    }, PlanetLander.CanPlayAd));
                }
                else {
                    items.Add(UIItem.CreateDynamicButton($"观看广告，拿走{Localization.Ins.Val<WoodPlank>(100)}", () => {
                        Television.Ad = AdSelection.WoodPlank100;
                        PlanetLander.TryPlayAd();
                    }, PlanetLander.CanPlayAd));
                }
            }

            if (Globals.Unlocked<BuildingPrefabrication>()) {

                if (Globals.Ins.Values.Get<AdChance_WoodPlank1000>().Max > 0) {
                    items.Add(UIItem.CreateDynamicButton($"观看广告，拿走{Localization.Ins.Val<BuildingPrefabrication>(1000)}。永久只能领取一次", () => {
                        Television.Ad = AdSelection.BuildingPrefabrication1000;
                        PlanetLander.TryPlayAd();
                    }, PlanetLander.CanPlayAd));
                }
                else {
                    items.Add(UIItem.CreateDynamicButton($"观看广告，拿走{Localization.Ins.Val<BuildingPrefabrication>(100)}", () => {
                        Television.Ad = AdSelection.BuildingPrefabrication100;
                        PlanetLander.TryPlayAd();
                    }, PlanetLander.CanPlayAd));
                }
            }

            if (Globals.Unlocked<ResidenceOfGrass>()) {
                items.Add(UIItem.CreateDynamicButton($"观看广告，拿走{Localization.Ins.Val<Worker>(3)}", () => {
                    Television.Ad = AdSelection.Population3;
                    PlanetLander.TryPlayAd();
                }, PlanetLander.CanPlayAd));
            }
            if (Globals.Unlocked<SchoolOfPhysics>()) {
                items.Add(UIItem.CreateDynamicButton($"观看广告，拿走{Localization.Ins.Val<Electricity>(10)}产量", () => {
                    Television.Ad = AdSelection.Electricity10;
                    PlanetLander.TryPlayAd();
                }, PlanetLander.CanPlayAd));
            }

            items.Add(UIItem.CreateValueProgress<AdCoolDown>(Globals.Ins.Values));
            items.Add(UIItem.CreateTimeProgress<AdCoolDown>(Globals.Ins.Values));



            items.Add(new UIItem {
                Type = IUIItemType.Image,
                // Content = "global",
                DynamicContent = () => $"first_{MapView.Ins.AnimationIndex % 16}",
                LeftPadding = 32,
                Scale = 6,
                OnTap = () => {
                    Localization.Ins.SwitchNextLanguage();
                    OnTapSettings();
                }
            });


            items.Add(UIItem.CreateButton("游戏遇到各种问题，怎么办? 常见问题解决方法", OpenFAQPage));

            items.Add(UIItem.CreateButton("作者又挖了什么新坑? 《挂机工厂2》", IndustarryPage));



            UI.Ins.ShowItems("游戏作者还在搬砖打工! 施舍点广告费吧! 求求了", items);
        }

        public void IndustarryPage() {
            var items = UI.Ins.GetItems();

            items.Add(UIItem.CreateReturnButton(AdPage));


            items.Add(UIItem.CreateText("作者是否健在? 参考下图"));

            items.Add(UIItem.CreateTileImage("cat", 1));

            UI.Ins.ShowItems("《挂机工厂2》介绍", items);
        }

        //private void IntroPage() {
        //    var items = UI.Ins.GetItems();

        //    items.Add(UIItem.CreateReturnButton(OnTapSettings));

        //    items.Add(UIItem.CreateMultilineText("游戏玩家都是人才! 教程做得比作者好多了"));
        //    items.Add(UIItem.CreateSeparator());
        //    items.Add(UIItem.CreateMultilineText("＃萌新上手＃关于设施的运行机制以及提高收益"));
        //    items.Add(UIItem.CreateButton("点此复制  taptap.com/topic/18076067", () => {
        //        UnityEngine.GUIUtility.systemCopyBuffer = "taptap.com/topic/18076067";
        //    }));
        //    items.Add(UIItem.CreateSeparator());
        //    items.Add(UIItem.CreateMultilineText("挂机工厂攻略（浪中闪亮原创）"));
        //    items.Add(UIItem.CreateButton("点此复制  taptap.com/topic/18023924", () => {
        //        UnityEngine.GUIUtility.systemCopyBuffer = "taptap.com/topic/18023924";
        //    }));

        //    items.Add(UIItem.CreateSeparator());
        //    items.Add(UIItem.CreateMultilineText("挂机工厂攻略（游戏作者原创）"));
        //    items.Add(UIItem.CreateMultilineText("造这个，造那个，用磁铁，阿巴阿巴"));
        //    items.Add(UIItem.CreateSeparator());

        //    UI.Ins.ShowItems("挂机工厂攻略", items);
        //}

        private void OpenFAQPage() {
            var items = UI.Ins.GetItems();

            items.Add(UIItem.CreateReturnButton(OnTapSettings));

            items.Add(UIItem.CreateFAQText("手机卡顿、发热, 怎么办? ", "在设置里关闭天气、关闭灯光"));
            items.Add(UIItem.CreateFAQText("建筑不工作, 怎么办? ", "点击建筑, 点击建筑功能, 查看是否满足建筑运行条件"));
            items.Add(UIItem.CreateFAQText("按钮不灵敏, 怎么办? ", "因为按钮代码问题, 按下按钮时, 手指不能移动, 必须原地松开, 才能有效按下按钮"));

            items.Add(UIItem.CreateFAQText("还是没有解决问题, 怎么办? ", "默念 “垃圾游戏, 费我时间, 颓我精神, 毁我青春”, 然后关闭并且卸载游戏"));

            UI.Ins.ShowItems("常见问题解决方法", items);
        }

        private void OnTapSaveGameButton() {
            Entry.SaveGame();
            UI.Ins.ShowItems("提示", new List<IUIItem> {
                UIItem.CreateText("已经保存"),

                UIItem.CreateReturnButton(OnTapSettings),

                UIItem.CreateSeparator(),

                UIItem.CreateButton("返回游戏", () => UI.Ins.Active = false),

                UIItem.CreateSeparator(),


                UIItem.CreateButton(Localization.Ins.Get<GameMenuExitGame>(), Entry.ExitGame)
            }); ; ; ;
        }

        private class UsePixelFont { }

        [Space]
        [SerializeField]
        private Font pixelFont;
        [SerializeField]
        private Font arialFont;
        [SerializeField]
        private GameObject[] objectsWithFonts;


        public void SetFont(bool pixel) {
            Globals.Ins.Bool<UsePixelFont>(pixel);
            SynchronizeFont();
        }
        public void ChangeFont() {
            Globals.Ins.Bool<UsePixelFont>(!Globals.Ins.Bool<UsePixelFont>());
        }
        public void SynchronizeFont() {
            Font fontUsed = Globals.Ins.Bool<UsePixelFont>() ? pixelFont : arialFont;
            // progressBar
            foreach (var obj in objectsWithFonts) {
                UnityEngine.UI.Text text = obj.GetComponent<UnityEngine.UI.Text>();
                if (text != null) {
                    text.font = fontUsed;
                    continue;
                }
                ProgressBar progressBar = obj.GetComponent<ProgressBar>();
                if (progressBar != null) {
                    progressBar.Text.font = fontUsed;
                    continue;
                }
                text = obj.GetComponentInChildren<UnityEngine.UI.Text>();
                if (text != null) {
                    text.font = fontUsed;
                }
                throw new Exception();
            }
            (UI.Ins as UI).Title.GetComponent<UnityEngine.UI.Text>().font = fontUsed;
            TileDescriptionForStandalone.font = fontUsed;
            Notification1Text.font = fontUsed;
            Notification2Text.font = fontUsed;
        }

        [SerializeField]
        private GameObject StaticButtonObject;
        private void SyncStaticButton() {
            UseStaticButton = Globals.Ins.Bool<StaticButton>();
            StaticButtonObject.SetActive(UseStaticButton);
        }

        private void SyncStaticAxis() {
            UseStatixAxis = Globals.Ins.Bool<StaticAxis>();
        }


        private void OpenConsole() {
            var items = UI.Ins.GetItems();
            UI ui = UI.Ins as UI;
            if (ui == null) throw new Exception();

            items.Add(UIItem.CreateButton("提交输入", () => {

                // 控制台解析
                string input = ui.InputFieldContent;

                if (input.StartsWith("cheat")) {
                    if (!GameConfig.CheatMode) {
                        GameConfig.CheatMode = true;
                        UIPreset.Notify(OpenConsole, "作弊模式已激活（免费建造、免费科研）");
                    } else {
                        GameConfig.CheatMode = false;
                        UIPreset.Notify(OpenConsole, "作弊模式已关闭");
                    }
                } else if (input.StartsWith("help")) {
                    string[] results = input.Split(' ');
                    if (results.Length >= 2 && int.TryParse(results[1], out int arg) && arg > 0) {
                        MapView.Ins.TheOnlyActiveMap.Inventory.Add<Worker>(arg);
                        UIPreset.Notify(OpenConsole, $"已经获得worker {arg}");
                    } else {
                        UIPreset.Notify(OpenConsole, "help指令参数无效");
                    }
                } else if (input.StartsWith("add")) {
                    string[] results = input.Split(' ');
                    if (results.Length >= 3 && int.TryParse(results[2], out int arg) && arg > 0) {
                        Type type = Type.GetType("Weathering." + results[1]);
                        if (type != null && Tag.IsValidTag(type)) {
                            MapView.Ins.TheOnlyActiveMap.Inventory.Add(type, arg);
                            UIPreset.Notify(OpenConsole, $"已经获得 {Localization.Ins.Val(type, arg)} {arg}");
                        } else {
                            UIPreset.Notify(OpenConsole, "add指令type参数无效. 指令格式: add <type> <quantity>");
                        }
                    } else {
                        UIPreset.Notify(OpenConsole, "add指令参数无效. 指令格式: add <type> <quantity>");
                    }
                } else {
                    UIPreset.Notify(OpenConsole, "指令无效");
                }

            }));

            ui.ShowInputFieldNextTime = true;
            UI.Ins.ShowItems("打开控制台", items);
        }

        private const long minAutoSave = 15;
        private const long maxAutoSave = 600;

        public void OpenGameSettingMenu() {

            UI.Ins.ShowItems(Localization.Ins.Get<GameSettings>(), new List<IUIItem>() {

                UIItem.CreateReturnButton(OnTapSettings),

                UIItem.CreateSeparator(),

                new UIItem {
                    Type = IUIItemType.Slider,
                    InitialSliderValue = Mathf.InverseLerp(50, 200, Globals.Ins.Values.GetOrCreate<MapView.TappingSensitivity>().Max),
                    DynamicSliderContent = (float x) => {

                        int sensitivity = (int)(camerSensitivityFactor*(1.5f*x+0.5f));
                        Globals.Ins.Values.GetOrCreate<MapView.TappingSensitivity>().Max = sensitivity;
                        SyncCameraSensitivity();
                        return $"镜头灵敏度 {sensitivity}";
                    }
                },


                new UIItem {
                    Type = IUIItemType.Slider,
                    InitialSliderValue =Globals.Ins.Values.GetOrCreate<UserInterfaceBackgroundTransparency>().Max/userInterfaceBackgroundTransparencyFactor,
                    DynamicSliderContent = (float x) => {
                        float alpha = x*userInterfaceBackgroundTransparencyFactor;
                        Globals.Ins.Values.GetOrCreate<UserInterfaceBackgroundTransparency>().Max = (long)alpha;
                        SyncUserInterfaceBackgroundTransparency();
                        return $"背景透明度 {alpha}";
                    }
                },

                UIItem.CreateSeparator(),

                new UIItem {
                    Type = IUIItemType.Button,
                    Content = Globals.Ins.Bool<ScreenAdaptation.DoubleSizeOption>() ? $"双倍视野：启用" : $"双倍视野：禁用",
                    OnTap = () => {
                        Globals.Ins.Bool<ScreenAdaptation.DoubleSizeOption>(!Globals.Ins.Bool<ScreenAdaptation.DoubleSizeOption>());
                        SyncDoubleSize();
                        // OpenGameSettingMenu();
                        UI.Ins.Active = false;
                    }
                },

                new UIItem {
                    Type = IUIItemType.Button,
                    Content = CalcToneMappingName(Globals.Ins.Values.GetOrCreate<ToneMapping>().Max),
                    OnTap = ToneMappingPage
                },

                new UIItem {
                    Type = IUIItemType.Button,
                    Content = Globals.Ins.Bool<InversedMovement>() ? $"控制反转：启用" : $"控制反转：禁用",
                    OnTap = () => {
                        Globals.Ins.Bool<InversedMovement>(!Globals.Ins.Bool<InversedMovement>());
                        SyncInversedMovement();
                        OpenGameSettingMenu();
                    }
                },

                new UIItem {
                    Type = IUIItemType.Button,
                    Content = Globals.Ins.Bool<EnableLight>() ? $"光影效果：启用" : $"光影效果：禁用",
                    OnTap = () => {
                        Globals.Ins.Bool<EnableLight>(!Globals.Ins.Bool<EnableLight>());
                        SyncEnableLight();
                        OpenGameSettingMenu();
                    }
                },

                new UIItem {
                    Type = IUIItemType.Button,
                    Content = Globals.Ins.Bool<EnableWeather>() ? $"天气效果：启用" : $"天气效果：禁用",
                    OnTap = () => {
                        bool result = !Globals.Ins.Bool<EnableWeather>();
                        Globals.Ins.Bool<EnableWeather>(result);
                        Globals.Ins.Bool<WeatherEnabled>(result);
                        SyncSound();
                        SyncEnableWeather();
                        OpenGameSettingMenu();
                    }
                },

                new UIItem {
                    Type = IUIItemType.Button,
                    Content = Globals.Ins.Bool<UsePixelFont>() ? "当前字体：像素字体" : "当前字体：圆滑字体",
                    OnTap = () => {
                        ChangeFont();
                        SynchronizeFont();
                        OpenGameSettingMenu();
                    }
                },

                new UIItem {
                    Type = IUIItemType.Button,
                    Content = Globals.Ins.Bool<UtilityButtonsOnTheLeft>() ? $"锤子磁铁按钮位置：左边" : $"锤子磁铁按钮位置：右边",
                    OnTap = () => {
                        Globals.Ins.Bool<UtilityButtonsOnTheLeft>(!Globals.Ins.Bool<UtilityButtonsOnTheLeft>());
                        SyncUtilityButtonPosition();
                        OpenGameSettingMenu();
                    }
                },

                new UIItem {
                    Type = IUIItemType.Button,
                    Content = Globals.Ins.Bool<StaticAxis>() ? "固定摇杆：已开启" : "固定摇杆：已关闭",
                    OnTap = () => {
                        Globals.Ins.Bool<StaticAxis>(!Globals.Ins.Bool<StaticAxis>());
                        SyncStaticAxis();
                        OpenGameSettingMenu();
                    }
                },

                new UIItem {
                    Type = IUIItemType.Button,
                    Content = Globals.Ins.Bool<StaticButton>() ? "方向按钮：已开启" : "方向按钮：已关闭",
                    OnTap = () => {
                        Globals.Ins.Bool<StaticButton>(!Globals.Ins.Bool<StaticButton>());
                        SyncStaticButton();
                        OpenGameSettingMenu();
                    }
                },

                new UIItem {
                    Type = IUIItemType.Button,
                    Content = Globals.Ins.Bool<LogisticsAnimationIsLinear>() ? $"物流动画：匀速" : $"物流动画：变速",
                    OnTap = () => {
                        Globals.Ins.Bool<LogisticsAnimationIsLinear>(!Globals.Ins.Bool<LogisticsAnimationIsLinear>());
                        SyncLogisticsAnimation();
                        OpenGameSettingMenu();
                    }
                },

                new UIItem {
                    Type = IUIItemType.Button,
                    Content = Globals.Ins.Bool<OneTapHammer>() ? $"新版锤子：开启" : $"新版锤子：关闭",
                    OnTap = () => {
                        Globals.Ins.Bool<OneTapHammer>(!Globals.Ins.Bool<OneTapHammer>());
                        SyncOneTapHammer();
                        OpenGameSettingMenu();
                    }
                },

                UIItem.CreateSeparator(),


                new UIItem {
                    Type = IUIItemType.Slider,
                    InitialSliderValue = (Globals.Ins.Values.Get<GameAutoSaveInterval>().Max-minAutoSave)/(float)(maxAutoSave-minAutoSave),
                    DynamicSliderContent = (float x) => {
                        long interval = (long)(x*(maxAutoSave-minAutoSave)+minAutoSave);
                        Globals.Ins.Values.Get<GameAutoSaveInterval>().Max = interval;
                        return $"自动存档间隔 {interval} 秒";
                    }
                },

                UIItem.CreateSeparator(),

                new UIItem {
                    Type = IUIItemType.Slider,
                    InitialSliderValue = Globals.Ins.Values.Get<SoundVolume>().Max / VolumeFactor,
                    DynamicSliderContent = (float x) => {
                        Globals.Ins.Values.Get<SoundVolume>().Max = (long)(x * VolumeFactor);
                        SyncSound();
                        return $"音效音量 {Math.Floor(x*100)}";
                    }
                },

                /// 游戏音效
                new UIItem {
                    Type = IUIItemType.Button,
                    DynamicContent = () => Globals.Ins.Bool<SoundEnabled>() ? "音效：已开启" : "音效：已关闭",
                    OnTap = () => {
                        Globals.Ins.Bool<SoundEnabled>(!Globals.Ins.Bool<SoundEnabled>());
                        SyncSound();
                        OpenGameSettingMenu();
                    }
                },

                UIItem.CreateSeparator(),

                new UIItem {
                    Type = IUIItemType.Slider,
                    InitialSliderValue = Globals.Ins.Values.Get<MusicVolume>().Max / VolumeFactor,
                    DynamicSliderContent = (float x) => {
                        Globals.Ins.Values.Get<MusicVolume>().Max = (long)(x * VolumeFactor);
                        SyncSound();
                        return $"音乐音量 {Math.Floor(x*100)}";
                    }
                },

                new UIItem {
                    Type = IUIItemType.Button,
                    Content = Globals.Ins.Bool<MusicEnabled>() ? "音乐：已开启" : "音乐：已关闭",
                    OnTap = () => {
                        Globals.Ins.Bool<MusicEnabled>(!Globals.Ins.Bool<MusicEnabled>());
                        SyncSound();
                        OpenGameSettingMenu();
                    }
                },

                Sound.Ins.IsPlaying ? UIItem.CreateDynamicText(() => $"《{Sound.Ins.PlayingMusicName}》播放中") : null,


                UIItem.CreateSeparator(),

                new UIItem {
                    Type = IUIItemType.Slider,
                    InitialSliderValue = Globals.Ins.Values.Get<WeatherVolume>().Max / VolumeFactor,
                    DynamicSliderContent = (float x) => {
                        Globals.Ins.Values.Get<WeatherVolume>().Max = (long)(x * VolumeFactor);
                        SyncSound();
                        return $"天气音量 {Math.Floor(x*100)}";
                    }
                },

                new UIItem {
                    Type = IUIItemType.Button,
                    Content = Globals.Ins.Bool<WeatherEnabled>() ? "雨声：已开启" : "雨声：已关闭",
                    OnTap = () => {
                        Globals.Ins.Bool<WeatherEnabled>(!Globals.Ins.Bool<WeatherEnabled>());
                        SyncSound();
                        OpenGameSettingMenu();
                    }
                },





                //UIItem.CreateSeparator(),

                //new UIItem {
                //    Type = IUIItemType.Button,
                //    Content = Globals.Ins.Bool<InventoryQueryInformationOfCostDisabled>() ? "获得资源时提示：已关闭" : "获得资源时提示：已开启",
                //    OnTap = () => {
                //        Globals.Ins.Bool<InventoryQueryInformationOfCostDisabled>(!Globals.Ins.Bool<InventoryQueryInformationOfCostDisabled>());
                //        OpenGameSettingMenu();
                //    }
                //},

                //new UIItem {
                //    Type = IUIItemType.Button,
                //    Content = Globals.Ins.Bool<InventoryQueryInformationOfRevenueDisabled>() ? "需求资源时提示：已关闭。推荐开启" : "需求资源时提示：已开启",
                //    OnTap = () => {
                //        Globals.Ins.Bool<InventoryQueryInformationOfRevenueDisabled>(!Globals.Ins.Bool<InventoryQueryInformationOfRevenueDisabled>());
                //        OpenGameSettingMenu();
                //    }
                //},

                UIItem.CreateSeparator(),

                UIItem.CreateButton("打开控制台", OpenConsole),

                new UIItem {
                    Type = IUIItemType.Button,
                    Content = $"还原默认设置",
                    OnTap = UIDecorator.ConfirmBefore(() => {
                        RestoreDefaultSettings();
                        SynchronizeSettings();
                        OpenGameSettingMenu();
                    }, OpenGameSettingMenu)
                },


                UIItem.CreateText($"游戏存档版本号 ${Globals.Ins.Refs.GetOrCreate<GameVersionCodeKey>().Value}"),
                /// 重置存档
                new UIItem {
                    Content = Localization.Ins.Get<GameMenuResetGame>(),
                    Type = IUIItemType.Button,
                    OnTap = UIDecorator.ConfirmBefore(Entry.DeleteGameSave, OpenGameSettingMenu, "确认重置存档吗? 需要重启游戏"),
                }
            });
        }

        private void PlayerQQPage() {
            var items = UI.Ins.GetItems();

            items.Add(UIItem.CreateReturnButton(OnTapSettings));


            items.Add(UIItem.CreateTileImage("qq_group_profile", 4));
            items.Add(UIItem.CreateText("点击下方按钮，复制QQ群号"));


            items.Add(UIItem.CreateButton("点此复制QQ群号 949555639  “图书馆” (新群)", () => {
                UnityEngine.GUIUtility.systemCopyBuffer = "949555639";
            }));

            items.Add(UIItem.CreateButton("点此复制QQ群号 768956301  “木板厂”", () => {
                UnityEngine.GUIUtility.systemCopyBuffer = "768956301";
            }));

            items.Add(UIItem.CreateButton("点此复制QQ群号 1056411477  “石砖厂”", () => {
                UnityEngine.GUIUtility.systemCopyBuffer = "1056411477";
            }));

            items.Add(UIItem.CreateButton("点此复制QQ群号 884032539  “红砖厂”", () => {
                UnityEngine.GUIUtility.systemCopyBuffer = "884032539";
            }));

            items.Add(UIItem.CreateButton("点此复制QQ群号 1074187249  “炼铁厂”", () => {
                UnityEngine.GUIUtility.systemCopyBuffer = "1074187249";
            }));

            items.Add(UIItem.CreateText("(若群已经满了, 可以QQ搜索 “挂机工厂” 加入或创建玩家群)"));

            items.Add(UIItem.CreateSeparator());

            items.Add(UIItem.CreateText("想一起开发游戏? 联系QQ：3532199579 （备注程序、美术、文案）"));
            items.Add(UIItem.CreateButton("点此复制QQ号 3532199579", () => {
                UnityEngine.GUIUtility.systemCopyBuffer = "3532199579";
            }));

            //items.Add(UIItem.CreateSeparator());

            //items.Add(UIItem.CreateText("想学习开发游戏? 联系QQ：2927126489"));
            //items.Add(UIItem.CreateButton("点此复制QQ号 2927126489", () => {
            //    UnityEngine.GUIUtility.systemCopyBuffer = "2927126489";
            //}));

            items.Add(UIItem.CreateText("开发者的QQ头像长下面这样"));
            items.Add(UIItem.CreateTileImage("qq_profile", 4));

            UI.Ins.ShowItems("加入玩家群", items);
        }

        private void OtherQQPage() {
            var items = UI.Ins.GetItems();

            items.Add(UIItem.CreateReturnButton(PlayerQQPage));


            items.Add(UIItem.CreateMultilineText("已经复制QQ群号 \n若群满，下方有其他群号"));




            // 
            UI.Ins.ShowItems(name, items);
        }


        private void ToneMappingPage() {
            var items = UI.Ins.GetItems();

            items.Add(UIItem.CreateReturnButton(OpenGameSettingMenu));

            long current = Globals.Ins.Values.GetOrCreate<ToneMapping>().Max;
            string name = CalcToneMappingName(current);

            items.Add(UIItem.CreateText(name));

            items.Add(CreateToneMappingButton(current, 0));
            items.Add(CreateToneMappingButton(current, 1));
            items.Add(CreateToneMappingButton(current, 2));

            UI.Ins.ShowItems(name, items);
        }
        private UIItem CreateToneMappingButton(long current, long other) {
            return UIItem.CreateStaticButton(CalcToneMappingName(other), () => {
                Globals.Ins.Values.GetOrCreate<ToneMapping>().Max = other;
                SyncToneMapping();
                ToneMappingPage();
            }, current != other);
        }

        private string CalcToneMappingName(long val) {
            string toneMappingName;
            switch (val) {
                case 1:
                    toneMappingName = "画面风格：自然";
                    break;
                case 2:
                    toneMappingName = "画面风格：鲜艳";
                    break;
                default:
                    toneMappingName = "画面风格：高效";
                    break;
            }
            return toneMappingName;
        }

        public class StaticAxis { }
        public bool UseStatixAxis { get; private set; }
        public class StaticButton { }
        public bool UseStaticButton { get; private set; }
        public Vector2 DeltaDistance { get; set; } = Vector2.zero;
        // 1 left right up down
        public void OnPointerChange(int arg) {

            MapView.InterceptInteractionOnce = true;

            if (arg < 0) {
                DeltaDistance = Vector2.zero;
            } else {
                switch (arg) {
                    case 0:
                        DeltaDistance = Vector2.left;
                        break;
                    case 1:
                        DeltaDistance = Vector2.right;
                        break;
                    case 2:
                        DeltaDistance = Vector2.up;
                        break;
                    case 3:
                        DeltaDistance = Vector2.down;
                        break;
                }
            }
        }
    }
}

