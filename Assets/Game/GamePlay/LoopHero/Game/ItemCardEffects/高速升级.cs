using cfg;
using Game.LoopHero.CardEffect;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.LoopHero
{
    public static partial class ItemCardEffectExecutes
    {
        
        /// <summary>
        /// 对目标己方怪兽使用。本次对战中，该怪兽的速度提高10点。如果该怪兽的种族是机械，则改为该怪兽的速度永久提高2点。
        /// </summary>
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
                return true;
            }

            pokemon.data.AddFightTempSpeed(10);
            return true;
        }
    }
}