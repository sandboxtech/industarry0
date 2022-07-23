//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using ByteDance.Union;
//using Weathering;
//public class PangleAd : MonoBehaviour
//{
//    private bool hasCached = false;
//    private AdNative adNative;
//    private RewardVideoAd rewardAd;
//    private static PangleAd instance = null;//实例
//    public static PangleAd Instance
//    {
//        get
//        {
//            if (instance == null)
//            {
//                GameObject obj = new GameObject();
//                instance = (PangleAd)obj.AddComponent(typeof(PangleAd));
//            }
//            return instance;
//        }
//    }
//    private void callbackmethod(bool success, string message)
//    {
//        if (success) {
//            Debug.Log("SDK初始化成功" + success + "SDK初始化成功信息" + message+" 先缓存一个广告");
//            LoadExpressRewardAd();
//        }
//        else{
//            Debug.Log("SDK初始化失败" + success + "SDK初始化失败信息" + message);
//        }
//    }
//    private void Awake()
//    {
//        instance = this;
//    }
//    void Start()
//    {
//        Pangle.InitializeSDK(callbackmethod);
//    }

//    private AdNative AdNative
//    {
//        get
//        {
//            if (this.adNative == null)
//            {
//                this.adNative = SDK.CreateAdNative();
//            }
//#if UNITY_ANDROID
//            SDK.RequestPermissionIfNecessary();
//#endif
//            return this.adNative;
//        }
//    }
//    public void AdvertisementCache()
//    {
//        hasCached = true;
//    }
//    public void LoadAdvertisement()
//    {
//        LoadExpressRewardAd();//如果没有缓存则缓存
//    }
//    /// <summary>
//    /// 播放广告
//    /// </summary>
//    public void PlayerAdvertisement()
//    {
//        if (hasCached)
//        {
//            hasCached = false;
//            ShowExpressRewardAd();//播放
//        }
//        else
//        {
//            LoadExpressRewardAd();//如果没有缓存则缓存
//        }
//    }

//    public void LoadExpressRewardAd()
//    {
//        if (this.rewardAd != null)
//        {
//            this.rewardAd.Dispose();
//            this.rewardAd = null;
//        }
//        var adSlot = new AdSlot.Builder()
//            // com.WeatheringStudio.weathering210427
//            // com.WeatheringStudio.weathering210501
//            .SetCodeId("946786771") // 挂机工厂 // appid
//            // .SetCodeId("947218011") // 土木世界 没用了
//            .SetSupportDeepLink(true)
//            .SetImageAcceptedSize(1080, 1920)
//            .SetUserID("user123") // 用户id,必传参数
//            .SetMediaExtra("media_extra") // 附加参数，可选
//            .SetOrientation(AdOrientation.Horizontal) // 必填参数，期望视频的播放方向
//#if UNITY_ANDROID
//            .SetDownloadType(DownloadType.DownloadTypeNoPopup)
//#endif
//            .Build();

//        this.AdNative.LoadRewardVideoAd(
//            adSlot, new RewardVideoAdListener(this));
//    }

//    /// <summary>
//    /// Show the reward Ad.
//    /// </summary>
//    public void ShowExpressRewardAd()
//    {
//        if (this.rewardAd == null)
//        {
//            Debug.LogError("请先加载广告");
//            return;
//        }
//        else
//        {
//            this.rewardAd.ShowRewardVideoAd();
//        }
//    }

//    private sealed class RewardVideoAdListener : IRewardVideoAdListener
//    {
//        private PangleAd pangleAd;

//        public RewardVideoAdListener(PangleAd pangleAd)
//        {
//            this.pangleAd = pangleAd;
//        }

//        public void OnError(int code, string message)
//        {
//            Debug.LogError("OnRewardError: " + message);
//        }

//        public void OnRewardVideoAdLoad(RewardVideoAd ad)
//        {
//#if UNITY_ANDROID
//            var info = ad.GetMediaExtraInfo();
//            Debug.Log($"OnRewardVideoAdLoad info:  expireTimestamp={info["expireTimestamp"]},materialMetaIsFromPreload={info["materialMetaIsFromPreload"]}");
//#endif
//            Debug.Log("OnRewardVideoAdLoad");

//            ad.SetRewardAdInteractionListener(
//                new RewardAdInteractionListener(this.pangleAd));
//            ad.SetAgainRewardAdInteractionListener(
//                    new RewardAgainAdInteractionListener(this.pangleAd));
//            ad.SetDownloadListener(
//                new AppDownloadListener(this.pangleAd));

//            this.pangleAd.rewardAd = ad;
//        }

