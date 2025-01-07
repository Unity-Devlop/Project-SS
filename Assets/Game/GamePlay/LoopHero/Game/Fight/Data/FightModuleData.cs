using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityToolkit;
using UnityToolkit.Collections;

namespace Game.LoopHero
{
    [Serializable]
    public sealed class FightModuleData : ObservationObject<FightModuleData>
    {
        public FighterData self;
        public FighterData enemy;

        private List<PokemonData> _trainerList;

        public void DestroyTempData()
        {
            _trainerList.Clear();
            _trainerList = null;
        }

        public void CreateTempData()
        {
            _trainerList = new List<PokemonData>(2)
            {
                self.teamData.playerData,
                enemy.teamData.playerData
            };
        }

        public IEnumerator<PokemonData> CreateBattlePokemonEnumerator()
        {
            return new ListEnumerator<PokemonData>(
                _trainerList,
                self.teamData.battlePokemonList,
                enemy.teamData.battlePokemonList
            );
        }

        public IEnumerator<PokemonData> CreateAllPokemonEnumerator()
        {
            return new ListEnumerator<PokemonData>(
                _trainerList,
                self.teamData.battlePokemonList,
                self.teamData.candidatePokemonList,
                enemy.teamData.battlePokemonList,
                enemy.teamData.candidatePokemonList
            );
        }

        public IEnumerator<BuffData> CreateBuffEnumerator()
        {
            return new ListEnumerator<BuffData>(
                self.teamData.currentBuffList,
                enemy.teamData.currentBuffList
            );
        }
    }
}