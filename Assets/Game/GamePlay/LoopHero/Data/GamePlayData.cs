using System;
using UnityEngine;

namespace Game.LoopHero
{
    /// <summary>
    /// 一把游戏的数据
    /// </summary>
    [Serializable]
    public class GamePlayData
    {
        public int index;
        [field: SerializeField] public bool newGame { get; private set; }
        [field: SerializeField] public TeamData teamData { get; private set; }

        public GamePlayData()
        {
            newGame = true;
            teamData = new TeamData();
        }
    }
}