//        public void OnExpressRewardVideoAdLoad(ExpressRewardVideoAd ad)
//        {
//            Debug.Log("OnExpressRewardVideoAdLoad");
//        }

//        public void OnRewardVideoCached()
//        {
//            Debug.Log("OnRewardVideoCached");
//            PangleAd.Instance.AdvertisementCache();
//        }

//        public void OnRewardVideoCached(RewardVideoAd ad)
//        {
//            Debug.Log("OnRewardVideoCached RewardVideoAd ad");
//            PangleAd.Instance.AdvertisementCache();
//        }
//    }
//    private sealed class ExpressRewardVideoAdListener : IRewardVideoAdListener
//    {
//        private PangleAd pangleAd;

//        public ExpressRewardVideoAdListener(PangleAd pangleAd)
//        {
//            this.pangleAd = pangleAd;
//        }

//        public void OnError(int code, string message)
//        {
//            Debug.LogError("OnRewardError: " + message);
//        }

//        public void OnRewardVideoAdLoad(RewardVideoAd ad)
//        {
//            Debug.Log("OnRewardVideoAdLoad");

//            ad.SetRewardAdInteractionListener(
//                new RewardAdInteractionListener(this.pangleAd));
//            ad.SetDownloadListener(
//                new AppDownloadListener(this.pangleAd));

//            this.pangleAd.rewardAd = ad;
//        }

//        // iOS
//        public void OnExpressRewardVideoAdLoad(ExpressRewardVideoAd ad)
//        {
//        }

//        public void OnRewardVideoCached()
//        {
//            Debug.Log("OnExpressRewardVideoCached");
//        }

//        public void OnRewardVideoCached(RewardVideoAd ad)
//        {
//            Debug.Log("OnRewardVideoCached RewardVideoAd ad");
//        }
//    }
//    private sealed class RewardAdInteractionListener : IRewardAdInteractionListener
//    {
//        private PangleAd pangleAd;

//        public RewardAdInteractionListener(PangleAd pangleAd)
//        {
//            this.pangleAd = pangleAd;
//        }

//        public void OnAdShow()
//        {
//            Debug.Log("rewardVideoAd show");
//        }

//        public void OnAdVideoBarClick()
//        {
//            Debug.Log("rewardVideoAd bar click");
//        }

//        public void OnAdClose()
//        {
//            Debug.Log("rewardVideoAd close");
//            if (this.pangleAd.rewardAd != null)
//            {
//                this.pangleAd.rewardAd.Dispose();
//                this.pangleAd.rewardAd = null;
//            }
//            PangleAd.Instance.LoadExpressRewardAd();
//        }

//        public void OnVideoSkip()
//        {
//            Debug.Log("rewardVideoAd skip");
//            Globals.Ins.Inventory.Add<AdSkipReward>(1);
//        }

//        public void OnVideoComplete()
//        {
//            Debug.Log("rewardVideoAd complete");
//            Globals.Ins.Inventory.Add<AdSuccessReward>(1);

//            Globals.Ins.Values.GetOrCreate<AdCoolDown>().Val--;

//            switch (Television.Ad) {
//                case AdSelection.Berry1:
//                    MapView.Ins.TheOnlyActiveMap.Inventory.TryAdd<Berry>(1);
//                    break;
//                case AdSelection.Berry100:
//                    MapView.Ins.TheOnlyActiveMap.Inventory.TryAdd<Berry>(100);
//                    break;
//                case AdSelection.Grain100:
//                    MapView.Ins.TheOnlyActiveMap.Inventory.TryAdd<Grain>(100);
//                    break;
//                case AdSelection.WoodPlank100:
//                    MapView.Ins.TheOnlyActiveMap.Inventory.TryAdd<WoodPlank>(100);
//                    break;
//                case AdSelection.BuildingPrefabrication100:
//                    MapView.Ins.TheOnlyActiveMap.Inventory.TryAdd<BuildingPrefabrication>(100);
//                    break;

//                case AdSelection.Berry1000:
//                    MapView.Ins.TheOnlyActiveMap.Inventory.TryAdd<Berry>(1000);
//                    Globals.Ins.Values.Get<AdChance_Berry1000>().Max--;
//                    break;
//                case AdSelection.Grain1000:
//                    MapView.Ins.TheOnlyActiveMap.Inventory.TryAdd<Grain>(1000);
//                    Globals.Ins.Values.Get<AdChance_Grain1000>().Max--;
//                    break;
//                case AdSelection.WoodPlant1000:
//                    MapView.Ins.TheOnlyActiveMap.Inventory.TryAdd<WoodPlank>(1000);
//                    Globals.Ins.Values.Get<AdChance_WoodPlank1000>().Max--;
//                    break;
//                case AdSelection.BuildingPrefabrication1000:
//                    MapView.Ins.TheOnlyActiveMap.Inventory.TryAdd<BuildingPrefabrication>(1000);
//                    Globals.Ins.Values.Get<AdChance_BuildingPrefabrication1000>().Max--;
//                    break;


