﻿
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Weathering
{


    public class DependAttribute : Attribute
    {
        public Type[] Types { get; private set; }
        public HashSet<Type> Set { get; private set; }
        public DependAttribute(params Type[] types) {
            Types = types;
            Set = new HashSet<Type>(types);
        }
    }

    /// <summary>
    /// Localization.Ins.Get<T> 中的 T 一般必须有ConceptAttribute, 
    /// </summary>
    public class ConceptAttribute : Attribute
    {
    }

    public class AttributesPreprocessor : MonoBehaviour, IComparer<Type>
    {
        [ContextMenu("生成本地化文件")]
        private void GenerateLocalizationFile() {
            // editor only script. 只在unity编辑器下使用
            Dictionary<string, string> result = new Dictionary<string, string>();
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes()) {
                Attribute[] attributes = Attribute.GetCustomAttributes(type);
                foreach (var attribtue in attributes) {
                    if (attribtue is ConceptAttribute) {
                        result.Add(type.FullName, "");
                        break;
                    }
                }
            }
            string fullPath = $"{Application.streamingAssetsPath}/localization.template.json";
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(fullPath, json);
            Debug.LogWarning("OK");
        }

        //public readonly HashSet<Type> RelavantAttributes = new HashSet<Type> {
        //    typeof(DependAttribute),
        //    // typeof(ConceptAttribute),
        //};

        //public readonly Dictionary<Type, Dictionary<Attribute, object>> Data
        //    = new Dictionary<Type, Dictionary<Attribute, object>>();
        private readonly Dictionary<Type, DependAttribute> DependAttribute = new Dictionary<Type, DependAttribute>(); // 缓存
        public readonly List<Type> DependAttributeList = new List<Type>(); // 排序表
        public readonly Dictionary<Type, int> IndexDict = new Dictionary<Type, int>(); // 排序表, 逆序
        public readonly Dictionary<Type, HashSet<Type>> FinalResult = new Dictionary<Type, HashSet<Type>>(); // 一个类Depend的所有其他类
        public readonly Dictionary<Type, List<Type>> FinalResultSorted = new Dictionary<Type, List<Type>>(); // 并且按照依赖关系排序

        public readonly Dictionary<Type, HashSet<Type>> FinalResultInversed = new Dictionary<Type, HashSet<Type>>(); // FinalResult的逆映射
        public readonly Dictionary<Type, List<Type>> FinalResultInversedSorted = new Dictionary<Type, List<Type>>(); // FinalResult的逆映射

        /// <summary>
        /// DependAttribute 构成的偏序关系就存在这里了
        /// </summary>
        public readonly Dictionary<Type, Dictionary<Type, object>> DependAttributeClosure
            = new Dictionary<Type, Dictionary<Type, object>>();

        //public bool HasAttribute(Type type, Type attr) {
        //    return Attribute.GetCustomAttribute(type, attr) == null;
        //}

        public static AttributesPreprocessor Ins { get; private set; }
        private void Awake() {
            if (Ins != null) {
                throw new Exception();
            }
            Ins = this;

            Assembly assembly = Assembly.GetExecutingAssembly();

            // 下面这段代码暂时用不到, 先注释掉了

            //// RelaventAttribtues里配置的类型, 必须是Attribute的子类
            //foreach (var type in RelavantAttributes) {
            //    if (!typeof(Attribute).IsAssignableFrom(type)) {
            //        throw new Exception(type.FullName);
            //    }
            //}
            //// 记录所有自定义的 attribute
            //// 结果存在 Dictionary<Type, Dictionary<Attribute, null>>
            //foreach (var type in assembly.GetTypes()) {
            //    bool dictCreated = false;
            //    Attribute[] attributes = Attribute.GetCustomAttributes(type);
            //    foreach (var attribute in attributes) {
            //        if (RelavantAttributes.Contains(attribute.GetType())) {
            //            Dictionary<Attribute, object> set;
            //            if (!dictCreated) {
            //                dictCreated = true;
            //                set = new Dictionary<Attribute, object>();
            //                Data.Add(type, set);
            //            } else {
            //                set = Data[type];
            //            }
            //            set.Add(attribute, null);
            //        }
            //    }
            //}

            // 因为上面的代码被注释掉了, 所以用下面这个针对DependAttribute的代码
            // 查找所有 DependAttribute
            // 记录所有自定义的 attribute
            // 结果存在 Dictionary<Type, Dictionary<Attribute, null>>
            foreach (var type in assembly.GetTypes()) {
                Attribute[] attributes = Attribute.GetCustomAttributes(type);
                foreach (var attribute in attributes) {
                    if (attribute is DependAttribute) {
                        DependAttribute.Add(type, attribute as DependAttribute);
                        DependAttributeList.Add(type);
                    }
                }
            }

            // 所有被依赖的都应该也有DependAttribute
            foreach (var pair in DependAttribute) {
                foreach (var dependee in DependAttribute[pair.Key].Set) {
                    if (!DependAttribute.ContainsKey(dependee)) {
                        throw new Exception($"{pair.Key} depends on {dependee}");
                    }
                }
            }

            // 这里效率太低, 需要优化。建议预编译
            for (int k = 0; k < DependAttributeList.Count; k++) {
                bool changed = false;
                for (int i = 0; i < DependAttributeList.Count; i++) {
                    for (int j = i; j < DependAttributeList.Count; j++) {
                        // if a[i] > a[j] 即 a[i] depend a[j]
                        if (Depend(DependAttributeList[i], DependAttributeList[j])) {
                            changed = true;
                            var t = DependAttributeList[i];
                            DependAttributeList[i] = DependAttributeList[j];
                            DependAttributeList[j] = t;
                        }
                    }
                }
                if (!changed) { break; }
            }

            // 检验循环依赖。检验过后, 不可能出现A依赖B,B依赖C,C又依赖A这样的情况。
            for (int i = 0; i < DependAttributeList.Count; i++) {
                for (int j = i; j < DependAttributeList.Count; j++) {
                    // if a[i] > a[j] 即 a[i] depend a[j]
                    if (Depend(DependAttributeList[i], DependAttributeList[j])) {
                        for (int k=0; k< DependAttributeList.Count; k++) {
                            Debug.LogWarning($"{k} {DependAttributeList[k].Name}");
                        }
                        throw new Exception($"{DependAttributeList[i]} 循环依赖 {DependAttributeList[j]}");
                    }
                }
            }

            // 记录下标, 缓存
            for (int i = 0; i < DependAttributeList.Count; i++) {
                IndexDict.Add(DependAttributeList[i], i);
            }

            // 对于每个元素
            for (int i = 0; i < DependAttributeList.Count; i++) {
                Type subclass = DependAttributeList[i];
                var set = new HashSet<Type>();
                FinalResult.Add(subclass, set);
                // 的依赖元素（在数组左边
                DependAttribute attribute = DependAttribute[subclass];
                foreach (var superclass in attribute.Set) {
                    if (!set.Contains(superclass)) {
                        set.Add(superclass);
                        var superclasses = FinalResult[superclass];
                        foreach (var superclassOfSuperclass in superclasses) {
                            if (!set.Contains(superclassOfSuperclass)) {
                                set.Add(superclassOfSuperclass);
                            }
                        }
                    }
                }
            }

            // 制造FinalResultSorted
            foreach (var pair in FinalResult) {
                Type type = pair.Key;
                var list = new List<Type>(pair.Value);
                list.Sort(this); // 用到了IComparer<Type>
                FinalResultSorted.Add(type, list);
            }

            // 制造制造FinalResultInversed
            foreach (var pair in FinalResult) {
                FinalResultInversed.Add(pair.Key, new HashSet<Type>());
            }
            foreach (var pair in FinalResult) {
                Type subclass = pair.Key;
                foreach (var superclass in pair.Value) {
                    FinalResultInversed[superclass].Add(subclass);
                }
            }

            // 制造制造FinalResultInversedSorted
            foreach (var pair in FinalResultInversed) {
                Type type = pair.Key;
                var list = new List<Type>(pair.Value);
                list.Sort(this);
                FinalResultInversedSorted.Add(type, list);
            }

            // // 用于测试是否成功
            //foreach (var pair in FinalResult) {
            //    Debug.LogWarning(pair.Key.Name);
            //    foreach (var type in pair.Value) {
            //        Debug.Log(type.Name);
            //    }
            //}
        }

        private bool Depend(Type type1, Type type2) {
            return DependAttribute[type1].Set.Contains(type2);
        }

        public int Compare(Type x, Type y) {
            return IndexDict[x] > IndexDict[y] ? 1 : -1;
        }
    }
}

