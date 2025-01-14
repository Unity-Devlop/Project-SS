using System;
using UnityEngine;
using UnityEngine.UI;
using UnityToolkit;

namespace Game.LoopHero.UI
{
    public class FightSettlementPanel : UIPanel
    {
        [SerializeField] private Button _confirmButton;

        private FightSettlementModule _module;

        private void Awake()
        {
            _confirmButton.onClick.AddListener(OnConfirm);
        }


        public void Bind(FightSettlementModule module)
        {
            _module = module;
            // TODO 根据情况判断要不要让玩家确认


            // TODO 奖励飞行动画   


            if (false)
            {
                _confirmButton.gameObject.SetActive(false);
                module.SettlementEnd();
            }
            else
            {
                _confirmButton.gameObject.SetActive(true);
                // TODO 等待玩家点确认按钮
            }
        }

        private void OnConfirm()
        {
            _module.SettlementEnd();
        }
    }
}