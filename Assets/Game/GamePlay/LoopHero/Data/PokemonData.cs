using System;
using cfg;
using UnityEngine;

namespace Game.LoopHero
{
    [Serializable]
    public class PokemonData
    {
        public PokemonEnum id;
        public PokemonConfig config => Core.Tables.PokemonTable.Get(id);

        /// <summary>
        /// 唯一标识
        /// </summary>
        public Guid guid;

        public bool alive => health > 0;
        public ushort level;

        public int health;
        public int basePower;
        public int baseDefense;
        public int baseAdaptability;
        public int baseSpeed;


        [field: SerializeField] public int additionalPower { get; private set; }

        [field: SerializeField] public int additionalDefense { get; private set; }

        [field: SerializeField] public int additionalAdaptability { get; private set; }

        [field: SerializeField] public int additionalSpeed { get; private set; }


#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
        private void Randomize()
        {
            guid = Guid.NewGuid();
            level = (ushort)UnityEngine.Random.Range(1, 100);
            health = UnityEngine.Random.Range(1, 100);
            basePower = UnityEngine.Random.Range(1, 100);
            baseDefense = UnityEngine.Random.Range(1, 100);
            baseAdaptability = UnityEngine.Random.Range(1, 100);
            baseSpeed = UnityEngine.Random.Range(1, 100);
        }
#endif
    }
}