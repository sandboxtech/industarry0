
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Weathering
{
    public class ClothingIndex { }

    public class CharacterView : MonoBehaviour
    {
        public Sprite DefaultSprite;

        //public Transform FlashLightTransform;

        //public Light2D FlashLight;

        [NonSerialized]
        public Sprite[] CurrentSprites;

        public Sprite[] TestSprites;
        public Sprite[] TestRealSprites;
        public Sprite[] BerrySprites;
        public Sprite[] CowBGSprites;
        public Sprite[] ScarecrowSprites;

        public Dictionary<long, (string, Sprite[], Type, string)> Dict { get; private set; }

        public static CharacterView Ins { get; private set; }

        private SpriteRenderer sr;
        private void Awake() {
            if (Ins != null) throw new Exception();
            Ins = this;

            sr = GetComponent<SpriteRenderer>();
            if (sr == null) throw new Exception();

            Dict = new Dictionary<long, (string, Sprite[], Type, string)>() {
                { 0, ("初始衣服", TestSprites, typeof(KnowledgeOfGatheringBerry), "好看!") },
                { 1, ("浆果装扮", BerrySprites, typeof(BerryBush_Clothed_Clothing), "换上这件衣服展现你对浆果的热爱! ") },
                { 2, ("牛仔套装", CowBGSprites, typeof(Pasture_Clothed_Clothing), "充满乡土气息的衣服, 适合把牛群护送到Kansas城火车站") },
                { 3, ("稻草人", ScarecrowSprites, typeof(Scarecrow_Clothed_Clothing), "就算站着一动不动，都能吓跑周围所有乌鸦。(注:使用本服饰即视为自愿放弃所有购买的烧伤保险)") },
            };
        }
        public void SetClothingID(long id) {
            if (id < 0 || id >= Dict.Count) {
                id = 0;
            }
            Clothing.Value = id;
            CurrentSprites = Dict[Clothing.Value].Item2;
            sr.sprite = CurrentSprites[0];
        }

        private IRef Clothing;
        private void Start() {

            Clothing = Globals.Ins.Refs.GetOrCreate<ClothingIndex>();

            SetClothingID(Clothing.Value);
        }

        private bool movingLast = false;
        private Vector2Int directionLast = Vector2Int.zero;

        private Vector3 lightVelocity = Vector3.zero;

        public float Distance = 2f;
        public void SetCharacterSprite(Vector2Int direction, bool moving) {

            bool needUpdateFlashLight = moving != movingLast || direction != directionLast;

            if (moving || needUpdateFlashLight) {
                int index;

                if (direction == Vector2Int.down) {
                    index = 0;
                } else if (direction == Vector2Int.left) {
                    index = 1;
                } else if (direction == Vector2Int.right) {
                    index = 2;
                } else if (direction == Vector2Int.up) {
                    index = 3;
                } else {
                    index = 0;
                }

                index *= 4;

                if (moving) index += TimeUtility.GetSimpleFrame(0.125f, 4);

                directionLast = direction;
                movingLast = moving;

                if (CurrentSprites == null) {
                    if (Clothing == null) {
                        Clothing = Globals.Ins.Refs.GetOrCreate<ClothingIndex>();
                    }
                    CurrentSprites = Dict[Clothing.Value].Item2;
                    sr.sprite = CurrentSprites[0];
                }
                sr.sprite = CurrentSprites[index];
            }

        }
    }
}

