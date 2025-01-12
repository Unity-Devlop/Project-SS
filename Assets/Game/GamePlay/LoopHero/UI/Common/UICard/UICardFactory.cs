using System;
using cfg;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityToolkit;

namespace Game.LoopHero.UI.Common
{
    public class UICardFactory : MonoBehaviour
    {
        public GameObject carSlotPrefab;
        public GameObject cardEffectPrefab;
        public GameObject cardTerrainPrefab;

        public UICard Spawn(UICardContainer container, ItemEnum id,int idx)
        {
            var config = Core.Tables.ItemTable.Get(id);
            Assert.IsTrue(config.Type == ItemTypeEnum.卡牌, $"物品{id}不是卡牌!!! 检查配置表");
            var index = Core.Tables.CardIndexTable.Get(id);
            GameObject prefab;
            if (index.Type == CardTypeEnum.地形卡)
            {
                prefab = cardTerrainPrefab;
            }
            else if (index.Type == CardTypeEnum.道具卡)
            {
                prefab = cardEffectPrefab;
            }
            else
            {
                throw new ArgumentException($"道具{id} 找不到对应的卡牌索引 是没有配置么?");
            }


            var cardSlot = Instantiate(carSlotPrefab, container.transform).transform as RectTransform;
            Assert.IsNotNull(cardSlot);
            var card = Instantiate(prefab, cardSlot.transform).GetComponent<UICard>();
            Assert.IsNotNull(card);
            card.SetSlot(cardSlot);
            if (card is ILoopHeroCard loopHeroCard)
            {
                loopHeroCard.Bind(id, idx);
            }

            return card;
        }

        public void DeSpawn(UICard uiCard)
        {
            GameLogger.Log.Information("[{0}]DeSpawn Card {1}", nameof(UICardFactory), uiCard);
            Destroy(uiCard.slot.gameObject);
        }
    }
}