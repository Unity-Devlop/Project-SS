using System;
using System.Collections.Generic;
using cfg;
using Newtonsoft.Json;
using UnityToolkit;


namespace Game.LoopHero
{
    /// <summary>
    /// 队伍数据
    /// </summary>
    [Serializable]
    public sealed class TeamData : Model<TeamData>
    {
        [JsonRequired] public int trainerId;
        [JsonIgnore] public const int MaxBattlePokemonCount = 6;

        [JsonRequired]
        [field: UnityEngine.SerializeField]
        public PokemonData playerData { get; private set; } // 玩家数据

        [JsonRequired]
        [field: UnityEngine.SerializeField]
        public PackageData package { get; private set; } // 背包数据

        [JsonRequired]
        [field: UnityEngine.SerializeField]
        public List<PokemonData> battlePokemonList { get; private set; } // 战斗队伍

        /// <summary>
        /// 只会在战斗中存在的数据
        /// </summary>
        [JsonIgnore]
        [field: UnityEngine.SerializeField]
        public Queue<PokemonData> candidatePokemonQueue { get; private set; } = new Queue<PokemonData>(); // 候选队伍

        [JsonRequired]
        [field: UnityEngine.SerializeField]
        public List<PokemonData> packagePokemonList { get; private set; } // 背包中的宝可梦

        /// <summary>
        /// 当前的buff列表
        /// </summary>
        [JsonRequired]
        [field: UnityEngine.SerializeField]
        public List<BuffData> currentBuffList { get; private set; }

        /// <summary>
        /// 拿到当前的所有用于战斗的宝可梦
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PokemonData> GetBattlePokemonList()
        {
            yield return playerData;
            foreach (var pokemonData in battlePokemonList)
            {
                if (!pokemonData.alive) continue;
                yield return pokemonData;
            }
        }

        public bool Validate()
        {
            if (battlePokemonList.Count > MaxBattlePokemonCount)
            {
                return false;
            }

            // TODO 暂时只检验宝可梦的trainerID
            if (playerData.trainerId != trainerId)
            {
                return false;
            }

            foreach (var pokemonData in battlePokemonList)
            {
                if (pokemonData.trainerId != trainerId)
                {
                    return false;
                }
            }

            foreach (var pokemonData in packagePokemonList)
            {
                if (pokemonData.trainerId != trainerId)
                {
                    return false;
                }
            }

            return true;
        }

#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
        private void MakeValidate()
        {
            // 反射获得trainerId的Set方法
            var type = typeof(PokemonData);
            var property = type.GetProperty("trainerId");
            var setMethod = property.GetSetMethod(true);

            setMethod.Invoke(playerData, new object[] { trainerId });
            foreach (var pokemonData in battlePokemonList)
            {
                setMethod.Invoke(pokemonData, new object[] { trainerId });
            }

            foreach (var pokemonData in candidatePokemonQueue)
            {
                setMethod.Invoke(pokemonData, new object[] { trainerId });
            }
        }

        [Obsolete("EDITOR")]
        public void RecoverHealth()
        {
            playerData.currentHealth = playerData.baseHealth;
            foreach (var pokemonData in battlePokemonList)
            {
                pokemonData.currentHealth = pokemonData.baseHealth;
            }

            foreach (var pokemonData in packagePokemonList)
            {
                pokemonData.currentHealth = pokemonData.baseHealth;
            }
        }
#endif
    }
}