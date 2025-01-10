using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.LoopHero
{


    /// <summary>
    /// 一把游戏的数据
    /// </summary>
    [Serializable]
    public class GamePlayData : IJsonData
    {
        [field: SerializeField] public bool newGame { get; private set; }
        [JsonRequired] [field: SerializeField] public TeamData teamData { get; private set; }

        // TODO 场景数据 包括上一次生成的地图 玩家当前位置等等等等
        public BigMapData bigmap;
    }
}