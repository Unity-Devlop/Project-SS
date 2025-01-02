using UnityEngine;

namespace Game.LoopHero
{
    public class Pokemon : MonoBehaviour
    {
        [field: SerializeField] public PokemonData data { get; private set; }
    }
}