//                case AdSelection.SanityMax:
//                    Globals.Ins.Values.Get<Sanity>().Max = 1000;
//                    Globals.Ins.Values.Get<Sanity>().Val = 1000;
//                    break;

//                case AdSelection.Television:
//                    Globals.Ins.Inventory.TryAdd<AdTelevision>(1);
//                    break;
//                case AdSelection.PlanetLander:
//                    Globals.Ins.Inventory.TryAdd<AdTelevision>(1);
//                    break;

//                case AdSelection.Electricity10:
//                    MapView.Ins.TheOnlyActiveMap.InventoryOfSupply.TryAdd<Electricity>(10);
//                    break;
//                case AdSelection.Population3:
//                    MapView.Ins.TheOnlyActiveMap.InventoryOfSupply.TryAdd<Worker>(3);
//                    break;

//                default:
//                    MapView.Ins.TheOnlyActiveMap.InventoryOfSupply.TryAdd<Worker>(10);
//                    break;
//            }
//            UI.Ins.Active = false;
//        }

//        public void OnVideoError()
//        {
//            Debug.LogError("rewardVideoAd error");
//        }

//        public void OnRewardVerify(
//            bool rewardVerify, int rewardAmount, string rewardName)
//        {
//            Debug.Log("rewardVideoAd verify:" + rewardVerify + " amount:" + rewardAmount +
//                      " name:" + rewardName + " 缓存一条新的广告");
//            IValue val = Globals.Ins.Values.GetOrCreate<ViewAdPoint>();
//            val.Max += 1;
//            val.Val += 1;
//        }
//    }
//    private sealed class RewardAgainAdInteractionListener : IRewardAdInteractionListener
//    {
//        private PangleAd pangleAd;

//        public RewardAgainAdInteractionListener(PangleAd pangleAd)
//        {
//            this.pangleAd = pangleAd;
//        }

//        public void OnAdShow()
//        {
//            Debug.Log("again rewardVideoAd show");
//        }

//        public void OnAdVideoBarClick()
//        {
//            Debug.Log("again rewardVideoAd bar click");
//        }

//        public void OnAdClose()
//        {
//            Debug.Log("again rewardVideoAd close");
//            if (this.pangleAd.rewardAd != null)
//            {
//                this.pangleAd.rewardAd.Dispose();
//                this.pangleAd.rewardAd = null;
//            }
//            PangleAd.Instance.LoadExpressRewardAd();
//        }

//        public void OnVideoSkip()
//        {
//            Debug.Log("again rewardVideoAd skip");
//        }


//        public void OnVideoComplete()
//        {
//            Debug.Log("again rewardVideoAd complete");
//        }

//        public void OnVideoError()
//        {
//            Debug.LogError("again rewardVideoAd error");
//        }

//        public void OnRewardVerify(
//            bool rewardVerify, int rewardAmount, string rewardName)
//        {
//            Debug.Log("again rewardVideoAd verify:" + rewardVerify + " amount:" + rewardAmount +
//                      " name:" + rewardName+" 缓存一条新的广告");
//            IValue val = Globals.Ins.Values.GetOrCreate<ViewAdPoint>();
//            val.Max += 1;
//            val.Val += 1;
//        }
//    }
//    private sealed class AppDownloadListener : IAppDownloadListener
//    {
//        private PangleAd pangleAd;

//        public AppDownloadListener(PangleAd pangleAd)
//        {
//            this.pangleAd = pangleAd;
//        }

//        public void OnIdle()
//        {
//        }

//        public void OnDownloadActive(
//            long totalBytes, long currBytes, string fileName, string appName)
//        {
//            Debug.Log("下载中，点击下载区域暂停");
//        }

//        public void OnDownloadPaused(
//            long totalBytes, long currBytes, string fileName, string appName)
//        {
//            Debug.Log("下载暂停，点击下载区域继续");
//        }

//        public void OnDownloadFailed(
//            long totalBytes, long currBytes, string fileName, string appName)
//        {
//            Debug.Log("下载失败，点击下载区域重新下载");
//        }

//        public void OnDownloadFinished(
//            long totalBytes, string fileName, string appName)
//        {
//            Debug.Log("下载完成");
//        }

//        public void OnInstalled(string fileName, string appName)
//        {
//            Debug.Log("安装完成，点击下载区域打开");
//        }
//    }

//}
