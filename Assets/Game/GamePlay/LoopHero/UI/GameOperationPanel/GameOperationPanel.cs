using System;
using cfg;
using Game.LoopHero.CardEffect;
using Game.LoopHero.UI;
using Game.LoopHero.UI.Common;
using TMPro;
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

        #region DEMO DEBUG

        [SerializeField] private GameObject debugUseCardArea;

        #endregion


        private void Awake()
        {
            _factory = cardContainer.GetComponent<UICardFactory>();
            cardContainer.OnAddEvent += OnAddCard;
            cardContainer.OnRemoveEvent += OnRemoveCard;

            debugUseCardArea.gameObject.SetActive(false);
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

        #region 使用卡牌

        // TODO 职责分离
        private void OnRemoveCard(UICard card)
        {
            card.OnEndDragEvent -= OnEndDragCard;
            card.OnBeginDragEvent -= OnBeginDragCard;
        }

        // TODO 职责分离
        private void OnAddCard(UICard card)
        {
            card.OnEndDragEvent += OnEndDragCard;
            card.OnBeginDragEvent += OnBeginDragCard;
        }

        private void OnBeginDragCard(UICard obj)
        {
            debugUseCardArea.gameObject.SetActive(true);
        }

        // TODO 职责分离
        private void OnEndDragCard(UICard obj)
        {
            debugUseCardArea.gameObject.SetActive(false);
            Vector3 screenPos = UIRoot.Singleton.UICamera.WorldToScreenPoint(obj.transform.position);
            if (!RectTransformUtility.RectangleContainsScreenPoint(useCardArea,
                    new Vector2(screenPos.x, screenPos.y),
                    UIRoot.Singleton.UICamera)) return;
            GameLogger.Log.Information("[{0}]Use Card {1}", nameof(GameOperationPanel), obj);
            // TODO 职责分离
            if (obj is not ILoopHeroCard loopHeroCard) return;
            _bindData.Get(loopHeroCard.idx, out var item);


            Assert.IsTrue(item.count == 1);
            var idx = Core.Tables.CardIndexTable.Get(item.id);
            bool success = false;

            Ray ray = Global.cameraSystem.mainCamera.ScreenPointToRay(screenPos);

#if UNITY_EDITOR
            UnityEngine.Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 10);
#endif

            switch (idx.Type)
            {
                case CardTypeEnum.地形卡:
                    var terrainCardConfig = Core.Tables.TerrainCardTable.Get(item.id);
                    success = ExecuteTerrainCard(terrainCardConfig);
                    break;
                case CardTypeEnum.道具卡:
                    var effectCardConfig = Core.Tables.EffectCardTable.Get(item.id);
                    success = ExecuteEffectCard(effectCardConfig, ref ray);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (success)
            {
                cardContainer.Remove(obj);
                _bindData.Remove(loopHeroCard.idx, 1);
                _factory.DeSpawn(obj);
            }
        }

        private bool ExecuteTerrainCard(TerrainCardConfig config)
        {
            return true;
        }

        private bool ExecuteEffectCard(EffectCardConfig config, ref Ray ray)
        {
            RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);

            return ItemCardEffects.Execute((config.Id, hit2D.collider));
        }

        #endregion
    }
}