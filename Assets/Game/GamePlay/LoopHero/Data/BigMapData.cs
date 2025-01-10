using System;
using UnityEngine;

namespace Game.LoopHero
{
    [Serializable]
    public class BigMapData : IJsonData
    {
        // TODO 场景数据 怪物数据 等等等等
        public Vector3 playerPos;

        [Obsolete]
        public static BigMapData Fake()
        {
            return new BigMapData();
        }
    }
}