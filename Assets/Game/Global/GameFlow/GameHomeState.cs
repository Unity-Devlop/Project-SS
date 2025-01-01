using FMOD;
using Game.Home;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityToolkit;

namespace Game.Flow
{
    public class GameHomeState : IState<GameFlow>
    {
        public GameObject bindedGameObject;

        public void OnInit(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            // throw new System.NotImplementedException();
        }

        public void OnEnter(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            Addressables.LoadSceneAsync(Global.Get<GameConfig>().homeScene).WaitForCompletion();
            // Global.Get<AudioSystem>().PlayBGM(FMODName.Event.BGM_game_home, out _);
            UIRoot.Singleton.OpenPanel<GameHomePanel>();
            var prefab = Addressables.LoadAssetAsync<GameObject>(Global.Get<GameConfig>().homeMgrPrefab)
                .WaitForCompletion();
            bindedGameObject = Object.Instantiate(prefab);
        }

        public void Transition(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            stateMachine.Change<GamePlayState>();
        }

        public void OnUpdate(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            // throw new System.NotImplementedException();
        }

        public void OnExit(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            // Global.Get<AudioSystem>().DisposeBGM(FMODName.Event.BGM_game_home);
            UIRoot.Singleton.Dispose<GameHomePanel>();
            if (bindedGameObject != null)
            {
                Object.Destroy(bindedGameObject);
            }
        }
    }
}