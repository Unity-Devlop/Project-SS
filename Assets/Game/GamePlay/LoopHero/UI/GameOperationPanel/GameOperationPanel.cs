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
        private UICardConfigurator _configurator;
        private PackageData _bindData;

        private void Awake()
        {
            _configurator = cardContainer.GetComponent<UICardConfigurator>();
        }

        public void Bind(PackageData data)
        {
            Assert.IsNull(_bindData);
            _bindData = data;
            foreach (var pair in data.items)
            {
                ItemEnum id = pair.id;
                var card = cardContainer.Add(_configurator.Spawn);
                // _configurator.Config(card);       
                card.Bind(id);
            }
        }
    }
}