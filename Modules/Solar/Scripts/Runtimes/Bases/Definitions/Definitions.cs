/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  Definitions.cs
 * author:    taoye
 * created:   2020/8/31
 * descrip:   类型定义
 ***************************************************************/

using System;
using System.Collections.Generic;

namespace Solar.Runtime
{
    public static partial class Definitions
    {
        /// <summary>
        /// Asset类型
        /// </summary>
        public enum AssetType
        {
            GameObject,
            Texture2D,
            Texture,
            Sprite,
            ScriptableObject,
            TileBase,
            Font,
            FontTMP,
            Shader,
            Material,
            TextAsset,
            JsonAsset,
            BinaryAsset,
            LuaAsset,
            LuacAsset,
            Scene,
            SoundMp3,
            SoundWav,
            Model,
            SpriteAtlas,
        }

        /// <summary>
        /// Asset后缀名
        /// </summary>
        public static Dictionary<AssetType, string> AssetSuffix = new Dictionary<AssetType, string>() {
            {AssetType.GameObject, ".prefab"},
            {AssetType.Texture2D, ".png"},
            {AssetType.Texture, ".png"},
            {AssetType.Sprite, ".png"},
            {AssetType.ScriptableObject, ".asset"},
            {AssetType.TileBase, ".asset"},
            {AssetType.Font, ".ttf"},
            {AssetType.FontTMP, ".asset"},
            {AssetType.Shader, ".shader"},
            {AssetType.Material, ".mat"},
            {AssetType.TextAsset, ".txt"},
            {AssetType.JsonAsset, ".json"},
            {AssetType.BinaryAsset, ".bytes"},
            {AssetType.LuaAsset, ".txt"},
            {AssetType.LuacAsset, ".bytes"},
            {AssetType.Scene, ".unity"},
            {AssetType.SoundMp3, ".mp3"},
            {AssetType.SoundWav, ".wav"},
            {AssetType.Model, ".fbx"},
            {AssetType.SpriteAtlas, ".spriteatlas"},
        };

    }
}
