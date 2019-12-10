using System.Collections.Generic;
using UnityEngine;
using FoxGame;
using UnityEngine.Events;
using System.Linq;
using Object = UnityEngine.Object;
using System;
using UnityEngine.U2D;

namespace FoxGame.Asset
{


    //资源管理器 (资源管理器 仅会把资源加载入内存)
    public class LoadMgr : MonoSingleton<LoadMgr>
    {
        //加载图集
        public Sprite LoadSprite(string altasName, string spriteName)
        {
            SpriteAtlas sp = AssetManager.Instance.LoadAsset<SpriteAtlas>(GameConfigs.GetSpriteAtlasPath(altasName));
            return sp.GetSprite(spriteName);
        }
        //加载预制体
        public GameObject LoadPrefab(string prefab)
        {
            GameObject obj  = AssetManager.Instance.LoadAsset<GameObject>(GameConfigs.GetPrefabsPath(prefab));
            return obj;
        }
    }

}
