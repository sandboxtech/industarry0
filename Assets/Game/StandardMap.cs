﻿

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Weathering
{
    public class CameraX { }
    public class CameraY { }

    public class CharacterX { }
    public class CharacterY { }

    //public class ClearColorR { }
    //public class ClearColorG { }
    //public class ClearColorB { }


    public abstract class StandardMap : IMapDefinition
    {

        public virtual bool ControlCharacter => false;


        public abstract Type DefaultTileType { get; }

        public uint HashCode { get; set; }

        public abstract int Width { get; }

        public abstract int Height { get; }

        public abstract ITile ParentTile { get; }

        public abstract void EnterParentMap();
        public abstract void EnterChildMap(Vector2Int pos);

        public virtual string GetSpriteKeyBedrock(Vector2Int pos) => null;
        public virtual string GetSpriteKeyGrass(Vector2Int pos) => null;
        public virtual string GetSpriteKeyWater(Vector2Int pos) => null;
        public virtual string GetSpriteKeyTree(Vector2Int pos) => null;
        public virtual string GetSpriteKeyHill(Vector2Int pos) => null;






        public virtual void OnConstruct() {
            if (Values == null) {
                Tiles = new ITileDefinition[Width, Height];
                Values = Weathering.Values.GetOne();
            } else {
                throw new Exception();
            }
            if (Refs == null) {
                Refs = Weathering.Refs.GetOne();
            } else {
                throw new Exception();
            }
            if (Inventory == null) {
                Inventory = Weathering.Inventory.GetOne();
            } else {
                throw new Exception();
            }
            if (InventoryOfSupply == null) {
                InventoryOfSupply = Weathering.Inventory.GetOne();
            } else {
                throw new Exception();
            }

            Values.Create<CharacterX>();
            Values.Create<CharacterY>();
            Values.Create<CameraX>();
            Values.Create<CameraY>();
            //Values.Create<ClearColorR>();
            //Values.Create<ClearColorG>();
            //Values.Create<ClearColorB>();
        }

        protected void SetCharacterPos(Vector2Int characterPosition, bool sync = false) {
            Values.Get<CharacterX>().Max = characterPosition.x;
            Values.Get<CharacterY>().Max = characterPosition.y;
            MapView.Ins.CharacterPosition = characterPosition;
            if (sync) {
                MapView.Ins.SyncCharacterPosition();
            }
        }

        protected void SetCameraPos(Vector2 cameraPos) {
            Values.Get<CameraX>().Max = (long)(cameraPos.x * factor);
            Values.Get<CameraY>().Max = (long)(cameraPos.y * factor);
            MapView.Ins.CameraPosition = cameraPos;
        }

        protected void SetClearColor(Color clearColor) {
            //Values.Get<ClearColorR>().Max = (long)(clearColor.r * factor);
            //Values.Get<ClearColorG>().Max = (long)(clearColor.g * factor);
            //Values.Get<ClearColorB>().Max = (long)(clearColor.b * factor);
            MapView.Ins.ClearColor = clearColor;
        }

        public virtual void OnEnable() {
            if (MapView.Ins.TheOnlyActiveMap == this) {
                // 非active map 的地图类型加载有问题。bug
                autoInc = RandomSeed;
                altitudeConfig = GetAltitudeConfig;
                moistureConfig = GetMoistureConfig;
                temporatureConfig = GetTemporatureConfig;
                GenerateNoise();

                Vector2 cameraPos = Vector2.zero;
                cameraPos.x = Values.Get<CameraX>().Max / factor;
                cameraPos.y = Values.Get<CameraY>().Max / factor;
                MapView.Ins.CameraPosition = cameraPos;

                //Color color = Color.black;
                //color.r = Values.Get<ClearColorR>().Max / factor;
                //color.g = Values.Get<ClearColorG>().Max / factor;
                //color.b = Values.Get<ClearColorB>().Max / factor;
                //MapView.Ins.ClearColor = color;

                MapView.Ins.CharacterPosition = new Vector2Int((int)Values.Get<CharacterX>().Max, (int)Values.Get<CharacterY>().Max);
            }
        }
        private const float factor = 1024f;
        public void OnDisable() {
            if (MapView.Ins.TheOnlyActiveMap == this) {
                SetCharacterPos(MapView.Ins.CharacterPosition);
                SetCameraPos(MapView.Ins.CameraPosition);
                SetClearColor(MapView.Ins.ClearColor);
            }
        }

        public IValues Values { get; protected set; }
        public void SetValues(IValues values) => Values = values;
        public IRefs Refs { get; set; }

        public void SetRefs(IRefs refs) => Refs = refs;
        public IInventory Inventory { get; protected set; }
        public void SetInventory(IInventory inventory) => Inventory = inventory;

        public IInventory InventoryOfSupply { get; protected set; }
        public void SetInventoryOfSupply(IInventory inventory) => InventoryOfSupply = inventory;

        // ------------------------------------------------------------


        public bool CanUpdateAt<T>(Vector2Int pos) => CanUpdateAt(typeof(T), pos.x, pos.y);
        public bool CanUpdateAt(Type type, Vector2Int pos) => CanUpdateAt(type, pos.x, pos.y);
        public bool CanUpdateAt<T>(int i, int j) => CanUpdateAt(typeof(T), i, j);
        public abstract bool CanUpdateAt(Type type, int i, int j);


        protected ITileDefinition[,] Tiles;

        public ITile Get(int i, int j) {
            Validate(ref i, ref j);
            ITile result = Tiles[i, j];
            return result;
        }

        public ITile Get(Vector2Int pos) {
            return Get(pos.x, pos.y);
        }

        public T UpdateAt<T>(ITile oldTile) where T : class, ITile {
            Vector2Int pos = oldTile.GetPos();
            return UpdateAt(typeof(T), pos.x, pos.y, oldTile) as T;
        }
        public ITile UpdateAt(Type type, ITile oldTile) {
            Vector2Int pos = oldTile.GetPos();
            return UpdateAt(type, pos.x, pos.y, oldTile);
        }
        //private T UpdateAt<T>(int i, int j) where T : class, ITile {
        //    return UpdateAt(typeof(T), i, j) as T;
        //}
        private ITile UpdateAt(Type newType, int i, int j, ITile oldTile) {
            Validate(ref i, ref j);

            ITileDefinition oldTileDefinition = oldTile as ITileDefinition;
            if (oldTileDefinition == null) throw new Exception();
            Type oldType = oldTile.GetType();

            if (!GameConfig.CheatMode) {
                // 居然在这里消耗资源, 架构不好

                // 拆除时返还资源
                CostInfo desctructOldCost = ConstructionCostBaseAttribute.GetCost(oldType, this, false);

                // 建筑时消耗资源
                CostInfo constructNewCost = ConstructionCostBaseAttribute.GetCost(newType, this, true);

                if (desctructOldCost.CostType != null) {
                    if (constructNewCost.CostType != null) throw new Exception(); // ???

                    if (desctructOldCost.UseResetPoint) {

                    } else {
                        // 背包空间查询
                        if (!Inventory.CanAdd((desctructOldCost.CostType, desctructOldCost.RealCostQuantity))) {
                            UI.Ins.ShowItems("背包空间不足", UIItem.CreateMultilineText($"{Localization.Ins.Val(desctructOldCost.CostType, desctructOldCost.RealCostQuantity)} 被拆建筑资源无法返还"));
                            return null;
                        }
                    }
                }
                if (constructNewCost.CostType != null) {
                    bool failed = false;
                    if (constructNewCost.UseResetPoint) {
                        if (Globals.Ins.Values.GetOrCreate(constructNewCost.CostType).Val < constructNewCost.RealCostQuantity) {
                            failed = true;
                        }
                    } else {
                        if (!Inventory.CanRemoveWithTag((constructNewCost.CostType, constructNewCost.RealCostQuantity))) {
                            failed = true;
                        }
                    }
                    if (failed) {
                        string costString = Localization.Ins.Val(constructNewCost.CostType, constructNewCost.RealCostQuantity);
                        if (UI.Ins.Active) {
                            var items = UI.Ins.GetItems();
                            items.Add(UIItem.CreateMultilineText($"无法建造{Localization.Ins.Get(newType)}"));
                            items.Add(UIItem.CreateButton("关闭", () => UI.Ins.Active = false));

                            items.Add(UIItem.CreateMultilineText($"需要{costString}"));

                            items.Add(UIItem.CreateSeparator());

                            UIItem.AddItemDescription(items, constructNewCost.CostType);

                            UI.Ins.ShowItems($"建筑资源不足", items);
                        } else {
                            GameMenu.Ins.PushNotification($"建筑资源{costString}不足");
                        }
                        return null;
                    }
                }
                if (constructNewCost.CostType != null) {
                    if (constructNewCost.UseResetPoint) {
                        Globals.Ins.Values.GetOrCreate(constructNewCost.CostType).Val -= constructNewCost.RealCostQuantity;
                    } else {
                        Inventory.RemoveWithTag(constructNewCost.CostType, constructNewCost.RealCostQuantity);
                    }
                    GameMenu.Ins.PushNotification($"建造{Localization.Ins.Get(newType)}, 花费{Localization.Ins.Val(constructNewCost.CostType, constructNewCost.RealCostQuantity)}");
                }
                if (desctructOldCost.CostType != null) {
                    if (desctructOldCost.UseResetPoint) {
                        Globals.Ins.Values.GetOrCreate(desctructOldCost.CostType).Val += desctructOldCost.RealCostQuantity;
                    } else {
                        Inventory.Add(desctructOldCost.CostType, desctructOldCost.RealCostQuantity);
                    }
                    GameMenu.Ins.PushNotification($"拆除{Localization.Ins.Get(oldType)}, 获得{Localization.Ins.Val(desctructOldCost.CostType, desctructOldCost.RealCostQuantity)}");
                }
            }


            // 通过建造验证

            ITileDefinition tile = (Activator.CreateInstance(newType) as ITileDefinition);
            if (tile == null) throw new Exception();

            Vector2Int pos = new Vector2Int(i, j);
            tile.Map = this;
            tile.Pos = pos;
            tile.TileHashCode = HashUtility.Hash(i, j, Width, Height, (int)HashCode);

            // Tiles[i, j] = tile;
            SetTile(pos, tile, true);

            LinkUtility.NeedUpdateNeighbors8(tile);

            oldTileDefinition.OnDestruct(tile); // new tile
            tile.OnConstruct(oldTile); // old tile

            tile.OnEnable();
            return tile;
        }

        public Vector2Int Validate(Vector2Int pos) {
            pos.x %= Width;
            if (pos.x < 0) pos.x += Width;
            pos.y %= Height;
            if (pos.y < 0) pos.y += Height;
            return pos;
        }
        private void Validate(ref int i, ref int j) {
            i %= Width;
            if (i < 0) i += Width;
            j %= Height;
            if (j < 0) j += Height;
        }

        // modify
        public void SetTile(Vector2Int pos, ITileDefinition tile, bool inConstruction = false) {
            if (Tiles == null) {
                Tiles = new ITileDefinition[Width, Height];
            }

            // 建筑计数
            if (inConstruction) {

                ITile oldTile = Tiles[pos.x, pos.y];
                if (oldTile != null) {
                    Refs.Get(oldTile.GetType()).Value--;
                    // Debug.LogWarning($"{oldTile.GetType().Name}--");
                }
                Refs.GetOrCreate(tile.GetType()).Value++;
                // Debug.LogWarning($"{tile.GetType().Name}++");


                if (oldTile != null) {
                    var oldInfluence = Tag.GetAttribute<PlanetInfluenceAttribute>(oldTile.GetType());
                    if (oldInfluence != null) {
                        Refs.Get(oldInfluence.Type).Value -= oldInfluence.Quantity;
                    } else {
                        long quantity = TemporaryInfluence(oldTile.GetType());
                        if (quantity != 0) Refs.GetOrCreate(typeof(Polution)).Value -= quantity;
                    }
                }

                var newInfluence = Tag.GetAttribute<PlanetInfluenceAttribute>(tile.GetType());
                if (newInfluence != null) {
                    Refs.Get(newInfluence.Type).Value += newInfluence.Quantity;
                } else {
                    long quantity = TemporaryInfluence(tile.GetType());
                    if (quantity != 0) Refs.GetOrCreate(typeof(Polution)).Value += quantity;
                }
            }

            Tiles[pos.x, pos.y] = tile;
        }
        private long TemporaryInfluence(Type type) {
            string name = type.Name;
            if (name.StartsWith("Factory")) {
                return 10;
            } else if (name.StartsWith("Workshop")) {
                return 1;
            } else if (name.StartsWith("Generator")) {
                if (type != typeof(PowerGeneratorOfSolarPanelStation) && type != typeof(PowerGeneratorOfWindTurbineStation)) {
                    return 20;
                }
                else {
                    return 0;
                }
            }
            return 0;
        }

        // public virtual void AfterGeneration() { }


        // ------------------------------------------------------------

        protected virtual AltitudeConfig GetAltitudeConfig { get => AltitudeConfig.Create(); }
        protected virtual MoistureConfig GetMoistureConfig { get => MoistureConfig.Create(); }
        protected virtual TemporatureConfig GetTemporatureConfig { get => TemporatureConfig.Create(); }

        public struct TemporatureConfig
        {
            public static TemporatureConfig Create() {
                var result = new TemporatureConfig();
                result.CanGenearate = false;
                result.BaseNoiseSize = 4;
                result.AltitudeInfluence = 0.8f;
                result.Max = 40;
                result.Min = -30;
                return result;
            }
            public bool CanGenearate;
            public int BaseNoiseSize;
            public float AltitudeInfluence;
            public int Min;
            public int Max;
        }

        public struct MoistureConfig
        {
            public static MoistureConfig Create() {
                var result = new MoistureConfig();
                result.CanGenerate = false;
                result.BaseNoiseSize = 4;
                result.Max = 100;
                result.Min = 0;
                return result;
            }
            public int BaseNoiseSize;
            public bool CanGenerate;
            public int Max;
            public int Min;
        }

        public struct AltitudeConfig
        {
            public static AltitudeConfig Create() {
                var result = new AltitudeConfig();
                result.CanGenerate = false;
                result.BaseNoiseSize = 3;
                result.Min = -10000;
                result.Max = 9800;
                result.EaseFunction = null;
                return result;
            }
            public int BaseNoiseSize;
            public bool CanGenerate;
            public int Min;
            public int Max;
            public Func<float, float> EaseFunction;
        }

        private AltitudeConfig altitudeConfig;
        private MoistureConfig moistureConfig;
        private TemporatureConfig temporatureConfig;


        public int[,] Altitudes { get; private set; }
        public Type[,] AltitudeTypes { get; private set; }
        public int[,] Moistures { get; private set; }
        public Type[,] MoistureTypes { get; private set; }
        public int[,] Temporatures { get; private set; }
        public Type[,] TemporatureTypes { get; private set; }
        public Type[,] ResourceTypes { get; private set; }


        protected virtual int RandomSeed { get; } = 0;
        private int autoInc = 0;
        private int AutoInc { get => autoInc++; }
        private void GenerateNoise() {
            GenerateAltitude();
            GenerateMoisture();
            GenerateTemporature();
            GenerateResources();
        }
        private void GenerateResources() {
            if (altitudeConfig.CanGenerate && moistureConfig.CanGenerate) {
                // TODO
            }
        }

        private void GenerateAltitude() {
            bool debugAltitude = false;
            Texture2D texAltitude = null;
            if (debugAltitude) texAltitude = new Texture2D(Width, Height);
            if (altitudeConfig.CanGenerate) {

                int noise0Size = altitudeConfig.BaseNoiseSize;
                int noise1Size = noise0Size * 2;
                int noise2Size = noise1Size * 2;
                Altitudes = new int[Width, Height];
                AltitudeTypes = new Type[Width, Height];
                int offset0 = AutoInc;
                int offset1 = AutoInc;
                int offset2 = AutoInc;
                for (int i = 0; i < Width; i++) {
                    for (int j = 0; j < Height; j++) {
                        //float noise0 = HashUtility.PerlinNoise((float)noise0Size * i / Width, (float)noise0Size * j / Height, noise0Size, noise0Size, offset0 + HashCode);
                        //float floatResult = (noise0+1)/2;
                        float noise0 = HashUtility.PerlinNoise((float)noise0Size * i / Width, (float)noise0Size * j / Height, noise0Size, noise0Size, offset0 + (int)HashCode);
                        float noise1 = HashUtility.PerlinNoise((float)noise1Size * i / Width, (float)noise1Size * j / Height, noise1Size, noise1Size, offset1 + (int)HashCode);
                        float noise2 = HashUtility.PerlinNoise((float)noise2Size * i / Width, (float)noise2Size * j / Height, noise2Size, noise2Size, offset2 + (int)HashCode);
                        float floatResult = (noise0 * 4 + noise1 * 2 + noise2 * 1 + 7) / 14;
                        if (altitudeConfig.EaseFunction != null) floatResult = altitudeConfig.EaseFunction(floatResult);

                        int altitude = (int)Mathf.Lerp(altitudeConfig.Min, altitudeConfig.Max, floatResult);
                        Altitudes[i, j] = altitude;
                        AltitudeTypes[i, j] = GeographyUtility.GetAltitudeType(altitude);

                        if (debugAltitude) texAltitude.SetPixel(i, j, Color.Lerp(Color.black, Color.white, floatResult));
                    }
                }
            }
            if (debugAltitude) System.IO.File.WriteAllBytes(Application.streamingAssetsPath + "/altitude.png", texAltitude.EncodeToPNG());
        }
        private void GenerateMoisture() {
            bool debugMoisture = false;
            Texture2D texMoisture = null;
            if (debugMoisture) texMoisture = new Texture2D(Width, Height);
            if (moistureConfig.CanGenerate) {
                int size = moistureConfig.BaseNoiseSize;
                Moistures = new int[Width, Height];
                MoistureTypes = new Type[Width, Height];
                int offset = AutoInc;
                for (int i = 0; i < Width; i++) {
                    for (int j = 0; j < Height; j++) {
                        float noise = HashUtility.PerlinNoise((float)size * i / Width, (float)size * j / Height, size, size, offset + (int)HashCode);
                        float floatResult = (noise + 1) / 2;

                        int moisture = (int)Mathf.Lerp(moistureConfig.Min, moistureConfig.Max, floatResult); ;
                        Moistures[i, j] = moisture;
                        MoistureTypes[i, j] = GeographyUtility.GetMoistureType(moisture);

                        if (debugMoisture) texMoisture.SetPixel(i, j, Color.Lerp(Color.black, Color.white, floatResult));
                    }
                }
            }
            if (debugMoisture) System.IO.File.WriteAllBytes(Application.streamingAssetsPath + "/moisture.png", texMoisture.EncodeToPNG());
        }
        private void GenerateTemporature() {
            bool debugTemporature = false;
            Texture2D texTemporature = null;
            if (debugTemporature) texTemporature = new Texture2D(Width, Height);
            if (temporatureConfig.CanGenearate) {
                if (!altitudeConfig.CanGenerate) throw new Exception();
                int size = temporatureConfig.BaseNoiseSize;
                Temporatures = new int[Width, Height];
                TemporatureTypes = new Type[Width, Height];
                int offset = AutoInc;
                for (int i = 0; i < Width; i++) {
                    for (int j = 0; j < Height; j++) {
                        float noise = HashUtility.PerlinNoise((float)size * i / Width, (float)size * j / Height, size, size, offset + (int)HashCode);
                        noise = (noise + 1) / 2;
                        float latitude = Mathf.Sin(Mathf.PI * j / Width);
                        float floatResult = Mathf.Lerp(noise, latitude, temporatureConfig.AltitudeInfluence);

                        // 海拔升高, 温度降低
                        int altitude = Altitudes[i, j];
                        if (altitude > 0 && altitudeConfig.Max > 0) {
                            float t = 0.02f * altitude / altitudeConfig.Max;
                            floatResult = Mathf.Lerp(floatResult, temporatureConfig.Min, t);
                        }

                        int temporature = temporatureConfig.Min + (int)(floatResult * (temporatureConfig.Max - temporatureConfig.Min));
                        Temporatures[i, j] = temporature;
                        TemporatureTypes[i, j] = GeographyUtility.GetTemporatureType(temporature);

                        if (debugTemporature) texTemporature.SetPixel(i, j, Color.Lerp(Color.black, Color.white, floatResult));
                    }
                }
            }
            if (debugTemporature) System.IO.File.WriteAllBytes(Application.streamingAssetsPath + "/temporature.png", texTemporature.EncodeToPNG());
        }

        // OnTapTile 生效前, 是否需要降落

        public string MapKey { get; set; }

        public string GetMapKey => MapKey;

        public virtual bool CanReset => false;

        public Vector2Int ParentPositionBuffer { get; set; } = new Vector2Int(-1, -1);

        public virtual void OnTapTile(ITile tile) {
            tile.OnTap();
        }

        public virtual void Reset() { }

        public ITileDefinition GetTileFast(int i, int j) {
            return Tiles[i, j];
        }

        public virtual void AfterConstructMapBody() {
        }
    }
}

