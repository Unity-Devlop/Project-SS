using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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
            await UniTask.CompletedTask;
        }
    }
}