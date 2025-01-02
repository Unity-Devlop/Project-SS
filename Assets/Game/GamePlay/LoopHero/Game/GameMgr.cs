using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityToolkit;

namespace Game.LoopHero
{
    public enum GameState
    {
        None,
        Camp,
        BigMap,
        Fight,
    }

    public class GameMgr : MonoSingleton<GameMgr>
    {
        // protected override bool DontDestroyOnLoad() => true;
        [field: SerializeField] public GameMgrConfig config { get; private set; }
        public StateMachine<GameMgr> stateMachine { get; private set; }

        public GameState gameState
        {
            get
            {
                if (stateMachine.currentState is BigMapModule)
                {
                    return GameState.BigMap;
                }

                if (stateMachine.currentState is CampModule)
                {
                    return GameState.Camp;
                }

                if (stateMachine.currentState is FightModule)
                {
                    return GameState.Fight;
                }

                return GameState.None;
            }
        }

        public BigMapModule bigMapModule { get; private set; }
        public CampModule campModule { get; private set; }

        protected override void OnInit()
        {
            GameLogger.Log("LoopHeroGameMgr OnInit");
            stateMachine.Add<BigMapModule>();
            stateMachine.Add<CampModule>();
            stateMachine.Add<FightModule>();

            bigMapModule = stateMachine.GetState<BigMapModule>();
            campModule = stateMachine.GetState<CampModule>();


            stateMachine.Run<CampModule>();

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

        public void ToCamp()
        {
            stateMachine.Change<CampModule>();
        }

        public void ToGame()
        {
            stateMachine.Change<BigMapModule>();
        }

        public void ToFight()
        {
            // TODO 传递数据
            stateMachine.Change<FightModule>();
        }
    }
}