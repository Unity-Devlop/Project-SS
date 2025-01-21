using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Game.LoopHero
{
    [Serializable]
    public class FighterData
    {
        public TeamData teamData;

        private FighterData()
        {
        }

        public FighterData(TeamData teamData)
        {
            this.teamData = teamData;
        }

        public bool canFight
        {
            get
            {
                bool flag = false;
                for (var i = 0; i < teamData.battlePokemonList.Count; i++)
                {
                    var pokemon = teamData.battlePokemonList[i];
                    flag |= pokemon.alive;
                }

                foreach (var pokemonData in teamData.candidatePokemonQueue)
                {
                    Assert.IsTrue(pokemonData.alive);
                    flag |= pokemonData.alive;
                }

                return flag;
            }
        }
    }
}