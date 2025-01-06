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
    }

}