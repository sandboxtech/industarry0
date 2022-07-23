﻿
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Weathering
{
    public class SliceImage : MonoBehaviour
    {

        public string PATH_Base = "D:\\UnityProjects\\Weathering\\Assets\\";
        public string PATH = "Tiles\\Planets\\PlanetBarren_Base";

        [ContextMenu("Slice")]
        private void Slice() {
            // string path = Application.streamingAssetsPath + "/export/";
            string path = PATH_Base + PATH;
            foreach (var sprite in Sprites) {
                System.IO.File.WriteAllBytes(path + sprite.name + ".png", GetByte(sprite));
            }
            Debug.LogWarning("OK");
        }

        public List<Sprite> Sprites;

        public byte[] GetByte(Sprite sprite) {
            //return sprite.texture.EncodeToPNG();

            Texture2D tex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, sprite.texture.format, false);
            tex.SetPixels(sprite.texture.GetPixels(
                (int)sprite.rect.xMin, (int)sprite.rect.yMin,
                (int)sprite.rect.width, (int)sprite.rect.height)
            );
            tex.Apply();
            return tex.EncodeToPNG();
        }

        public Sprite GetSprite(byte[] bytes) {
            Texture2D texture = new Texture2D(10, 10);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            return sprite;
        }

    }
}

