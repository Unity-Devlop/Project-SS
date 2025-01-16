using cfg;
using Game.LoopHero.CardEffect;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.LoopHero
{
    public static partial class ItemCardEffectExecutes
    {
        /// <summary>
        /// 对目标己方怪兽使用。本次对战中，该怪兽的力量和防御分别提高10点。如果该怪兽的种族是机械，则改为该怪兽的力量和防御分别永久提高1点。
        /// </summary>
        [EffectForItemCard(ItemEnum.装甲升级)]
        public static bool Exe装甲升级(ItemEnum cardId, Collider2D collider2D)
        {
            GameLogger.Log.Debug($"Exe装甲升级 {cardId} {collider2D}");
            Assert.IsTrue(cardId == ItemEnum.装甲升级);
            if (collider2D == null) return false;
            if (!collider2D.TryGetComponent(out Pokemon pokemon)) return false;
            if (!pokemon.IsMine()) return false;
            if (pokemon.HasRace(RaceEnum.机械))
            {
                pokemon.data.AddPermanentPower(1);
                pokemon.data.AddPermanentDefense(1);
                return true;
            }

            pokemon.data.AddFightTempPower(10);
            pokemon.data.AddFightTempDefense(10);
            return true;
        }
    }
}