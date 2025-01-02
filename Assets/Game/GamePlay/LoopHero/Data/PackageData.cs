using System;
using System.Collections.Generic;
using cfg;

namespace Game.LoopHero
{
    [Serializable]
    public class PackageData
    {
        [Serializable]
        public class Pair
        {
            public ItemEnum id;
            public uint count;
        }

        public List<Pair> items;
    }
}