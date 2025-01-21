#if UNITY_EDITOR

using System;
using cfg;
using UnityEngine;

namespace Game.LoopHero.Debug
{
    [CreateAssetMenu(menuName = "Game/FightDebugSO")]
    public class FightDebugSO : ScriptableObject
    {
        public FightModuleData data;


        [Sirenix.OdinInspector.Button]
        private void Random()
        {
            throw new NotImplementedException();
        }


        [Obsolete]
        [Sirenix.OdinInspector.Button]
        internal void AddCandidatePokemonSelf()
        {
            var pokemon = PokemonData.New(PokemonEnum.烈火领主, data.self.teamData.trainerId);
            pokemon.baseHealth = 100;
            pokemon.currentHealth = 100;
            data.self.teamData.candidatePokemonQueue.Enqueue(pokemon);
        }

        [Obsolete]
        [Sirenix.OdinInspector.Button]
        internal void AddCandidatePokemonEnemy()
        {
            var pokemon = PokemonData.New(PokemonEnum.烈火领主, data.enemy.teamData.trainerId);
            pokemon.baseHealth = 100;
            pokemon.currentHealth = 100;
            data.enemy.teamData.candidatePokemonQueue.Enqueue(pokemon);
        }
        //
    }
}
#endif