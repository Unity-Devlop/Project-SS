using System;
using UnityEngine;

namespace Game.LoopHero
{
    [Serializable]
    public class BigMapData : IJsonData
    {
        // TODO 场景数据 怪物数据 等等等等
        [Sirenix.OdinInspector.ShowInInspector]
        public UnityToolkit.MathTypes.Vector3 playerPos; // 不用Unity 避免循环引用 导致的序列化问题

        [Obsolete]
        public static BigMapData Fake()
        {
            return new BigMapData();
        }
    }
}