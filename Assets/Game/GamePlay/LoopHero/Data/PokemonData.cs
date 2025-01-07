using System;
using cfg;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.LoopHero
{
    [Serializable]
    public class PokemonData //: IEquatable<PokemonData>
    {
        public PokemonEnum id;

        [JsonIgnore] [LiteDB.BsonIgnore] public PokemonConfig config => Core.Tables.PokemonTable.Get(id);

        /// <summary>
        /// 唯一标识
        /// </summary>
        public Guid guid { get; private set; }
        
        public int trainerId;

        [JsonIgnore] public bool alive => health > 0;
        public ushort level;

        public int health;
        public int basePower;
        public int baseDefense;
        public int baseAdaptability;
        public int baseSpeed;


        public int additionalPower;

        public int additionalDefense;

        public int additionalAdaptability;

        public int additionalSpeed;


        [JsonIgnore] public int finalPower => basePower + additionalPower;
        [JsonIgnore] public int finalDefense => baseDefense + additionalDefense;
        [JsonIgnore] public int finalAdaptability => baseAdaptability + additionalAdaptability;
        [JsonIgnore] public int finalSpeed => baseSpeed + additionalSpeed;


        public static PokemonData New(PokemonEnum id)
        {
            return new PokemonData()
            {
                id = id,
                guid = Guid.NewGuid()
            };
        }

        // public override int GetHashCode()
        // {
        //     return guid.GetHashCode();
        // }
        //
        //
        // public bool Equals(PokemonData other)
        // {
        //     if (ReferenceEquals(null, other)) return false;
        //     if (ReferenceEquals(this, other)) return true;
        //     return guid == other.guid;
        // }
        //
        // public override bool Equals(object obj)
        // {
        //     if (ReferenceEquals(null, obj)) return false;
        //     if (ReferenceEquals(this, obj)) return true;
        //     if (obj.GetType() != this.GetType()) return false;
        //     return Equals((PokemonData)obj);
        // }


        #region Editor

#if UNITY_EDITOR
        [Sirenix.OdinInspector.HorizontalGroup("Editor")]
        [Sirenix.OdinInspector.Button]
        private void Randomize()
        {
            // if (id != PokemonEnum.玩家)
            // {
            //     guid = Guid.NewGuid();
            // }
            level = (ushort)UnityEngine.Random.Range(1, 100);
            health = UnityEngine.Random.Range(1, 100);
            basePower = UnityEngine.Random.Range(1, 100);
            baseDefense = UnityEngine.Random.Range(1, 100);
            baseAdaptability = UnityEngine.Random.Range(1, 100);
            baseSpeed = UnityEngine.Random.Range(1, 100);
        }

        [Sirenix.OdinInspector.HorizontalGroup("Editor")]
        [Sirenix.OdinInspector.Button]
        private void NewGuid()
        {
            guid = Guid.NewGuid();
        }

        [JsonIgnore]
        // Guid String
        [Sirenix.OdinInspector.ReadOnly, Sirenix.OdinInspector.ShowInInspector]
        private string guidString => guid.ToString();
#endif

        #endregion
    }
}