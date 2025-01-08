using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.LoopHero
{
    public class Pokemon : MonoBehaviour
    {
        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        public PokemonData data { get; private set; }

        public event Action<PokemonData> OnEnterBattle;
        public event Action OnAction;

        public event Action<PokemonData> OnExitBattle;

        public virtual async UniTask EnterBattle(PokemonData data)
        {
            Assert.IsNull(this.data);
            this.data = data;
            OnEnterBattle?.Invoke(data);
        }

        public virtual async UniTask Action()
        {
            Assert.IsNotNull(data);
            float localY = transform.localPosition.y;
            // TODO 占位攻击动作
            await transform.DOLocalMoveY(localY + 0.5f, 0.5f);
            await transform.DOLocalMoveY(localY, 0.5f);
            OnAction?.Invoke();
            await UniTask.CompletedTask;
        }

        public async UniTask ExitBattle()
        {
            Assert.IsNotNull(data);
            OnExitBattle?.Invoke(data);
            data = null;
            await UniTask.CompletedTask;
        }
    }
}