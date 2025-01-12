using System;
using cfg;
using Game.LoopHero.UI;
using Game.LoopHero.UI.Common;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityToolkit;

namespace Game.LoopHero
{
    public class GameOperationPanel : UIPanel
    {
        [SerializeField] private UICardContainer cardContainer;
        private UICardFactory _factory;
        private PackageData _bindData;
        [SerializeField] private RectTransform useCardArea;

        private void Awake()
        {
            _factory = cardContainer.GetComponent<UICardFactory>();
            cardContainer.OnAddEvent += OnAddCard;
            cardContainer.OnRemoveEvent += OnRemoveCard;
        }

        // TODO 职责分离
        private void OnRemoveCard(UICard card)
        {
            card.OnEndDragEvent -= OnEndDragCard;
        }

        // TODO 职责分离
        private void OnAddCard(UICard card)
        {
            card.OnEndDragEvent += OnEndDragCard;
        }

        // TODO 职责分离
        private void OnEndDragCard(UICard obj)
        {
            Vector3 screenPos = UIRoot.Singleton.UICamera.WorldToScreenPoint(obj.transform.position);
            if (RectTransformUtility.RectangleContainsScreenPoint(useCardArea, new Vector2(screenPos.x, screenPos.y),
                    UIRoot.Singleton.UICamera))
            {
                GameLogger.Log.Information("[{0}]Use Card {1}", nameof(GameOperationPanel), obj);
                // TODO 职责分离
                if (obj is ILoopHeroCard loopHeroCard)
                {
                    cardContainer.Remove(obj);
                    _bindData.Remove(loopHeroCard.idx, 1);
                    _factory.DeSpawn(obj);
                }
            }
        }

        public void Bind(PackageData data)
        {
            Assert.IsNull(_bindData);
            _bindData = data;
            foreach (var pair in data.items)
            {
                ItemEnum id = pair.id;
                var card = _factory.Spawn(cardContainer, id, pair.idx);
                cardContainer.Add(card);
            }
        }
    }
}