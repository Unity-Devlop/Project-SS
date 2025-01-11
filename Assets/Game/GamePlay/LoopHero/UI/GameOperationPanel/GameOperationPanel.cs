using System;
using cfg;
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

        private void Awake()
        {
            _factory = cardContainer.GetComponent<UICardFactory>();
        }

        public void Bind(PackageData data)
        {
            Assert.IsNull(_bindData);
            _bindData = data;
            foreach (var pair in data.items)
            {
                ItemEnum id = pair.id;
                var card = _factory.Spawn(cardContainer, id);
                cardContainer.Add(card);
            }
        }
    }
}