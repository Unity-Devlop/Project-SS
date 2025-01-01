using System;
using Game.Flow;
using Game.GamePlay;
using UnityEngine;
using UnityEngine.UI;
using UnityToolkit;

namespace Game.LoopHero
{
    public class GameHomePanel : UIPanel
    {
        [SerializeField] private Button startGameButton;

        [SerializeField] private Button exitGameButton;

        private void Awake()
        {
            startGameButton.onClick.AddListener(OnStartGame);
        }

        private void OnStartGame()
        {
            Global.Get<GameFlow>().Change<GamePlayState>();
        }
    }
}