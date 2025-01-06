using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.LoopHero
{
    public class Pokemon : MonoBehaviour
    {
        [field: SerializeField] public PokemonData data { get; private set; }

        public virtual async UniTask Bind(PokemonData data)
        {
            this.data = data;
        }
    }
}