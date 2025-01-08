using System;
using System.Collections.Generic;
using System.Diagnostics;
using cfg;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Game.LoopHero.UI
{
    public class UICarContainer : MonoBehaviour
    {
        private List<UICard> _cards;
        
        [SerializeField] private bool autoSizing = true;
        [SerializeField] public float standardWidth = 100;
        [SerializeField] public float standardHeight = 150;
        [SerializeField] public int standardCount = 8;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _cards = new List<UICard>(GetComponentsInChildren<UICard>());
        }

        private void Update()
        {
            if (!autoSizing) return;
            // 根据手牌的数量 动态调整自己的大小
            float maxWidth = standardWidth * standardCount;
            float width = standardWidth * _cards.Count;
            if (width < maxWidth)
            {
                _rectTransform.sizeDelta = new Vector2(width, standardHeight);
            }
            else
            {
                _rectTransform.sizeDelta = new Vector2(maxWidth, standardHeight);
            }
        }

        // private void OnGetCard(ItemEnum id)
        // {
        //     ValidateCard(id);
        //     var itemConfig = Core.Tables.ItemTable.Get(id);
        //     var cardConfig = Core.Tables.ItemCardTable.Get(id);
        // }
        //
        // private void OnRemoveCard(ItemEnum id)
        // {
        //     ValidateCard(id);
        //     var itemConfig = Core.Tables.ItemTable.Get(id);
        //     var cardConfig = Core.Tables.ItemCardTable.Get(id);
        // }


        [Conditional("UNITY_ASSERTIONS")]
        private void ValidateCard(ItemEnum id)
        {
            var itemConfig = Core.Tables.ItemTable.Get(id);
            Assert.IsTrue(itemConfig != null, $"不存在的物品配置:{id}");
            Assert.IsTrue(itemConfig.Type != ItemTypeEnum.卡牌, $"道具{id}不是卡牌类型的道具");
            var cardConfig = Core.Tables.ItemCardTable.Get(id);
            Assert.IsTrue(cardConfig != null, $"不存在的卡牌配置:{id}");
        }
    }
}