using System;
using System.Collections.Generic;
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

        private GamePlayData _data;

        protected override async void OnInit()
        {
            GameLogger.Log.Debug("LoopHeroGameMgr OnInit");

            _data = Core.Singleton.playData;

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

            var panel = UIRoot.Singleton.OpenPanel<GameOperationPanel>();
            panel.Bind(_data.teamData.package);
        }

        private void Update()
        {
            if (!stateMachine.running) return;
            stateMachine.OnUpdate();
        }

        protected override void OnDispose()
        {
            GameLogger.Log.Debug("LoopHeroGameMgr OnDispose");
            try
            {
                stateMachine.Stop();
            }
            catch (NullReferenceException)
            {
                // TODO MonoBehavior 在Editor下退出游戏时 可能会在 GameMgr 销毁前销毁了
            }

            if (UIRoot.SingletonNullable != null)
            {
                UIRoot.Singleton.Dispose<GameOperationPanel>();
            }

            try
            {
                Addressables.UnloadSceneAsync(_bigMapSceneHandle);
                Addressables.UnloadSceneAsync(_campSceneHandle);
                Addressables.UnloadSceneAsync(_fightSceneHandle);
            }
            catch (NullReferenceException)
            {
                // handle可能在Editor退出时自动释放了
            }
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

        public void ToBigMap(BigMapData data)
        {
            stateMachine.SetParam(nameof(BigMapData), data);
            stateMachine.Change<BigMapModule>();
        }
    }
}