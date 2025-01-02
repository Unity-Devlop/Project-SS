using System;
using cfg;

namespace Game.LoopHero
{
    [Serializable]
    public class PokemonData
    {
        public PokemonEnum id;
        public PokemonConfig config => Core.Tables.PokemonTable.Get(id);

        public ushort level;
        public int health;
        public int power;
        public int defense;
        public int adaptability;
        public int speed;
        
    }
}