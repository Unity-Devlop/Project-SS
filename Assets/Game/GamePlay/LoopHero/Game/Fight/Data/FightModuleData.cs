using System;
using System.Collections.Generic;
using UnityToolkit;

namespace Game.LoopHero
{

    [Serializable]
    public sealed class FightModuleData : ObservationObject<FightModuleData>
    {
        public FighterData self;
        public FighterData enemy;
    }
}