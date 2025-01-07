using System;
using System.Collections.Generic;
using cfg;
using Newtonsoft.Json;


namespace Game.LoopHero
{
    /// <summary>
    /// 队伍数据
    /// </summary>
    [Serializable]
    public sealed class TeamData
    {
        [JsonRequired]
        [field: UnityEngine.SerializeField]
        public PokemonData playerData { get; private set; } // 玩家数据

        [JsonRequired]
        [field: UnityEngine.SerializeField]
        public PackageData package { get; private set; } // 背包数据

        [JsonRequired]
        [field: UnityEngine.SerializeField]
        public List<PokemonData> battlePokemonList { get; private set; } // 战斗队伍

        [JsonRequired]
        [field: UnityEngine.SerializeField]
        public List<PokemonData> candidatePokemonList { get; private set; } // 候选队伍

        /// <summary>
        /// 当前的buff列表
        /// </summary>
        [JsonRequired]
        [field: UnityEngine.SerializeField]
        public List<BuffData> currentBuffList { get; private set; }
    }
}