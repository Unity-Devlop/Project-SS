using System.Threading.Tasks;
using UnityEngine;

namespace Game.LoopHero
{
    public class Pokemon : MonoBehaviour
    {
        [field: SerializeField] public PokemonData data { get; private set; }

        public async Task Bind(PokemonData data)
        {
            this.data = data;
        }
    }
}