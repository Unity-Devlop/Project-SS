using cfg;
using Game.LoopHero.CardEffect;
using Game.LoopHero.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.LoopHero
{
    public static partial class ItemCardEffectExecutes
    {
        #region 通用方法

        /// <summary>
        /// 判断是否是己方怪兽
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        static bool IsMine(this Pokemon pokemon)
        {
            return Core.Singleton.playData.teamData.trainerId == pokemon.trainerId;
        }

        static bool HasRace(this Pokemon pokemon, RaceEnum raceEnum)
        {
            return pokemon.data.config.RaceA == raceEnum || pokemon.data.config.RaceB == raceEnum;
        }

        static void AddBuff(this Pokemon pokemon, BuffEnum buffEnum, float time)
        {
            TeamData data;
            if (pokemon.data.trainerId == FightMgr.Singleton.data.self.teamData.trainerId)
            {
                data = FightMgr.Singleton.data.self.teamData;
            }
            else
            {
                data = FightMgr.Singleton.data.enemy.teamData;
            }

            if (data == null) return;
            data.currentBuffList.Add(new BuffData(buffEnum, time, pokemon.data.guid));
        }

        #endregion
    }
}