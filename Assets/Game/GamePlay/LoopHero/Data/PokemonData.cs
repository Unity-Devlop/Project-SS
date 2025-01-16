using System;
using cfg;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;
using UnityToolkit;

namespace Game.LoopHero
{
    [Serializable]
    public class PokemonData : Model<PokemonData>
    //: IEquatable<PokemonData>
    {
        public PokemonEnum id;

        [JsonIgnore] [LiteDB.BsonIgnore] public PokemonConfig config => Core.Tables.PokemonTable.Get(id);

        /// <summary>
        /// 唯一标识
        /// </summary>
        [JsonRequired]
        public Guid guid { get; private set; }

        [JsonRequired] [field: SerializeField] public int trainerId { get; private set; }

        [JsonIgnore] public bool alive => currentHealth > 0;
        public ushort level;

        [Sirenix.OdinInspector.HorizontalGroup("Health")]
        public int baseHealth;

        [Sirenix.OdinInspector.HorizontalGroup("Health")]
        public int currentHealth;

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
        [JsonIgnore] public int finalSpeed => baseSpeed + additionalSpeed + fightTempSpeed;

        [JsonIgnore] public int fightTempSpeed;


        public void AddFightTempSpeed(int i)
        {
            fightTempSpeed += i;
        }

        public void AddPermanentSpeed(int i)
        {
            baseSpeed += i;
        }

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
            baseHealth = UnityEngine.Random.Range(1, 100);
            currentHealth = baseHealth;
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