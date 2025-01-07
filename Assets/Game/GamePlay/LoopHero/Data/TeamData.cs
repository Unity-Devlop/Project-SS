using System;
using System.Collections.Generic;


namespace Game.LoopHero
{
    /// <summary>
    /// 队伍数据
    /// </summary>
    [Serializable]
    public sealed class TeamData
    {
        [field: UnityEngine.SerializeField] public PlayerData playerData { get; private set; } // 玩家数据
        [field: UnityEngine.SerializeField] public PackageData package { get; private set; } // 背包数据
        [field: UnityEngine.SerializeField] public List<PokemonData> battlePokemonList { get; private set; } // 战斗队伍
        [field: UnityEngine.SerializeField] public List<PokemonData> candidatePokemonList { get; private set; } // 候选队伍

        /// <summary>
        /// 当前的buff列表
        /// </summary>
        [field: UnityEngine.SerializeField]
        public List<BuffData> currentBuffList { get; private set; }

        public TeamData()
        {
            playerData = new PlayerData();
        }
    }
}