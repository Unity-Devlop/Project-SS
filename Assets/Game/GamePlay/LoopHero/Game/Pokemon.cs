using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.LoopHero
{
    public class Pokemon : MonoBehaviour
    {
        [field: SerializeField] public PokemonData data { get; private set; }

        public virtual async UniTask Bind(PokemonData data)
        {
            this.data = data;
        }

        public virtual async UniTask Action()
        {
            Assert.IsNotNull(data);
            float localY = transform.localPosition.y;
            // TODO 占位攻击动作
            await transform.DOLocalMoveY(localY+ 0.5f, 0.5f);
            await transform.DOLocalMoveY(localY, 0.5f);
            await UniTask.CompletedTask;
        }
    }
}