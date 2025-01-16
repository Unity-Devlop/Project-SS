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
        [EffectForItemCard(ItemEnum.脏拖把)]
        public static bool Exe脏拖把(ItemEnum cardId, Collider2D collider2D)
        {
            GameLogger.Log.Debug($"Exe 脏拖把 {cardId} {collider2D}");
            Assert.IsTrue(cardId == ItemEnum.脏拖把);
            if (collider2D == null) return false;
            if (!collider2D.TryGetComponent(out Pokemon pokemon)) return false;
            if (!pokemon.IsMine())
            {
                pokemon.data.TakeTrueDamage(30);
            }
            return true;
        }
    }
}