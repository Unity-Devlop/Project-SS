using System.Collections.Generic;
using UnityToolkit;

namespace Game.LoopHero
{
    public class BuffTicker : IOnUpdate
    {
        private List<PokemonData> allPokemonList;
        private List<BuffData> allBuffList;

        public BuffTicker(List<PokemonData> allPokemonList, List<BuffData> allBuffList)
        {
            this.allPokemonList = allPokemonList;
            this.allBuffList = allBuffList;
        }
        

        public void OnUpdate(float deltaTime)
        {
        }
    }
}