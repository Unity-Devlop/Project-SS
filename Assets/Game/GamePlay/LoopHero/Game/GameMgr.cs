using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
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

        private AsyncOperationHandle<SceneInstance> _bigMapSceneHandle;
        private AsyncOperationHandle<SceneInstance> _campSceneHandle;
        private AsyncOperationHandle<SceneInstance> _fightSceneHandle;

        protected override async void OnInit()
        {
            GameLogger.Log("LoopHeroGameMgr OnInit");
            
            stateMachine = new StateMachine<GameMgr>(this);
            
            _bigMapSceneHandle = Addressables.LoadSceneAsync(config.bigMapScene, LoadSceneMode.Additive);
            _campSceneHandle = Addressables.LoadSceneAsync(config.campScene, LoadSceneMode.Additive);
            _fightSceneHandle = Addressables.LoadSceneAsync(config.fightScene, LoadSceneMode.Additive);

            await _bigMapSceneHandle.ToUniTask(this);
            await _campSceneHandle.ToUniTask(this);
            await _fightSceneHandle.ToUniTask(this);
            
            CampMgr.Singleton.enabled = false;
            BigMapMgr.Singleton.enabled = false;
            FightMgr.Singleton.enabled = false;
            
            
            stateMachine.Add<BigMapModule>();
            stateMachine.Add<CampModule>();
            stateMachine.Add<FightModule>();
            stateMachine.Add<FightSettlementModule>();

            bigMapModule = stateMachine.GetState<BigMapModule>();
            campModule = stateMachine.GetState<CampModule>();

            // Additive 加载三个模块的场景


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

            Addressables.UnloadSceneAsync(_bigMapSceneHandle);
            Addressables.UnloadSceneAsync(_campSceneHandle);
            Addressables.UnloadSceneAsync(_fightSceneHandle);
        }

        public void ToCamp()
        {
            stateMachine.Change<CampModule>();
        }

        public void ToFight(FightModuleData data)
        {
            // TODO 传递数据
            stateMachine.SetParam(nameof(FightModuleData), data);
            stateMachine.Change<FightModule>();
        }

        public void ToBigMap()
        {
            stateMachine.Change<BigMapModule>();
        }
    }
}