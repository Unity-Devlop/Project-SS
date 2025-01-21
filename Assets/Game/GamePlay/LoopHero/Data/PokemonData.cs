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


        [JsonIgnore] public int finalPower => (int)(powerPercent * (basePower + additionalPower + fightTempPower));
        [JsonIgnore] public int finalDefense => baseDefense + additionalDefense + fightTempDefense;
        [JsonIgnore] public int finalAdaptability => baseAdaptability + additionalAdaptability;
        [JsonIgnore] public int finalSpeed => baseSpeed + additionalSpeed + fightTempSpeed;

        [JsonIgnore] public int fightTempSpeed;
        [JsonIgnore] public int fightTempPower;
        [JsonIgnore] public int fightTempDefense;


        /// <summary>
        /// 威力有效百分比
        /// </summary>
        [JsonRequired]
        public float powerPercent { get; private set; } = 1;

        public static PokemonData New(PokemonEnum id,int trainerId)
        {
            return new PokemonData()
            {
                id = id,
                trainerId = trainerId,
                guid = Guid.NewGuid()
            };
        }

        public override string ToString()
        {
            return $"{id}-{trainerId}-{guid}";
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

// --------------------- TEMP ---------------------
        public void AddTempHealthPercent(float f)
        {
            currentHealth += (int)(baseHealth * f);
            currentHealth = Mathf.Min(currentHealth, baseHealth);
            GameLogger.Log.Debug("战斗临时血量增加{0}", (int)(baseHealth * f));
        }

        public void AddFightTempPower(int i)
        {
            fightTempPower += i;
            GameLogger.Log.Debug("战斗临时攻击增加{0}", i);
        }

        public void AddFightTempDefense(int i)
        {
            fightTempDefense += i;
            GameLogger.Log.Debug("战斗临时防御增加{0}", i);
        }

        public void AddFightTempSpeed(int i)
        {
            fightTempSpeed += i;
            GameLogger.Log.Debug("战斗临时速度增加{0}", i);
        }
// --------------------- TEMP ---------------------


// --------------------- Permanent ---------------------

        public void AddPermanentHealth(int i)
        {
            baseHealth += i;
        }

        public void AddPermanentPower(int i)
        {
            basePower += i;
        }

        public void AddPermanentDefense(int i)
        {
            baseDefense += i;
        }

        public void AddPermanentSpeed(int i)
        {
            baseSpeed += i;
        }

        public void DecreasePowerPercent(float f)
        {
            powerPercent -= f;
            powerPercent = Mathf.Max(0, powerPercent);
        }

// --------------------- Permanent ---------------------


        public void TakeTrueDamage(int i)
        {
            currentHealth -= i;
            currentHealth = Mathf.Max(0, currentHealth);
        }
    }
}