﻿
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Weathering
{
    public interface IGlobals
    {
        IValues Values { get; }
        IRefs Refs { get; }
        IInventory Inventory { get; }
        Dictionary<string, string> PlayerPreferences { get; }

        string String(string key);
        void String(string key, string val);

        string String<T>();
        void String<T>(string val);
        string String(Type type);
        void String(Type type, string val);

        bool Bool<T>();
        void Bool<T>(bool val);
        bool Bool(Type type);
        void Bool(Type type, bool val);
    }

    public interface IGlobalsDefinition : IGlobals
    {
        IValues ValuesInternal { set; }
        IRefs RefsInternal { set; }
        Dictionary<string, string> PlayerPreferencesInternal { set; }
        IInventory InventoryInternal { get; set; }
    }

    public class Globals : MonoBehaviour, IGlobalsDefinition
    {
        private void Awake() {
            if (Ins != null) throw new Exception();
            Ins = this;
        }

        //public string GetPreference(string pref) {
        //    PlayerPreferences.TryGetValue(pref, out string value);
        //    return value;
        //}

        //public void SetPreference(string pref, string content) {
        //    if (content == null) {
        //        if (PlayerPreferences.ContainsKey(pref)) {
        //            PlayerPreferences.Remove(pref);
        //        }
        //    }
        //    else {
        //        if (PlayerPreferences.ContainsKey(pref)) {
        //            PlayerPreferences[pref] = content;
        //        } else {
        //            PlayerPreferences.Add(pref, content);
        //        }
        //    }
        //}
        public static bool Unlocked<T>() {
            return Unlocked(typeof(T));
        }
        public static bool Unlocked(Type type) {
            if (GameConfig.CheatMode) return true;
            return Ins.Bool(type);
        }
        public static void Unlock<T>() {
            Unlock(typeof(T));
        }
        public static void Unlock(Type type) {
            Ins.Bool(type, true);
        }

        public bool Bool<T>() {
            return Bool(typeof(T));
        }
        public bool Bool(Type type) {
            return PlayerPreferences.ContainsKey(type.FullName);
        }

        public void Bool<T>(bool val) {
            Bool(typeof(T), val);
        }
        public void Bool(Type type, bool val) {
            if (val) {
                if (!PlayerPreferences.ContainsKey(type.FullName)) {
                    PlayerPreferences.Add(type.FullName, null);
                }
            } else {
                if (PlayerPreferences.ContainsKey(type.FullName)) {
                    PlayerPreferences.Remove(type.FullName);
                }
            }
        }


        public string String<T>() => String(typeof(T));
        public void String<T>(string val) => String(typeof(T), val);
        public string String(Type type) {
            string result;
            PlayerPreferences.TryGetValue(type.FullName, out result);
            return result;
        }

        public void String(Type type, string val) {
            if (val != null) {
                if (!PlayerPreferences.ContainsKey(type.FullName)) {
                    PlayerPreferences.Add(type.FullName, val);
                } else {
                    PlayerPreferences[type.FullName] = val;
                }
            } else {
                if (PlayerPreferences.ContainsKey(type.FullName)) {
                    PlayerPreferences.Remove(type.FullName);
                }
            };
        }

        public string String(string key) {
            string result;
            PlayerPreferences.TryGetValue(key, out result);
            return result;
        }

        public void String(string key, string val) {
            if (val != null) {
                if (!PlayerPreferences.ContainsKey(key)) {
                    PlayerPreferences.Add(key, val);
                } else {
                    PlayerPreferences[key] = val;
                }
            } else {
                if (PlayerPreferences.ContainsKey(key)) {
                    PlayerPreferences.Remove(key);
                }
            };
        }

        public static IGlobals Ins;

        private static IValue sanity;
        public static IValue Sanity {
            get {
                if (sanity == null) sanity = Ins.Values.Get<Sanity>();
                return sanity;
            }
        }
        public static bool SanityCheck(long cost = 1) {
            if (sanity == null) sanity = Ins.Values.Get<Sanity>();
            if (sanity.Val < cost) {
                string notice = $"{Localization.Ins.ValUnit(typeof(Sanity))}不足";
                if (UI.Ins.Active) {
                    UI.Ins.ShowItems(notice, UIItem.CreateSeparator());
                } else {
                    GameMenu.Ins.PushNotification(notice);
                }
                return false;
            }
            sanity.Val -= cost;
            return true;
        }


        private static IValue adCoolDown;
        public static IValue AdCoolDown {
            get {
                if (adCoolDown == null) adCoolDown = Ins.Values.Get<CoolDown>();
                return adCoolDown;
            }
        }


        private static IValue cooldown;
        public static IValue CoolDown {
            get {
                if (cooldown == null) cooldown = Ins.Values.Get<CoolDown>();
                return cooldown;
            }
        }
        public static bool IsCool {
            get {
                if (cooldown == null) cooldown = Ins.Values.Get<CoolDown>();
                return cooldown.Maxed;
            }
        }
        public static long SetCooldown {
            set {
                if (cooldown == null) cooldown = Ins.Values.Get<CoolDown>();
                cooldown.Del = value * Value.Second;
                cooldown.Val = 0;
            }
        }

        public IValues ValuesInternal { get; set; }
        // public void SetValues(IValues values) => ValuesInternal = values;
        public IRefs RefsInternal { get; set; }
        // public void SetRefs(IRefs refs) => RefsInternal = refs;
        public Dictionary<string, string> PlayerPreferencesInternal { get; set; }


        public IValues Values => ValuesInternal;
        public IRefs Refs => RefsInternal;

        // 用于储存字符串
        public Dictionary<string, string> PlayerPreferences { get => PlayerPreferencesInternal; }

        public IInventory InventoryInternal { get; set; }
        public IInventory Inventory => InventoryInternal;
    }
}

