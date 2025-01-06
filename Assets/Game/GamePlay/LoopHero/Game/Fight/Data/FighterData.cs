using System;
using System.Collections.Generic;

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

                return flag;
            }
        }
    }
}