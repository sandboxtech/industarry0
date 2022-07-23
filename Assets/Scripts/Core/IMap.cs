﻿
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Weathering
{
    public interface ISavable
    {
        IValues Values { get; }
        IRefs Refs { get; }
        IInventory Inventory { get; }
        IInventory InventoryOfSupply { get; }
    }

    public interface ISavableDefinition : ISavable
    {
        void SetValues(IValues values);
        void SetRefs(IRefs refs);
        void SetInventory(IInventory inventory);
        void SetInventoryOfSupply(IInventory inventory);
    }

    public interface IMap : ISavable
    {
        int Width { get; }
        int Height { get; }
        bool ControlCharacter { get; }

        string GetMapKey { get; }
        ITile ParentTile { get; }
        void EnterParentMap();
        void EnterChildMap(Vector2Int pos);


        // 目前有两种方案定义DefaultTileType, 目前采用DefaultTileType够用
        // Type GenerateTileType(Vector2Int pos);
        Type DefaultTileType { get; }

        string GetSpriteKeyBedrock(Vector2Int pos);
        string GetSpriteKeyWater(Vector2Int pos);
        string GetSpriteKeyGrass(Vector2Int pos);
        string GetSpriteKeyTree(Vector2Int pos);
        string GetSpriteKeyHill(Vector2Int pos);


        bool CanUpdateAt<T>(Vector2Int pos);
        bool CanUpdateAt(Type type, Vector2Int pos);
        bool CanUpdateAt<T>(int i, int j);
        bool CanUpdateAt(Type type, int i, int j);


        ITile Get(int i, int j);
        ITile Get(Vector2Int pos);


        T UpdateAt<T>(ITile oldTile) where T : class, ITile;
        ITile UpdateAt(Type type, ITile oldTile);
    }

    public interface IMapDefinition : IMap, ISavableDefinition
    {

        Vector2Int ParentPositionBuffer { get; set; }

        string MapKey { get; set; }

        uint HashCode { get; set; }
        void SetTile(Vector2Int pos, ITileDefinition tile, bool inConstruction=false);
        void OnEnable();
        void OnDisable();
        void OnConstruct();
        void AfterConstructMapBody();

        ITileDefinition GetTileFast(int i, int j);

        // void AfterGeneration();

        void OnTapTile(ITile tile);

        bool CanReset { get; }
        void Reset();
    }
}

