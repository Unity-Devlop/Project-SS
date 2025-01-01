using Cysharp.Threading.Tasks;
using Game.GamePlay;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityToolkit;

namespace Game.Flow
{
    public class GamePlayState : IState<GameFlow>
    {
        private GameObject bindedGameObject;

        public void OnInit(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
        }

        public async void OnEnter(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            await Addressables.LoadSceneAsync(Global.Get<GameConfig>().playScene);
            var prefab = await Global.Get<GameConfig>().playMgrPrefab.LoadAssetAsync<GameObject>();
            bindedGameObject = Object.Instantiate(prefab);
            await UniTask.DelayFrame(1, cancellationToken: owner.destroyCancellationToken);
        }

        public void Transition(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
        }

        public void OnUpdate(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
        }

        public void OnExit(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            if (bindedGameObject != null)
            {
                Object.Destroy(bindedGameObject);
            }
        }
    }
}