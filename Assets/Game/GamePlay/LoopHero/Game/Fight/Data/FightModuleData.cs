using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityToolkit;
using UnityToolkit.Collections;

namespace Game.LoopHero
{
    [Serializable]
    public sealed class FightModuleData : Model<FightModuleData>
    {
        public FighterData self;
        public FighterData enemy;

        private List<PokemonData> _trainerList = new List<PokemonData>();


        public IEnumerator<PokemonData> CreateBattlePokemonEnumerator()
        {
            _trainerList.Clear();
            _trainerList.Add(self.teamData.playerData);
            _trainerList.Add(enemy.teamData.playerData);
            
            Assert.IsNotNull(_trainerList);
            Assert.IsNotNull(self.teamData.battlePokemonList);
            Assert.IsNotNull(enemy.teamData.battlePokemonList);
            return new ListEnumerator<PokemonData>(
                _trainerList,
                self.teamData.battlePokemonList,
                enemy.teamData.battlePokemonList
            );
        }

        public IEnumerator<PokemonData> CreateAllPokemonEnumerator()
        {
            _trainerList.Clear();
            _trainerList.Add(self.teamData.playerData);
            _trainerList.Add(enemy.teamData.playerData);
            Assert.IsNotNull(_trainerList);
            Assert.IsNotNull(self.teamData.battlePokemonList);
            Assert.IsNotNull(self.teamData.candidatePokemonList);
            Assert.IsNotNull(enemy.teamData.battlePokemonList);
            Assert.IsNotNull(enemy.teamData.candidatePokemonList);
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
            Assert.IsNotNull(self.teamData.currentBuffList);
            Assert.IsNotNull(enemy.teamData.currentBuffList);
            return new ListEnumerator<BuffData>(
                self.teamData.currentBuffList,
                enemy.teamData.currentBuffList
            );
        }
    }
}