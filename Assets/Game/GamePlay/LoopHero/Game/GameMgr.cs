using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityToolkit;

namespace Game.LoopHero
{
    public class GameMgr : MonoSingleton<GameMgr>
    {
        // protected override bool DontDestroyOnLoad() => true;
        public StateMachine<GameMgr> stateMachine { get; private set; }
        [SerializeField] private AssetReferenceT<Player> playerPrefab;

        protected override void OnInit()
        {
            GameLogger.Log("LoopHeroGameMgr OnInit");
            stateMachine.Add<FightState>();
            stateMachine.Add<FightWinState>();
            stateMachine.Add<PauseState>();
            stateMachine.Add<WalkState>();
            stateMachine.Add<WaitForStartState>();

            stateMachine.Run<WaitForStartState>();

            UIRoot.Singleton.OpenPanel<GamePlayPanel>();
        }

        private void Update()
        {
            if (stateMachine.running)
            {
                stateMachine.OnUpdate();
            }
        }

        protected override void OnDispose()
        {
            GameLogger.Log("LoopHeroGameMgr OnDispose");
            stateMachine.Stop();

            if (UIRoot.SingletonNullable != null)
            {
                UIRoot.Singleton.Dispose<GamePlayPanel>();
            }
        }
    }
}