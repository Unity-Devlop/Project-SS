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

        #endregion

        /// <summary>
        /// 对目标己方怪兽使用。本次对战中，该怪兽的速度提高10点。如果该怪兽的种族是机械，则改为该怪兽的速度永久提高2点。
        /// </summary>
        /// <returns></returns>
        [EffectForItemCard(ItemEnum.高速升级)]
        public static bool Exe高速升级(ItemEnum cardId, Collider2D collider2D)
        {
            GameLogger.Log.Debug($"Exe高速升级 {cardId} {collider2D}");
            Assert.IsTrue(cardId == ItemEnum.高速升级);
            if (collider2D == null) return false;
            if (!collider2D.TryGetComponent(out Pokemon pokemon)) return false;
            if (!pokemon.IsMine()) return false;
            if (pokemon.HasRace(RaceEnum.机械))
            {
                pokemon.data.AddFightTempSpeed(2);
            }
            else
            {
                pokemon.data.AddPermanentSpeed(2);
            }

            return false;
        }
    }
}