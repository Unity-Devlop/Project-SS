using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace Game.LoopHero
{
    public class Fighter : MonoBehaviour
    {
        [NonSerialized, Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        private FighterData _data;

        [Sirenix.OdinInspector.HorizontalGroup("DEBUG")] [Obsolete("TODO 后面删除掉 读表")] // TODO
        public GameObject pokemonPrefab;


        [Sirenix.OdinInspector.HorizontalGroup("DEBUG")] [Obsolete("TODO 后面删除掉 读表")] // TODO
        public GameObject trainerPrefab;

        private Transform[] _positions;
        private Transform _trainerPos;

        private Trainer _trainer;
        private List<Pokemon> _pokemons;

        private void Awake()
        {
            _pokemons = new List<Pokemon>(6);

            _positions = new Transform[6];

            _positions[0] = transform.Find("P0");
            _positions[1] = transform.Find("P1");
            _positions[2] = transform.Find("P2");
            _positions[3] = transform.Find("P3");
            _positions[4] = transform.Find("P4");
            _positions[5] = transform.Find("P5");

            _trainerPos = transform.Find("Trainer");
        }

        public async UniTask Bind(FighterData data)
        {
            Assert.IsNull(_data);
            _data = data;
            _pokemons.Clear();
        }


        public async UniTask FightStart()
        {
            // 训练家入场
            await EnterBattle(_data.teamData.playerData.self, _trainerPos, trainerPrefab);
            // 宝可梦入场
            for (var i = 0; i < _data.teamData.battlePokemonList.Count; i++)
            {
                await EnterBattle(_data.teamData.battlePokemonList[i], _positions[i], pokemonPrefab);
            }

            await InitializeFight();
        }

        private async UniTask InitializeFight()
        {
            // TODO 战斗开始效果结算
            await UniTask.CompletedTask;
        }

        private async UniTask EnterBattle(PokemonData pokemon, Transform target, GameObject prefab)
        {
            var go = Instantiate(prefab, target.position, Quaternion.identity, target);
            // DOTWEEN 移动
            Vector3 targetPosition = go.transform.localPosition;
            Vector3 startPosition = targetPosition - new Vector3(2, 0, 0);
            go.transform.localPosition = startPosition;
            await go.transform.DOLocalMoveX(targetPosition.x, 0.5f).SetEase(Ease.Linear);
            var p = go.GetComponent<Pokemon>();
            await p.Bind(pokemon);
            _pokemons.Add(p);
        }

        public async UniTask RoundStart()
        {
            //TODO 回合开始效果结算
            await UniTask.CompletedTask;
        }
    }
}